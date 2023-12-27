using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Maliban.Pos.Api.Helpers;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class PurchaseOrderController : BaseController
    {
        #region PurchaseConstructor
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private PurchaseOrder order;

        // private IDistributedCache cache;

        [Obsolete]
        public PurchaseOrderController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, PurchaseOrder orderService, IDistributedCache cache) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            order = orderService;
            //  this.cache = cache;
        }
        #endregion
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallpurchaseorderheaders"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallpurchaseorderheaders", Name = "GetAllPurchaseOrderHeaders")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllPurchaseOrderHeaders(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var purchaseOrderHeaders = await _repoWrapper.PurchaseOrderHeader.GetPurchaseOrderHeadersAsync(userParams, BranchLocationId);
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
            if (purchaseOrderHeaders.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = purchaseOrderHeaders.Where(p => p.Active = true);
                    Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = purchaseOrderHeaders.Where(p => p.Active = false);
                    Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                    return Ok(resultfalse);
                    /*                default:
                                        Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                                        return Ok(purchaseOrderHeaders);*/
            }
        }
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getpurchaseorderheader/{purchaseOrderHeaderId}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getpurchaseorderheader/{purchaseOrderHeaderId}", Name = "GetPurchaseOrderHeader")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetPurchaseOrderHeader([FromQuery] bool status, int purchaseOrderHeaderId)
        {
            var purchaseOrderHeaders = await _repoWrapper.PurchaseOrderHeader.FindByCondition(p => p.PurchaseOrderHeaderId == purchaseOrderHeaderId).Include(s => s.Supplier).Include(b => b.BranchLocation).Include(p => p.PurchaseOrderDetails).ThenInclude(pr => pr.SupplierPrice).ThenInclude(p => p.Product).FirstOrDefaultAsync();
            foreach (var item in purchaseOrderHeaders.PurchaseOrderDetails)
            {
                var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == item.ProductId).FirstOrDefaultAsync();
                item.Product = product;
            }
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
            if (purchaseOrderHeaders != null)
            {
                return Ok(purchaseOrderHeaders);
            }
            return NoContent();
        }
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallpurchaseorderdetails"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallpurchaseorderdetails", Name = "GetAllPurchaseOrderDetails")]
        [MapToApiVersion("1.1")]
        public IActionResult GetAllPurchaseOrderDetails(bool status, [FromQuery] UserParams userParams, int purchaseOrderHeaderId)
        {
            var purchaseOrderHeaders = _repoWrapper.PurchaseOrderDetail.GetPurchaseOrderDetailsAsync(userParams, purchaseOrderHeaderId);
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
            if (purchaseOrderHeaders.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = purchaseOrderHeaders.Where(p => p.Active = true);
                    Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = purchaseOrderHeaders.Where(p => p.Active = false);
                    Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                    return Ok(resultfalse);
                    /*                default:
                                        Response.AddPagination(purchaseOrderHeaders.CurrentPage, purchaseOrderHeaders.PageSize, purchaseOrderHeaders.TotalCount, purchaseOrderHeaders.TotalPages);
                                        return Ok(purchaseOrderHeaders);*/
            }
        }
        /// <summary>
        ///  Get PurchaseOrderItemDetails.
        /// </summary> 
        /// <param name="Get/{purchaseorderId}"></param> 
        [AllowAnonymous]
        [HttpGet("Get/{purchaseorderId}", Name = "GetPurchaseOrderItemDetails")]
        public async Task<ActionResult<PurchaseOrderDetail>> GetPurchaseOrderItemDetails(int? purchaseorderId)
        {
            List<PurchaseOrderDetail> po = new List<PurchaseOrderDetail>();
            var purchaseOrderItemDetails = await _context.PurchaseOrderDetails.
               Where(p => p.PurchaseOrderHeaderId == purchaseorderId).Include(sp => sp.SupplierPrice).Include(p => p.Product).Include(p => p.PurchaseOrderHeaders).ThenInclude(b => b.BranchLocation).ToListAsync();
            //  var stockDetails = await _context.TransactionDetails.Where(s => s.IsDeleted == false && s.BranchLocationId == purchaseOrderItemDetails[0].PurchaseOrderHeaders.BranchLocationId && s.dQuantity - s.dIssueQuantity > 0).GroupBy(t => new { t.ProductId, t.Barcode, t.ProductName,t.ProductExpireDate }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dQuantity - t.dIssueQuantity), Barcode = p.Key.Barcode, ProductName = p.Key.ProductName, ProductExpiryDate=p.Key.ProductExpireDate }).ToListAsync();
            //   var purchaseOrderItemDetailswithStockQty = from p in purchaseOrderItemDetails join s in stockDetails on p.ProductId equals s.ProductId into gj from substockdetail in gj.DefaultIfEmpty() select new { p, dNetQuantity = substockdetail?.dNetQuantity ?? 0 };
            foreach (var item in purchaseOrderItemDetails)
            {
                var stockDetails = await _context.TransactionDetails.Where(s => s.ProductId == item.ProductId && s.dCostPrice == item.SupplierPrice.UnitPrice && s.IsDeleted == false && s.BranchLocationId == purchaseOrderItemDetails[0].PurchaseOrderHeaders.BranchLocationId && s.dQuantity - s.dIssueQuantity > 0).GroupBy(t => new { t.ProductId, t.Barcode, t.ProductName }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dQuantity - t.dIssueQuantity), Barcode = p.Key.Barcode, ProductName = p.Key.ProductName }).FirstOrDefaultAsync();
                item.CurrentQuantity = stockDetails?.dNetQuantity == null ? 0 : stockDetails.dNetQuantity;
                po.Add(item);
            }
            if (po.Count() > 0)
            {
                return Ok(po);
            }
            return NotFound("PurchaseorderId doesn't exists.");
        }
        /// <summary>
        ///  Get PurchaseOrderItemDetails.
        /// </summary> 
        /// <param name="Search/{purchaseorderId}"></param> 
        [AllowAnonymous]
        [HttpGet("Search/{purchaseorderId}", Name = "SearchPurchaseOrder")]
        public async Task<IActionResult> SearchPurchaseOrder(int? purchaseorderId)
        {

            var purchaseOrderItemDetails = await _context.PurchaseOrderHeaders.
            Where(p => p.PurchaseOrderHeaderId == purchaseorderId)
            .Select(s => new { s.BranchLocationId, s.SupplierId }).ToListAsync();
            return Ok(purchaseOrderItemDetails);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="createpurchaseorderitemdetails"></param> 
        [AllowAnonymous]
        [HttpPost("createpurchaseorderitemdetails")]
        public async Task<IActionResult> CreatePurchaseOrderItemDetails([FromForm] CreatePurchaseOrderDetailsDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PurchaseOrder cartnew = GetSale();

            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
                var SupplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == createSaleItemDetailsDto.SupplierPriceId && sp.SupplierId == createSaleItemDetailsDto.SupplierId && sp.BranchLocationId == createSaleItemDetailsDto.BranchLocationId && sp.ProductId == product.ProductId).Include(p => p.Product).OrderByDescending(sp => sp.CreateDate).FirstOrDefaultAsync();
                order.AddItem(createSaleItemDetailsDto.quantity, SupplierPrice, createSaleItemDetailsDto.ProductExpiryDate);
                SaveCart(order);
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            PurchaseOrder cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //   cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="createpurchaseorderitemdetaillist"></param> 
        [AllowAnonymous]
        [HttpPost("createpurchaseorderitemdetaillist")]
        public async Task<IActionResult> CreatePurchaseOrderItemDetailList([FromBody] CreatePurchaseOrderDetailsDto[] createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PurchaseOrder cartnew = GetSale();
            foreach (var item in createSaleItemDetailsDto)
            {
                if (await _repoWrapper.VerifyBarCode(item.barCode))
                {
                    var product = await _repoWrapper.Product.GetProductAsync(item.barCode);
                    var SupplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == item.SupplierPriceId && sp.SupplierId == item.SupplierId && sp.BranchLocationId == item.BranchLocationId && sp.ProductId == product.ProductId).Include(p => p.Product).OrderByDescending(sp => sp.CreateDate).FirstOrDefaultAsync();
                    order.AddItem(item.quantity, SupplierPrice, item.ProductExpiryDate, item.approveQuantity, item.cApprovalType);
                    SaveCart(order);
                }
                else
                {
                    return NotFound("Barcode doesn't exists.");
                }
            }
            PurchaseOrder cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //   cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="updateapprovequantity"></param> 
        [AllowAnonymous]
        [HttpPost("updateapprovequantity")]
        public async Task<IActionResult> UpdateApproveQuantity([FromForm] CreatePurchaseOrderApproveQuantityDto createPurchaseOrderApproveQuantityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(createPurchaseOrderApproveQuantityDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createPurchaseOrderApproveQuantityDto.barCode);
                PurchaseOrder cartnew = GetSale();
                if (cartnew.Lines.Count > 0)
                {
                    var receipt = cartnew.Lines.Where(a => a.ProductId == product.ProductId && a.SupplierPriceId == createPurchaseOrderApproveQuantityDto.SupplierPriceId && a.ProductExpiryDate == createPurchaseOrderApproveQuantityDto.ProductExpiryDate).FirstOrDefault();
                    var SupplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == createPurchaseOrderApproveQuantityDto.SupplierPriceId && sp.SupplierId == createPurchaseOrderApproveQuantityDto.SupplierId && sp.BranchLocationId == createPurchaseOrderApproveQuantityDto.BranchLocationId && sp.ProductId == product.ProductId).Include(p => p.Product).OrderByDescending(sp => sp.CreateDate).FirstOrDefaultAsync();
                    if (createPurchaseOrderApproveQuantityDto.approveQuantity <= receipt.Quantity)
                    {
                        order.ApproveQuantity(product, createPurchaseOrderApproveQuantityDto.approveQuantity, receipt.Quantity, SupplierPrice, createPurchaseOrderApproveQuantityDto.ProductExpiryDate);
                        SaveCart(order);
                    }
                    else
                    {
                        return NotFound("Quantity mismatch.");
                    }
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }

            PurchaseOrder cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //   cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="updateapprovestatus"></param> 
        [AllowAnonymous]
        [HttpPost("updateapprovestatus")]
        public async Task<IActionResult> UpdateApproveStatus([FromForm] CreatePurchaseOrderApproveStatusyDto createPurchaseOrderApproveStatusyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(createPurchaseOrderApproveStatusyDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createPurchaseOrderApproveStatusyDto.barCode);
                PurchaseOrder cartnew = GetSale();
                if (cartnew.Lines.Count > 0)
                {
                    var receipt = cartnew.Lines.Where(a => a.ProductId == product.ProductId && a.SupplierPriceId == createPurchaseOrderApproveStatusyDto.SupplierPriceId && a.ProductExpiryDate == createPurchaseOrderApproveStatusyDto.ProductExpiryDate).FirstOrDefault();
                    var SupplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == createPurchaseOrderApproveStatusyDto.SupplierPriceId && sp.SupplierId == createPurchaseOrderApproveStatusyDto.SupplierId && sp.BranchLocationId == createPurchaseOrderApproveStatusyDto.BranchLocationId && sp.ProductId == product.ProductId).Include(p => p.Product).OrderByDescending(sp => sp.CreateDate).FirstOrDefaultAsync();
                    if (receipt.ApproveQuantity > 0)
                    {
                        order.ApproveStatus(product, createPurchaseOrderApproveStatusyDto.approveStatus, receipt.Quantity, SupplierPrice, createPurchaseOrderApproveStatusyDto.ProductExpiryDate);
                        SaveCart(order);
                    }
                    else
                    {
                        return NotFound("Please update approve quantity.");
                    }
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }

            PurchaseOrder cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //   cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }

        /*  /// <summary>
         ///  Create SaleDetailDto.
         /// </summary> 
         /// <param name="savepurchaseorderheader"></param> 
         [HttpPost("savepurchaseorderheader")]
         public IActionResult SavePurchaseOrderHeader([FromForm] SavePurchaseOrderHeaderDto savePurchaseOrderHeaderDto)
         {
           if (!ModelState.IsValid)
           {
             return BadRequest(ModelState);
           }
           order.AddHeder(savePurchaseOrderHeaderDto);
           SaveCart(order);
           return Ok(order.SaveHeader);
         } */

        /// <summary>
        /// You can call checkout method.Please make sure purchaserequest details already exists in the list
        /// </summary> 
        /// <param name="Check/createpurchaseorderheader"></param> 
        [AllowAnonymous]
        [HttpPost("Check/createpurchaseorderheader")]
        public async Task<IActionResult> CreatePurchaseOrderHeader([FromForm] CreatePurchaseOrderHeaderDto createPurchaseOrderHeaderDto)
        {
            int result;
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            PurchaseOrder cart1 = GetSale();
            var saleitem = _mapper.Map<PurchaseOrderHeader>(createPurchaseOrderHeaderDto);
            var count = await _context.PurchaseOrderHeaders.Where(c => c.BranchLocationId == createPurchaseOrderHeaderDto.BranchLocationId).CountAsync() + 1;
            var branchCode = await _context.BranchLocations.Where(u => u.BranchLocationId == createPurchaseOrderHeaderDto.BranchLocationId).Select(c => new { c.BranchCode }).FirstOrDefaultAsync();
            saleitem.PurchaseOrderReferenceNo = branchCode.BranchCode.Trim().ToString() + (DateTime.Now.Year.ToString()) + (count.ToString());
            saleitem.PurchaseAmount = cart1.ComputeGrandTotalValue.Value;
            saleitem.Id = createPurchaseOrderHeaderDto.Id;
            List<PurchaseOrderDetail> lineCollection = new List<PurchaseOrderDetail>();
            foreach (PurchaseOrderDetail item in cart1.Lines.ToList())
            {
                // var supplierPrice = await _context.SupplierPrices.FirstOrDefaultAsync(sp => sp.SupplierPriceId == item.SupplierPriceId);
                var pod = item;
                pod.Product = null;
                pod.SupplierPrice = null;
                pod.CreatedBy = createPurchaseOrderHeaderDto.CreatedBy;
                pod.CreateDate = DateTime.Now;
                pod.ApproveQuantity = pod.Quantity;
                pod.SupplierPrice = null;
                lineCollection.Add(pod);
            }
            saleitem.PurchaseOrderDetails = lineCollection;
            if (saleitem != null && cart1.Lines.Count > 0)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.AttachRange(saleitem.PurchaseOrderDetails);
                            await _context.PurchaseOrderHeaders.AddAsync(saleitem);
                            // Attempt to save changes to the database
                            result = await _context.SaveChangesAsync();
                            if (result > 0)
                            {
                                // order.Clear();
                            }
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                            //return BadRequest(e);
                        }
                        finally
                        {
                            transaction.Dispose();
                            order.Clear();

                        }
                    }
                });
                return Ok(new { message = "Checkout Successfully.", purchaseOrderId = saleitem.PurchaseOrderHeaderId });

                // retrun the cart1 object
                // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
            }
            //get session data
            return NoContent();
            // return BadRequest("Please check the order details.");
        }
        /// <summary>
		/// You can call checkout method.Please make sure Saleitem details already exists in the list
		/// </summary> 
		/// <param name="Approve/{purchaseorderId}"></param> 
		[HttpGet("Approve/{purchaseorderId}")]
        public async Task<IActionResult> GetPurchaseOrderHeaderDetails(int? purchaseorderId)
        {
            var purchaseOrderDetails = await _context.PurchaseOrderDetails.Where(h => h.PurchaseOrderHeaderId == purchaseorderId && h.IsDeleted == false).Select(c => new { c.PurchaseOrderHeaderId, c.ProductId, c.ApproveQuantity, SupplierPriceId = c.SupplierPriceId, c.Quantity, c.ProductExpiryDate, c.cApprovalType }).ToListAsync();
            var purchaseOrderHeader = await _context.PurchaseOrderHeaders.Where(h => h.PurchaseOrderHeaderId == purchaseorderId).FirstOrDefaultAsync();

            if (purchaseOrderDetails.Count > 0 && purchaseOrderDetails != null)
            {
                foreach (var item in purchaseOrderDetails)
                {
                    var supplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == item.SupplierPriceId && sp.ProductId == item.ProductId).Include(p => p.Product).FirstOrDefaultAsync();
                    order.AddItem(item.Quantity, supplierPrice, item.ProductExpiryDate, item.ApproveQuantity, item.cApprovalType);
                    SaveCart(order);
                }
                PurchaseOrder cart1 = GetSale();
                // object to string
                string value = JsonConvert.SerializeObject(cart1);
                // string set to cache the name is cart1
                //   cache.SetString("cart1", value);
                // get customer information from external database
                //var cust = await GetCustomersInfo();
                // retrun the cart1 object
                return Ok(new { cart1.Lines, PurchaseOrderHeader = purchaseOrderHeader });

            }
            return BadRequest("Please check your Order number.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure purchaserequest details already exists in the list
        /// </summary> 
        /// <param name="Confirm/confirmpurchaseorderheader"></param> 
        [AllowAnonymous]
        [HttpPut("Confirm/confirmpurchaseorderheader")]
        public async Task<IActionResult> ConfirmPurchaseOrderHeader([FromForm] UpdatePurchaseOrderHeaderDto createPurchaseOrderHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            PurchaseOrder cart1 = GetSale();
            var entity = await _context.PurchaseOrderHeaders.Where(s => s.PurchaseOrderHeaderId == createPurchaseOrderHeaderDto.PurchaseOrderHeaderId).Include(c => c.PurchaseOrderDetails).AsNoTracking().FirstOrDefaultAsync<PurchaseOrderHeader>();
            var saleitem = _mapper.Map<PurchaseOrderHeader>(createPurchaseOrderHeaderDto);
            saleitem.ModifiedDate = DateTime.Now;
            saleitem.ModifiedBy = createPurchaseOrderHeaderDto.CreatedBy;
            saleitem.DeliveryDate = createPurchaseOrderHeaderDto.DeliveryDate;
            //saleitem.PurchaseOrderDetails = cart1.Lines;
            saleitem.PurchaseAmount = cart1.ComputeGrandTotalValue.Value;
            saleitem.PurchaseOrderReferenceNo = entity.PurchaseOrderReferenceNo;
            saleitem.IsComfirm = true;
            saleitem.IsApprove = true;
            List<PurchaseOrderDetail> lineCollection = new List<PurchaseOrderDetail>();
            foreach (PurchaseOrderDetail pd in cart1.Lines)
            {
                var pod = pd;
                pod.PurchaseOrderHeaderId = 0;
                pod.PurchaseOrderHeaderId = createPurchaseOrderHeaderDto.PurchaseOrderHeaderId;
                var product = await _context.Products.Where(p => p.ProductId == pd.SupplierPrice.ProductId).SingleOrDefaultAsync();
                pod.Product = product;
                pod.CreatedBy = createPurchaseOrderHeaderDto.CreatedBy;
                lineCollection.Add(pod);
            }
            // Check that the record exists.
            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + createPurchaseOrderHeaderDto.PurchaseOrderHeaderId + "'.");
            }
            saleitem.PurchaseOrderDetails = lineCollection;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.PurchaseOrderDetails.RemoveRange(entity.PurchaseOrderDetails);
                        _context.AttachRange(lineCollection);
                        //_context.PurchaseOrderDetails.AddRange(lineCollection);
                        // Update changes if any of the properties have been modified.
                        //_context.PurchaseOrderHeaders.Update(saleitem);
                        // Update changes if any of the properties have been modified.
                        _context.Entry(entity).CurrentValues.SetValues(saleitem);
                        _context.Entry(entity).State = EntityState.Modified;
                        if (_context.Entry(entity).Properties.Any(property => property.IsModified))
                        {
                            await _context.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        //return BadRequest(e);
                        // return Conflict(ex);
                        transaction.Rollback();
                        logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);

                    }
                    finally
                    {
                        order.Clear();
                        transaction.Dispose();
                    }
                }
            });
            return Ok(new { message = "Confirm Successfully.", purchaseOrderId = saleitem.PurchaseOrderHeaderId });
        }
        /// <summary>
        /// You can call checkout method.Please make sure purchaserequest details already exists in the list
        /// </summary> 
        /// <param name="update/updatepurchaseorderheader"></param> 
        [AllowAnonymous]
        [HttpPut("update/updatepurchaseorderheader")]
        public async Task<IActionResult> UpdatePurchaseOrderHeader([FromForm] UpdatePurchaseOrderHeaderDto createPurchaseOrderHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            PurchaseOrder cart1 = GetSale();
            var entity = await _context.PurchaseOrderHeaders.Where(s => s.PurchaseOrderHeaderId == createPurchaseOrderHeaderDto.PurchaseOrderHeaderId).Include(c => c.PurchaseOrderDetails).AsNoTracking().FirstOrDefaultAsync<PurchaseOrderHeader>();
            var saleitem = _mapper.Map<PurchaseOrderHeader>(createPurchaseOrderHeaderDto);
            saleitem.ModifiedDate = DateTime.Now;
            saleitem.CreatedBy = entity.CreatedBy;
            saleitem.ModifiedBy = createPurchaseOrderHeaderDto.CreatedBy;
            saleitem.DeliveryDate = createPurchaseOrderHeaderDto.DeliveryDate;
            //saleitem.PurchaseOrderDetails = cart1.Lines;
            saleitem.PurchaseAmount = cart1.ComputeGrandTotalValue.Value;
            saleitem.PurchaseOrderReferenceNo = entity.PurchaseOrderReferenceNo;
            List<PurchaseOrderDetail> lineCollection = new List<PurchaseOrderDetail>();
            foreach (PurchaseOrderDetail pd in cart1.Lines)
            {
                var pod = pd;
                pod.SupplierPrice = null;
                pod.PurchaseOrderHeaderId = createPurchaseOrderHeaderDto.PurchaseOrderHeaderId;
                var supplierPrice = await _context.SupplierPrices.AsNoTracking().FirstOrDefaultAsync(sp => sp.SupplierPriceId == pd.SupplierPriceId);
                pod.SupplierPrice = supplierPrice;
                var product = await _context.Products.Where(p => p.ProductId == supplierPrice.ProductId).FirstOrDefaultAsync();
                pod.Product = product;
                pod.ApproveQuantity = pod.Quantity;
                pod.CreatedBy = entity.CreatedBy;
                pod.SupplierPrice = null;
                lineCollection.Add(pod);
            }
            // Check that the record exists.
            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + createPurchaseOrderHeaderDto.PurchaseOrderHeaderId + "'.");
            }
            saleitem.PurchaseOrderDetails = lineCollection;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.PurchaseOrderDetails.RemoveRange(entity.PurchaseOrderDetails);
                        _context.AttachRange(lineCollection);
                        //_context.PurchaseOrderDetails.AddRange(lineCollection);
                        // Update changes if any of the properties have been modified.
                        //_context.PurchaseOrderHeaders.Update(saleitem);
                        // Update changes if any of the properties have been modified.
                        _context.Entry(entity).CurrentValues.SetValues(saleitem);
                        _context.Entry(entity).State = EntityState.Modified;
                        if (_context.Entry(entity).Properties.Any(property => property.IsModified))
                        {
                            await _context.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        //return BadRequest(e);
                        // return Conflict(ex);
                        transaction.Rollback();
                        logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);

                    }
                    finally
                    {
                        order.Clear();
                        transaction.Dispose();
                    }
                }
            });
            return Ok(new { message = "Update Successfully.", purchaseOrderId = saleitem.PurchaseOrderHeaderId });
        }
        /// <summary>
        /// You can call checkout method.Please make sure purchaserequest details already exists in the list
        /// </summary> 
        /// <param name="approve/approvepurchaseorderheader"></param> 
        [AllowAnonymous]
        [HttpPost("approve/approvepurchaseorderheader")]
        public async Task<IActionResult> ApprovePurchaseOrderHeader([FromBody] ApprovePurchaseOrderHeaderDto approvePurchaseOrderHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var entity = await _context.PurchaseOrderHeaders.Where(s => s.PurchaseOrderHeaderId == approvePurchaseOrderHeaderDto.PurchaseOrderHeaderId).FirstOrDefaultAsync<PurchaseOrderHeader>();
            // Check that the record exists.
            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + approvePurchaseOrderHeaderDto.PurchaseOrderHeaderId + "'.");
            }
            entity.ModifiedDate = DateTime.Now;
            entity.ApprovedBy = approvePurchaseOrderHeaderDto.createdBy;
            entity.ModifiedBy = approvePurchaseOrderHeaderDto.createdBy;
            entity.Id = approvePurchaseOrderHeaderDto.id;
            entity.cApprovalType = approvePurchaseOrderHeaderDto.cApprovalType;
            if ((int)approvePurchaseOrderHeaderDto.cApprovalType == 1 && entity.IsApprove == false)
            {
                entity.IsApprove = true;
                entity.IsComfirm = true;
            }
            if ((int)approvePurchaseOrderHeaderDto.cApprovalType == 2 && entity.IsApprove == false && entity.IsProcess == false)
            {
                entity.IsApprove = false;
                entity.IsComfirm = false;
            }
            if ((int)approvePurchaseOrderHeaderDto.cApprovalType == 3 && entity.IsApprove == false && entity.IsProcess == false)
            {
                entity.IsDeleted = true;
            }
            if ((int)approvePurchaseOrderHeaderDto.cApprovalType == 3 && entity.IsApprove == true && entity.IsProcess == true)
            {
                return BadRequest("You've unable to cancel po with id '" + entity.PurchaseOrderReferenceNo);
            }
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.PurchaseOrderHeaders.Update(entity);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        //return BadRequest(e);
                        // return Conflict(ex);
                        transaction.Rollback();
                        logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);

                    }
                    finally
                    {
                        order.Clear();
                        transaction.Dispose();
                    }
                }
            });
            return Ok(new { message = "Successfully Approved.", purchaseOrderId = entity.PurchaseOrderHeaderId });
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="removepurchaseorderitemdetails"></param> 
        [AllowAnonymous]
        [HttpPost("removepurchaseorderitemdetails")]
        public async Task<IActionResult> RemovePurchaseOrderItemDetails([FromForm] RemovePurchaseOrderDetailDto removeSaleDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(removeSaleDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(removeSaleDetailDto.barCode);
                var supplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.SupplierPriceId == removeSaleDetailDto.SupplierPriceId && sp.ProductId == product.ProductId).Include(p => p.Product).FirstOrDefaultAsync();
                order.RemoveLine(product, supplierPrice, removeSaleDetailDto.ProductExpiryDate);
                SaveCart(order);
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            PurchaseOrder cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //    cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<PurchaseOrder>> Get()
        {
            PurchaseOrder cart1 = GetSale();
            // get cache byte array
            //   var data = cache.Get("cart1");
            // convert byte to string
            //  string value = Encoding.UTF8.GetString(data);
            // convert string to Cart object
            //  var t = JsonConvert.DeserializeAnonymousType(value, cart);
            // get a cache value
            //var timestamp = cache.GetString("cart1");
            return Ok(cart1);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //get object in the session storage 
            PurchaseOrder cart2 = GetSale();
            return Ok(cart2);
        }
        /// <summary>
        /// Test .
        /// </summary> 
        /// <param name="clear"></param> 
        [AllowAnonymous]
        [HttpGet("clear")]
        public ActionResult Clear()
        {
            PurchaseOrder cart2 = GetSale();
            if (cart2.Lines.Count > 0)
            {
                HttpContext.Session.Clear();
                PurchaseOrder clearcart = GetSale();
                return Ok(new { clearcart = clearcart });
            }
            return Ok(new { clearcart = cart2 });
        }
        #region initial method
        private PurchaseOrder GetSale()
        {
            //get object in the session storage 
            PurchaseOrder cart = HttpContext.Session.GetJson<PurchaseOrder>("PurchaseOrder") ?? new PurchaseOrder();
            return cart;
        }
        private void SaveCart(PurchaseOrder cart)
        {
            // store object in session
            HttpContext.Session.SetJson("PurchaseOrder", cart);
        }

        #endregion
    }

}