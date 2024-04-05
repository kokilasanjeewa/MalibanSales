using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Newtonsoft.Json;
using Maliban.Pos.Api.Helper;
using System.Text.RegularExpressions;

namespace Maliban.Pos.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class MealTokenController : ControllerBase
    {
        #region SaleConstructor
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISqlDataAccess _db;

        [Obsolete]
        public MealTokenController(IConfiguration config, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, IHttpContextAccessor httpContextAccessor, ISqlDataAccess db)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            _httpContextAccessor = httpContextAccessor;
            _db = db;

        }
        #endregion

        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="check-out/mobilesaleheader"></param> 
        [HttpPost("check-out/mobilesaleheader")]
        public async Task<IActionResult> CreateSaleMobileHeader([FromForm] CreateMealOrderMobileHeaderDto createMealOrderMobile)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            int result;
            decimal quantity = 1;
            var exsitmealOrderHeader = _context.MealOrder_Headers.Where(c => c.mohid == createMealOrderMobile.mohid && c.RequestDate.Date == DateTime.Now.Date && c.BranchLocationId == createMealOrderMobile.BranchLocationId && c.MealTypeId == createMealOrderMobile.MealTypeId && c.MI_Employer_EmployeeId == createMealOrderMobile.MI_Employer_EmployeeId).FirstOrDefault();
            if (exsitmealOrderHeader != null)
            {
                IList<SaleDetail> lineCollection = new List<SaleDetail>();
                var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == createMealOrderMobile.BranchLocationId).FirstOrDefaultAsync();
                var customer = await _repoWrapper.Employee.FindByCondition(c => (c.MI_Employer_EmployeeId == createMealOrderMobile.MI_Employer_EmployeeId) && c.Deleted == 0).FirstOrDefaultAsync();
                if (customer != null)
                {
                    var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == createMealOrderMobile.ProductId).FirstOrDefaultAsync();
                    var getAvailableStock = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createMealOrderMobile.BranchLocationId, xBarcode = product.Barcode, xQty = quantity }).Result.AsQueryable();
                    foreach (var item in getAvailableStock)
                    {
                        decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createMealOrderMobile.BranchLocationId, item.dSalePrice, item.dCostPrice);
                        var price = await _repoWrapper.Price.FindByCondition(p => p.ProductId == item.ProductId && p.BranchLocationId == createMealOrderMobile.BranchLocationId && p.PriceId == item.PriceId).FirstOrDefaultAsync();
                        lineCollection.Add(new SaleDetail
                        {
                            Product = product,
                            PriceId = price.PriceId,
                            ProductId = product.ProductId,
                            ItemPrice = price.SalePrice,
                            ItemDiscount = 0,
                            Quantity = quantity,
                            ReturnQuantity = 0,
                            StockQuantity = stockQty,
                            Total = (price.SalePrice) * (quantity),
                            SubTotal = (price.SalePrice) * (quantity),
                            BillDate = DateTime.Now.Date,
                            ProductExpireDate = item.ProductExpireDate,
                            CreateDate = DateTime.Now.Date,
                            CreatedBy = createMealOrderMobile.CreatedBy,
                            ActiveBill = false,
                            TransactionDetailId = item.TransactionDetailId
                        });
                    }


                    var saleitem = new SaleHeader
                    {
                        PaymentId = 1,
                        CreatedBy = createMealOrderMobile.CreatedBy,
                        TotalAmount = lineCollection.Sum(c => c.Total),
                        Id = createMealOrderMobile.Id,
                        BranchLocationId = branch.BranchLocationId,
                        MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId,
                        EmployeeCode = customer.EPFNo,
                        OrderDate = DateTime.Now.Date,
                        OrderTimeStamp = DateTime.Now.Date,
                        Lines = lineCollection
                    };

                    Utility utility = new Utility(_repoWrapper, _context);

                    customer.CreditLimit = utility.CreditBalanceCalculation(customer.Number, createMealOrderMobile.BranchLocationId);

                    if (createMealOrderMobile.PaymentId == 1)
                    {
                        saleitem.CreditBalance = customer.CreditBalance + saleitem.TotalAmount;
                        saleitem.PreviousAmount = customer.CreditBalance;
                    }
                    saleitem.MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId;

                    if (createMealOrderMobile.PaymentId == 2)
                    {
                        saleitem.Paid = true;
                        saleitem.PaymentReferenceId = createMealOrderMobile.PaymentReferenceId;
                        saleitem.CashBalance = customer.CashBalance + +saleitem.TotalAmount;
                        saleitem.PreviousCashAmount = customer.CashBalance;
                    }
                    if (createMealOrderMobile.PaymentId == 1 && (customer.CreditBalance + +saleitem.TotalAmount > customer.CreditLimit))
                    {
                        return BadRequest("Please check your credit limit.");
                    }
                    if (createMealOrderMobile.PaymentId == 2 && (customer.CashBalance + +saleitem.TotalAmount > customer.CashLimit))
                    {
                        return BadRequest("Please check your cash limit.");
                    }

                    if ((lineCollection.Count > 0 && saleitem != null && customer != null && customer.CreditBalance < customer.CreditLimit && createMealOrderMobile.PaymentId == 1) || (saleitem.Lines.Count > 0 && saleitem != null && customer != null && createMealOrderMobile.PaymentId == 2 && customer.CashBalance < customer.CashLimit))
                    {
                        var strategy = _context.Database.CreateExecutionStrategy();
                        await strategy.ExecuteAsync(async () =>
                        {
                            using var transaction = _context.Database.BeginTransaction();
                            try
                            {
                                _context.AttachRange(saleitem.Lines);
                                _context.SaleHeaders.Add(saleitem);
                                result = _context.SaveChanges();
                                if (result > 0)
                                {
                                    foreach (var item in saleitem.Lines.AsQueryable())
                                    {
                                        var itemCode = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();
                                        var fifo = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createMealOrderMobile.BranchLocationId, xBarcode = itemCode.Barcode, xQty = quantity }).Result.AsQueryable();
                                        foreach (var tr in fifo)
                                        {
                                            var updateTransaction = await _context.TransactionDetails.Where(t => t.TransactionDetailId == tr.TransactionDetailId && t.dQuantity - t.dIssueQuantity > 0).FirstOrDefaultAsync();
                                            if (updateTransaction.dQuantity - updateTransaction.dIssueQuantity >= tr.dNetQuantity)
                                            {
                                                updateTransaction.dIssueQuantity += tr.dNetQuantity;
                                                _context.TransactionDetails.Update(updateTransaction);
                                                _context.SaveChanges();
                                            }
                                            logger.Information("{\"BranchLocationId\":" + updateTransaction.BranchLocationId + ",\"Barcode\":\"" + updateTransaction.Barcode +
                                                "\",\"ProductId\":" + updateTransaction.ProductId + ",\"PriceId\":" + updateTransaction.PriceId + ",\"dQuantity\":" + updateTransaction.dQuantity +
                                                ",\"dIssueQuantity\":" + updateTransaction.dIssueQuantity + ",\"MI_Employer_EmployeeId\":" + saleitem.MI_Employer_EmployeeId + ",\"dSaleQuantity\":" + item.Quantity + ",\"dReturnQuantity\":" + item.ReturnQuantity +
                                                ",\"TransactionDetailId\":" + updateTransaction.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"},");
                                            //https://www.atatus.com/tools/json-generator to json
                                            //https://konklone.io/json/ to csv
                                        }
                                    }

                                    //update transaction update sale invoice start

                                    List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                                    foreach (var item in saleitem.Lines.AsQueryable().ToList())
                                    {
                                        var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == createMealOrderMobile.BranchLocationId).FirstOrDefaultAsync<Price>();
                                        var product = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                        TransactionDetail transactionDetail = new TransactionDetail
                                        {
                                            dQuantity = quantity,
                                            ProductAvailable = true,
                                            BranchLocationId = createMealOrderMobile.BranchLocationId,
                                            ProductId = item.ProductId,
                                            PriceId = item.PriceId,
                                            iDocID = item.SaleDetailId,
                                            cDocNo = "INV" + item.SaleDetailId,
                                            dDocDate = DateTime.Now,
                                            cDocType = cDocType.INV,
                                            cExtRefNum = "Sale Invoice Out",
                                            dIssueQuantity = quantity,
                                            dNetQuantity = -quantity,
                                            dCostPrice = price.ModifiedUnitPrice,
                                            dSalePrice = item.ItemPrice,
                                            dValue = -quantity * price.ModifiedUnitPrice,
                                            dNetValue = 0,
                                            dIN = 0,
                                            dOUT = quantity,
                                            Barcode = product.Barcode,
                                            CreatedBy = createMealOrderMobile.CreatedBy,
                                            CreateDate = DateTime.Now,
                                            ProductExpireDate = item.ProductExpireDate,
                                            ProductName = product.ProductName
                                        };
                                        updateshopStock.Add(transactionDetail);
                                    }
                                    _context.TransactionDetails.AddRange(updateshopStock);
                                    _context.SaveChanges();

                                    //update transaction update sale invoice stop

                                    // update Mi_Employee table credit balance start
                                    if (await _context.MI_Employer_Employees.AnyAsync(s => s.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && s.Deleted == 0))
                                    {
                                        var updateemployeebalance = await _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync<MI_Employer_Employee>();
                                        if (createMealOrderMobile.PaymentId == 1)
                                        {
                                            updateemployeebalance.PreviousCreditBalance = updateemployeebalance.CreditBalance;
                                            updateemployeebalance.CreditBalance += saleitem.TotalAmount;
                                        }
                                        if (createMealOrderMobile.PaymentId == 2)
                                        {
                                            updateemployeebalance.PreviousCashBalance = updateemployeebalance.CashBalance;
                                            updateemployeebalance.CashBalance += saleitem.TotalAmount;
                                        }

                                        _context.MI_Employer_Employees.Update(updateemployeebalance);
                                        _context.SaveChanges();
                                    }
                                    //update Mi_Employee table credit balance stop

                                }

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                logger.Error(string.Join(',', lineCollection) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                            }
                            finally
                            {
                                transaction.Dispose();
                            }
                        });

                        return Ok(new { lineCollection, SaleHeader = saleitem.SaleHeaderId });

                    }
                }
            }
            return BadRequest("Please order item.");
        }


    }


}


/*       /// <summary>
        /// You can call checkout method.Please make sure Saleitems already exists in the list
        /// </summary> 
        /// <param name="Check/createmealorderheader"></param> 
        [AllowAnonymous]
        [HttpPost("check/createmealorderheader")]
        public async Task<IActionResult> CreateMealTokenHeader([FromForm] CreateMealOrderHeaderDto createMealOrderHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            IList<ReturnDto> errorList = new List<ReturnDto>();

            var mealOrderHeader = _mapper.Map<MealOrderHeader>(createMealOrderHeaderDto);
            var exsitmealOrderHeader = _context.MealOrderHeaders.Where(c => c.BookDateFrom >= createMealOrderHeaderDto.BookDateFrom && c.BookDateTo <= createMealOrderHeaderDto.BookDateTo && c.BranchLocationId == createMealOrderHeaderDto.BranchLocationId && c.MealType == (MealType)createMealOrderHeaderDto.MealTypeId && c.MI_Employer_EmployeeId == createMealOrderHeaderDto.MI_Employer_EmployeeId).ToList();
            if (exsitmealOrderHeader.Count > 0)
            {
                errorList.Add(new ReturnDto { Status = "Already booked !!!", Result = 2 });
                return Ok(errorList);
            }
            var mealOrderDetail = JsonConvert.DeserializeObject<IList<MealOrderDetail>>(createMealOrderHeaderDto.List).AsQueryable();
            mealOrderHeader.Lines = mealOrderDetail.ToList();
            if (mealOrderDetail.Count() > 0 && mealOrderHeader != null)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(() =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.AttachRange(mealOrderDetail);
                            _context.MealOrderHeaders.Add(mealOrderHeader);
                            _context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            errorList.Add(new ReturnDto { ErrorMessage = e.InnerException.Message, Status = e.StackTrace, Result = 3 });
                            logger.Error(string.Join(',', mealOrderHeader) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);

                        }
                        finally
                        {
                            transaction.Dispose();
                        }
                    }

                    return Task.CompletedTask;
                });

                errorList.Add(new ReturnDto { Status = "Save Successfully !!!", Result = 1 });

                return Ok(errorList);
            }
            return BadRequest(errorList);
        }

        /// <summary>
        /// You can call checkout method.Please make sure Saleitems already exists in the list
        /// </summary> 
        /// <param name="Check/createmealorderheader"></param> 
        [AllowAnonymous]
        [HttpGet("{branchlocationId}")]
        public async Task<IActionResult> GetMealTokenCart(int branchlocationId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            List<CartItem> products = new List<CartItem>{
                new CartItem{
                  src= "../assets/img/4218ca1d09174218364162cd0b1a8cc1__3_-removebg-preview.png",
                  alt= "Vegetable Rice and Curry",
                  menuitemheading= "Vegetable Rice and Curry",
                  price= 0,
                  priceId= 0,
                  productId= 51725,
                  quantity= 0,
                },
                 new CartItem{
                  src= "../assets/img/4218ca1d09174218364162cd0b1a8cc1-removebg-preview.png",
                  alt= "Chicken Rice and Curry",
                  menuitemheading= "Chicken Rice and Curry",
                  price= 0,
                  priceId= 0,
                  productId= 51726,
                  quantity= 0,
                },
                 new CartItem{
                  src= "../assets/img/4218ca1d09174218364162cd0b1a8cc1__1_-removebg-preview.png",
                  alt= "Fish Rice and Curry",
                  menuitemheading= "Fish Rice and Curry",
                  price= 0,
                  priceId= 0,
                  productId= 51727,
                  quantity= 0,
                },
              };
            foreach (var item in products)
            {
                var product = await _context.Products.Where(p => p.ProductId == item.productId).FirstOrDefaultAsync();
                var productWithPrice = (from p in _context.Prices
                                        where p.ProductId == product.ProductId
                                        where p.BranchLocationId == branchlocationId
                                        select new { productId = p.ProductId, priceId = p.PriceId, price = p.SalePrice }).OrderByDescending(c => c.priceId).FirstOrDefault();
                cartItems.Add(new CartItem { productId = productWithPrice.productId, priceId = productWithPrice.priceId, price = productWithPrice.price, quantity = 0, menuitemheading = product.ProductName, alt = product.ProductName, src = item.src });
            }
            return Ok(cartItems);
        }

        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="check-out/createsaleitem"></param> 
        [HttpPost("check-out/createsaleitem")]
        public async Task<IActionResult> CreateSaleHeader([FromForm] CreateMealOrderHeaderDto createMealOrderHeaderDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            int result;
            decimal quantity = 1;
            var exsitmealOrderHeader = _context.MealOrderHeaders.Where(c => c.BookDateFrom >= DateTime.Now.Date && c.BranchLocationId == createMealOrderHeaderDto.BranchLocationId && c.MealType == (MealType)createMealOrderHeaderDto.MealTypeId && c.MI_Employer_EmployeeId == createMealOrderHeaderDto.MI_Employer_EmployeeId).Include(c => c.Lines).FirstOrDefault();
            IList<SaleDetail> lineCollection = new List<SaleDetail>();
            var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == createMealOrderHeaderDto.BranchLocationId).FirstOrDefaultAsync();
            var customer = await _repoWrapper.Employee.FindByCondition(c => (c.Number == createMealOrderHeaderDto.EmployeeCode) && c.EPFNo == createMealOrderHeaderDto.EmployeeCode.TrimStart(new Char[] { '0' })).FirstOrDefaultAsync();
            if (customer != null)
            {
                foreach (var exsitmeal in exsitmealOrderHeader.Lines.AsQueryable().ToList())
                {

                    var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == exsitmeal.ProductId).FirstOrDefaultAsync();
                    var getAvailableStock = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createMealOrderHeaderDto.BranchLocationId, xBarcode = product.Barcode, xQty = quantity }).Result.AsQueryable();
                    foreach (var item in getAvailableStock)
                    {
                        decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createMealOrderHeaderDto.BranchLocationId, item.dSalePrice, item.dCostPrice);
                        var price = await _repoWrapper.Price.FindByCondition(p => p.ProductId == item.ProductId && p.BranchLocationId == createMealOrderHeaderDto.BranchLocationId && p.PriceId == item.PriceId).FirstOrDefaultAsync();
                        lineCollection.Add(new SaleDetail
                        {
                            Product = product,
                            PriceId = price.PriceId,
                            ProductId = product.ProductId,
                            ItemPrice = price.SalePrice,
                            ItemDiscount = 0,
                            Quantity = quantity,
                            ReturnQuantity = 0,
                            StockQuantity = stockQty,
                            Total = (price.SalePrice) * (quantity),
                            SubTotal = (price.SalePrice) * (quantity),
                            BillDate = DateTime.Now.Date,
                            ProductExpireDate = item.ProductExpireDate,
                            CreateDate = DateTime.Now.Date,
                            CreatedBy = createMealOrderHeaderDto.CreatedBy,
                            ActiveBill = false,
                            TransactionDetailId = item.TransactionDetailId
                        });
                    }
                }
            }
            var saleitem = new SaleHeader
            {
                PaymentId = 1,
                CreatedBy = createMealOrderHeaderDto.CreatedBy,
                TotalAmount = exsitmealOrderHeader.Lines.Sum(c => c.Price.SalePrice * quantity),
                Id = createMealOrderHeaderDto.Id,
                BranchLocationId = branch.BranchLocationId,
                MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId,
                EmployeeCode = customer.EPFNo,
                OrderDate = DateTime.Now.Date,
                OrderTimeStamp = DateTime.Now.Date,
                Lines = lineCollection
            };

            Utility utility = new Utility(_repoWrapper, _context);

            customer.CreditLimit = utility.CreditBalanceCalculation(customer.Number, createMealOrderHeaderDto.BranchLocationId);

            if (createMealOrderHeaderDto.PaymentId == 1)
            {
                saleitem.CreditBalance = customer.CreditBalance + saleitem.TotalAmount;
                saleitem.PreviousAmount = customer.CreditBalance;
            }
            saleitem.MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId;

            if (createMealOrderHeaderDto.PaymentId == 2)
            {
                saleitem.Paid = true;
                saleitem.PaymentReferenceId = createMealOrderHeaderDto.PaymentReferenceId;
                saleitem.CashBalance = customer.CashBalance + +saleitem.TotalAmount;
                saleitem.PreviousCashAmount = customer.CashBalance;
            }
            if (createMealOrderHeaderDto.PaymentId == 1 && (customer.CreditBalance + +saleitem.TotalAmount > customer.CreditLimit))
            {
                return BadRequest("Please check your credit limit.");
            }
            if (createMealOrderHeaderDto.PaymentId == 2 && (customer.CashBalance + +saleitem.TotalAmount > customer.CashLimit))
            {
                return BadRequest("Please check your cash limit.");
            }
            if ((exsitmealOrderHeader.Lines.Count > 0 && saleitem != null && customer != null && customer.CreditBalance < customer.CreditLimit && createMealOrderHeaderDto.PaymentId == 1) || (saleitem.Lines.Count > 0 && saleitem != null && customer != null && createMealOrderHeaderDto.PaymentId == 2 && customer.CashBalance < customer.CashLimit))
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = _context.Database.BeginTransaction();
                    try
                    {
                        _context.AttachRange(saleitem.Lines);
                        _context.SaleHeaders.Add(saleitem);
                        result = _context.SaveChanges();
                        if (result > 0)
                        {
                            foreach (var item in saleitem.Lines.AsQueryable())
                            {
                                var itemCode = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();
                                var fifo = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createMealOrderHeaderDto.BranchLocationId, xBarcode = itemCode.Barcode, xQty = quantity }).Result.AsQueryable();
                                foreach (var tr in fifo)
                                {
                                    var updateTransaction = await _context.TransactionDetails.Where(t => t.TransactionDetailId == tr.TransactionDetailId && t.dQuantity - t.dIssueQuantity > 0).FirstOrDefaultAsync();
                                    if (updateTransaction.dQuantity - updateTransaction.dIssueQuantity >= tr.dNetQuantity)
                                    {
                                        updateTransaction.dIssueQuantity += tr.dNetQuantity;
                                        _context.TransactionDetails.Update(updateTransaction);
                                        _context.SaveChanges();
                                    }
                                    logger.Information("{\"BranchLocationId\":" + updateTransaction.BranchLocationId + ",\"Barcode\":\"" + updateTransaction.Barcode +
                                        "\",\"ProductId\":" + updateTransaction.ProductId + ",\"PriceId\":" + updateTransaction.PriceId + ",\"dQuantity\":" + updateTransaction.dQuantity +
                                        ",\"dIssueQuantity\":" + updateTransaction.dIssueQuantity + ",\"MI_Employer_EmployeeId\":" + saleitem.MI_Employer_EmployeeId + ",\"dSaleQuantity\":" + item.Quantity + ",\"dReturnQuantity\":" + item.ReturnQuantity +
                                        ",\"TransactionDetailId\":" + updateTransaction.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"},");
                                    //https://www.atatus.com/tools/json-generator to json
                                    //https://konklone.io/json/ to csv
                                }
                            }

                            //update transaction update sale invoice start

                            List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                            foreach (var item in saleitem.Lines.AsQueryable().ToList())
                            {
                                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == createMealOrderHeaderDto.BranchLocationId).FirstOrDefaultAsync<Price>();
                                var product = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                TransactionDetail transactionDetail = new TransactionDetail
                                {
                                    dQuantity = quantity,
                                    ProductAvailable = true,
                                    BranchLocationId = createMealOrderHeaderDto.BranchLocationId,
                                    ProductId = item.ProductId,
                                    PriceId = item.PriceId,
                                    iDocID = item.SaleDetailId,
                                    cDocNo = "INV" + item.SaleDetailId,
                                    dDocDate = DateTime.Now,
                                    cDocType = cDocType.INV,
                                    cExtRefNum = "Sale Invoice Out",
                                    dIssueQuantity = quantity,
                                    dNetQuantity = -quantity,
                                    dCostPrice = price.ModifiedUnitPrice,
                                    dSalePrice = item.ItemPrice,
                                    dValue = -quantity * price.ModifiedUnitPrice,
                                    dNetValue = 0,
                                    dIN = 0,
                                    dOUT = quantity,
                                    Barcode = product.Barcode,
                                    CreatedBy = createMealOrderHeaderDto.CreatedBy,
                                    CreateDate = DateTime.Now,
                                    ProductExpireDate = item.ProductExpireDate,
                                    ProductName = product.ProductName
                                };
                                updateshopStock.Add(transactionDetail);
                            }
                            _context.TransactionDetails.AddRange(updateshopStock);
                            _context.SaveChanges();

                            //update transaction update sale invoice stop

                            // update Mi_Employee table credit balance start
                            if (await _context.MI_Employer_Employees.AnyAsync(s => s.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && s.Deleted == 0))
                            {
                                var updateemployeebalance = await _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync<MI_Employer_Employee>();
                                if (createMealOrderHeaderDto.PaymentId == 1)
                                {
                                    updateemployeebalance.PreviousCreditBalance = updateemployeebalance.CreditBalance;
                                    updateemployeebalance.CreditBalance += saleitem.TotalAmount;
                                }
                                if (createMealOrderHeaderDto.PaymentId == 2)
                                {
                                    updateemployeebalance.PreviousCashBalance = updateemployeebalance.CashBalance;
                                    updateemployeebalance.CashBalance += saleitem.TotalAmount;
                                }

                                _context.MI_Employer_Employees.Update(updateemployeebalance);
                                _context.SaveChanges();
                            }
                            //update Mi_Employee table credit balance stop

                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        logger.Error(string.Join(',', exsitmealOrderHeader.Lines) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                });

                return Ok(new { exsitmealOrderHeader.Lines, SaleHeader = saleitem.SaleHeaderId });
            }
            return BadRequest("Please order item.");
        }*/