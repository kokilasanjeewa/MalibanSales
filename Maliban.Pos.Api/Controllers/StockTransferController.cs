using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class StockTransfer : BaseController
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;

        [System.Obsolete]
        public StockTransfer(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
        }

        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getstockTransferHeader/{stockTransferHeaderId}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getstockTransferHeader/{stockTransferHeaderId}", Name = "GetStockTransferHeader")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetStockTransferHeader([FromQuery] bool status, int stockTransferHeaderId)
        {
            var stockTransferHeaders = await _context.StockTransferHeaders.Where(p => p.StockTransferHeaderId == stockTransferHeaderId).Include(b => b.BranchLocation).Include(p => p.Lines).FirstOrDefaultAsync();
            var transferBranch = await _context.BranchLocations.Where(p => p.BranchLocationId == stockTransferHeaders.TransferBranchLocationId).FirstOrDefaultAsync();
            stockTransferHeaders.TransferBranchLocation = transferBranch;
            stockTransferHeaders.TotalDiscountAmount = 0;
            stockTransferHeaders.TotalBillAmount = stockTransferHeaders.Lines.Sum(l => l.ApproveQuantity > 0 ? l.ApproveQuantity * l.dCostPrice : l.dTransferQuantity * l.dCostPrice);
            stockTransferHeaders.TotalCost = stockTransferHeaders.Lines.Sum(l => l.ApproveQuantity > 0 ? l.ApproveQuantity * l.dCostPrice : l.dTransferQuantity * l.dCostPrice);

            if (stockTransferHeaders != null)
            {
                return Ok(stockTransferHeaders);
            }
            return NoContent();
        }

        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallstocks"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getstocktransferlist", Name = "GetStockTransferList")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetStockTransferList(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var shopstocks = await _repoWrapper.StockTransfer.GetStockTransferHeaderAsync(userParams, BranchLocationId);
            if (shopstocks.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = shopstocks.Where(p => p.Active = true);
                    Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = shopstocks.Where(p => p.Active = false);
                    Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
                    return Ok(resultfalse);
            }

        }

        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="createstocktransferdetails"></param> 
        [HttpPost("createstocktransferdetails")]
        public async Task<IActionResult> CreateStockTransferDetails([FromForm] CreateStockTransferDetailDto stockTransferDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _repoWrapper.VerifyBarCode(stockTransferDetail.barCode))
            {
                return NotFound("Please scan valid item code.");
            }
            var stockTransferItems = await _repoWrapper.StockTransfer.CreateStockTransferDetails(stockTransferDetail);
            if (stockTransferItems == null || stockTransferItems.Count == 0)
            {
                return NotFound("Please check your bin card or item quantity is zero.");
            }
            return Ok(new
            {
                stockTransferItems,
                computeGrandTotalValue = _repoWrapper.StockTransfer.ComputeGrandTotalValue,
                computeTotalValue = _repoWrapper.StockTransfer.ComputeTotalValue,
                computeDiscountValue = _repoWrapper.StockTransfer.ComputeDiscountValue

            });
        }

        /// <summary>
        ///  Get Stock Products.
        /// </summary> 
        /// <param name="getstockproducts"></param> 
        [HttpGet("getstockproducts/{branchLocationId}/{isProduct}")]
        public IActionResult GetStockProducts(int branchLocationId, bool isProduct)
        {
            var getStockProducts = _repoWrapper.StockTransfer.GetStockProducts(branchLocationId, isProduct);
            if (getStockProducts == null || getStockProducts.Count == 0)
            {
                return NotFound("Please check your bin card or item quantity is zero.");
            }
            return Ok(getStockProducts);
        }

        /// <summary>
        ///  Get Stock Products.
        /// </summary> 
        /// <param name="getcachestocktransferdetails"></param> 
        [HttpGet("getcachestocktransferdetails/{branchLocationId}")]
        public async Task<IActionResult> GetCacheStockTransferDetailAsync(int branchLocationId)
        {
            var getCacheStockTransferDetail = await _repoWrapper.StockTransfer.GetCacheStockTransferDetailAsync(branchLocationId, "stockTransferListCacheKey");
            if (getCacheStockTransferDetail == null)
            {
                return NotFound("Please check your order.");
            }
            return Ok(new
            {
                getCacheStockTransferDetail,
                computeGrandTotalValue = _repoWrapper.StockTransfer.ComputeGrandTotalValue,
                computeTotalValue = _repoWrapper.StockTransfer.ComputeTotalValue,
                computeDiscountValue = _repoWrapper.StockTransfer.ComputeDiscountValue
            });
        }

        /// </remarks>
        /// <param name="getproductpricesbyproductId/{branchLocationId}/{productId}"></param>
        /// <returns>A fetched price</returns>
        /// <response code="200">Returns the fetched item</response>
        /// <response code="404">If the item is null</response> 
        [HttpGet("getproductpricesbyproductId/{branchLocationId}/{productId}", Name = "GetProductPricesByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1.1")]
        public IActionResult GetProductPricesByProductId(int branchLocationId, Int64 productId)
        {
            var getStockProductPrices = _repoWrapper.StockTransfer.GetPricesByStockProduct(branchLocationId, productId);
            if (getStockProductPrices == null || getStockProductPrices.Count == 0)
            {
                return NotFound($"You've unable to find the product price by the given Product {productId}.");

            }
            return Ok(getStockProductPrices);
        }

        /// <summary>
        ///  Remove Line.
        /// </summary> 
        /// <param name="removeline"></param> 
        [HttpDelete("removeline/{lineId}")]
        public async Task<IActionResult> RemoveLine(string lineId)
        {
            await _repoWrapper.StockTransfer.RemoveLine(lineId);
            return Ok("Done.");
        }

        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Check/createstocktransferheader"></param> 
        [HttpPost("Check/createstocktransferheader")]
        public async Task<IActionResult> CreateStockTransferHeader([FromForm] CreateStockTransferHeaderDto transferHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            int result;
            var stockTransferHeader = _mapper.Map<StockTransferHeader>(transferHeaderDto);
            var getCacheStockTransferDetails = await _repoWrapper.StockTransfer.GetCacheStockTransferDetailAsync(transferHeaderDto.BranchLocationId, "stockTransferListCacheKey");

            stockTransferHeader.StockTransferHeaderFactoryId = await _context.StockTransferHeaders.Where(c => c.BranchLocationId == transferHeaderDto.BranchLocationId).CountAsync() + 1;
            var count = await _context.StockTransferHeaders.Where(c => c.BranchLocationId == transferHeaderDto.BranchLocationId).CountAsync() + 1;
            var branchCode = await _context.BranchLocations.Where(u => u.BranchLocationId == transferHeaderDto.BranchLocationId).Select(c => new { c.BranchCode }).FirstOrDefaultAsync();
            stockTransferHeader.StockTransferHeaderReferenceNo = branchCode.BranchCode.Trim().ToString() + (DateTime.Now.Year.ToString()) + (count.ToString());
            stockTransferHeader.ApplicationUser = await _userManager.Users.Where(u => u.Id == transferHeaderDto.Id).FirstOrDefaultAsync();
            stockTransferHeader.CreatedBy = transferHeaderDto.createdBy;
            stockTransferHeader.BranchLocationId = transferHeaderDto.BranchLocationId;
            stockTransferHeader.TransferBranchLocationId = transferHeaderDto.TransferBranchLocationId;
            stockTransferHeader.TotalCost = _repoWrapper.StockTransfer.ComputeTotalValue;
            stockTransferHeader.TotalDiscountAmount = _repoWrapper.StockTransfer.ComputeDiscountValue;
            stockTransferHeader.TotalBillAmount = _repoWrapper.StockTransfer.ComputeGrandTotalValue;
            stockTransferHeader.cDocType = cDocType.STO;
            List<StockTransferDetail> lineCollection = new List<StockTransferDetail>();
            foreach (StockTransferDetail item in getCacheStockTransferDetails.AsEnumerable().ToList())
            {
                var pod = item;
                pod.CreatedBy = transferHeaderDto.createdBy;
                pod.CreateDate = DateTime.Now;
                lineCollection.Add(pod);
            }
            stockTransferHeader.Lines = lineCollection;
            if (getCacheStockTransferDetails.ToList().Count > 0 && stockTransferHeader != null)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.AttachRange(stockTransferHeader.Lines.ToList());
                            await _context.StockTransferHeaders.AddAsync(stockTransferHeader);
                            // Attempt to save changes to the database
                            result = await _context.SaveChangesAsync();
                            if (result > 0)
                            {
                                foreach (var item in getCacheStockTransferDetails.ToList())
                                {
                                    var itemCode = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                    var updateTransaction = await _context.TransactionDetails.Where(t => t.TransactionDetailId == item.TransactionDetailId && t.dQuantity - t.dIssueQuantity > 0).FirstOrDefaultAsync();
                                    if (updateTransaction.dQuantity - updateTransaction.dIssueQuantity >= item.dTransferQuantity)
                                    {
                                        updateTransaction.dIssueQuantity += item.dTransferQuantity;
                                        _context.TransactionDetails.Update(updateTransaction);
                                        _context.SaveChanges();
                                    }
                                    logger.Information("{\"BranchLocationId\":" + updateTransaction.BranchLocationId + ",\"Barcode\":\"" + updateTransaction.Barcode +
                                        "\",\"ProductId\":" + updateTransaction.ProductId + ",\"PriceId\":" + updateTransaction.PriceId + ",\"dQuantity\":" + updateTransaction.dQuantity +
                                        ",\"dIssueQuantity\":" + updateTransaction.dIssueQuantity + ",\"StockTransferHeaderId\":" + stockTransferHeader.StockTransferHeaderId + ",\"dTransferQuantity\":" + item.dTransferQuantity + ",\"ApproveQuantity\":" + 0 +
                                        ",\"TransactionDetailId\":" + updateTransaction.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"},");

                                }

                                //stop method _repoWrapper.ShopStock.UpdateSaleTransaction() and run here
                                //update transaction update sale invoice start

                                List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                                foreach (var item in getCacheStockTransferDetails.ToList())
                                {
                                    var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == transferHeaderDto.BranchLocationId).FirstOrDefaultAsync<Price>();
                                    var product = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                    TransactionDetail transactionDetail = new TransactionDetail
                                    {
                                        dQuantity = item.dTransferQuantity,
                                        ProductAvailable = true,
                                        BranchLocationId = transferHeaderDto.BranchLocationId,
                                        ProductId = item.ProductId,
                                        PriceId = item.PriceId,
                                        iDocID = item.StockTransferDetailId,
                                        cDocNo = "STO" + item.StockTransferDetailId,
                                        dDocDate = DateTime.Now,
                                        cDocType = cDocType.STO,
                                        cExtRefNum = "Stock Transfer Out",
                                        dIssueQuantity = item.dTransferQuantity,
                                        dNetQuantity = -item.dTransferQuantity,
                                        dCostPrice = price.ModifiedUnitPrice,
                                        dSalePrice = price.SalePrice,
                                        dValue = -item.dTransferQuantity * price.ModifiedUnitPrice,
                                        dNetValue = 0,
                                        dIN = 0,
                                        dOUT = item.dTransferQuantity,
                                        Barcode = product.Barcode,
                                        CreatedBy = transferHeaderDto.createdBy,
                                        CreateDate = DateTime.Now,
                                        ProductExpireDate = item.ProductExpireDate,
                                        ProductName = product.ProductName
                                    };
                                    updateshopStock.Add(transactionDetail);
                                }
                                _context.TransactionDetails.AddRange(updateshopStock);
                                _context.SaveChanges();
                            }
                            transaction.Commit();
                        }

                        catch (Exception e)
                        {
                            transaction.Rollback();
                            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                        }
                        finally
                        {
                            transaction.Dispose();
                            await _repoWrapper.StockTransfer.Remove();

                        }

                    }

                });
                return Ok(new { message = "Save Successfully.", stockTransferHeaderId = stockTransferHeader.StockTransferHeaderId });
            }
            return NoContent();
        }

        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Confirm/confirmstocktransferheader"></param> 
        [HttpPost("receive/receivestocktransferheader")]
        public async Task<IActionResult> ReceiveStockTransferHeader([FromBody] ApproveStocktransferHeaderDto transferHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var stockTransferHeader = await _context.StockTransferHeaders.Where(s => s.StockTransferHeaderId == transferHeaderDto.StockTransferHeaderId).Include(l => l.Lines).FirstOrDefaultAsync<StockTransferHeader>();

            stockTransferHeader.ReceivedBy = transferHeaderDto.createdBy;
            stockTransferHeader.DateReceived = DateTime.Now;

            if (stockTransferHeader.Lines.ToList().Count > 0 && stockTransferHeader != null)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {

                        try
                        {
                            #region

                            foreach (var item in stockTransferHeader.Lines.ToList())
                            {
                                var price = await _context.Prices.AsNoTracking().FirstOrDefaultAsync(p => p.PriceId == item.PriceId);
                                Price priceTransfer = new Price();
                                var product = await _context.Products.FirstOrDefaultAsync(sp => sp.ProductId == item.ProductId);
                                price.PriceId = 0;
                                price.PriceUid = Guid.NewGuid().ToString();
                                priceTransfer.BranchLocationId = stockTransferHeader.TransferBranchLocationId;
                                priceTransfer.CreatedBy = transferHeaderDto.createdBy;
                                priceTransfer = price;
                                await _repoWrapper.Price.CreateAsync(priceTransfer);
                                _context.SaveChanges();
                                TransactionDetail transactionDetail = new TransactionDetail
                                {
                                    dCostPrice = priceTransfer.ModifiedUnitPrice,
                                    dSalePrice = priceTransfer.SalePrice,
                                    dQuantity = item.ApproveQuantity,
                                    dIN = item.ApproveQuantity,
                                    dOUT = 0,
                                    dIssueQuantity = 0,
                                    dNetQuantity = item.ApproveQuantity,
                                    dValue = priceTransfer.ModifiedUnitPrice * (item.ApproveQuantity),
                                    dNetValue = priceTransfer.ModifiedUnitPrice * (item.ApproveQuantity),
                                    cExtRefNum = "Stock Transfer In",
                                    Product = product,
                                    Barcode = product.Barcode,
                                    ProductName = product.ProductName,
                                    iDocID = stockTransferHeader.StockTransferHeaderId,
                                    cDocType = cDocType.STI,
                                    cDocNo = "STI" + stockTransferHeader.StockTransferHeaderId,
                                    ProductId = product.ProductId,
                                    PriceId = priceTransfer.PriceId,
                                    Price = priceTransfer,
                                    CreatedBy = transferHeaderDto.createdBy,
                                    CreateDate = DateTime.Now,
                                    BranchLocationId = stockTransferHeader.TransferBranchLocationId,
                                    ProductExpireDate = item.ProductExpireDate,
                                    dDocDate = DateTime.Now
                                };
                                await _repoWrapper.TransactionDetail.CreateAsync(transactionDetail);
                                _context.SaveChanges();
                                logger.Information("{\"BranchLocationId\":" + stockTransferHeader.TransferBranchLocationId + ",\"Barcode\":\"" + item.Barcode +
                                "\",\"ProductId\":" + item.ProductId + ",\"PriceId\":" + item.PriceId + ",\"dQuantity\":" + item.dQuantity +
                                ",\"dIssueQuantity\":" + item.dIssueQuantity + ",\"StockTransferHeaderId\":" + stockTransferHeader.StockTransferHeaderId + ",\"dTransferQuantity\":" + item.dTransferQuantity + ",\"ApproveQuantity\":" + item.ApproveQuantity +
                                ",\"TransactionDetailId\":" + item.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"},");
                            }
                            stockTransferHeader.IsProcess = true;
                            stockTransferHeader.IsComfirm = true;
                            _context.StockTransferHeaders.Update(stockTransferHeader);
                            _context.SaveChanges();
                            #endregion
                            transaction.Commit();
                        }
                        catch (DbUpdateException e)
                        {
                            transaction.Rollback();
                            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                        }
                        finally
                        {
                            transaction.Dispose();
                        }

                    }

                });
                return Ok(new { message = "Received Successfully.", stockTransferHeaderId = stockTransferHeader.StockTransferHeaderId });
            }
            return NoContent();
        }

        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="approveStockTransferDetail/{stockTransferDetailUid}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("approve/{stockTransferDetailUid}/{status}", Name = "ApproveStockTransferDetail")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> ApproveStockTransferDetail([FromQuery] bool status, string stockTransferDetailUid)
        {
            var entity = await _context.StockTransferDetails.Where(p => p.StockTransferDetailUid == stockTransferDetailUid).FirstOrDefaultAsync();
            entity.cApprovalType = status ? (cApprovalType)1 : (cApprovalType)0;
            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + entity.StockTransferDetailUid + "'.");
            }
            // Update changes if any of the properties have been modified.
            _context.Entry(entity).CurrentValues.SetValues(entity);
            _context.Entry(entity).State = EntityState.Modified;
            try
            {

                if (_context.Entry(entity).Properties.Any(property => property.IsModified))
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }
            return Ok(new { message = "Successfully approved." });
        }

        /// <summary>
        /// You can call approve method.Please make sure stock transfer details already exists in the list
        /// </summary> 
        /// <param name="approve/approvestocktransferheader"></param> 
        [AllowAnonymous]
        [HttpPost("approve/approvestocktransferheader")]
        public async Task<IActionResult> ApproveStockTransferHeader([FromBody] ApproveStocktransferHeaderDto stocktransferHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var entity = await _context.StockTransferHeaders.Where(s => s.StockTransferHeaderId == stocktransferHeaderDto.StockTransferHeaderId).FirstOrDefaultAsync<StockTransferHeader>();
            // Check that the record exists.
            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + stocktransferHeaderDto.StockTransferHeaderId + "'.");
            }
            entity.ModifiedDate = DateTime.Now;
            entity.ApprovedBy = stocktransferHeaderDto.createdBy;
            entity.ModifiedBy = stocktransferHeaderDto.createdBy;
            entity.Id = stocktransferHeaderDto.id;
            entity.cApprovalType = stocktransferHeaderDto.cApprovalType;
            if ((int)stocktransferHeaderDto.cApprovalType == 1 && entity.IsApprove == false)
            {
                entity.IsApprove = true;
                entity.IsComfirm = true;
            }
            if ((int)stocktransferHeaderDto.cApprovalType == 2 && entity.IsApprove == false && entity.IsProcess == false)
            {
                entity.IsApprove = false;
                entity.IsComfirm = false;
            }
            if ((int)stocktransferHeaderDto.cApprovalType == 3 && entity.IsApprove == false && entity.IsProcess == false)
            {
                entity.IsDeleted = true;
            }
            if ((int)stocktransferHeaderDto.cApprovalType == 3 && entity.IsApprove == true && entity.IsProcess == true)
            {
                return BadRequest("You've unable to cancel po with id '" + entity.StockTransferHeaderReferenceNo);
            }
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.StockTransferHeaders.Update(entity);
                        var entities = await _context.StockTransferDetails.Where(p => p.StockTransferHeaderId == stocktransferHeaderDto.StockTransferHeaderId).ToListAsync();
                        foreach (var entitydetail in entities)
                        {
                            entitydetail.cApprovalType = stocktransferHeaderDto.cApprovalType;
                            if (entitydetail.cApprovalType == cApprovalType.Approve)
                            {
                                entitydetail.ApproveQuantity = entitydetail.dTransferQuantity;

                            }
                            if (_context.Entry(entitydetail).Properties.Any(property => property.IsModified))
                            {
                                // Update changes if any of the properties have been modified.
                                _context.Entry(entitydetail).CurrentValues.SetValues(entitydetail);
                                _context.Entry(entitydetail).State = EntityState.Modified;
                            }
                        }

                        // Update changes if any of the properties have been modified.
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        transaction.Rollback();
                        logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);

                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            });
            return Ok(new { message = "Successfully Approved.", stockTransferHeaderId = entity.StockTransferHeaderId });
        }
    }
}

