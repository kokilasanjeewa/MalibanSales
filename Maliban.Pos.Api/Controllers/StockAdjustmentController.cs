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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.XlsIO;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class StockAdjustmentController : BaseController
    {
        #region StockAdjustmentConstructor
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnviroment;

        private StockAdjustment cart;
        //private IDistributedCache _cache;
        private readonly ISqlDataAccess _db;

        [Obsolete]
        public StockAdjustmentController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, StockAdjustment cartService, IDistributedCache cache, ISqlDataAccess db) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            cart = cartService;
            // _cache = cache;
            _db = db;
            _hostingEnviroment = hostingEnviroment;

        }
        #endregion
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallstockadjustmentheaders"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallstockadjustmentheaders", Name = "getallstockadjustmentheaders")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllStockAdjustmentHeaders(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var shopstocks = await _repoWrapper.StockAdjustmentHeader.GetStockAdjustmentHeadersAsync(userParams, BranchLocationId);
            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
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
                default:
#pragma warning disable CS0162 // Unreachable code detected
                    Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
                    return Ok(shopstocks);
            }

        }
        /// <summary>
        /// Get Stocks 
        /// </summary>
        /// <param name="getallstockadjustmentdetails"></param>
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallstockadjustmentdetails", Name = "getallstockadjustmentdetails")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllStockAdjustmentDetails(bool status, [FromQuery] UserParams userParams, int stockadjustmentHeaderId)
        {
            var shopstocks = await _repoWrapper.StockAdjustmentDetail.GetStockAdjustmentDetailsAsync(userParams, stockadjustmentHeaderId);

            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
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
                default:
#pragma warning disable CS0162 // Unreachable code detected
                    Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
                    return Ok(shopstocks);
            }

        }
        /// <summary>
        ///  Create StockAdjustmentDetailDto.
        /// </summary> 
        /// <param name="createstockadjustmentitemdetails"></param> 
        [HttpPost("createstockadjustmentitemdetails")]
        public async Task<IActionResult> CreateStockAdjustmentItemDetails([FromForm] CreateStockAdjustmentDetailDto createStockAdjustmentItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //get session data
            StockAdjustment cart = GetStockAdjustment();
            decimal cartQuantity = 0;
            if (cart.Lines.Count > 0)
            {
                cartQuantity = cart.Lines.Where(c => c.Barcode == createStockAdjustmentItemDetailsDto.barcode).Sum(c => c.AdjustmentQuantity);
            }

            int stockAdjustmentQuantity = Convert.ToInt32(createStockAdjustmentItemDetailsDto.stockAdjustmentQuantity);
            var shopstocks = await _context.TransactionDetails.Where(s => s.IsDeleted == false && s.BranchLocationId == createStockAdjustmentItemDetailsDto.branchLocationId && s.Barcode == createStockAdjustmentItemDetailsDto.barcode && s.dQuantity - s.dIssueQuantity > 0).AsQueryable().ToListAsync();
            if ((Math.Abs(stockAdjustmentQuantity) + Math.Abs(cartQuantity)) > shopstocks.Sum(t => t.dQuantity - t.dIssueQuantity) && stockAdjustmentQuantity < 0)
            {
                return NotFound("Quantity doesn't exists.");
            }
            if (await _repoWrapper.VerifyBarCode(createStockAdjustmentItemDetailsDto.barcode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createStockAdjustmentItemDetailsDto.barcode);
                if (stockAdjustmentQuantity > 0)
                {
                    var latestPriceByStock = await _context.TransactionDetails.Where(c => c.ProductId == product.ProductId && c.BranchLocationId == createStockAdjustmentItemDetailsDto.branchLocationId && c.cDocType == 0).GroupBy(s => new { PriceId = s.PriceId, TransactionDetailId = s.TransactionDetailId }).Select(c => new { PriceId = c.Key.PriceId, TransactionDetailId = c.Min(x => x.TransactionDetailId) }).AsNoTracking().OrderBy(c => c.PriceId).AsQueryable().LastAsync();
                    var latestPrice = await _context.Prices.Where(c => c.PriceId == latestPriceByStock.PriceId).AsNoTracking().OrderBy(c => c.CreateDate).LastAsync<Price>();
                    decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId, latestPrice.SalePrice, latestPrice.ModifiedUnitPrice);
                    var productExpireDate = _repoWrapper.ShopStock.StockDetailQuantityWithExpiryDateAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId, latestPrice.SalePrice, latestPrice.ModifiedUnitPrice);
                    cart.AddItem(product, stockAdjustmentQuantity, latestPrice, stockQty, "In", productExpireDate.Date, latestPriceByStock.TransactionDetailId);
                    SaveCart(cart);
                }
                // update as soon as possible
                if (stockAdjustmentQuantity < 0)
                {

                    var detail = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createStockAdjustmentItemDetailsDto.branchLocationId, xBarcode = createStockAdjustmentItemDetailsDto.barcode, xQty = Math.Abs(stockAdjustmentQuantity) }).Result.AsQueryable();

                    foreach (var item in detail)
                    {
                        var oldPrice = await _context.Prices.Where(c => c.ProductId == product.ProductId && c.BranchLocationId == createStockAdjustmentItemDetailsDto.branchLocationId && c.PriceId == item.PriceId).AsNoTracking().FirstOrDefaultAsync<Price>();
                        var productExpireDate = _repoWrapper.ShopStock.StockDetailQuantityWithExpiryDateAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId, oldPrice.SalePrice, oldPrice.ModifiedUnitPrice);
                        cart.AddItem(product, -item.dNetQuantity, oldPrice, item.dStockQuantity, "Out", productExpireDate.Date, item.TransactionDetailId);
                        SaveCart(cart);
                    }
                }
            }
            else
            {
                return NotFound("Please scan item.");
            }
            StockAdjustment cart1 = GetStockAdjustment();
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
        ///   update lunch token to the system
        /// </summary>
        /// <param name="uploadstockadjustmentdetails"></param>
        [HttpPost("uploadstockadjustmentdetails")]
        [Obsolete]
        public async Task<IActionResult> UploadStockAdjustmentDetails([FromForm] CreateUploadStockAdjestmentDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            IList<string> errorList = new List<string>();
            IList<StockAdjestmentDto> createStockAdjustmentItemDetailList = new List<StockAdjestmentDto>();
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                createStockAdjustmentItemDetailList = worksheet.ExportData<StockAdjestmentDto>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
            }
            foreach (var createStockAdjustmentItemDetailsDto in createStockAdjustmentItemDetailList.AsEnumerable())
            {
                int stockAdjustmentQuantity = Convert.ToInt32(createStockAdjustmentItemDetailsDto.stockAdjustmentQuantity);
                var shopstocks = await _context.TransactionDetails.Where(s => s.IsDeleted == false && s.BranchLocationId == createUploadExcelDto.branchLocationId && s.Barcode == createStockAdjustmentItemDetailsDto.barcode && s.dQuantity - s.dIssueQuantity > 0).ToListAsync();
                if (Math.Abs(stockAdjustmentQuantity) > shopstocks.Sum(t => t.dQuantity - t.dIssueQuantity) && stockAdjustmentQuantity < 0)
                {
                    errorList.Add($"The {createStockAdjustmentItemDetailsDto.barcode} Quantity  {createStockAdjustmentItemDetailsDto.stockAdjustmentQuantity} doesn't exists.");
                }

                if (await _repoWrapper.VerifyBarCode(createStockAdjustmentItemDetailsDto.barcode))
                {
                    var product = await _repoWrapper.Product.GetProductAsync(createStockAdjustmentItemDetailsDto.barcode);
                    if (stockAdjustmentQuantity > 0)
                    {
                        var latestPriceByStock = await _context.TransactionDetails.Where(c => c.ProductId == product.ProductId && c.BranchLocationId == createUploadExcelDto.branchLocationId && c.cDocType == 0).GroupBy(s => new { PriceId = s.PriceId, TransactionDetailId = s.TransactionDetailId }).Select(c => new { PriceId = c.Key.PriceId, TransactionDetailId = c.Min(x => x.TransactionDetailId) }).AsNoTracking().OrderBy(c => c.PriceId).LastAsync();
                        var latestPrice = await _context.Prices.Where(c => c.PriceId == latestPriceByStock.PriceId).AsNoTracking().OrderBy(c => c.CreateDate).LastAsync<Price>();
                        decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createUploadExcelDto.branchLocationId, latestPrice.SalePrice, latestPrice.ModifiedUnitPrice);
                        var productExpireDate = _repoWrapper.ShopStock.StockDetailQuantityWithExpiryDateAsync(product.ProductId, createUploadExcelDto.branchLocationId, latestPrice.SalePrice, latestPrice.ModifiedUnitPrice);
                        cart.AddItem(product, stockAdjustmentQuantity, latestPrice, stockQty, "In", productExpireDate.Date, latestPriceByStock.TransactionDetailId);
                        SaveCart(cart);
                    }
                    // update as soon as possible
                    if (stockAdjustmentQuantity < 0)
                    {
                        // var detail = SubtractAdjustmentListArray(shopstocks, Math.Abs(stockAdjustmentQuantity));
                        var detail = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createUploadExcelDto.branchLocationId, xBarcode = createStockAdjustmentItemDetailsDto.barcode, xQty = Math.Abs(stockAdjustmentQuantity) }).Result.AsQueryable();

                        foreach (var item in detail)
                        {
                            var oldPrice = await _context.Prices.Where(c => c.ProductId == product.ProductId && c.BranchLocationId == createUploadExcelDto.branchLocationId && c.PriceId == item.PriceId).AsNoTracking().FirstOrDefaultAsync<Price>();
                            // decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId, oldPrice.SalePrice);
                            var productExpireDate = _repoWrapper.ShopStock.StockDetailQuantityWithExpiryDateAsync(product.ProductId, createUploadExcelDto.branchLocationId, oldPrice.SalePrice, oldPrice.ModifiedUnitPrice);
                            //if (Math.Abs(stockAdjustmentQuantity) > stockQty)
                            //{
                            //  return NotFound("Quantity doesn't exists.");
                            //}
                            cart.AddItem(product, -item.dNetQuantity, oldPrice, item.dStockQuantity, "Out", productExpireDate.Date, item.TransactionDetailId);
                            SaveCart(cart);
                        }
                    }
                }

            }
            StockAdjustment cart1 = GetStockAdjustment();
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
        /// <param name="Approve/{stockAdjustmentHeaderId}"></param> 
        [HttpGet("Approve/{stockAdjustmentHeaderId}")]
        public async Task<IActionResult> GetStockAdjustmentHeaderDetails(int? stockAdjustmentHeaderId)
        {
            var stockAdjustmentDetails = await _context.StockAdjustmentDetails.Where(h => h.StockAdjustmentHeaderId == stockAdjustmentHeaderId && h.IsDeleted == false).Select(c => new { c.StockAdjustmentHeaderId, c.AdjustmentQuantity, itemPrice = c.Price.SalePrice, CostPrice = c.Price.ModifiedUnitPrice, Total = (c.AdjustmentQuantity - c.ApproveQuantity) * c.Price.SalePrice, c.Product, c.Price, c.ApproveQuantity, c.ProductExpireDate, c.TransactionDetailId, c.AdjustmentType, c.cApprovalType }).AsQueryable().ToListAsync();
            var stockAdjustmentHeader = await _context.StockAdjustmentHeaders.Where(h => h.StockAdjustmentHeaderId == stockAdjustmentHeaderId).AsQueryable().FirstOrDefaultAsync();

            if (stockAdjustmentDetails.Count > 0 && stockAdjustmentHeader != null)
            {
                foreach (var item in stockAdjustmentDetails)
                {
                    decimal stockQty = _repoWrapper.TransactionDetail.FindByCondition(t => t.TransactionDetailId == item.TransactionDetailId).Select(c => c.dQuantity - c.dIssueQuantity).AsQueryable().FirstOrDefault();
                    cart.AddItem(item.Product, item.AdjustmentQuantity, item.Price, stockQty, item.AdjustmentType, item.ProductExpireDate, item.TransactionDetailId, item.ApproveQuantity, item.cApprovalType);
                    SaveCart(cart);
                }
                StockAdjustment cart1 = GetStockAdjustment();
                // object to string
                string value = JsonConvert.SerializeObject(cart1);
                // string set to cache the name is cart1
                //   cache.SetString("cart1", value);
                // get customer information from external database
                //var cust = await GetCustomersInfo();
                // retrun the cart1 object
                return Ok(new { cart1.Lines, StockAdjustmentHeader = stockAdjustmentHeader });

            }
            return BadRequest("Please check your Order number.");
        }
        /// <summary>
        ///  Remove StockAdjustment Item Quantity.
        /// </summary> 
        /// <param name="removestockadjustmentitemquantity"></param> 
        [HttpPost("removestockadjustmentitemquantity")]
        public async Task<IActionResult> RemoveStockAdjustmentItemQuantity([FromForm] RemoveStockAdjustmentItemQuantityDto createStockAdjustmentItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //  await GetCustomersInfo();
            if (await _repoWrapper.VerifyBarCode(createStockAdjustmentItemDetailsDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createStockAdjustmentItemDetailsDto.barCode);
                decimal stockQty = await _repoWrapper.ShopStock.StockQuantityAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId);
                if (stockQty < 1)
                {
                    return NotFound("Product Unavailable.");
                }
                //IPriceRepository GetPriceAsync
                var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, createStockAdjustmentItemDetailsDto.branchLocationId);
                StockAdjustment cartr = GetStockAdjustment();
                cartr.RemoveItem(product, createStockAdjustmentItemDetailsDto.quantity);
                SaveCart(cartr);
            }
            else
            {
                return NotFound("Please scan item.");
            }
            StockAdjustment cart1 = GetStockAdjustment();
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
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="updateapprovestatus"></param> 
        [HttpPost("updateapprovestatus")]
        public async Task<IActionResult> UpdateApproveStatus([FromForm] CreateStockAdjustmentApproveStatusDto updateApproveQuantityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(updateApproveQuantityDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(updateApproveQuantityDto.barCode);
                StockAdjustment cart2 = GetStockAdjustment();
                if (cart2.Lines.Count > 0)
                {
                    var receipt = cart2.Lines.Where(a => a.ProductId == product.ProductId && a.PriceId == updateApproveQuantityDto.PriceId && a.TransactionDetailId == updateApproveQuantityDto.TransactionDetailId).FirstOrDefault();
                    var price = await _context.Prices.Where(c => c.ProductId == product.ProductId && c.BranchLocationId == updateApproveQuantityDto.branchLocationId && c.PriceId == updateApproveQuantityDto.PriceId).AsNoTracking().FirstOrDefaultAsync<Price>();
                    cart.ApproveStatus(product, updateApproveQuantityDto.approveStatus, receipt.AdjustmentQuantity, price, updateApproveQuantityDto.TransactionDetailId);
                    SaveCart(cart);
                }
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            StockAdjustment cart1 = GetStockAdjustment();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            // cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }
        /// <summary>
        /// Remove StockAdjustmentDetailDto.
        /// </summary> 
        /// <param name="removestockadjustmentitemdetails"></param> 
        [HttpPost("removestockadjustmentitemdetails")]
        public async Task<IActionResult> RemoveStockAdjustmentItemDetails([FromForm] RemoveStockAdjustmentDetailDto removeStockAdjustmentDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // await GetCustomersInfo();
            if (await _repoWrapper.VerifyBarCode(removeStockAdjustmentDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(removeStockAdjustmentDetailDto.barCode);
                //IPriceRepository GetPriceAsync
                var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, removeStockAdjustmentDetailDto.branchLocationId, removeStockAdjustmentDetailDto.dSalePrice);
                cart.RemoveLine(product, price);
                SaveCart(cart);
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }
            StockAdjustment cart1 = GetStockAdjustment();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache the name is cart1
            //    cache.SetString("cart1", value);
            // retrun the cart1 object
            return Ok(cart1);
        }

        /// <summary>
        /// You can call checkout method.Please make sure StockAdjustmentitem details already exists in the list
        /// </summary> 
        /// <param name="Check/createstockadjustmentitem"></param> 
        [HttpPost("Check/createstockadjustmentitem")]
        public async Task<IActionResult> CreateStockAdjustmentHeader([FromForm] CreateStockAdjustmentHeaderDto createStockAdjustmentItemDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            int result;
            //get session data
            StockAdjustment cart1 = GetStockAdjustment();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache,the name is cart1
            //  cache.SetString("cart1", value);
            var stockadjustmentitem = _mapper.Map<StockAdjustmentHeader>(createStockAdjustmentItemDto);
            List<StockAdjustmentDetail> lineCollection = new List<StockAdjustmentDetail>();
            foreach (StockAdjustmentDetail item in cart1.Lines.ToList())
            {
                item.Product = null;
                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                item.Price = price;
                item.CreatedBy = createStockAdjustmentItemDto.CreatedBy;
                lineCollection.Add(item);
            }
            stockadjustmentitem.Lines = lineCollection;
            stockadjustmentitem.AdjustmentTypeId = createStockAdjustmentItemDto.AdjustmentTypeId;
            stockadjustmentitem.StockTakenDate = createStockAdjustmentItemDto.StockTakenDate;
            stockadjustmentitem.CreatedBy = createStockAdjustmentItemDto.CreatedBy;
            stockadjustmentitem.StockAdjustmentCostAmount = cart1.ComputeGrandTotalValue;
            stockadjustmentitem.Id = createStockAdjustmentItemDto.Id;
            if (cart1.Lines.Count > 0)
            {
                _context.AttachRange(stockadjustmentitem.Lines);
                await _context.StockAdjustmentHeaders.AddAsync(stockadjustmentitem);
                try
                {
                    result = await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    return BadRequest(e);
                }
                if (result > 0)
                {
                    cart1.Clear();
                }
                return Ok(new { cart1.Lines, StockAdjustmentHeader = stockadjustmentitem.StockAdjustmentHeaderId });
            }
            return BadRequest("Please order item.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure StockAdjustmentitem details already exists in the list
        /// </summary> 
        /// <param name="Confirm/confirmstockadjustmentitem"></param> 
        [HttpPost("Confirm/confirmstockadjustmentitem")]
        public async Task<IActionResult> ConfirmStockAdjustmentHeader([FromForm] UpdateStockAdjustmentHeaderDto createStockAdjustmentItemDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            //get session data
            StockAdjustment cart1 = GetStockAdjustment();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            var stockAdjustmentHeader = _context.StockAdjustmentHeaders.Where(s => s.StockAdjustmentHeaderId == createStockAdjustmentItemDto.StockAdjustmentHeaderId).Include(c => c.Lines).AsNoTracking().FirstOrDefault();
            var saleitem = _mapper.Map<StockAdjustmentHeader>(createStockAdjustmentItemDto);
            List<StockAdjustmentDetail> lineCollection = new List<StockAdjustmentDetail>();
            foreach (StockAdjustmentDetail item in cart1.Lines.AsQueryable().ToList())
            {
                var pod = item;
                pod.Product = null;
                pod.StockAdjustmentHeaderId = createStockAdjustmentItemDto.StockAdjustmentHeaderId;
                pod.CreatedBy = stockAdjustmentHeader.CreatedBy;
                pod.CreateDate = stockAdjustmentHeader.CreateDate;
                pod.ModifiedBy = createStockAdjustmentItemDto.ModifiedBy;
                pod.ModifiedDate = DateTime.Now;
                pod.ApproveQuantity = item.AdjustmentQuantity;
                pod.cApprovalType = item.cApprovalType;
                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                pod.Price = price;
                lineCollection.Add(pod);
            }
            //stockAdjustmentHeader.Lines = lineCollection;
            saleitem.AdjustmentTypeId = createStockAdjustmentItemDto.AdjustmentTypeId;
            saleitem.StockTakenDate = createStockAdjustmentItemDto.StockTakenDate;
            saleitem.ModifiedBy = createStockAdjustmentItemDto.ModifiedBy;
            saleitem.CreatedBy = stockAdjustmentHeader.CreatedBy;
            saleitem.CreateDate = stockAdjustmentHeader.CreateDate;
            saleitem.ModifiedDate = DateTime.Now;
            saleitem.StockAdjustmentCostAmount = cart1.ComputeGrandTotalValue;
            saleitem.Id = createStockAdjustmentItemDto.Id;
            saleitem.Lines = lineCollection;
            saleitem.IsApprove = true;
            saleitem.IsProcess = true;

            var strategy = _context.Database.CreateExecutionStrategy();
            if (cart1.Lines.Count > 0)
            {
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.StockAdjustmentDetails.RemoveRange(stockAdjustmentHeader.Lines);
                            _context.AttachRange(lineCollection);
                            _context.Entry(stockAdjustmentHeader).CurrentValues.SetValues(saleitem);
                            _context.Entry(stockAdjustmentHeader).State = EntityState.Modified;
                            if (_context.Entry(stockAdjustmentHeader).Properties.Any(property => property.IsModified))
                            {
                                await _context.SaveChangesAsync();
                            }
                            //depricated dnetquantity subtract
                            //update when approved quantity greater than zero
                            foreach (var item in lineCollection.AsQueryable())
                            {
                                if (item.AdjustmentType == "Out" || item.AdjustmentQuantity < 0)
                                {
                                    // 4/3/2022  5.13AM update -item.AdjustmentQuantity
                                    var stockdetails = await _repoWrapper.ShopStock.ModifyStockDetailAsync(item.ProductId, createStockAdjustmentItemDto.BranchLocationId, -item.AdjustmentQuantity, createStockAdjustmentItemDto.ModifiedBy, item.Price.SalePrice, item.Price.ModifiedUnitPrice);
                                }
                            }
                            //     update when approved quantity greater than zero
                            await _repoWrapper.ShopStock.UpdateStockAdjustmentTransaction(lineCollection.AsQueryable().ToList(), createStockAdjustmentItemDto.BranchLocationId, createStockAdjustmentItemDto.ModifiedBy);
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
                            cart1.Clear();
                            transaction.Dispose();
                        }
                    }
                });
                return Ok(new { cart1.Lines, StockAdjustmentHeader = createStockAdjustmentItemDto.StockAdjustmentHeaderId });
            }
            return BadRequest("Please order item.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure StockAdjustmentitem details already exists in the list
        /// </summary> 
        /// <param name="Update/updatestockadjustmentitem"></param> 
        [HttpPost("Update/updatestockadjustmentitem")]
        public async Task<IActionResult> UpdateStockAdjustmentHeader([FromForm] UpdateStockAdjustmentHeaderDto createStockAdjustmentItemDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            //get session data
            StockAdjustment cart1 = GetStockAdjustment();
            // object to string
            string value = JsonConvert.SerializeObject(cart1);
            // string set to cache,the name is cart1
            //  cache.SetString("cart1", value);
            var stockAdjustmentHeader = _context.StockAdjustmentHeaders.Where(s => s.StockAdjustmentHeaderId == createStockAdjustmentItemDto.StockAdjustmentHeaderId).Include(c => c.Lines).AsNoTracking().FirstOrDefault();
            var saleitem = _mapper.Map<StockAdjustmentHeader>(createStockAdjustmentItemDto);
            List<StockAdjustmentDetail> lineCollection = new List<StockAdjustmentDetail>();
            foreach (StockAdjustmentDetail item in cart1.Lines.ToList())
            {
                var pod = item;
                pod.Product = null;
                pod.StockAdjustmentHeaderId = createStockAdjustmentItemDto.StockAdjustmentHeaderId;
                pod.CreatedBy = stockAdjustmentHeader.CreatedBy;
                pod.CreateDate = stockAdjustmentHeader.CreateDate;
                pod.ModifiedBy = createStockAdjustmentItemDto.ModifiedBy;
                pod.ModifiedDate = DateTime.Now;
                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                pod.Price = price;
                lineCollection.Add(pod);
            }
            //stockAdjustmentHeader.Lines = lineCollection;
            saleitem.AdjustmentTypeId = createStockAdjustmentItemDto.AdjustmentTypeId;
            saleitem.StockTakenDate = createStockAdjustmentItemDto.StockTakenDate;
            saleitem.ModifiedBy = createStockAdjustmentItemDto.ModifiedBy;
            saleitem.CreatedBy = stockAdjustmentHeader.CreatedBy;
            saleitem.CreateDate = stockAdjustmentHeader.CreateDate;
            saleitem.ModifiedDate = DateTime.Now;
            saleitem.StockAdjustmentCostAmount = cart1.ComputeGrandTotalValue;
            saleitem.Id = createStockAdjustmentItemDto.Id;
            saleitem.Lines = lineCollection;
            saleitem.IsApprove = true;
            var strategy = _context.Database.CreateExecutionStrategy();
            if (cart1.Lines.Count > 0)
            {
                await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.StockAdjustmentDetails.RemoveRange(stockAdjustmentHeader.Lines);
                        _context.AttachRange(lineCollection);
                        _context.Entry(stockAdjustmentHeader).CurrentValues.SetValues(saleitem);
                        _context.Entry(stockAdjustmentHeader).State = EntityState.Modified;
                        if (_context.Entry(stockAdjustmentHeader).Properties.Any(property => property.IsModified))
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
                        cart1.Clear();
                        transaction.Dispose();
                    }
                }
            });
                return Ok(new { cart1.Lines, StockAdjustmentHeader = stockAdjustmentHeader.StockAdjustmentHeaderId });
            }
            return BadRequest("Please order item.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure StockAdjustmentitem details already exists in the list
        /// </summary> 
        /// <param name="Receipt/{stockadjustmentHeaderId}"></param> 
        [HttpGet("Receipt/{stockadjustmentHeaderId}")]
        public async Task<IActionResult> GetStockAdjustmentDetails(int? stockadjustmentHeaderId)
        {
            var stockadjustmentDetails = await _context.StockAdjustmentDetails.Where(h => h.StockAdjustmentHeaderId == stockadjustmentHeaderId).Select(c => new { c.StockAdjustmentDetailId, c.AdjustmentQuantity, c.Price, c.Product }).ToListAsync();

            if (stockadjustmentDetails != null)
            {
                return Ok(stockadjustmentDetails);
            }
            return BadRequest("Please check your Order.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure GetStockAdjustmentItemQuantityByProduct already exists in the list
        /// </summary> 
        //[HttpGet("GetStockAdjustmentItemQuantityByProduct")]
        /// <param name="GetStockAdjustmentItemQuantityByProduct"></param> 
        [HttpPost("getstockadjustmentitemquantitybyproduct", Name = "getstockadjustmentitemquantitybyproduct")]
        public async Task<IActionResult> GetStockAdjustmentItemQuantityByProduct([FromForm] GetStockAdjustmentItemQuantityByProductDto getStockAdjustmentItemQuantityByProductDto)
        {
            var stocks = await _context.TransactionDetails.Where(s => s.Barcode == getStockAdjustmentItemQuantityByProductDto.barCode && s.BranchLocationId == getStockAdjustmentItemQuantityByProductDto.branchLocationId && s.dQuantity - s.dIssueQuantity > 0)
         .GroupBy(p => new { p.dSalePrice, p.ProductId, p.Barcode, p.ProductName }).Select(p => new { ProductId = p.Key.ProductId, ProductName = p.Key.ProductName, Barcoded = p.Key.Barcode, dSalePrice = p.Key.dSalePrice, dQuantity = p.Sum(t => t.dQuantity - t.dIssueQuantity) }).ToListAsync();
            if (stocks != null)
            {
                return Ok(stocks);
            }
            return BadRequest("Please check your Order.");
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<StockAdjustment>> Get()
        {
            StockAdjustment cart1 = GetStockAdjustment();
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
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //get object in the session storage 
            StockAdjustment cart2 = GetStockAdjustment();
            return Ok(cart2);
        }
        /// <summary>
        /// Test .
        /// </summary> 
        /// <param name="clear"></param> 
        [HttpGet("clear")]
        public ActionResult Clear()
        {
            StockAdjustment cart2 = GetStockAdjustment();
            if (cart2.Lines.Count > 0)
            {
                HttpContext.Session.Clear();
                cart2.Clear();
                return NoContent();
            }
            return NoContent();
        }
        #region initial method
        private StockAdjustment GetStockAdjustment()
        {
            //get object in the session storage 
            StockAdjustment cart = HttpContext.Session.GetJson<StockAdjustment>("StockAdjustment") ?? new StockAdjustment();
            return cart;
        }
        private void SaveCart(StockAdjustment cart)
        {
            // store object in session
            HttpContext.Session.SetJson("StockAdjustment", cart);
        }

        #endregion

        /*        private List<TransactionDetail> SubtractAdjustmentListArray1(List<TransactionDetail> array, decimal subtraction)
        {
            List<TransactionDetail> newArray = new List<TransactionDetail>();
            for (int i = 0; i < array.Count; i++)
            {
                if (subtraction > 0)
                {
                    decimal dNetQuantity = array[i].dQuantity - array[i].dIssueQuantity;
                    decimal newValue = dNetQuantity - subtraction;
                    array[i].dPreNetQuantity = dNetQuantity;
                    if (newValue < 1)
                    {
                        newValue = 0;
                        subtraction = subtraction - dNetQuantity;
                    }
                    dNetQuantity = newValue;
                    array[i].dIssueQuantity = array[i].dPreNetQuantity - dNetQuantity;
                    newArray.Add(array[i]);
                    if (newValue > 1)
                    {
                        break;
                    }
                }
            }
            return newArray;
        }*/

    }
    class StockAdjestmentDto
    {

        [Required]
        public string barcode { get; set; }
        [Required]
        public decimal stockAdjustmentQuantity { get; set; }

    }
    public class CreateUploadStockAdjestmentDto
    {
        public IFormFile Files { get; set; }
        public int branchLocationId { get; set; }
        public string createdBy { get; set; }
    }
}