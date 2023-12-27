using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class SaleController : BaseController
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
        private Sale cart;
        //private IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISqlDataAccess _db;

        [Obsolete]
        public SaleController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, Sale cartService, IDistributedCache cache, IHttpContextAccessor httpContextAccessor, ISqlDataAccess db) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
            _httpContextAccessor = httpContextAccessor;
            _db = db;

        }
        #endregion
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="createsaleitemdetails"></param> 
        [HttpPost("createsaleitemdetails")]
        public async Task<IActionResult> CreateSaleItemDetails([FromForm] CreateSaleDetailDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Utility utility = new Utility(_repoWrapper, _context);
            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode) && createSaleItemDetailsDto.quantity > 0)
            {
                decimal creditLimit = utility.CreditBalanceCalculation(createSaleItemDetailsDto.number, createSaleItemDetailsDto.branchLocationId);

                var employee = await _context.MI_Employer_Employees.Where(e => e.Number.Contains(createSaleItemDetailsDto.number)).Select(c => new { CreditBalance = creditLimit - c.CreditBalance, CashBalance = c.CashLimit - c.CashBalance }).FirstOrDefaultAsync();

                // check exsisting quantity in same product enter by uses
                var productInCart = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
                var previouscartQty = cart.Lines.Where(c => c.ProductId == productInCart.ProductId).Sum(q => q.Quantity);
                if (previouscartQty >= 0)
                {
                    cart.RemoveLine(productInCart);
                    SaveCart(cart);
                }

                // update issue quantity

                //  _db.LoadData<int, dynamic>("dbo.sp_ReUpdatetheIssueQtyinTransectionTable", new { xBranchlocation = createSaleItemDetailsDto.branchLocationId, xBarcode = createSaleItemDetailsDto.barCode }).Result.AsQueryable();

                // get available stock in the given product quantity

                var getAvailableStock = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createSaleItemDetailsDto.branchLocationId, xBarcode = createSaleItemDetailsDto.barCode, xQty = createSaleItemDetailsDto.quantity + previouscartQty }).Result.AsQueryable();
                foreach (var item in getAvailableStock)
                {
                    var itemProduct = await _repoWrapper.Product.GetProductAsync(item.Barcode);
                    var itemPrice = await _repoWrapper.Price.FindByCondition(p => p.ProductId == itemProduct.ProductId && p.BranchLocationId == createSaleItemDetailsDto.branchLocationId && p.PriceId == item.PriceId).FirstOrDefaultAsync();

                    cart.AddItem(itemProduct, item.dNetQuantity, itemPrice, item.dStockQuantity, false, 0, item.ProductExpireDate, item.TransactionDetailId);
                    SaveCart(cart);

                    if (cart.ComputeGrandTotalValue > (createSaleItemDetailsDto.paymentId == 1 ? employee.CreditBalance : employee.CashBalance))
                    {
                        cart.UpdateQuantity(itemProduct, previouscartQty, item.dStockQuantity, itemPrice, item.TransactionDetailId);
                        SaveCart(cart);
                        if (previouscartQty == 0)
                        {
                            cart.RemoveLine(itemProduct);
                            SaveCart(cart);
                        }

                        Sale cart2 = GetSale();
                        return BadRequest(new { Message = "Your credit limit is over.", Cart = cart2 });
                    }
                }

            }

            if (!await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                return NotFound("Please scan item.");
            }

            Sale cart1 = GetSale();
            // object convert to string
            //string value = JsonConvert.SerializeObject(cart1);

            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="quantity/createsaleitemquantity"></param> 
        [HttpPost("quantity/createsaleitemquantity")]
        public async Task<IActionResult> CreateSaleItemQuantity([FromForm] CreateSaleQuantityDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode) && createSaleItemDetailsDto.quantity >= 0)
            {
                var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);

                Utility utility = new Utility(_repoWrapper, _context);

                decimal creditLimit = utility.CreditBalanceCalculation(createSaleItemDetailsDto.number, createSaleItemDetailsDto.branchLocationId);
                var employee = await _context.MI_Employer_Employees.Where(e => e.Number.Contains(createSaleItemDetailsDto.number)).Select(c => new { CreditBalance = creditLimit - c.CreditBalance, CashBalance = c.CashLimit - c.CashBalance }).FirstOrDefaultAsync();

                var stockQty = _repoWrapper.TransactionDetail.FindByCondition(t => t.TransactionDetailId == createSaleItemDetailsDto.transactionDetailId).Select(c => new { dStockQty = (c.dQuantity - c.dIssueQuantity), priceId = c.PriceId }).FirstOrDefault();
                var price = _context.Prices.Where(p => p.ProductId == product.ProductId && p.BranchLocationId == createSaleItemDetailsDto.branchLocationId && p.PriceId == stockQty.priceId).FirstOrDefault();

                if (createSaleItemDetailsDto.quantity <= stockQty.dStockQty || (stockQty.dStockQty == 0 && createSaleItemDetailsDto.quantity > 0))
                {
                    //activate on salereturn
                    var qty = cart.Lines.Where(c => c.ProductId == product.ProductId && c.TransactionDetailId == createSaleItemDetailsDto.transactionDetailId && (c.PriceId == price.PriceId) || c.ItemPrice == createSaleItemDetailsDto.dSalePrice).Select(q => new { q.Quantity, q.ActiveBill }).FirstOrDefault();
                    if (qty.ActiveBill && (qty.Quantity <= createSaleItemDetailsDto.quantity))
                    {
                        cart.UpdateQuantity(product, qty.Quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                    }
                    //activate on salereturn
                    if (qty.ActiveBill && qty.Quantity >= createSaleItemDetailsDto.quantity)
                    {
                        cart.UpdateQuantity(product, createSaleItemDetailsDto.quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                    }
                    //activate on sale
                    if (!qty.ActiveBill)
                    {
                        cart.UpdateQuantity(product, createSaleItemDetailsDto.quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                    }
                    //activate on sale
                    if (!qty.ActiveBill && cart.ComputeGrandTotalValue > (createSaleItemDetailsDto.paymentId == 1 ? employee.CreditBalance : employee.CashBalance))
                    {
                        cart.UpdateQuantity(product, qty.Quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                        Sale cart2 = GetSale();
                        return BadRequest(new { Message = "Your credit limit is over.", Cart = cart2 });
                    }

                }
                if (createSaleItemDetailsDto.quantity > stockQty.dStockQty && stockQty.dStockQty != 0)
                {
                    var qty = cart.Lines.Where(c => c.TransactionDetailId == createSaleItemDetailsDto.transactionDetailId && c.ProductId == product.ProductId && (c.PriceId == price.PriceId) || c.ItemPrice == createSaleItemDetailsDto.dSalePrice).Select(q => new { q.Quantity, q.ActiveBill }).FirstOrDefault();
                    //activate on salereturn
                    if (qty.ActiveBill && qty.Quantity <= createSaleItemDetailsDto.quantity)
                    {
                        cart.UpdateQuantity(product, qty.Quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                    }
                    //activate on salereturn
                    if (qty.ActiveBill && qty.Quantity >= createSaleItemDetailsDto.quantity)
                    {
                        cart.UpdateQuantity(product, createSaleItemDetailsDto.quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                    }
                    //activate on sale
                    if (!qty.ActiveBill)
                    {
                        Sale cart3 = GetSale();
                        return BadRequest(new { Message = "order quantity exceeds stock quantity.", Cart = cart3 });
                        // return NotFound("order quantity exceeds stock quantity.");

                    }
                    //activate on sale
                    if (!qty.ActiveBill && (cart.ComputeGrandTotalValue > (createSaleItemDetailsDto.paymentId == 1 ? employee.CreditBalance : employee.CashBalance)))
                    {
                        cart.UpdateQuantity(product, qty.Quantity, stockQty.dStockQty, price, createSaleItemDetailsDto.transactionDetailId);
                        SaveCart(cart);
                        Sale cart3 = GetSale();
                        return BadRequest(new { Message = "Your credit limit is over.", Cart = cart3 });
                    }

                }

            }

            Sale cart1 = GetSale();
            // object convert to string
            //string value = JsonConvert.SerializeObject(cart1);

            return Ok(cart1);
        }
        /// <summary>
        ///  Create SaleDetailDto.
        /// </summary> 
        /// <param name="fifoquantity/fifocreatesaleitemquantity"></param> 
        [HttpPost("fifoquantity/fifocreatesaleitemquantity")]
        public async Task<IActionResult> FifoCreateSaleItemQuantity([FromForm] CreateSaleQuantityDto createSaleItemDetailsDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Utility utility = new Utility(_repoWrapper, _context);

            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                decimal creditLimit = utility.CreditBalanceCalculation(createSaleItemDetailsDto.number, createSaleItemDetailsDto.branchLocationId);
                var employee = await _context.MI_Employer_Employees.Where(e => e.Number.Contains(createSaleItemDetailsDto.number)).Select(c => new { CreditBalance = creditLimit - c.CreditBalance, CashBalance = c.CashLimit - c.CashBalance }).FirstOrDefaultAsync();

                var product1 = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
                var qty = cart.Lines.Where(c => c.ProductId == product1.ProductId).Sum(q => q.Quantity);
                var updateQty = cart.Lines.Where(c => c.ProductId == product1.ProductId && c.TransactionDetailId == createSaleItemDetailsDto.transactionDetailId).Sum(q => q.Quantity);
                qty = qty - (updateQty - createSaleItemDetailsDto.quantity);
                if (qty >= 0)
                {
                    cart.RemoveLine(product1);
                    SaveCart(cart);
                }

                var item = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createSaleItemDetailsDto.branchLocationId, xBarcode = createSaleItemDetailsDto.barCode, xQty = qty }).Result.AsQueryable();
                foreach (var item1 in item)
                {
                    var product = await _repoWrapper.Product.GetProductAsync(item1.Barcode);
                    var price = await _repoWrapper.Price.FindByCondition(p => p.ProductId == product.ProductId && p.BranchLocationId == createSaleItemDetailsDto.branchLocationId && p.PriceId == item1.PriceId).FirstOrDefaultAsync();
                    cart.AddItem(product, item1.dNetQuantity, price, item1.dStockQuantity, false, 0, item1.ProductExpireDate, item1.TransactionDetailId);
                    SaveCart(cart);

                    if (cart.ComputeGrandTotalValue > (createSaleItemDetailsDto.paymentId == 1 ? employee.CreditBalance : employee.CashBalance))
                    {
                        cart.UpdateQuantity(product, qty, item1.dStockQuantity, price, item1.TransactionDetailId);
                        SaveCart(cart);
                        if (qty == 0)
                        {
                            cart.RemoveLine(product);
                            SaveCart(cart);
                        }
                        Sale cart2 = GetSale();
                        return BadRequest(new { Message = "Your credit limit is over.", Cart = cart2 });
                    }
                }

            }
            if (!await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                return NotFound("Please scan item.");
            }
            Sale cart1 = GetSale();
            // object to string
            //string value = JsonConvert.SerializeObject(cart1);
            return Ok(cart1);
        }
        /// <summary>
        ///  Remove Sale Item Quantity.
        /// </summary> 
        /// <param name="removesaleitemquantity"></param> 
        [HttpPost("removesaleitemquantity")]
        public async Task<IActionResult> RemoveSaleItemQuantity([FromForm] RemoveSaleItemQuantityDto createSaleItemDetailsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
                decimal stockQty = await _repoWrapper.ShopStock.StockQuantityAsync(product.ProductId, createSaleItemDetailsDto.branchLocationId);
                if (stockQty < 1)
                {
                    return NotFound("Product Unavailable.");
                }
                //IPriceRepository GetPriceAsync
                var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, createSaleItemDetailsDto.branchLocationId);
                Sale cartr = GetSale();
                cartr.RemoveItem(product, createSaleItemDetailsDto.quantity);
                SaveCart(cartr);
            }
            else
            {
                return NotFound("Please scan item.");
            }

            Sale cart1 = GetSale();
            // object convert to string
            //string value = JsonConvert.SerializeObject(cart1);

            return Ok(cart1);
        }
        /// <summary>
        /// Remove SaleDetailDto.
        /// </summary> 
        /// <param name="removesaleitemdetails"></param> 
        [HttpPost("removesaleitemdetails")]
        public async Task<IActionResult> RemoveSaleItemDetails([FromForm] RemoveSaleDetailDto removeSaleDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // await GetCustomersInfo();
            if (await _repoWrapper.VerifyBarCode(removeSaleDetailDto.barCode))
            {
                var product = await _repoWrapper.Product.GetProductAsync(removeSaleDetailDto.barCode);
                //IPriceRepository GetPriceAsync
                var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, removeSaleDetailDto.branchLocationId, removeSaleDetailDto.dSalePrice);
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            else
            {
                return NotFound("Barcode doesn't exists.");
            }

            Sale cart1 = GetSale();

            // object convert to string
            //string value = JsonConvert.SerializeObject(cart1);
  
            return Ok(cart1);
        }
        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Check/createsaleitem"></param> 
        [HttpPost("Check/createsaleitem")]
        public async Task<IActionResult> CreateSaleHeader([FromForm] CreateSaleHeaderDto createSaleItemDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            int result;

            //get session data
            Sale cart1 = GetSale();
            // object convert to string
            //string value = JsonConvert.SerializeObject(cart1);

            var saleitem = _mapper.Map<SaleHeader>(createSaleItemDto);

            List<SaleDetail> lineCollection = new List<SaleDetail>();
            foreach (SaleDetail item in cart1.Lines.AsQueryable().ToList())
            {
                var pod = item;
                pod.Product = null;
                pod.CreatedBy = createSaleItemDto.CreatedBy;
                pod.CreateDate = DateTime.Now;
                if (createSaleItemDto.PaymentId == 2)
                {
                    pod.Paid = true;
                }
                lineCollection.Add(pod);
            }

            saleitem.Lines = lineCollection;
            saleitem.EmployeeCode = createSaleItemDto.EmployeeCode.TrimStart(new Char[] { '0' });
            char first = createSaleItemDto.EmployeeCode.Remove(0, 2).First();
            Boolean characters = Char.IsLetter(first);
            if (first == '0')
            {
                saleitem.EmployeeCode = createSaleItemDto.EmployeeCode.Remove(0, 3).TrimStart(new Char[] { '0' });
            }
            else
            {
                saleitem.EmployeeCode = createSaleItemDto.EmployeeCode.Remove(0, 2).TrimStart(new Char[] { '0' });
            }
            if (characters)
            {
                string pattern = "([a-zA-Z]+)([0-9]+)";
                Match match = Regex.Match(createSaleItemDto.EmployeeCode, pattern);
                string numbers = match.Groups[2].Value;
                saleitem.EmployeeCode = numbers.TrimStart(new Char[] { '0' });
            }

            saleitem.PaymentId = createSaleItemDto.PaymentId;
            saleitem.CreatedBy = createSaleItemDto.CreatedBy;
            saleitem.TotalAmount = cart1.ComputeGrandTotalValue;
            saleitem.Id = createSaleItemDto.Id;

            var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == createSaleItemDto.BranchLocationId).FirstOrDefaultAsync();
            var customer = await _repoWrapper.Employee.FindByCondition(c => (c.Number == createSaleItemDto.EmployeeCode) && c.EPFNo == saleitem.EmployeeCode.TrimStart(new Char[] { '0' })).FirstOrDefaultAsync();

            Utility utility = new Utility(_repoWrapper, _context);

            customer.CreditLimit = utility.CreditBalanceCalculation(customer.Number, createSaleItemDto.BranchLocationId);

            if (createSaleItemDto.PaymentId == 1)
            {
                saleitem.CreditBalance = customer.CreditBalance + cart1.ComputeGrandTotalValue;
                saleitem.PreviousAmount = customer.CreditBalance;
            }
            saleitem.MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId;

            if (createSaleItemDto.PaymentId == 2)
            {
                saleitem.Paid = true;
                saleitem.PaymentReferenceId = createSaleItemDto.PaymentReferenceId;
                saleitem.CashBalance = customer.CashBalance + cart1.ComputeGrandTotalValue;
                saleitem.PreviousCashAmount = customer.CashBalance;
            }
            if (createSaleItemDto.PaymentId == 1 && (customer.CreditBalance + cart1.ComputeGrandTotalValue > customer.CreditLimit))
            {
                return BadRequest("Please check your credit limit.");
            }
            if (createSaleItemDto.PaymentId == 2 && (customer.CashBalance + cart1.ComputeGrandTotalValue > customer.CashLimit))
            {
                return BadRequest("Please check your cash limit.");
            }
            if ((cart1.Lines.Count > 0 && saleitem != null && customer != null && customer.CreditBalance < customer.CreditLimit && createSaleItemDto.PaymentId == 1) || (cart1.Lines.Count > 0 && saleitem != null && customer != null && createSaleItemDto.PaymentId == 2 && customer.CashBalance < customer.CashLimit))
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.AttachRange(saleitem.Lines);
                            _context.SaleHeaders.Add(saleitem);
                            result = _context.SaveChanges();
                            if (result > 0)
                            {
                                foreach (var item in cart1.Lines.AsQueryable())
                                {
                                    var itemCode = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();
                                    var fifo = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createSaleItemDto.BranchLocationId, xBarcode = itemCode.Barcode, xQty = item.Quantity }).Result.AsQueryable();
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

                                //stop method _repoWrapper.ShopStock.UpdateSaleTransaction() and run here
                                //update transaction update sale invoice start

                                List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                                foreach (var item in cart1.Lines.AsQueryable().ToList())
                                {
                                    var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == createSaleItemDto.BranchLocationId).FirstOrDefaultAsync<Price>();
                                    var product = await _context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                                    TransactionDetail transactionDetail = new TransactionDetail
                                    {
                                        dQuantity = item.Quantity,
                                        ProductAvailable = true,
                                        BranchLocationId = createSaleItemDto.BranchLocationId,
                                        ProductId = item.ProductId,
                                        PriceId = item.PriceId,
                                        iDocID = item.SaleDetailId,
                                        cDocNo = "INV" + item.SaleDetailId,
                                        dDocDate = DateTime.Now,
                                        cDocType = cDocType.INV,
                                        cExtRefNum = "Sale Invoice Out",
                                        dIssueQuantity = item.Quantity,
                                        dNetQuantity = -item.Quantity,
                                        dCostPrice = price.ModifiedUnitPrice,
                                        dSalePrice = item.ItemPrice,
                                        dValue = -item.Quantity * price.ModifiedUnitPrice,
                                        dNetValue = 0,
                                        dIN = 0,
                                        dOUT = item.Quantity,
                                        Barcode = product.Barcode,
                                        CreatedBy = createSaleItemDto.CreatedBy,
                                        CreateDate = DateTime.Now,
                                        ProductExpireDate = item.ProductExpireDate,
                                        ProductName = product.ProductName
                                    };
                                    updateshopStock.Add(transactionDetail);
                                }
                                _context.TransactionDetails.AddRange(updateshopStock);
                                _context.SaveChanges();

                                //update transaction update sale invoice stop

                                // stop method _repoWrapper.ShopStock.UpdateEmployeeBalanceAsync() and run here 
                                // update Mi_Employee table credit balance start
                                if (await _context.MI_Employer_Employees.AnyAsync(s => s.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && s.Deleted == 0))
                                {
                                    var updateemployeebalance = await _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync<MI_Employer_Employee>();
                                    if (createSaleItemDto.PaymentId == 1)
                                    {
                                        updateemployeebalance.PreviousCreditBalance = updateemployeebalance.CreditBalance;
                                        updateemployeebalance.CreditBalance += cart1.ComputeGrandTotalValue;
                                    }
                                    if (createSaleItemDto.PaymentId == 2)
                                    {
                                        updateemployeebalance.PreviousCashBalance = updateemployeebalance.CashBalance;
                                        updateemployeebalance.CashBalance += cart1.ComputeGrandTotalValue;
                                    }

                                    _context.MI_Employer_Employees.Update(updateemployeebalance);
                                    _context.SaveChanges();
                                }
                                //update Mi_Employee table credit balance stop

                                cart1.Clear();
                            }

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            logger.Error(string.Join(',', cart1.Lines) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                        }
                        finally
                        {
                            transaction.Dispose();
                            cart.Clear();
                        }
                    }
                });

                return Ok(new { cart1.Lines, SaleHeader = saleitem.SaleHeaderId });
            }
            return BadRequest("Please order item.");
        }
        /// <summary>
        ///   update lunch token to the system
        /// </summary>
        /// <param name="updatelunchtoken"></param>
        [HttpPost("Check/updatelunchtoken")]
        public async Task<IActionResult> UpdateLunchToken([FromForm] CreateUploadLunchTokenDto createUploadExcelDto)
        {
            int result;
            IList<string> errorList = new List<string>();
            IList<SaleLunchByEmployee> users = new List<SaleLunchByEmployee>();
   
            foreach (var user in users)
            {
                var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == createUploadExcelDto.BranchLocationId).FirstOrDefaultAsync();
                var customer = await _repoWrapper.Employee.FindByCondition(c => c.Number == user.EmployeeNumber && c.Number.Contains(branch.BranchCode) && c.Deleted == 0).FirstOrDefaultAsync();
                if (customer != null)
                {
                    IList<SaleDetail> lineCollection = new List<SaleDetail>();
                    var stockDetails = await _context.TransactionDetails.Where(s => s.IsDeleted == false && s.BranchLocationId == createUploadExcelDto.BranchLocationId && s.dQuantity - s.dIssueQuantity > 0 && s.Barcode == user.BarCode).GroupBy(t => new { t.ProductId, t.dSalePrice, t.dCostPrice, t.Barcode, t.ProductName, t.ProductExpireDate }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dQuantity - t.dIssueQuantity), dSalePrice = p.Key.dSalePrice, dCostPrice = p.Key.dCostPrice, Barcode = p.Key.Barcode, ProductName = p.Key.ProductName, ProductExpireDate = p.Key.ProductExpireDate }).FirstOrDefaultAsync();
                    var product = await _repoWrapper.Product.GetProductAsync(user.BarCode);
                    decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createUploadExcelDto.BranchLocationId, stockDetails.dSalePrice, stockDetails.dCostPrice);
                    //IPriceRepository GetPriceAsync
                    var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, createUploadExcelDto.BranchLocationId, stockDetails.dSalePrice);
                    lineCollection.Add(new SaleDetail
                    {
                        Product = product,
                        PriceId = price.PriceId,
                        ProductId = product.ProductId,
                        ItemPrice = price.SalePrice,
                        // ItemDiscount = price.DiscountPrecentage,
                        ItemDiscount = 0,
                        Quantity = user.Quantity,
                        ReturnQuantity = 0,
                        StockQuantity = stockQty,
                        Total = (price.SalePrice) * (user.Quantity),
                        SubTotal = (price.SalePrice) * (user.Quantity),
                        BillDate = user.OrderDate,
                        ProductExpireDate = stockDetails.ProductExpireDate,
                        CreateDate = user.OrderDate,
                        CreatedBy = createUploadExcelDto.CreatedBy,
                        ActiveBill = false
                    });
                    decimal ComputeGrandTotalValue = (price.SalePrice) * (user.Quantity);
                    var saleitem = new SaleHeader
                    {
                        PaymentId = 1,
                        CreatedBy = createUploadExcelDto.CreatedBy,
                        TotalAmount = ComputeGrandTotalValue,
                        Id = createUploadExcelDto.Id,
                        BranchLocationId = branch.BranchLocationId,
                        MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId,
                        EmployeeCode = customer.EPFNo,
                        OrderDate = user.OrderDate,
                        OrderTimeStamp = user.OrderDate,
                        Lines = lineCollection
                    };

                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                _context.AttachRange(saleitem.Lines);
                                _context.SaleHeaders.Add(saleitem);
                                result = _context.SaveChanges();
                                if (result > 0)
                                {
                                    //depricated dnetquantity subtract
                                    //foreach (var item in saleitem.Lines)
                                    //{
                                    //    var stockdetails = await _repoWrapper.ShopStock.ModifyStockDetailAsync(item.ProductId, createUploadExcelDto.BranchLocationId, item.Quantity, createUploadExcelDto.CreatedBy, item.ItemPrice);
                                    //}
                                    await _repoWrapper.ShopStock.UpdateSaleTransaction(lineCollection.ToList(), createUploadExcelDto.BranchLocationId, createUploadExcelDto.CreatedBy);
                                    await _repoWrapper.ShopStock.UpdateEmployeeBalanceAsync(customer.MI_Employer_EmployeeId, ComputeGrandTotalValue, createUploadExcelDto.CreatedBy);
                                }
                                transaction.Commit();
                                errorList.Add($"Successfully saved the code {customer.EPFNo} to employee table.");

                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                logger.Error("EmployeeCode" + user.EmployeeNumber + string.Join(',', lineCollection) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                                errorList.Add("EmployeeCode" + user.EmployeeNumber + string.Join(',', lineCollection) + e.Message);
                                //return BadRequest(e);
                            }
                        }

                    });
                }
                else
                {
                    logger.Error("EmployeeCode" + user.EmployeeNumber);
                    errorList.Add("EmployeeCode" + user.EmployeeNumber);
                }

            }


            return Ok(errorList);
        }
        [HttpGet("Check/printer/{saleHeaderId}")]
        public async Task<IActionResult> PrintAPI(Int64 SaleHeaderId)
        {
            var saleHeader = await _context.SaleHeaders.Where(s => s.SaleHeaderId == SaleHeaderId).Include(b => b.BranchLocation).FirstOrDefaultAsync();
            var employee = await _context.MI_Employer_Employees.FindAsync(saleHeader.MI_Employer_Employee);
            PrintJob1 pj = new PrintJob1(cart, saleHeader, employee, saleHeader.BranchLocation);
            pj.Print(@"\\Kokila-LAP\EPSON TM-U220 Receipt");
            return Ok();
        }
 
        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="SaleReturn/createsalereturnitem"></param> 
        [HttpPost("SaleReturn/createsalereturnitem")]
        public async Task<IActionResult> CreateSaleReturnHeader([FromForm] CreateSaleReturnHeaderDto createSaleItemDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            int result;
            decimal customerReturnAmount = 0;
            //get session data
            Sale cart1 = GetSale();
            // object to string
           // string value = JsonConvert.SerializeObject(cart1);
            // string set to cache,the name is cart1
            //  cache.SetString("cart1", value);
            var saleitem = _mapper.Map<CustomerReturnHeader>(createSaleItemDto);
            var saleHeader = await _context.SaleHeaders.FindAsync(createSaleItemDto.SaleHeaderId);
            List<SaleDetail> lineCollection = new List<SaleDetail>();
            foreach (SaleDetail item in cart1.Lines.AsQueryable().ToList())
            {
                var pod = item;
                var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                var saleDetail = await _context.SaleDetails.Where(s => s.SaleHeaderId == createSaleItemDto.SaleHeaderId && s.ProductId == item.ProductId && s.PriceId == item.PriceId).FirstOrDefaultAsync();
                pod.SaleDetailId = saleDetail.SaleDetailId;
                pod.Price = price;
                lineCollection.Add(pod);
            }
            saleitem.Lines = await MapToClassAsync(lineCollection);
            saleitem.EmployeeCode = saleHeader.EmployeeCode;
            saleitem.PaymentId = saleHeader.PaymentId;
            saleitem.CreatedBy = createSaleItemDto.CreatedBy;
            saleitem.CustomerReturnAmount = saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice);
            customerReturnAmount = saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice);
            saleitem.Id = createSaleItemDto.Id;
            saleitem.CustomerReturnDate = DateTime.Now;
            saleitem.SaleHeaderId = saleHeader.SaleHeaderId;
            saleitem.TotalAmount = saleHeader.TotalAmount;

            var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == saleHeader.BranchLocationId).FirstOrDefaultAsync();
            var customer = await _repoWrapper.Employee.FindByCondition(c => c.MI_Employer_EmployeeId == saleHeader.MI_Employer_EmployeeId && c.EPFNo == saleHeader.EmployeeCode.TrimStart(new Char[] { '0' })).FirstOrDefaultAsync();
            Utility utility = new Utility(_repoWrapper, _context);
            customer.CreditLimit = utility.CreditBalanceCalculation(customer.Number, saleHeader.BranchLocationId);
            if (!_context.SaleHeaders.Any(e => e.EmployeeCode == customer.EPFNo))
            {
                return BadRequest("Please check your employee number.");
            }
            //saleitem.MI_Employer_Employee = customer;
            saleitem.MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId;
            saleitem.PreviousBillAmount = customer.CreditBalance;
            saleitem.CreditBalance = customer.CreditBalance - saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice);
            decimal customerCreditBalanceReturn = saleHeader.TotalAmount - saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice);
            if ((saleHeader.PaymentId == 1 ? customer.CreditBalance - customerCreditBalanceReturn : customer.CashBalance - customerCreditBalanceReturn) > (saleHeader.PaymentId == 1 ? customer.CreditLimit : customer.CashLimit))
            {
                return BadRequest("Please check your credit limit.");
            }
            if (cart1.Lines.Sum(r => r.ReturnQuantity) > 0 && saleitem != null && customer != null && (customer.CreditBalance - customerCreditBalanceReturn) < customer.CreditLimit)
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.AttachRange(saleitem.Lines);
                            await _context.CustomerReturnHeaders.AddAsync(saleitem);
                            result = await _context.SaveChangesAsync();
                            if (result > 0)
                            {
                                foreach (var item in saleitem.Lines.AsQueryable())
                                {
                                    var price = await _context.Prices.Where(s => s.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                                    item.PriceId = price.PriceId;
                                    var saleDetail = await _context.SaleDetails.Where(s => s.SaleHeaderId == createSaleItemDto.SaleHeaderId && s.ProductId == item.ProductId && s.PriceId == price.PriceId && s.TransactionDetailId == item.TransactionDetailId).FirstOrDefaultAsync();
                                    saleDetail.ReturnQuantity = saleDetail.ReturnQuantity + item.ReturnQuantity;
                                    saleDetail.IsReturn = true;
                                    saleDetail.Price = price;
                                    saleDetail.PriceId = price.PriceId;
                                    _context.SaleDetails.Update(saleDetail);
                                    logger.Information("{\"Return_MI_Employer_EmployeeId\":" + customer.MI_Employer_EmployeeId + ",\"SaleHeaderId\":" + createSaleItemDto.SaleHeaderId + ",\"ProductId\":" + item.ProductId + ",\"PriceId\":" + item.PriceId + ",\"dReturnQuantity\":" + item.ReturnQuantity + ",\"TransactionDetailId\":" + item.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"}");
                                }
                                //decimal saleHeaderAmount = saleHeader.TotalAmount;
                                //saleHeader.TotalAmount = cart1.ComputeGrandTotalValue;
                                saleHeader.CustomerReturnAmount = customerReturnAmount + saleHeader.CustomerReturnAmount;
                                _context.SaleHeaders.Update(saleHeader);
                                // throw new Exception();
                                //await _repoWrapper.ShopStock.UpdateSaleReturnTransaction(saleitem.Lines.ToList(), createSaleItemDto.BranchLocationId, createSaleItemDto.CreatedBy);
                                List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
                                foreach (var item in saleitem.Lines.AsQueryable().ToList())
                                {
                                    var price = await _context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == createSaleItemDto.BranchLocationId).FirstOrDefaultAsync<Price>();
                                    TransactionDetail transactionDetail = new TransactionDetail
                                    {
                                        dQuantity = item.ReturnQuantity,
                                        ProductAvailable = true,
                                        BranchLocationId = createSaleItemDto.BranchLocationId,
                                        ProductId = item.ProductId,
                                        PriceId = price.PriceId,
                                        Price = price,
                                        iDocID = item.CustomerReturnDetailId,
                                        cDocNo = "SLR" + item.CustomerReturnDetailId,
                                        dDocDate = DateTime.Now,
                                        cDocType = cDocType.SLR,
                                        cExtRefNum = "Sale Return",
                                        dIssueQuantity = 0,
                                        dNetQuantity = item.ReturnQuantity,
                                        dCostPrice = price.ModifiedUnitPrice,
                                        dSalePrice = price.SalePrice,
                                        dValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                                        dNetValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                                        ProductExpireDate = item.ProductExpireDate,
                                        dIN = item.ReturnQuantity,
                                        dOUT = 0,
                                        Barcode = item.Product.Barcode,
                                        CreatedBy = createSaleItemDto.CreatedBy,
                                        CreateDate = DateTime.Now,
                                        ProductName = item.Product.ProductName
                                    };
                                    updateshopStock.Add(transactionDetail);

                                }
                                _context.TransactionDetails.AddRange(updateshopStock);
                                await _context.SaveChangesAsync();
                                //await _repoWrapper.ShopStock.UpdateEmployeeBalanceAsync(customer.MI_Employer_EmployeeId, -(saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice)), createSaleItemDto.CreatedBy);
                                if (await _context.MI_Employer_Employees.AnyAsync(s => s.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && s.Deleted == 0))
                                {
                                    var updateemployeebalance = await _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == customer.MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync<MI_Employer_Employee>();
                                    if (saleHeader.PaymentId == 1)
                                    {
                                        updateemployeebalance.PreviousCreditBalance = updateemployeebalance.CreditBalance;
                                        updateemployeebalance.CreditBalance += -(saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice));
                                    }
                                    if (saleHeader.PaymentId == 2)
                                    {
                                        updateemployeebalance.PreviousCashBalance = updateemployeebalance.CashBalance;
                                        updateemployeebalance.CashBalance += -(saleitem.Lines.Sum(l => l.ReturnQuantity * l.Price.SalePrice));
                                    }
                                    //updateemployeebalance.ModifyDate = DateTime.Now;
                                    //updateemployeebalance.ModifiedBy = modifiedBy;
                                    _context.MI_Employer_Employees.Update(updateemployeebalance);
                                    _context.SaveChanges();
                                    //  transaction.Commit();
                                }
                                cart1.Clear();
                            }
                            // await _context.SaveChangesAsync();
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
                            cart.Clear();

                        }

                    }
                });
                return Ok(new { cart1.Lines, SaleHeader = createSaleItemDto.SaleHeaderId });
            }
            return BadRequest("Please update return quantity.");
        }

        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Receipt/{saleHeaderId}"></param> 
        [HttpGet("Receipt/{saleHeaderId}")]
        public async Task<IActionResult> GetReceiptDetails(int? saleHeaderId)
        {
            var receiptDetails = await _context.SaleDetails.Where(h => h.SaleHeaderId == saleHeaderId && h.Quantity - h.ReturnQuantity > 0).Select(c => new { c.SaleDetailId, c.Quantity, c.ReturnQuantity, c.Price, c.Product }).ToListAsync();
            var customer = await _context.SaleHeaders.Where(h => h.SaleHeaderId == saleHeaderId).Include(c => c.MI_Employer_Employee).Select(c => new { c.EmployeeCode, c.MI_Employer_Employee.Number, c.MI_Employer_Employee.FullName, c.MI_Employer_Employee.CreditBalance, c.MI_Employer_Employee.CreditLimit, c.MI_Employer_Employee.PreviousCreditBalance, c.PaymentReferenceId, c.PaymentId, c.MI_Employer_Employee.PreviousCashBalance, c.MI_Employer_Employee.CashBalance, c.MI_Employer_Employee.CashLimit }).FirstOrDefaultAsync();
            if (receiptDetails != null)
            {
                return Ok(new { receiptDetails = receiptDetails, customer = customer });
            }
            return BadRequest("Please check your Order.");
        }
        /// <summary>
        /// You can call checkout method.Please make sure Saleitem details already exists in the list
        /// </summary> 
        /// <param name="Return/{saleHeaderId}/{branchlocationId}"></param> 
        [HttpGet("Return/{saleHeaderId}/{branchlocationId}")]
        public async Task<IActionResult> GetReturnDetails(int? saleHeaderId, int branchlocationId)
        {
            var saleHeader = await _context.SaleHeaders.Where(h => h.SaleHeaderId == saleHeaderId && h.BranchLocationId == branchlocationId && (h.PaymentId == 1 ? h.Paid == false : h.Paid == true)).Select(c => new { c.BranchLocationId, c.PaymentId }).FirstOrDefaultAsync();
            var receiptDetails = await _context.SaleDetails.Where(h => h.SaleHeaderId == saleHeaderId && (saleHeader.PaymentId == 1 ? h.Paid == false : h.Paid == true) && h.Quantity - h.ReturnQuantity > 0).Select(c => new { c.SaleDetailId, c.Quantity, itemPrice = c.Price.SalePrice, CostPrice = c.Price.ModifiedUnitPrice, Total = c.Quantity * c.Price.SalePrice, c.Product, c.Price, c.ReturnQuantity, c.ProductExpireDate, c.TransactionDetailId }).ToListAsync();

            //var query12 = (from b in _context.SaleDetails.Where(h => h.SaleHeaderId == saleHeaderId)
            //                 select new { Total = _context.SaleReturnTotalAmount(b.SaleHeaderId) }).Distinct();
            if (receiptDetails.Count > 0 && saleHeader != null)
            {
                foreach (var item in receiptDetails)
                {
                    decimal stockQty = _repoWrapper.TransactionDetail.FindByCondition(t => t.TransactionDetailId == item.TransactionDetailId).Select(c => c.dQuantity - c.dIssueQuantity).FirstOrDefault();
                    cart.AddItem(item.Product, item.Quantity - item.ReturnQuantity, item.Price, stockQty, true, 0, item.ProductExpireDate, item.TransactionDetailId);
                    SaveCart(cart);
                }
                Sale cart1 = GetSale();
                // object to string
               // string value = JsonConvert.SerializeObject(cart1);
                // string set to cache the name is cart1
                //   cache.SetString("cart1", value);
                // get customer information from external database
                //var cust = await GetCustomersInfo();
                // retrun the cart1 object
                return Ok(new { cart1, paymentType = saleHeader.PaymentId });
            }


            return BadRequest("Please check your Order number.");
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Sale>> Get()
        {
            Sale cart1 = GetSale();
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
            Sale cart2 = GetSale();
            return Ok(cart2);
        }
        /// <summary>
        /// Test .
        /// </summary> 
        /// <param name="clear"></param> 
        [HttpGet("clear")]
        public ActionResult Clear()
        {
            Sale cart2 = GetSale();
            if (cart2.Lines.Count > 0)
            {
                HttpContext.Session.Clear();
                cart2.Clear();
                return NoContent();
            }
            return NoContent();
        }
        #region initial method
        private Sale GetSale()
        {
            //get object in the session storage 
            Sale cart = HttpContext.Session.GetJson<Sale>("Sale") ?? new Sale();
            return cart;
        }
        private void SaveCart(Sale cart)
        {
            // store object in session
            HttpContext.Session.SetJson("Sale", cart);
        }
        private async Task<ICollection<Customer>> GetCustomersInfo()
        {
            Collection<Customer> customers = new Collection<Customer>();
            //https://vispad.blogspot.com/2018/12/raw-sql-query-without-dbset-entity.html
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("PayrollConnection")))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.SPGetCustomers", connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 200;
                //sqlCommand.Parameters.AddWithValue("@TeacherId", (object)teacherId);
                await connection.OpenAsync();
                using (var sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        customers.Add(MapToValue(sqlDataReader));
                        /*teacherInfo.TeacherId = teacherId;
						teacherInfo.CurrentBalance = Convert.ToDecimal(sqlDataReader["CurrentBalance"]);
						teacherInfo.LastPaymentDate = Convert.ToDateTime(sqlDataReader["LastPaymentDate"]);
						teacherInfo.NoOfCardsAssgined = Convert.ToInt32(sqlDataReader["NoOfCardsAssgined"]);
						teacherInfo.NoOfClasses = Convert.ToInt32(sqlDataReader["NoOfClasses"]);
						teacherInfo.NoOfInstitutes = Convert.ToInt32(sqlDataReader["NoOfInstitutes"]);
						teacherInfo.NoOfStudents = Convert.ToInt16(sqlDataReader["NoOfStudents"]);
						teacherInfo.NoOfStudentsActiveLastMonth = Convert.ToInt32(sqlDataReader["NoOfStudentsActiveLastMonth"]);
						teacherInfo.LastMonthPaidStudentCount = Convert.ToInt32(sqlDataReader["NoOfStudentsPaidLastMonth"]);
						teacherInfo.NoOfStudentsDataEntered = Convert.ToInt32(sqlDataReader["NoOfStudentsDataEntered"]);
						teacherInfo.NoOfStudentsHasProfileImage = Convert.ToInt32(sqlDataReader["NoOfStudentsHasProfileImage"]);*/
                    }
                    sqlDataReader.Close();
                    connection.Close();
                }
            }
            return customers;
        }
        private Customer MapToValue(SqlDataReader reader)
        {
            return new Customer()
            {
                FirstName = Convert.ToString(reader["Name"])
                /*MiddleName 
				LastName
				Surname 
				Number 
				TextCardNumber
			  Id = Convert.ToInt32(reader["Id"]),
			  Uid = Convert.ToString(reader["Uid"]),
			  InstituteName = Convert.ToString(reader["InstituteName"]),
			  ClassName = Convert.ToString(reader["ClassName"]),
			  Greade = Convert.ToString(reader["Greade"]),
			  SubjectName = Convert.ToString(reader["SubjectName"]),
			  InstituteId = Convert.ToString(reader["InstituteId"]),
			  CreatedBy = Convert.ToString(reader["CreatedBy"]),
			  ModifiedBy = Convert.ToString(reader["ModifiedBy"]),
			  RegDate = Convert.ToDateTime(reader["RegDate"]),
			  NoOfStudents = Convert.ToInt32(reader["NoOfStudents"]), */
            };
        }
        private async Task<List<CustomerReturnDetail>> MapToClassAsync(List<SaleDetail> SaleDetails)
        {
            List<CustomerReturnDetail> customerReturnDetails = new List<CustomerReturnDetail>();

            foreach (var item in SaleDetails)
            {
                var price = await _context.Prices.Where(p => p.PriceId == item.PriceId).FirstOrDefaultAsync<Price>();
                item.Price = price;
                item.PriceId = price.PriceId;
                var product = await _context.Products.Where(p => p.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();
                if (item.ReturnQuantity > 0)
                {
                    CustomerReturnDetail customerReturnDetail = new CustomerReturnDetail
                    {
                        Product = product,
                        PriceId = price.PriceId,
                        ProductId = product.ProductId,
                        Price = price,
                        ReturnQuantity = item.ReturnQuantity,
                        Amount = item.ReturnQuantity * price.SalePrice,
                        CurrentQuantity = item.StockQuantity,
                        BalanceQuantity = item.StockQuantity + item.ReturnQuantity,
                        SaleQuantity = item.Quantity,
                        ProductExpireDate = item.ProductExpireDate,
                        SaleDetailId = item.SaleDetailId,
                        CreateDate = DateTime.Now,
                        TransactionDetailId = item.TransactionDetailId
                    };
                    customerReturnDetails.Add(customerReturnDetail);
                }
            }
            return customerReturnDetails.ToList();
        }
        private List<CartItemList> SubtractCartListArray(List<CartItemList> array, decimal subtraction)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (subtraction > 0)
                {
                    decimal newValue = array[i].dNetQuantity - subtraction;
                    array[i].dPreNetQuantity = array[i].dNetQuantity;
                    if (newValue < 1)
                    {
                        newValue = 0;
                        subtraction = subtraction - array[i].dNetQuantity;
                    }
                    array[i].dNetQuantity = newValue;
                    array[i].dIssueQuantity += array[i].dPreNetQuantity - array[i].dNetQuantity;
                    if (newValue > 1)
                    {
                        break;
                    }
                }
            }
            return array;
        }

        #endregion
    }
    public class OrderItemGet
    {
        public string PrintTitle { get; set; }
    }
    public class CartItemList
    {
        public Int64 TransactionDetailId { get; set; }
        public Int64 ProductId { get; set; }
        public decimal dNetQuantity { get; set; }
        public decimal dStockQuantity { get; set; }
        public decimal dBalanceStockQuantity { get; set; }
        public decimal dSalePrice { get; set; }
        public decimal dCostPrice { get; set; }
        public Int64 PriceId { get; set; }
        public decimal MaximumRetailPrice { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductExpireDate { get; set; }
        [NotMapped]
        public decimal dPreNetQuantity { get; set; }
        [NotMapped]
        public decimal dIssueQuantity { get; set; }
    }
    public class CartItemListOld
    {
        public Int64 TransactionDetailId { get; set; }
        public Int64 ProductId { get; set; }
        public decimal dNetQuantity { get; set; }
        public decimal dSalePrice { get; set; }
        public decimal dCostPrice { get; set; }
        public Int64 PriceId { get; set; }
        public decimal MaximumRetailPrice { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public DateTime ProductExpireDate { get; set; }
        [NotMapped]
        public decimal dPreNetQuantity { get; set; }
        [NotMapped]
        public decimal dIssueQuantity { get; set; }
    }
}
