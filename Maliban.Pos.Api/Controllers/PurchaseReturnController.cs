using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
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
    public class PurchaseReturnController : BaseController
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
        private PurchaseReturn order;
        // private IDistributedCache cache;
        private readonly ISqlDataAccess _db;

        [Obsolete]
        public PurchaseReturnController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, PurchaseReturn orderService, IDistributedCache cache, ISqlDataAccess db) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
            _db = db;
            //  this.cache = cache;
        }
        #endregion
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallpurchasereturnheaders"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallpurchasereturnheaders", Name = "GetAllPurchaseReturnHeaders")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllPurchaseReturnHeaders(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var purchaseOrderHeaders = await _repoWrapper.PurchaseReturnHeader.GetPurchaseReturnHeadersAsync(userParams, BranchLocationId);
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
        /// <param name="getpurchasereturnheader/{purchaseReturnHeaderId}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getpurchasereturnheader/{purchaseReturnHeaderId}", Name = "GetPurchaseReturnHeader")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetPurchaseReturnHeader([FromQuery] bool status, int purchaseReturnHeaderId)
        {
            var purchaseOrderHeaders = await _repoWrapper.PurchaseReturnHeader.FindByCondition(p => p.PurchaseReturnHeaderId == purchaseReturnHeaderId).Include(p => p.Lines).ThenInclude(p => p.Product).ThenInclude(pr => pr.Prices).Include(s => s.Supplier).Include(b => b.BranchLocation).FirstOrDefaultAsync();
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
        /// <param name="getpurchasereturnheaderbygrnid/{goodReceiptNoteHeaderId}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getpurchasereturnheaderbygrnid/{goodReceiptNoteHeaderId}", Name = "GetPurchaseReturnHeaderByGRNId")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetPurchaseReturnHeaderByGRNId([FromQuery] bool status, int goodReceiptNoteHeaderId)
        {
            var purchaseOrderHeaders = await _repoWrapper.PurchaseReturnHeader.FindByCondition(p => p.GoodReceptNoteHeaderId == goodReceiptNoteHeaderId).Include(p => p.Lines).ThenInclude(p => p.Product).ThenInclude(pr => pr.Prices).Include(s => s.Supplier).Include(b => b.BranchLocation).ToListAsync();
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
        /// <param name="getallpurchasereturndetails"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallpurchasereturndetails", Name = "GetAllPurchaseReturnDetails")]
        [MapToApiVersion("1.1")]
        public IActionResult GetAllPurchaseReturnDetails(bool status, [FromQuery] UserParams userParams, int purchaseOrderHeaderId)
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
        /// <param name="Get/{purchasereturnId}"></param> 
        [AllowAnonymous]
        [HttpGet("Get/{purchasereturnId}", Name = "GetPurchaseReturnItemDetails")]
        public async Task<ActionResult<PurchaseReturnDetail>> GetPurchaseReturnItemDetails(int? purchaseorderId)
        {

            var purchaseOrderItemDetails = await _context.PurchaseOrderDetails.
               Where(p => p.PurchaseOrderHeaderId == purchaseorderId).Include(sp => sp.SupplierPrice).Include(p => p.Product).Include(p => p.PurchaseOrderHeaders).ThenInclude(b => b.BranchLocation).AsNoTracking().ToListAsync();
            if (purchaseOrderItemDetails.Count() > 0)
            {
                return Ok(purchaseOrderItemDetails);
            }
            return NotFound("PurchaseorderId doesn't exists.");
        }
        /// <summary>
        ///  Get PurchaseOrderItemDetails.
        /// </summary> 
        /// <param name="Search/{purchasereturnId}"></param> 
        [AllowAnonymous]
        [HttpGet("Search/{purchasereturnId}", Name = "SearchPurchaseReturn")]
        public async Task<IActionResult> SearchPurchaseReturn(int? purchasereturnId)
        {

            var purchaseOrderItemDetails = await _context.PurchaseOrderHeaders.
            Where(p => p.PurchaseOrderHeaderId == purchasereturnId)
            .Select(s => new { s.BranchLocationId, s.SupplierId }).ToListAsync();
            return Ok(purchaseOrderItemDetails);
        }

        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="createpurchasereturnitemdetails"></param> 
        [AllowAnonymous]
        [HttpPost("createpurchasereturnitemdetails")]
        public async Task<IActionResult> CreatePurchaseReturnItemDetails([FromForm] CreatePurchaseReturnDetailsDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //      PurchaseReturn cartnew = GetSale();

            if (await _repoWrapper.Product.AnyAsync(p => p.ProductId == createSaleItemDetailsDto.productId))
            {
                var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == createSaleItemDetailsDto.productId).FirstOrDefaultAsync<Product>();
                var price = await _context.Prices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.BranchLocationId == createSaleItemDetailsDto.BranchLocationId && sp.ProductId == product.ProductId && sp.PriceId == createSaleItemDetailsDto.priceId).FirstOrDefaultAsync<Price>();
                price.Product = product;
                var qty = order.Lines.Where(c => c.ProductId == product.ProductId && c.PriceId == createSaleItemDetailsDto.priceId).Sum(q => q.ReturnQuantity);
                if (qty > 0)
                {
                    order.RemoveLine(product, price);
                    SaveCart(order);
                }
                var items = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetPurchaseReturnStockAdJOutByFIFO", new { xBranchlocation = createSaleItemDetailsDto.BranchLocationId, @xProductId = createSaleItemDetailsDto.productId, @xPriceId = createSaleItemDetailsDto.priceId, @xGoodReceptNoteHeaderId = createSaleItemDetailsDto.goodReceptNoteHeaderId, xQty = createSaleItemDetailsDto.quantity + qty }).Result.AsQueryable().ToList();
                foreach (var item in items)
                {
                    if (item.dNetQuantity <= item.dStockQuantity)
                        order.AddItem(item.dNetQuantity, price, item.ProductExpireDate.Date, item.TransactionDetailId);
                    SaveCart(order);
                    if (item.dNetQuantity > item.dStockQuantity)
                        return NotFound("Quantity doesn't exists.");
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }

            PurchaseReturn cart1 = GetSale();
            // object to string
            //string value = JsonConvert.SerializeObject(cart1);
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
        /// <param name="Check/createpurchasereturnheader"></param> 
        [AllowAnonymous]
        [HttpPost("Check/createpurchasereturnheader")]
        public async Task<IActionResult> CreatePurchaseReturnHeader([FromForm] CreatePurchaseReturnHeaderDto createPurchaseOrderHeaderDto)
        {
            int result;
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            PurchaseReturn cart1 = GetSale();
            var saleitem = _mapper.Map<PurchaseReturnHeader>(createPurchaseOrderHeaderDto);

            List<PurchaseReturnDetail> lineCollection = new List<PurchaseReturnDetail>();
            foreach (PurchaseReturnDetail item in cart1.Lines.ToList())
            {
                var pod = item;
                pod.Product = null;
                pod.Price = null;
                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                pod.Price = price;
                pod.CreatedBy = createPurchaseOrderHeaderDto.CreatedBy;
                pod.CreateDate = DateTime.Now;
                lineCollection.Add(pod);
            }

            saleitem.Lines = lineCollection;
            saleitem.GoodReceptNoteHeaderId = createPurchaseOrderHeaderDto.GoodReceptNoteHeaderId;
            saleitem.PurchaseReturnAmount = cart1.ComputeGrandTotalValue.Value;

            if (saleitem != null && cart1.Lines.Count > 0)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {

                        try
                        {
                            _context.AttachRange(cart1.Lines);
                            await _context.PurchaseReturnHeaders.AddAsync(saleitem);

                            // Attempt to save changes to the database
                            result = await _context.SaveChangesAsync();

                            if (result > 0)
                            {
                                //depricated dnetquantity subtract
                                foreach (var item in cart1.Lines)
                                {
                                    /*await _repoWrapper.ShopStock.ModifyStockDetailForPurchaseReturnAsync(item.ProductId,createPurchaseOrderHeaderDto.BranchLocationId,item.ReturnQuantity,
                                      createPurchaseOrderHeaderDto.CreatedBy,item.Price.SalePrice,item.Price.ModifiedUnitPrice,item.TransactionDetailId);*/

                                    var fifo = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetPurchaseReturnStockAdJOutByFIFO", new { xBranchlocation = createPurchaseOrderHeaderDto.BranchLocationId, @xProductId = item.ProductId, @xPriceId = item.PriceId, @xGoodReceptNoteHeaderId = createPurchaseOrderHeaderDto.GoodReceptNoteHeaderId, xQty = item.ReturnQuantity }).Result.AsQueryable().ToList();
                                    foreach (var tr in fifo)
                                    {
                                        var updateTransaction = await _context.TransactionDetails.Where(t => t.TransactionDetailId == tr.TransactionDetailId).FirstOrDefaultAsync();
                                        if (updateTransaction.dQuantity - updateTransaction.dIssueQuantity >= tr.dNetQuantity)
                                        {
                                            updateTransaction.dIssueQuantity += tr.dNetQuantity;
                                            _context.TransactionDetails.Update(updateTransaction);
                                            _context.SaveChanges();
                                        }

                                        logger.Error(string.Join(',', "location" + updateTransaction.BranchLocationId + "product" + updateTransaction.Barcode + "dQuantity" + updateTransaction.dQuantity) + "dIssueQuantity" + updateTransaction.dIssueQuantity + "ReturnQuantity" + item.ReturnQuantity);
                                    }

                                }

                                //  await _repoWrapper.ShopStock.UpdatePurchaseReturnTransaction(cart1.Lines.ToList(), createPurchaseOrderHeaderDto.BranchLocationId, createPurchaseOrderHeaderDto.CreatedBy);

                                List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                                foreach (var item in cart1.Lines.ToList())
                                {
                                    var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == createPurchaseOrderHeaderDto.BranchLocationId).FirstOrDefaultAsync<Price>();
                                    var product = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                    TransactionDetail transactionDetail = new TransactionDetail
                                    {
                                        dQuantity = item.ReturnQuantity,
                                        ProductAvailable = true,
                                        BranchLocationId = createPurchaseOrderHeaderDto.BranchLocationId,
                                        ProductId = item.ProductId,
                                        PriceId = item.PriceId,
                                        iDocID = item.PurchaseReturnDetailId,
                                        cDocNo = "PRN" + item.PurchaseReturnDetailId,
                                        dDocDate = DateTime.Now,
                                        cDocType = cDocType.PRN,
                                        cExtRefNum = "Purchase Return",
                                        dIssueQuantity = item.ReturnQuantity,
                                        dNetQuantity = -item.ReturnQuantity,
                                        dCostPrice = price.ModifiedUnitPrice,
                                        dSalePrice = item.Price.SalePrice,
                                        dValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                                        ProductExpireDate = item.ProductExpireDate,
                                        dNetValue = -item.ReturnQuantity * price.ModifiedUnitPrice,
                                        dIN = 0,
                                        dOUT = item.ReturnQuantity,
                                        Barcode = product.Barcode,
                                        CreatedBy = createPurchaseOrderHeaderDto.CreatedBy,
                                        CreateDate = DateTime.Now,
                                        ProductName = product.ProductName
                                    };
                                    updateshopStock.Add(transactionDetail);
                                }
                                _context.TransactionDetails.AddRange(updateshopStock);
                                await _context.SaveChangesAsync();
                                order.Clear();
                            }
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
                            order.Clear();

                        }

                    }
                });
                return Ok(new { message = "Checkout Successfully.", PurchaseReturnId = saleitem.PurchaseReturnHeaderId });

            }
            return NoContent();
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="removepurchasereturnitemdetails"></param> 
        [AllowAnonymous]
        [HttpPost("removepurchasereturnitemdetails")]
        public async Task<IActionResult> RemovePurchaseReturnItemDetails([FromForm] RemoveSaleDetailDto removeSaleDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(removeSaleDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(removeSaleDetailDto.barCode);
                //IPriceRepository GetPriceAsync
                var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, removeSaleDetailDto.branchLocationId, removeSaleDetailDto.dSalePrice);
                order.RemoveLine(product, price);
                SaveCart(order);
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }

            PurchaseReturn cart1 = GetSale();
            // object to string
            // string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object

            return Ok(cart1);
        }
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<PurchaseReturn>> Get()
        {
            PurchaseReturn cart1 = GetSale();
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
            PurchaseReturn cart2 = GetSale();
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
            PurchaseReturn cart2 = GetSale();
            if (cart2.Lines.Count > 0)
            {
                HttpContext.Session.Clear();
                return Ok("Clear Cart Successfully.");
            }
            return NoContent();
        }
        #region initial method
        private PurchaseReturn GetSale()
        {
            //get object in the session storage 
            PurchaseReturn cart = HttpContext.Session.GetJson<PurchaseReturn>("PurchaseReturn") ?? new PurchaseReturn();
            return cart;
        }
        private void SaveCart(PurchaseReturn cart)
        {
            // store object in session
            HttpContext.Session.SetJson("PurchaseReturn", cart);
        }
        #endregion
    }

}