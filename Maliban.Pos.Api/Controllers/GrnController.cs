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
using static Maliban.Pos.Api.Models.Status;
using System.IO;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class GrnController : BaseController
    {
        #region SaleConstructor
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private GoodReceptNote grn;

        //private IDistributedCache cache;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnviroment;

        [Obsolete]
        public GrnController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, GoodReceptNote cartService, IDistributedCache cache) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            grn = cartService;
            _hostingEnviroment = hostingEnviroment;
            //  this.cache = cache;
        }
        #endregion
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallgoodreceiptheaders"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallgoodreceiptheaders", Name = "GetAllGoodReceiptHeaders")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllGoodReceiptHeaders(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var goodreceiptHeaders = await _repoWrapper.GoodReceiptHeader.GetGoodReceptNoteHeadersAsync(userParams, BranchLocationId);
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
            if (goodreceiptHeaders.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = goodreceiptHeaders.Where(p => p.Active = true);
                    Response.AddPagination(goodreceiptHeaders.CurrentPage, goodreceiptHeaders.PageSize, goodreceiptHeaders.TotalCount, goodreceiptHeaders.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = goodreceiptHeaders.Where(p => p.Active = false);
                    Response.AddPagination(goodreceiptHeaders.CurrentPage, goodreceiptHeaders.PageSize, goodreceiptHeaders.TotalCount, goodreceiptHeaders.TotalPages);
                    return Ok(resultfalse);
                //default:
                //    Response.AddPagination(goodreceiptHeaders.CurrentPage, goodreceiptHeaders.PageSize, goodreceiptHeaders.TotalCount, goodreceiptHeaders.TotalPages);
                //    return Ok(goodreceiptHeaders);
            }
        }
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getgoodreceiptheader/{goodreceiptHeaderId}"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getgoodreceiptheader/{goodreceiptHeaderId}", Name = "GetGoodReceiptHeader")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetGoodReceiptHeader([FromQuery] bool status, int goodreceiptHeaderId)
        {
            var goodreceiptHeaders = await _repoWrapper.GoodReceiptHeader.FindByCondition(g => g.GoodReceptNoteHeaderId == goodreceiptHeaderId).Include(l => l.Lines).ThenInclude(p => p.Product).Include(s => s.Supplier).Include(b => b.BranchLocation).Include(ph => ph.PurchaseOrderHeader).FirstOrDefaultAsync();

            if (goodreceiptHeaders != null)
            {
                return Ok(goodreceiptHeaders);
            }
            return NoContent();
        }
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallgoodreceiptdetails"></param>
        //  [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallgoodreceiptdetails", Name = "GetAllGoodReceiptDetails")]
        [MapToApiVersion("1.1")]
        public IActionResult GetAllGoodReceiptDetails(bool status, [FromQuery] UserParams userParams, int goodReceptNoteHeaderId)
        {
            var goodreceiptDetails = _repoWrapper.GoodReceiptDetail.GetGoodReceiptDetailsAsync(userParams, goodReceptNoteHeaderId);
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
            if (goodreceiptDetails.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = goodreceiptDetails.Where(p => p.Active = true);
                    Response.AddPagination(goodreceiptDetails.CurrentPage, goodreceiptDetails.PageSize, goodreceiptDetails.TotalCount, goodreceiptDetails.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = goodreceiptDetails.Where(p => p.Active = false);
                    Response.AddPagination(goodreceiptDetails.CurrentPage, goodreceiptDetails.PageSize, goodreceiptDetails.TotalCount, goodreceiptDetails.TotalPages);
                    return Ok(resultfalse);
                //default:
                //    Response.AddPagination(goodreceiptDetails.CurrentPage, goodreceiptDetails.PageSize, goodreceiptDetails.TotalCount, goodreceiptDetails.TotalPages);
                //    return Ok(goodreceiptDetails);
            }
        }

        /// <summary>
        /// Create Member.
        /// </summary> 
        /// <param name="creategrnitemdetaillist"></param> 
        [HttpPost("creategrnitemdetaillist")]
        public async Task<IActionResult> CreateGrnItemDetailList([FromBody] CreateProductGrnItemDetailDto[] createGrnItemDetailDto)
        {
            decimal? grnQuantity = 0;
            foreach (var item in createGrnItemDetailDto)
            {
                if (await _repoWrapper.VerifyBarCode(item.barCode))
                {
                    var product = await _repoWrapper.Product.FindByCondition(p => p.Barcode == item.barCode).FirstOrDefaultAsync();
                    var poQantity = _context.PurchaseOrderDetails.Where(p => p.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && p.ProductId == product.ProductId && p.SupplierPriceId == item.priceId && p.ProductExpiryDate == item.productExpiryDate).
                    Select(o => new { o.Quantity,o.ApproveQuantity, o.SupplierPriceId }).FirstOrDefault();
                    var po = await _context.PurchaseOrderHeaders.Where(p => p.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && p.PurchaseOrderDetails.Any(o => o.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && o.ProductId == product.ProductId)).FirstOrDefaultAsync();
                    var supplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.ProductId == product.ProductId && sp.BranchLocationId == po.BranchLocationId && sp.SupplierId == po.SupplierId && sp.SupplierPriceId == poQantity.SupplierPriceId).FirstOrDefaultAsync();
                    if (supplierPrice == null)
                    {
                        logger.Debug(product.ProductId.ToString() + poQantity.SupplierPriceId.ToString() + po.SupplierId.ToString());

                    }
                    var companyMaster = await _context.CalculationMasters.SingleOrDefaultAsync(c => c.BranchLocationId == po.BranchLocationId);
                    GoodReceptNoteDetail goodReceptNote = new GoodReceptNoteDetail
                    {
                        SupplierPriceId = item.priceId,
                        BatchNumber = item.batchnumber,
                        FreeItemQuantity = item.freequantity,
                        CreatedBy = item.createdBy,
                        ProductExpiryDate = item.productExpiryDate,
                        ItemQuantity = item.quantity,
                        dNetQuantity = item.dNetQuantity,
                        PurchaseOrderHeaderId = item.PurchaseOrderHeaderId,
                        Product = product,
                        ProductId = product.ProductId,
                        CostPrice = supplierPrice.UnitPrice,
                        RetailPrice = supplierPrice.MaximumRetailPrice,
                        SupplierPrice = supplierPrice,
                        PurchaseOrderHeader = po,
                        BalanceItemQuantity = poQantity.Quantity - item.quantity,
                        PricePrecentage = companyMaster.SalePricePrecentageEmployee,
                        VATPrecentage = companyMaster.VATPrecentage,
                        NBTPrecentage = companyMaster.NBTPrecentage
                    };

                    GoodReceptNote cart3 = GetSale();
                    if (cart3.Lines.Count > 0)
                    {
                        grnQuantity = cart3.Lines.Where(l => l.PurchaseOrderHeaderId == po.PurchaseOrderHeaderId && l.ProductId == product.ProductId && l.SupplierPriceId == item.priceId && l.ProductExpiryDate == item.productExpiryDate).Select(o => o.ItemQuantity).FirstOrDefault();
                    }
                    if (po != null && grnQuantity <= poQantity.Quantity)
                    {
                        grn.AddItem(goodReceptNote);
                        SaveCart(grn);
                    }
                }
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            //cart1.Clear();
            //cache.Remove("cart1");
            return Ok(cart1);
        }

        /// <summary>
        /// Create Member.
        /// </summary> 
        /// <param name="updategrnitemdetaillist"></param> 
        [HttpPost("updategrnitemdetaillist")]
        public async Task<IActionResult> UpdateGrnItemDetailList([FromBody] CreateProductGrnItemDetailDto[] createGrnItemDetailDto)
        {
            decimal? grnQuantity = 0;
            foreach (var item in createGrnItemDetailDto)
            {
                if (await _repoWrapper.VerifyBarCode(item.barCode))
                {
                    var product = await _repoWrapper.Product.FindByCondition(p => p.Barcode == item.barCode).FirstOrDefaultAsync();
                    var poQantity = _context.PurchaseOrderDetails.Where(p => p.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && p.ProductId == product.ProductId && p.SupplierPriceId == item.priceId).
                    Select(o => new { o.Quantity, o.SupplierPriceId }).FirstOrDefault();
                    var po = await _context.PurchaseOrderHeaders.Where(p => p.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && p.PurchaseOrderDetails.Any(o => o.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId && o.ProductId == product.ProductId)).FirstOrDefaultAsync();
                    var supplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.ProductId == product.ProductId && sp.BranchLocationId == po.BranchLocationId && sp.SupplierId == po.SupplierId && sp.SupplierPriceId == poQantity.SupplierPriceId).FirstOrDefaultAsync();
                    var availableStock = await _context.TransactionDetails.Where(t => t.iDocID == 22647 && t.cDocNo == "GRN" + "22647" && t.Barcode == item.barCode && t.BranchLocationId == po.BranchLocationId && t.dCostPrice == supplierPrice.UnitPrice).FirstOrDefaultAsync();
                    if (supplierPrice == null)
                    {
                        logger.Debug(product.ProductId.ToString() + poQantity.SupplierPriceId.ToString() + po.SupplierId.ToString());

                    }
                    var companyMaster = await _context.CalculationMasters.SingleOrDefaultAsync(c => c.BranchLocationId == po.BranchLocationId);
                    GoodReceptNoteDetail goodReceptNote = new GoodReceptNoteDetail
                    {
                        SupplierPriceId = item.priceId,
                        BatchNumber = item.batchnumber,
                        FreeItemQuantity = item.freequantity,
                        CreatedBy = item.createdBy,
                        ProductExpiryDate = item.productExpiryDate,
                        ItemQuantity = item.quantity - availableStock.dIssueQuantity,
                        dNetQuantity = item.dNetQuantity,
                        PurchaseOrderHeaderId = item.PurchaseOrderHeaderId,
                        Product = product,
                        ProductId = product.ProductId,
                        CostPrice = supplierPrice.UnitPrice,
                        RetailPrice = supplierPrice.MaximumRetailPrice,
                        SupplierPrice = supplierPrice,
                        PurchaseOrderHeader = po,
                        BalanceItemQuantity = poQantity.Quantity - item.quantity,
                        PricePrecentage = companyMaster.SalePricePrecentageEmployee,
                        VATPrecentage = companyMaster.VATPrecentage,
                        NBTPrecentage = companyMaster.NBTPrecentage
                    };

                    GoodReceptNote cart3 = GetSale();
                    if (cart3.Lines.Count > 0)
                    {
                        grnQuantity = cart3.Lines.Where(l => l.PurchaseOrderHeaderId == po.PurchaseOrderHeaderId && l.ProductId == product.ProductId).Select(o => o.ItemQuantity).FirstOrDefault();
                    }
                    if (po != null && grnQuantity <= poQantity.Quantity)
                    {
                        grn.AddItem(goodReceptNote);
                        SaveCart(grn);
                    }
                }
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            //cart1.Clear();
            //cache.Remove("cart1");
            return Ok(cart1);
        }

        /// <summary>
        /// Create Member.
        /// </summary> 
        /// <param name="creategrnitemdetails"></param> 
        [HttpPost("creategrnitemdetails")]
        public async Task<IActionResult> CreateGrnItemDetails([FromForm] CreateGrnItemDetailDto createGrnItemDetailDto)
        {
            decimal? grnQuantity = 0;
            if (await verifyItemCode(createGrnItemDetailDto.productId))
            {
                var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == createGrnItemDetailDto.productId).FirstOrDefaultAsync();
                var poQantity = _context.PurchaseOrderDetails.Where(p => p.PurchaseOrderHeaderId == createGrnItemDetailDto.PurchaseOrderHeaderId && p.ProductId == product.ProductId && p.SupplierPriceId == createGrnItemDetailDto.priceId).Select(o => new { o.Quantity, o.SupplierPriceId }).FirstOrDefault();
                var po = await _context.PurchaseOrderHeaders.Where(p => p.PurchaseOrderHeaderId == createGrnItemDetailDto.PurchaseOrderHeaderId && p.PurchaseOrderDetails.Any(o => o.PurchaseOrderHeaderId == createGrnItemDetailDto.PurchaseOrderHeaderId && o.ProductId == product.ProductId)).FirstOrDefaultAsync();
                var supplierPrice = await _context.SupplierPrices.Where(sp => sp.Active == true && sp.IsDeleted == false && sp.ProductId == product.ProductId && sp.BranchLocationId == po.BranchLocationId && sp.SupplierId == po.SupplierId && sp.SupplierPriceId == poQantity.SupplierPriceId).FirstOrDefaultAsync<SupplierPrice>();
                GoodReceptNoteDetail goodReceptNote = new GoodReceptNoteDetail
                {
                    SupplierPriceId = createGrnItemDetailDto.priceId,
                    BatchNumber = createGrnItemDetailDto.batchnumber,
                    FreeItemQuantity = createGrnItemDetailDto.freequantity,
                    CreatedBy = createGrnItemDetailDto.createdBy,
                    ProductExpiryDate = createGrnItemDetailDto.productExpiryDate,
                    ItemQuantity = createGrnItemDetailDto.quantity,
                    PurchaseOrderHeaderId = createGrnItemDetailDto.PurchaseOrderHeaderId,
                    Product = product,
                    ProductId = product.ProductId,
                    CostPrice = supplierPrice.UnitPrice,
                    RetailPrice = createGrnItemDetailDto.RetailPrice,
                    SupplierPrice = supplierPrice,
                    PurchaseOrderHeader = po,
                    BalanceItemQuantity = poQantity.Quantity - createGrnItemDetailDto.quantity,
                };

                GoodReceptNote cart3 = GetSale();
                if (cart3.Lines.Count > 0)
                {
                    grnQuantity = cart3.Lines.Where(l => l.PurchaseOrderHeaderId == createGrnItemDetailDto.PurchaseOrderHeaderId && l.ProductId == product.ProductId && l.SupplierPriceId == createGrnItemDetailDto.priceId).Select(o => o.ItemQuantity).FirstOrDefault();
                }
                if (po != null && grnQuantity < poQantity.Quantity)
                {
                    grn.AddItem(goodReceptNote);
                    SaveCart(grn);
                }
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            //cart1.Clear();
            //cache.Remove("cart1");
            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="quantity/updategrnitemquantity"></param> 
        [HttpPost("quantity/updategrnitemquantity")]
        public async Task<IActionResult> UpdateGrnItemQuantity([FromForm] UpdateGrnQuantityDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //  await GetCustomersInfo();
            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
                var poQantity = _context.PurchaseOrderDetails.Where(p => p.PurchaseOrderHeaderId == createSaleItemDetailsDto.purchaseOrderHeaderId && p.ProductId == product.ProductId && p.SupplierPriceId == createSaleItemDetailsDto.SupplierPriceId).
                                Select(o => new { o.Quantity, o.SupplierPriceId }).FirstOrDefault();
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0 && createSaleItemDetailsDto.quantity <= poQantity.Quantity)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == createSaleItemDetailsDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == createSaleItemDetailsDto.SupplierPriceId).FirstOrDefault();
                    grn.UpdateQuantity(receipt, createSaleItemDetailsDto.quantity);
                    SaveCart(grn);
                }
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //   cache.SetString("cart1", value);
            // get customer information from external database
            //var cust = await GetCustomersInfo();
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Check/creategrnheader"></param> 
        [HttpPost("Check/creategrnheader")]
        public async Task<IActionResult> CreateGrnHeader([FromForm] CreateGrnHeaderDto createGrnHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            int result;
            //get session data
            GoodReceptNote cart1 = GetSale();
            var goodReceptNoteHeader = _mapper.Map<GoodReceptNoteHeader>(createGrnHeaderDto);

            goodReceptNoteHeader.GoodReceptNoteHeaderFactoryId = await _context.GoodReceptNoteHeaders.Where(c => c.BranchLocationId == createGrnHeaderDto.BranchLocationId).CountAsync() + 1;
            var count = await _context.GoodReceptNoteHeaders.Where(c => c.BranchLocationId == createGrnHeaderDto.BranchLocationId).CountAsync() + 1;
            var branchCode = await _context.BranchLocations.Where(u => u.BranchLocationId == createGrnHeaderDto.BranchLocationId).Select(c => new { c.BranchCode }).FirstOrDefaultAsync();
            goodReceptNoteHeader.GoodReceptNoteReferenceNo = branchCode.BranchCode.Trim().ToString() + (DateTime.Now.Year.ToString()) + (count.ToString());
            goodReceptNoteHeader.TotalCost = cart1.ComputeTotalValue;
            goodReceptNoteHeader.TotalBillAmount = cart1.ComputeGrandTotalValue;
            goodReceptNoteHeader.TotalDiscountAmount = cart1.ComputeDiscountValue + createGrnHeaderDto.TotalDiscountAmount;
            goodReceptNoteHeader.ApplicationUser = await _userManager.Users.Where(u => u.Id == createGrnHeaderDto.ApplicationUserId).FirstOrDefaultAsync();
            goodReceptNoteHeader.CreatedBy = createGrnHeaderDto.CreatedBy;
            goodReceptNoteHeader.PurchaseOrderHeader = await _context.PurchaseOrderHeaders.Where(u => u.PurchaseOrderHeaderId == createGrnHeaderDto.PurchaseOrderHeaderId).FirstOrDefaultAsync();
            goodReceptNoteHeader.BranchLocation = await _context.BranchLocations.Where(u => u.BranchLocationId == createGrnHeaderDto.BranchLocationId).FirstOrDefaultAsync();
            goodReceptNoteHeader.Supplier = await _context.Suppliers.Where(u => u.SupplierId == createGrnHeaderDto.SupplierId).FirstOrDefaultAsync();
            List<GoodReceptNoteDetail> lineCollection = new List<GoodReceptNoteDetail>();
            foreach (GoodReceptNoteDetail item in cart1.Lines.ToList())
            {
                var pod = item;
                pod.Product = null;
                pod.SupplierPrice = null;
                pod.CreatedBy = createGrnHeaderDto.CreatedBy;
                pod.CreateDate = DateTime.Now;
                lineCollection.Add(pod);
            }
            goodReceptNoteHeader.Lines = lineCollection;
            if (cart1.Lines.Count > 0 && goodReceptNoteHeader != null)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.AttachRange(goodReceptNoteHeader.Lines);
                        await _context.GoodReceptNoteHeaders.AddAsync(goodReceptNoteHeader);
                        try
                        {
                            result = await _context.SaveChangesAsync();

                            if (result > 0)
                            {
                                //add factory price by hand
                                // start comment add initial saleprice by factory wise,please comment if it's running on production 
                                /* IList<DummySalePriceUpdate> salePriceDetails = new List<DummySalePriceUpdate>();
                                 salePriceDetails = InitializePriceUpdate("PriceUpdate.json", _hostingEnviroment.WebRootPath);*/
                                // end comment add initial saleprice by factory wise,please comment if it's running on production 

                                foreach (var item in cart1.Lines)
                                {
                                    //please comment on production
                                    //   var saleprice = salePriceDetails.Where(s => s.BarCode == item.Product.Barcode && s.BranchLocationId == item.GoodReceptNoteHeader.BranchLocation.BranchLocationId).FirstOrDefault<DummySalePriceUpdate>();
                                    var supplierPrice = await _context.SupplierPrices.FirstOrDefaultAsync(sp => sp.SupplierPriceId == item.SupplierPriceId);
                                    var product = await _context.Products.FirstOrDefaultAsync(sp => sp.ProductId == item.ProductId);
                                    decimal SalePrice = 0;
                                    if (item.PricePrecentage > 0 || supplierPrice.Profit == 0)
                                    {
                                        SalePrice = _repoWrapper.Price.CalculatePriceAsync(item.VATPrecentage > 0 ? item.CostPrice : supplierPrice.UnitPrice, item.RetailPrice, createGrnHeaderDto.BranchLocationId, item.PricePrecentage);

                                    }
                                    if (supplierPrice.Profit > 0)
                                    {
                                        SalePrice = item.CostPrice + supplierPrice.Profit;
                                    }
                                    Price price = new Price
                                    {
                                        UnitPrice = supplierPrice.UnitPrice,
                                        MaximumRetailPrice = item.RetailPrice,
                                        ModifiedUnitPrice = item.VATPrecentage>0? item.CostPrice:supplierPrice.UnitPrice,
                                        //please uncomment if it's running on production
                                        SalePrice = SalePrice, //_repoWrapper.Price.CalculatePriceAsync(supplierPrice.UnitPrice, item.RetailPrice, createGrnHeaderDto.BranchLocationId, item.PricePrecentage),
                                                               //please uncomment if it's running on on factory default add factory price by hand
                                                               //  SalePrice = saleprice.SalePrice,
                                        Product = product,
                                        ProductId = product.ProductId,
                                        BranchLocationId = item.GoodReceptNoteHeader.BranchLocation.BranchLocationId,
                                        BranchLocation = item.GoodReceptNoteHeader.BranchLocation,
                                        //please uncomment if it's running on production
                                        DiscountPrecentage = item.PricePrecentage,
                                        //please uncomment if it's running on factory default add factory price by hand
                                        //  DiscountPrecentage = 0,
                                        CreatedBy = item.CreatedBy,
                                    };
                                    var price1 = await _repoWrapper.Price.UpdatePriceAsync(price);
                                    if (item.overWriteStockPrice)
                                    {
                                        await _repoWrapper.ShopStock.OverWriteStockDetailPriceAsync(product, createGrnHeaderDto.BranchLocationId, price1);
                                    }
                                    #region
                                    /*             ShopStock shopStock = new ShopStock
                                                {
                                                  UnitsInStock = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                                                  ReorderLevel = ReorderLevel.Available,
                                                  ProductExpireDate = item.ProductExpiryDate,
                                                  CreateDate = DateTime.Now,
                                                  CreatedBy = item.CreatedBy,
                                                  ProductId = item.ProductId,
                                                  PriceId = price.PriceId,
                                                  Price = price,
                                                  BranchLocationId = createGrnHeaderDto.BranchLocationId,
                                                  BranchLocation = item.GoodReceptNoteHeader.BranchLocation,
                                                };
                                                await _repoWrapper.ShopStock.UpdateShopStockAsync(shopStock); */
                                    //  var po = await _context.PurchaseOrderDetails.Where(p => p.PurchaseOrderHeaderId == item.PurchaseOrderHeaderId &&  p.ProductId == item.ProductId).FirstOrDefaultAsync<PurchaseOrderDetail>();
                                    #endregion
                                    TransactionDetail transactionDetail = new TransactionDetail
                                    {
                                        dCostPrice = price1.ModifiedUnitPrice,
                                        dSalePrice = price1.SalePrice,
                                        dQuantity = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                                        dIN = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                                        dOUT = 0,
                                        dIssueQuantity = 0,
                                        dNetQuantity = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                                        dValue = price1.ModifiedUnitPrice * (item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity)),
                                        dNetValue = price1.ModifiedUnitPrice * (item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity)),
                                        cExtRefNum = "Goods In",
                                        Product = product,
                                        Barcode = product.Barcode,
                                        ProductName = product.ProductName,
                                        iDocID = goodReceptNoteHeader.GoodReceptNoteHeaderId,
                                        cDocType = cDocType.GRN,
                                        cDocNo = "GRN" + goodReceptNoteHeader.GoodReceptNoteHeaderId,
                                        ProductId = product.ProductId,
                                        PriceId = price1.PriceId,
                                        Price = price1,
                                        CreatedBy = item.CreatedBy,
                                        CreateDate = DateTime.Now,
                                        BranchLocationId = createGrnHeaderDto.BranchLocationId,
                                        BranchLocation = item.GoodReceptNoteHeader.BranchLocation,
                                        ProductExpireDate = item.ProductExpiryDate.Date,
                                        dDocDate = DateTime.Now
                                    };
                                    await _repoWrapper.TransactionDetail.CreateAsync(transactionDetail);
                                    await _repoWrapper.SaveAllAsync();
                                }
                                var po = await _context.PurchaseOrderHeaders.Where(u => u.PurchaseOrderHeaderId == createGrnHeaderDto.PurchaseOrderHeaderId).SingleAsync();
                                po.IsProcess = true;
                                _context.PurchaseOrderHeaders.Update(po);
                                await _repoWrapper.SaveAllAsync();
                                // grn.Clear();
                                //  cache.Remove("cart1");
                                //  cache.Refresh("cart1");
                            }
                            transaction.Commit();
                        }
                        catch (DbUpdateException e)
                        {
                            transaction.Rollback();
                            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                            //return BadRequest(e);
                            //return BadRequest(e);
                        }
                        finally
                        {
                            transaction.Dispose();
                            grn.Clear();

                        }

                    }

                });
                return Ok(new { GrnCart = grn.Lines, GoodReceptNoteHeaderId = goodReceptNoteHeader.GoodReceptNoteHeaderId });
            }
            return NoContent();
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="removegoodreceptnoteitemdetails"></param> 
        [HttpPost("removegoodreceptnoteitemdetails")]
        public async Task<IActionResult> RemoveGoodReceptNoteItemDetails([FromForm] RemoveGrnDetailDto removeGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(removeGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(removeGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == removeGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == removeGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    grn.RemoveLine(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="updategoodreceptnoteitemdetails"></param> 
        [HttpPost("updategoodreceptnoteitemdetails")]
        public async Task<IActionResult> UpdateGoodReceptNoteItemDetails([FromForm] UpdateGrnDetailDto updateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(updateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(updateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == updateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == updateGrnDetailDto.SupplierPriceId && a.ProductExpiryDate.Date==updateGrnDetailDto.ProductExpiryDate.Date).FirstOrDefault();
                    receipt.FreeItemQuantity = updateGrnDetailDto.quantity;
                    grn.UpdateFreeQuantity(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="dateupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("dateupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> DateUpdateGoodReceptNoteItemDetails([FromForm] DateUpdateGrnDetailDto updatedateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(updatedateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(updatedateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == updatedateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == updatedateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.ProductExpiryDate = updatedateGrnDetailDto.productExpiryDate;
                    grn.UpdateItem(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="discountupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("discountupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> DiscountUpdateGoodReceptNoteItemDetails([FromForm] DiscountUpdateGrnDetailDto discountUpdateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(discountUpdateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(discountUpdateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == discountUpdateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == discountUpdateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.PricePrecentage = discountUpdateGrnDetailDto.pricePrecentage;
                    grn.UpdateDiscount(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="vatupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("vatupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> VatUpdateGoodReceptNoteItemDetails([FromForm] VatUpdateGrnDetailDto vatUpdateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(vatUpdateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(vatUpdateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == vatUpdateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == vatUpdateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    var supplierPrice = await _context.SupplierPrices.FindAsync(receipt.SupplierPriceId);
                    var newcost = supplierPrice.UnitPrice * (1 + vatUpdateGrnDetailDto.vatPrecentage / 100);
                    if(newcost< supplierPrice.MaximumRetailPrice)
                    {
                        receipt.VATPrecentage = vatUpdateGrnDetailDto.vatPrecentage;
                        grn.UpdateVat(receipt, supplierPrice.UnitPrice);
                        SaveCart(grn);
                    }
                    if (newcost > supplierPrice.MaximumRetailPrice)
                    {
                        receipt.VATPrecentage = 0;
                        grn.UpdateVat(receipt, supplierPrice.UnitPrice);
                        SaveCart(grn);
                    }
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="nbtupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("nbtupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> NbtUpdateGoodReceptNoteItemDetails([FromForm] NbtUpdateGrnDetailDto nbtUpdateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(nbtUpdateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(nbtUpdateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == nbtUpdateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == nbtUpdateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.NBTPrecentage = nbtUpdateGrnDetailDto.nbtPrecentage;
                    grn.UpdateNbt(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="groupdiscountupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("groupdiscountupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> GroupDiscountUpdateGoodReceptNoteItemDetails([FromForm] GroupDiscountUpdateGrnDetailDto discountUpdateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(discountUpdateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(discountUpdateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == discountUpdateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == discountUpdateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.GroupDiscountAmount = discountUpdateGrnDetailDto.groupDiscountAmount;
                    grn.UpdateGroupDiscount(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="groupdiscountamountupdategoodreceptnoteitemdetails"></param> 
        [HttpPost("groupdiscountamountupdategoodreceptnoteitemdetails")]
        public async Task<IActionResult> GroupDiscountAmountUpdateGoodReceptNoteItemDetails([FromForm] GroupDiscountAmountUpdateGrnDetailDto discountUpdateGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(discountUpdateGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(discountUpdateGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == discountUpdateGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == discountUpdateGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.GroupDiscountAmount = (discountUpdateGrnDetailDto.groupDiscountAmountAmount / (receipt.CostPrice * receipt.ItemQuantity)) * 100;
                    grn.UpdateGroupDiscount(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="overwritestockpricegoodreceptnoteitemdetails"></param> 
        [HttpPost("overwritestockpricegoodreceptnoteitemdetails")]
        public async Task<IActionResult> OverWriteStockPriceGoodReceptNoteItemDetails([FromForm] OverWriteStockPriceGrnDetailDto overwritestockpriceGrnDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(overwritestockpriceGrnDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(overwritestockpriceGrnDetailDto.barCode);
                GoodReceptNote cart2 = GetSale();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.PurchaseOrderHeaderId == overwritestockpriceGrnDetailDto.purchaseOrderHeaderId && a.ProductId == product.ProductId && a.SupplierPriceId == overwritestockpriceGrnDetailDto.SupplierPriceId).FirstOrDefault();
                    receipt.overWriteStockPrice = overwritestockpriceGrnDetailDto.overWriteStockPrice;
                    grn.OverWriteStockPrice(receipt);
                    SaveCart(grn);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            GoodReceptNote cart1 = GetSale();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        // GET api/grn
        [HttpGet]
        public ActionResult<IEnumerable<GoodReceptNote>> Get()
        {
            GoodReceptNote grn1 = GetSale();
            // get cache byte array
            /*       var data = cache.Get("cart1");
                  // convert byte to string
                  if (data != null)
                  {
                    string value = Encoding.UTF8.GetString(data);
                    // convert string to Cart object
                    var t = JsonConvert.DeserializeAnonymousType(value, grn);
                    // get a cache value
                    //var timestamp = cache.GetString("cart1");
                    return Ok(t);
                  } */
            return Ok(grn1);
        }
        /// <summary>
        /// Get All GRN 
        /// </summary>
        /// <param name="getgrnbysupplier"></param>
        [HttpGet("getgrnbysupplier", Name = "getgrnbysupplier")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetGRNBySupplier(int branchLocationId, int supplierId)
        {
            var sections = await _context.GoodReceptNoteHeaders.Where(b => b.Supplier.SupplierId == supplierId && b.IsDeleted == false && b.BranchLocationId == branchLocationId).OrderBy(c => c.GoodReceptNoteHeaderId).Select(b => new { b.GoodReceptNoteHeaderId }).LastAsync();
            if (sections == null)
            {
                return NotFound("Don't have any section.");
            }
            return Ok(sections);
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //get object in the session storage 
            GoodReceptNote cart2 = GetSale();

            return Ok(cart2);
        }
        #region initial method
        /// <summary>
        /// Test .
        /// </summary> 
        /// <param name="clear"></param> 
        [HttpGet("clear")]
        public ActionResult Clear()
        {
            GoodReceptNote cart2 = GetSale();
            if (cart2.Lines.Count > 0)
            {
                HttpContext.Session.Clear();
                //  cache.Remove("cart1");
                //  cache.Refresh("cart1");
                // cart2.Clear();
                GoodReceptNote clearcart = GetSale();
                return Ok(new { clearcart = clearcart });
            }
            return Ok(new { clearcart = cart2 });
        }
        /*     private void OrderClear()
            {
              GoodReceptNote cart2 = GetSale();
              if (cart2.Lines.Count > 0)
              {
                HttpContext.Session.Clear();
                //  cache.Remove("cart1");
                //  cache.Refresh("cart1");
                cart2.Clear();
              }
            } */
        private GoodReceptNote GetSale()
        {
            //get object in the session storage 
            GoodReceptNote cart = HttpContext.Session.GetJson<GoodReceptNote>("GoodReceptNote") ?? new GoodReceptNote();
            return cart;
        }
        private void SaveCart(GoodReceptNote cart)
        {
            // store object in session
            HttpContext.Session.SetJson("GoodReceptNote", cart);
        }
        private async Task<bool> verifyItemCode(Int64? id) => await _context.Products.AnyAsync(p => p.ProductId == id);
        //please comment on production
        private IList<DummySalePriceUpdate> InitializePriceUpdate(string jsonfilename, string wwwrootpath)
        {
            IList<string> errorList = new List<string>();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind7>>(JSON);
            IList<DummySalePriceUpdate> salePriceDetails = new List<DummySalePriceUpdate>();
            foreach (var item in root)
            {
                var DummySalePrice = new DummySalePriceUpdate
                {
                    SalePrice = item.SalePrice,
                    CostPrice = item.CostPrice,
                    ModifiedBy = item.ModifiedBy,
                    BranchLocationId = item.BranchLocationId,
                    BarCode = item.BarCode
                };
                salePriceDetails.Add(DummySalePrice);
            }

            return salePriceDetails;
        }
        #endregion
    }
}