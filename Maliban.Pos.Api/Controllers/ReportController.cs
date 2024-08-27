using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using Dapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Maliban.Pos.Api.Controllers
{
	[Route("api/[controller]")]
	// [AllowAnonymous]
	[ApiVersion("1.0")]
	[ApiVersion("1.1")]
	public class ReportController : BaseController
	{
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
		private readonly DapperContext _dpContext;

        //private readonly ISqlDataAccess _db;
        [Obsolete]
        public ReportController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, DapperContext dpContext) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
		{
			_config = config;
			_userManager = userManager;
			_signinManager = signinManager;
			_mapper = mapper;
			_context = context;
			this.logger = logger;
			_smtpSettings = smtpSettings.Value;
			_repoWrapper = repoWrapper;
            _hostingEnviroment = hostingEnviroment;
			_dpContext = dpContext;
			//_db = db;
		}
		private async Task<List<int>> GetSaleBranchCodes(int branchLocationI, string epfNo)
		{
			List<int> branches = new List<int>();
			var mapBranch = await _context.MI_Employer_Maps.Where(em => em.EPFNo == epfNo && em.BranchLocationId == branchLocationI).Select(c => new { c.MI_Employer_EmployeeId, c.BranchLocationId }).FirstOrDefaultAsync();
            if (mapBranch != null)
            {
				var mapBranchs = await _context.MI_Employer_Maps.Where(em => em.MI_Employer_EmployeeId == mapBranch.MI_Employer_EmployeeId).Select(c => new { c.BranchLocationId }).ToListAsync();
				var employee = await _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == mapBranch.MI_Employer_EmployeeId).Select(c => new { c.Location }).FirstOrDefaultAsync();
				var branch = await _context.BranchLocations.Where(b => b.BranchCode == employee.Location).FirstOrDefaultAsync();

				branches.Add(branch.BranchLocationId);
				foreach (var item in mapBranchs)
				{
					branches.Add(item.BranchLocationId);
				}
				return branches;
			}
			branches.Add(branchLocationI);
			return branches;
		}
		/// <summary>
		/// Get Suppliers 
		/// </summary>
		/// <param name="getreportdetails"></param>
		[Authorize(Policy = "UserAccess")]
		[HttpGet("getreportdetails", Name = "GetReportDetails")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetReportDetails(string reportName, int branchLocationId, [FromQuery] UserParams userParams)
		{

			#region 
			/*       // NTLM Secured URL
				  var uri = new Uri("http://kokila-lap:5007/ReportServer2017/Pages/ReportViewer.aspx?%2FPoS%2FCustomerDeduction&rc:showbackbutton=true");
				  // Create a new Credential - note NTLM is not documented but works
				  var credentialsCache = new CredentialCache();
				  credentialsCache.Add( uri, "NTLM", new NetworkCredential("sa","Sqlserver2017"));
	  var handler = new HttpClientHandler() { Credentials = credentialsCache, PreAuthenticate = true };
	  var httpClient = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 10) };

	  var response = await httpClient.GetAsync(uri);

	  var result = await response.Content.ReadAsStringAsync(); */
			#endregion
			switch (reportName)
			{
				case "Item Wise Bin Card":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetTransactionsByItemCode";
							var parameters = new DynamicParameters();
							parameters.Add("Locationid", branchLocationId, DbType.Int32);
							parameters.Add("ProductId", userParams.Id, DbType.Int32);
							parameters.Add("Cfdate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("Ctdate", userParams.ToDate.Date, DbType.Date);
							var transactionDetails = (await connection.QueryAsync<GetBinByItemCodeDto>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(transactionDetails.ToList());
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Stock Transfer":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var getStockTransferByDateRange = "sp_GetStockTransferByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("Locationid", branchLocationId, DbType.Int32);
							parameters.Add("Cfdate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("Ctdate", userParams.ToDate.Date, DbType.Date);
							var getStockTransfers = (await connection.QueryAsync<GetStockTransferByDateRangeDto>(getStockTransferByDateRange, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(getStockTransfers.ToList());
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Purchase Order List":
                    try {
						using (var connection = _dpContext.CreateConnection())
						{
							var poListCriteria = JsonConvert.DeserializeObject<POListCriteria>(userParams.POListCriteria);
							var procedureName1 = "sp_GetPurchaseOrderListFilterByCriteria";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", Convert.ToInt32(poListCriteria.BranchLocationId), DbType.Int32);
							parameters.Add("supplierId", Convert.ToInt64(poListCriteria.SupplierId), DbType.Int64);
							parameters.Add("productId", Convert.ToInt64(poListCriteria.ProductId), DbType.Int64);
							parameters.Add("pofromDate", poListCriteria.POCreateFromDate.Value.Date, DbType.Date);
							parameters.Add("potoDate", poListCriteria.POCreateToDate.Value.Date, DbType.Date);
							parameters.Add("deliveryfromDate", poListCriteria.DeliveryFromDate.Value.Date, DbType.Date);
							parameters.Add("deliverytoDate", poListCriteria.DeliveryToDate.Value.Date, DbType.Date);
							parameters.Add("filterby", Convert.ToInt32(poListCriteria.filterby), DbType.Int32);
							var transactionDetails = (await connection.QueryAsync<PurchaseOrderListReport>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(transactionDetails.ToList());
						}
					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Product Sales":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetItemWiseSaleDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("productId", userParams.Id, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var dateWiseProductSales = (await connection.QueryAsync<SaleReportItemWiseDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(dateWiseProductSales.ToList());
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Receipt Summary":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetReciptDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var receiptReportDetails = (await connection.QueryAsync<ReciptDetailsByDateRange>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(receiptReportDetails.ToList());
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Lunch Packet Summary":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetItemSalesSummaryByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("productId", userParams.Id, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var lunchPackSalesSummary = (await connection.QueryAsync<LunchPackSalesSummary>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(lunchPackSalesSummary.ToList());
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Customer Deduction Unpaid Details":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetCustomerUnpaidDetails";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("schemeCode", userParams.SchemeCode, DbType.Int32);
							//  parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var customeriIQ111 = (await connection.QueryAsync<CustomerDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(customeriIQ111);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Customer Deduction Details":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetCustomerDeductionDetails_v1";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("schemeCode", userParams.SchemeCode, DbType.Int32);
							//  parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var customeriIQ = (await connection.QueryAsync<CustomerDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(customeriIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Item Wise Moving Nonmoving Stock":
					try
					{
						//refactor dapper
						/* var results = await _db.LoadData<ShopStockDetail, dynamic>("sp_NonMovementStock", new { branchLocationId = branchLocationId, fDate= userParams.FromDate.Date, tDate= userParams.ToDate.Date }, "DefaultConnection");
						   return Ok(results.AsList()); */
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_NonMovementStock";
							var parameters = new DynamicParameters();
							parameters.Add("@branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("@fDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("@tDate", userParams.ToDate.Date, DbType.Date);
							var stockIQ = (await connection.QueryAsync<StockNonMovement>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Stock Details":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetStockDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var stockIQ = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Stock Details":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetStockToday";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							var stockIQ = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Item Expiry Report":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetexpirationstockToday";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							var stockIQ = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Sales Details":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetSaleDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var saleIQ = (await connection.QueryAsync<SaleReportDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(saleIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Sale Details Summary":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetSaleDetailSummaryByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var saleIQ = (await connection.QueryAsync<SaleReportDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							return Ok(saleIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Sales Details By Customer":
					var data88 = from sd in _context.SaleDetails
								 join sh in _context.SaleHeaders on sd.SaleHeaderId equals sh.SaleHeaderId
								 where sh.Paid == userParams.Status && sh.EmployeeCode == userParams.Id.ToString() && sh.BranchLocationId == branchLocationId && sd.BillDate.Date >= userParams.FromDate.Date && sd.BillDate.Date <= userParams.ToDate.Date
								 select new { sd.ProductId, sd.PriceId, sd.Quantity, sd.ReturnQuantity, sd.ProductExpireDate, sd.SaleHeaderId, sd.CreateDate, sd.Paid, sh.BranchLocation.BranchCode,sh.PaymentId };
					List<SaleReportDetailByCustomer> sale7IQ = new List<SaleReportDetailByCustomer>();
					foreach (var item in data88.OrderBy(d => d.SaleHeaderId))
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						sale7IQ.Add(new SaleReportDetailByCustomer { BranchCode = item.BranchCode, ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, QTY = item.Quantity, ReturnQTY = item.ReturnQuantity, Cost = price.ModifiedUnitPrice, SaleValue = (item.Quantity - item.ReturnQuantity) * price.SalePrice, StockValue = item.Quantity * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, SaleHeaderId = item.SaleHeaderId, CreateDate = item.CreateDate, Paid = item.Paid,PaymentType=item.PaymentId==1?"Credit":"Card" });
					}
					return Ok(sale7IQ);
				case "Sales Summary Details":
					var salesSummaryDetails = from sh in _context.SaleHeaders
											  where sh.BranchLocationId == branchLocationId && sh.OrderDate.Date >= userParams.FromDate && sh.OrderDate.Date <= userParams.ToDate
											  select new { sh.EmployeeCode, sh.OrderDate, sh.TotalAmount, sh.CustomerReturnAmount, sh.SaleHeaderId, sh.Paid, sh.PaymentDate, sh.BranchLocation.BranchCode, PaymentType=sh.PaymentId==1?"Credit":"Cash" };
					List<SaleSummaryReportDetail> salesIQ = new List<SaleSummaryReportDetail>();
					foreach (var item in salesSummaryDetails.OrderBy(d => d.SaleHeaderId))
					{
						salesIQ.Add(new SaleSummaryReportDetail
						{
							EmployeeCode = item.EmployeeCode,
							OrderDate = item.OrderDate,
							TotalAmount = item.TotalAmount,
							CustomerReturnAmount = item.CustomerReturnAmount,
							SaleHeaderId = item.SaleHeaderId,
							Paid = item.Paid,
							PaymentDate = item.PaymentDate,
							BranchCode = item.BranchCode,
							PaymentType=item.PaymentType
							
						});
					}
					return Ok(salesIQ);
				case "Sales Summary By Customer":

					List<int> branches = await GetSaleBranchCodes(branchLocationId, userParams.Id.ToString());

					var salesSummaryDetailsByCustomer = from sh in _context.SaleHeaders
														where sh.EmployeeCode == userParams.Id.ToString() && branches.Contains(sh.BranchLocationId) && sh.OrderDate.Date >= userParams.FromDate && sh.OrderDate.Date <= userParams.ToDate
														select new { sh.EmployeeCode, sh.OrderDate, sh.TotalAmount, sh.CustomerReturnAmount, sh.SaleHeaderId, sh.Paid, sh.PaymentDate, sh.BranchLocation.BranchCode,sh.PaymentId };
					List<SaleSummaryReportDetail> salesIQByCustomer = new List<SaleSummaryReportDetail>();
					foreach (var item in salesSummaryDetailsByCustomer.OrderBy(d => d.SaleHeaderId))
					{
						salesIQByCustomer.Add(new SaleSummaryReportDetail
						{
							EmployeeCode = item.EmployeeCode,
							OrderDate = item.OrderDate,
							TotalAmount = item.TotalAmount,
							CustomerReturnAmount = item.CustomerReturnAmount,
							SaleHeaderId = item.SaleHeaderId,
							Paid = item.Paid,
							PaymentDate = item.PaymentDate,
							BranchCode = item.BranchCode,
							PaymentType=item.PaymentId==1? "Credit":"Cash"

						});
					}
					return Ok(salesIQByCustomer);
				case "Stock Adjustment":
					if (userParams.Id != null)
					{
						var data8 = from ph in _context.StockAdjustmentHeaders
									join pd in _context.StockAdjustmentDetails on ph.StockAdjustmentHeaderId equals pd.StockAdjustmentHeaderId
									where ph.BranchLocationId == branchLocationId && pd.StockAdjustmentHeaderId == userParams.Id
									group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
									select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.AdjustmentQuantity) };
						List<PurchaseReturnReportDetail> prIQId = new List<PurchaseReturnReportDetail>();
						foreach (var item in data8)
						{
							var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
							var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
							prIQId.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate });
						}
						return Ok(prIQId);
					}
					var data7 = from ph in _context.StockAdjustmentHeaders
								join pd in _context.StockAdjustmentDetails on ph.StockAdjustmentHeaderId equals pd.StockAdjustmentHeaderId
								where ph.BranchLocationId == branchLocationId && pd.CreateDate >= userParams.FromDate && pd.CreateDate <= userParams.ToDate
								group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
								select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.AdjustmentQuantity) };
					List<PurchaseReturnReportDetail> stIQ = new List<PurchaseReturnReportDetail>();
					foreach (var item in data7)
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						stIQ.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, DisAmount = item.Sum * price.DiscountPrice, Dis = price.DiscountPrice });
					}
					return Ok(stIQ);
				case "Purchase Order":
					break;
				case "Purchase Return":
					if (userParams.Id != null)
					{
						var data4 = from ph in _context.PurchaseReturnHeaders
									join pd in _context.PurchaseReturnDetails on ph.PurchaseReturnHeaderId equals pd.PurchaseReturnHeaderId
									where ph.BranchLocationId == branchLocationId && pd.PurchaseReturnHeaderId == userParams.Id
									group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
									select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity) };
						List<PurchaseReturnReportDetail> prIQId = new List<PurchaseReturnReportDetail>();
						foreach (var item in data4)
						{
							var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
							var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
							prIQId.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate });
						}
						return Ok(prIQId);
					}
					var data3 = from ph in _context.PurchaseReturnHeaders
								join pd in _context.PurchaseReturnDetails on ph.PurchaseReturnHeaderId equals pd.PurchaseReturnHeaderId
								where ph.BranchLocationId == branchLocationId && pd.CreateDate >= userParams.FromDate && pd.CreateDate <= userParams.ToDate
								group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
								select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity) };
					List<PurchaseReturnReportDetail> prIQ = new List<PurchaseReturnReportDetail>();
					foreach (var item in data3)
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						prIQ.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, DisAmount = item.Sum * price.DiscountPrice, Dis = price.DiscountPrice });
					}
					return Ok(prIQ);
				case "Sale Return":
					if (userParams.Id != null)
					{
						var data5 = from ph in _context.CustomerReturnHeaders
									join pd in _context.CustomerReturnDetails on ph.CustomerReturnHeaderId equals pd.CustomerReturnHeaderId
									where ph.BranchLocationId == branchLocationId && pd.CustomerReturnHeaderId == userParams.Id
									group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
									select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity), Sum1 = g.Sum(x => x.SaleQuantity) };
						List<CustomerReturnReportDetail> prIQId = new List<CustomerReturnReportDetail>();
						foreach (var item in data5)
						{
							var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
							var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
							prIQId.Add(new CustomerReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, SaleQuantity = item.Sum1, SalePrice = price.SalePrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate });
						}
						return Ok(prIQId);
					}
					var data6 = from ph in _context.CustomerReturnHeaders
								join pd in _context.CustomerReturnDetails on ph.CustomerReturnHeaderId equals pd.CustomerReturnHeaderId
								where ph.BranchLocationId == branchLocationId && pd.CreateDate >= userParams.FromDate && pd.CreateDate <= userParams.ToDate
								group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
								select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity), Sum1 = g.Sum(x => x.SaleQuantity) };
					List<CustomerReturnReportDetail> srIQ = new List<CustomerReturnReportDetail>();
					foreach (var item in data6)
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						srIQ.Add(new CustomerReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, SaleQuantity = item.Sum1, SalePrice = price.SalePrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, DisAmount = item.Sum * price.DiscountPrice, Dis = price.DiscountPrice });
					}
					return Ok(srIQ);
				case "Supplier List":
					var supplierList = await _context.Suppliers.Where(s => s.BranchLocationId == userParams.Id).Include(t => t.Title).Include(t => t.City).Include(t => t.District).Include(t => t.Provinces).ToListAsync();
					return Ok(supplierList);
				case "Order Details By ID":
					break;
				default:
					return NotFound();
			}
			return NoContent();
		}
		//https://blog.elmah.io/export-data-to-excel-with-asp-net-core/
		/// <summary>
		/// Get Suppliers 
		/// </summary>
		/// <param name="getexcelreport"></param>
		[Authorize(Policy = "UserAccess")]
		[HttpGet("getexcelreport", Name = "GetExcelReport")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetExcelReport(string reportName, int branchLocationId, [FromQuery] UserParams userParams)
		{
			var branch = await _context.BranchLocations.FindAsync(branchLocationId);
			switch (reportName)
			{
				case "Customer Deduction Unpaid Details":
					List<CustomerDetail> customerIQ11 = new List<CustomerDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetCustomerUnpaidDetails";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("schemeCode", userParams.SchemeCode, DbType.Int32);
							//  parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							customerIQ11 = (await connection.QueryAsync<CustomerDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//  return Ok(customerIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Customer Deduction Unpaid");
						var currentRow = 6;
						var range = worksheet.Range("A1:E1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:E2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:E3");
						range2.Value = "Customer Deduction Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:E4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Value = "Employee Number";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "FullName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Monthly Credit Limit";
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Value = "Deduction";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var customer in customerIQ11)
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = customer.CustomerId;
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = customer.FullName;
							worksheet.Cell(currentRow, 4).Value = customer.BasicSalary.ToString("C", siLk);
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//    worksheet.Cell(currentRow, 5).Value = customer.Total.ToString("C", siLk);
							worksheet.Cell(currentRow, 5).Value = customer.Total;
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDeductionUnpaid"+DateTime.Now+".xlsx");
						}
					};
				case "Customer Deduction Details":
					List<CustomerDetail> customerIQ1 = new List<CustomerDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetCustomerDeductionDetails_v1";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("schemeCode", userParams.SchemeCode, DbType.Int32);
							//  parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							customerIQ1 = (await connection.QueryAsync<CustomerDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//  return Ok(customerIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Customer Deduction Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:E1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:E2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:E3");
						range2.Value = "Customer Deduction Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:E4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Value = "Employee Number";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "FullName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Monthly Credit Limit";
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Value = "Deduction";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var customer in customerIQ1)
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = customer.CustomerId;
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = customer.FullName;
							worksheet.Cell(currentRow, 4).Value = customer.BasicSalary.ToString("C", siLk);
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//    worksheet.Cell(currentRow, 5).Value = customer.Total.ToString("C", siLk);
							worksheet.Cell(currentRow, 5).Value = customer.Total;
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDeductionDetails"+DateTime.Now+".xlsx");
						}
					};
				case "Sale Return":
					if (userParams.Id != null)
					{
						var data5 = from ph in _context.CustomerReturnHeaders
									join pd in _context.CustomerReturnDetails on ph.CustomerReturnHeaderId equals pd.CustomerReturnHeaderId
									where ph.BranchLocationId == branchLocationId && pd.CustomerReturnHeaderId == userParams.Id
									group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
									select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity), Sum1 = g.Sum(x => x.SaleQuantity) };
						var cr = await _context.CustomerReturnHeaders.Where(ph => ph.CustomerReturnHeaderId == userParams.Id).Include(s => s.BranchLocation).Include(r => r.ApplicationUser).Include(e => e.MI_Employer_Employee).SingleAsync<CustomerReturnHeader>();
						List<CustomerReturnReportDetail> prIQId = new List<CustomerReturnReportDetail>();
						foreach (var item in data5)
						{
							var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
							var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
							prIQId.Add(new CustomerReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, SaleQuantity = item.Sum1, SalePrice = price.SalePrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.SalePrice, ProductExpireDate = item.ProductExpireDate });
						}
						using (var workbook = new XLWorkbook())
						{
							var worksheet = workbook.Worksheets.Add("Sale Return Details");
							var currentRow = 9;
							var range = worksheet.Range("A1:j1");
							range.Value = $"{branch.BranchName}";
							range.Merge().Style.Font.SetBold().Font.FontSize = 18;
							range.Merge().Style.Font.FontColor = XLColor.Black;
							range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range.Style.Font.Bold = true;
							var range3 = worksheet.Range("A2:j2");
							range3.Value = $"No: {branch.Address}.";
							range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
							range3.Merge().Style.Font.FontColor = XLColor.Black;
							range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range3.Style.Font.Bold = true;
							var range2 = worksheet.Range("A3:j3");
							range2.Value = "Sale Return Details";
							range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
							range2.Merge().Style.Font.FontColor = XLColor.White;
							range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
							range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							var range1 = worksheet.Range("A4:j4");
							range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")}";
							range1.Merge().Style.Font.FontColor = XLColor.Black;
							range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range1.Style.Font.Bold = true;
							var range4 = worksheet.Range("A5:B5");
							range4.Value = $"SRN NO:  {cr.CustomerReturnHeaderId}";
							range4.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range4.Merge().Style.Font.FontColor = XLColor.Black;
							range4.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range7 = worksheet.Range("F7:j7");
							range7.Value = $"Employee Name:  {cr.MI_Employer_Employee.FullName}";
							range7.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range7.Merge().Style.Font.FontColor = XLColor.Black;
							range7.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range5 = worksheet.Range("A6:C6");
							range5.Value = $"SRN Description:  {cr.CustomerReturnNote}  ";
							range5.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range5.Merge().Style.Font.FontColor = XLColor.Black;
							range5.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range6 = worksheet.Range("A7:B7");
							range6.Value = $"SRN Date:  {cr.CustomerReturnDate.Date}";
							range6.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range6.Merge().Style.Font.FontColor = XLColor.Black;
							range6.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							//    worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
							//      worksheet.Cell(1, 3).MergedRange();
							worksheet.Cell(currentRow, 1).Value = "S No:";
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 2).Value = "Item Code";
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
							worksheet.Column("A").Width = 10;
							worksheet.Column("B").Width = 20;
							worksheet.Column("C").Width = 40;
							worksheet.Column("D").Width = 15;
							worksheet.Column("E").Width = 15;
							worksheet.Column("F").Width = 15;
							worksheet.Column("G").Width = 15;
							worksheet.Column("H").Width = 15;
							worksheet.Column("i").Width = 15;
							worksheet.Column("j").Width = 15;
							worksheet.Cell(currentRow, 3).Value = "ProductName";
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 4).Value = "Sale QTY";
							worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 5).Value = "Return QTY";
							worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Value = "Return Price";
							worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 7).Value = "Gross";
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 8).Value = "Dis%";
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 9).Value = "Dis Amount";
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 10).Value = "Amount";
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
							CultureInfo siLk = new CultureInfo("si-LK");
							int count = 1;
							foreach (var sale in prIQId)
							{
								currentRow++;
								worksheet.Cell(currentRow, 1).Value = count;
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Value = sale.ItemCode.ToString();
								worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Value = sale.ProductName;
								worksheet.Cell(currentRow, 4).Value = sale.SaleQuantity;
								worksheet.Cell(currentRow, 5).Value = sale.ReturnQTY;
								//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
								//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
								//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
								worksheet.Cell(currentRow, 6).Value = sale.ReturnPrice;
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
								worksheet.Cell(currentRow, 7).Value = sale.StockValue;
								worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Value = sale.Dis;
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 9).Value = sale.DisAmount;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = sale.StockValue;
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								count++;
							}
							using (var stream = new MemoryStream())
							{
								workbook.SaveAs(stream);
								var content = stream.ToArray();
								return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SaleReturnDetails"+DateTime.Now+".xlsx");
							}
						}
					};
					return File("", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDeductionDetails"+DateTime.Now+".xlsx");
				case "Customer Deduction Process":
					List<CustomerDetail> customerpIQ = new List<CustomerDetail>();
					var param = new SqlParameter[] {
						new SqlParameter() {
							ParameterName = "@branchLocationId",
							SqlDbType =  System.Data.SqlDbType.Int,
							Direction = System.Data.ParameterDirection.Input,
							Value = branchLocationId
						},
						new SqlParameter() {
							ParameterName = "@fromDate",
							SqlDbType =  System.Data.SqlDbType.Date,
							Direction = System.Data.ParameterDirection.Input,
							Value = userParams.FromDate.Date
						},
								new SqlParameter() {
							ParameterName = "@toDate",
							SqlDbType =  System.Data.SqlDbType.Date,
							Direction = System.Data.ParameterDirection.Input,
							Value = userParams.ToDate.Date
						},
								new SqlParameter() {
							ParameterName = "@UserID",
							SqlDbType =  System.Data.SqlDbType.Int,
							Direction = System.Data.ParameterDirection.Input,
							Value = userParams.Id
						},
					 new SqlParameter() {
							ParameterName = "@category",
							SqlDbType =  System.Data.SqlDbType.Int,
							Direction = System.Data.ParameterDirection.Input,
							Value = userParams.SchemeCode
						}
						};
					using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
					{
						SqlCommand sqlCommand = new SqlCommand("[dbo].[sp_CustomerDeductionProcess_V1]", connection);
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandTimeout = 100;
						sqlCommand.Parameters.Clear();
						sqlCommand.Parameters.AddRange(param);
						await connection.OpenAsync();
						using (var sqlDataReader = await sqlCommand.ExecuteReaderAsync())
						{
							while (await sqlDataReader.ReadAsync())
							{
								customerpIQ.Add(new CustomerDetail
								{
									FullName = Convert.ToString(sqlDataReader["FullName"]),
									BasicSalary = Convert.ToDecimal(sqlDataReader["BasicSalary"]),
									CustomerId = Convert.ToString(sqlDataReader["CustomerId"]),
									CustomerNumber = Convert.ToString(sqlDataReader["CustomerNumber"]),
									Total = Convert.ToDecimal(sqlDataReader["Total"]),
								});
							}
							sqlDataReader.Close();
							connection.Close();
						}

					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Customer Deduction Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:E1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:E2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:E3");
						range2.Value = "Customer Deduction Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:E4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Value = "Employee Number";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "FullName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Monthly Credit Limit";
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Value = "Deduction";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var customer in customerpIQ)
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = customer.CustomerId;
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = customer.FullName;
							worksheet.Cell(currentRow, 4).Value = customer.BasicSalary.ToString("C", siLk);
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//    worksheet.Cell(currentRow, 5).Value = customer.Total.ToString("C", siLk);
							worksheet.Cell(currentRow, 5).Value = customer.Total;
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDeductionDetails"+DateTime.Now+".xlsx");
						}
					};
				case "Date Wise Stock Details":
					List<ShopStockDetail> stockIQ = new List<ShopStockDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetStockDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							stockIQ = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Date Wise Stock Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:J1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:J2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:J3");
						range2.Value = "Date Wise Stock Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:J4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Item Code";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "ProductName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Value = "QTY";
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Value = "Cost";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Retail Price";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "CostValue";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "UnitPrice";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "SaleValue";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var stock in stockIQ.Where(c => c.QTY > 0))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = stock.ItemCode.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = stock.ProductName;
							worksheet.Cell(currentRow, 4).Value = stock.ProductExpireDate;
							worksheet.Cell(currentRow, 5).Value = stock.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Value = stock.Cost;
							worksheet.Cell(currentRow, 7).Value = stock.MaximumRetailPrice;
							worksheet.Cell(currentRow, 8).Value = stock.CostValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 9).Value = stock.UnitPrice;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 10).Value = stock.SaleValue;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"DateWiseStockDetails{userParams.ToDate.Date}.xlsx");
						}
					};
				case "Stock Details":
					List<ShopStockDetail> stockIQ27 = new List<ShopStockDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetStockToday";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							stockIQ27 = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Stock Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:J1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:J2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:J3");
						range2.Value = "Stock Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:J4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Item Code";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "ProductName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Value = "QTY";
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Value = "Cost";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Retail Price";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "CostValue";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "UnitPrice";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "SaleValue";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var stock in stockIQ27.Where(c => c.QTY > 0))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = stock.ItemCode.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = stock.ProductName;
							worksheet.Cell(currentRow, 4).Value = stock.ProductExpireDate;
							worksheet.Cell(currentRow, 5).Value = stock.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Value = stock.Cost;
							worksheet.Cell(currentRow, 7).Value = stock.MaximumRetailPrice;
							worksheet.Cell(currentRow, 8).Value = stock.CostValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 9).Value = stock.UnitPrice;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 10).Value = stock.SaleValue;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockDetails"+DateTime.Now+".xlsx");
						}
					};
				case "Item Expiry Report":
					List<ShopStockDetail> stockIQ277 = new List<ShopStockDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetexpirationstockToday";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							stockIQ277 = (await connection.QueryAsync<ShopStockDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Expiry Item Report Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:J1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:J2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:J3");
						range2.Value = "Stock Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:J4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Item Code";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "ProductName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Value = "QTY";
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Value = "Cost";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Retail Price";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "CostValue";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "UnitPrice";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "SaleValue";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var stock in stockIQ277.Where(c => c.QTY > 0))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = stock.ItemCode.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = stock.ProductName;
							worksheet.Cell(currentRow, 4).Value = stock.ProductExpireDate;
							worksheet.Cell(currentRow, 5).Value = stock.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Value = stock.Cost;
							worksheet.Cell(currentRow, 7).Value = stock.MaximumRetailPrice;
							worksheet.Cell(currentRow, 8).Value = stock.CostValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 9).Value = stock.UnitPrice;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 10).Value = stock.SaleValue;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Expiry Item Report Details" + DateTime.Now + ".xlsx");
						}
					};
				case "Date Wise Sale Details Summary":
					List<SaleReportDetail> saleIQ28 = new List<SaleReportDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetSaleDetailSummaryByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							saleIQ28 = (await connection.QueryAsync<SaleReportDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							// return Ok(saleIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("DateWiseSaleDetailsSummary");
						var currentRow = 6;
						var range = worksheet.Range("A1:J1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:J2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:J3");
						range2.Value = "Date Wise SaleD etails Summary";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:J4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Item Code";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 30;
						worksheet.Column("D").Width = 20;
						worksheet.Column("E").Width = 8;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
                        worksheet.Column("J").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "ProductName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Value = "QTY";
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(currentRow, 6).Value = "Retail Price";
                        worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;



                        worksheet.Cell(currentRow, 7).Value = "SalePrice";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "SaleValue";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "Cost";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "Stock Value";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in saleIQ28.OrderBy(d => d.SaleHeaderId))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.ItemCode.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.ProductName;
							worksheet.Cell(currentRow, 4).Value = sale.ProductExpireDate.Date;
							worksheet.Cell(currentRow, 5).Value = sale.QTY;
                            //   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
                            //    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
                            //    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            //worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
                            worksheet.Cell(currentRow, 6).Value = sale.MaximumRetailPrice;
                            worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            worksheet.Cell(currentRow, 7).Value = sale.CashPrice;
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = sale.SaleValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Value = sale.Cost;
							worksheet.Cell(currentRow, 10).Value = sale.StockValue;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DateWiseSaleDetailsSummary"+DateTime.Now+".xlsx");
						}
					};
				case "Sales Details":
					List<SaleReportDetail> saleIQ = new List<SaleReportDetail>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetSaleDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("paid", userParams.Status, DbType.Boolean);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							saleIQ = (await connection.QueryAsync<SaleReportDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							// return Ok(saleIQ);
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Sales Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:L1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:L2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:L3");
						range2.Value = "Sales Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:L4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Bill Number:";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 3).Value = "Bill Date:";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Item Code";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 20;
						worksheet.Column("D").Width = 20;
						worksheet.Column("E").Width = 40;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Column("K").Width = 15;
						worksheet.Column("L").Width = 15;
						worksheet.Cell(currentRow, 5).Value = "ProductName";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "QTY";
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Value = "SalePrice";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "SaleValue";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "Cost";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 11).Value = "Stock Value";
						worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 12).Value = "Payment";
						worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in saleIQ.OrderBy(d => d.SaleHeaderId))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.SaleHeaderId.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Number);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.CreateDate;
							worksheet.Cell(currentRow, 3).SetDataType(XLDataType.DateTime);
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 4).Value = sale.ItemCode.ToString();
							worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 5).Value = sale.ProductName;
							worksheet.Cell(currentRow, 6).Value = sale.ProductExpireDate.Date;
							worksheet.Cell(currentRow, 7).Value = sale.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = sale.CashPrice;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 9).Value = sale.SaleValue;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 10).Value = sale.Cost;
							worksheet.Cell(currentRow, 11).Value = sale.StockValue;
							worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 12).Value = sale.PaymentType;
							worksheet.Cell(currentRow, 12).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SaleDetails" + DateTime.Now + ".xlsx");
						}
					};
				case "Sales Summary Details":
					var salesSummaryDetails = from sh in _context.SaleHeaders
											  where sh.BranchLocationId == branchLocationId && sh.OrderDate.Date >= userParams.FromDate && sh.OrderDate.Date <= userParams.ToDate
											  select new { sh.EmployeeCode, sh.OrderDate, sh.TotalAmount, sh.CustomerReturnAmount, sh.SaleHeaderId, sh.Paid, sh.PaymentDate };
					List<SaleSummaryReportDetail> salesIQ = new List<SaleSummaryReportDetail>();
					foreach (var item in salesSummaryDetails.OrderBy(d => d.SaleHeaderId))
					{
						salesIQ.Add(new SaleSummaryReportDetail
						{
							EmployeeCode = item.EmployeeCode,
							OrderDate = item.OrderDate,
							TotalAmount = item.TotalAmount,
							CustomerReturnAmount = item.CustomerReturnAmount,
							SaleHeaderId = item.SaleHeaderId,
							Paid = item.Paid,
							PaymentDate = item.PaymentDate
						});
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Sales Summary Details");
						var currentRow = 6;
						var range = worksheet.Range("A1:I1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:I2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:I3");
						range2.Value = "Sales Summary Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:I4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Bill Number:";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 3).Value = "Bill Date:";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Employee Code";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 20;
						worksheet.Column("D").Width = 20;
						worksheet.Column("E").Width = 40;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Cell(currentRow, 5).Value = "Paid";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "Payment Date";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Return Amount";
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Value = "Sale Amount";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "Total Amount";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in salesIQ.OrderBy(d => d.SaleHeaderId))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.SaleHeaderId.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Number);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.OrderDate;
							worksheet.Cell(currentRow, 3).SetDataType(XLDataType.DateTime);
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 4).Value = sale.EmployeeCode;
							worksheet.Cell(currentRow, 5).Value = sale.Paid;
							worksheet.Cell(currentRow, 6).Value = sale.PaymentDate;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 7).Value = sale.CustomerReturnAmount;
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = sale.TotalAmount;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Value = sale.TotalAmount - sale.CustomerReturnAmount;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesSummaryDetails"+DateTime.Now+".xlsx");
						}
					};
				case "Purchase Details":
					break;
				case "Purchase Details By ID":
					break;
				case "Purchase Return":
					if (userParams.Id != null)
					{
						var data4 = from ph in _context.PurchaseReturnHeaders
									join pd in _context.PurchaseReturnDetails on ph.PurchaseReturnHeaderId equals pd.PurchaseReturnHeaderId
									where ph.BranchLocationId == branchLocationId && pd.PurchaseReturnHeaderId == userParams.Id
									group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
									select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity) };
						var pr = await _context.PurchaseReturnHeaders.Where(ph => ph.PurchaseReturnHeaderId == userParams.Id).Include(s => s.Supplier).Include(r => r.ReturnReason).SingleAsync<PurchaseReturnHeader>();
						List<PurchaseReturnReportDetail> prIQId = new List<PurchaseReturnReportDetail>();
						foreach (var item in data4)
						{
							var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
							var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
							prIQId.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate });
						}
						using (var workbook = new XLWorkbook())
						{
							var worksheet = workbook.Worksheets.Add("Purchase Return Details");
							var currentRow = 9;
							var range = worksheet.Range("A1:i1");
							range.Value = $"{branch.BranchName}";
							range.Merge().Style.Font.SetBold().Font.FontSize = 18;
							range.Merge().Style.Font.FontColor = XLColor.Black;
							range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range.Style.Font.Bold = true;
							var range3 = worksheet.Range("A2:i2");
							range3.Value = $"No: {branch.Address}.";
							range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
							range3.Merge().Style.Font.FontColor = XLColor.Black;
							range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range3.Style.Font.Bold = true;
							var range2 = worksheet.Range("A3:i3");
							range2.Value = "Purchase Return Details";
							range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
							range2.Merge().Style.Font.FontColor = XLColor.White;
							range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
							range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							var range1 = worksheet.Range("A4:i4");
							range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")}";
							range1.Merge().Style.Font.FontColor = XLColor.Black;
							range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							range1.Style.Font.Bold = true;
							var range4 = worksheet.Range("A5:B5");
							range4.Value = $"PRN NO:  {pr.PurchaseReturnHeaderId}";
							range4.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range4.Merge().Style.Font.FontColor = XLColor.Black;
							range4.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range7 = worksheet.Range("F7:i7");
							range7.Value = $"Supplier Name:  {pr.Supplier.SupplierName}";
							range7.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range7.Merge().Style.Font.FontColor = XLColor.Black;
							range7.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range5 = worksheet.Range("A6:C6");
							range5.Value = $"PRN Description:  {pr.ReturnReason.ReturnDescription}  ";
							range5.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range5.Merge().Style.Font.FontColor = XLColor.Black;
							range5.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							var range6 = worksheet.Range("A7:B7");
							range6.Value = $"PRN Date:  {pr.PurchaseReturnDate.Date}";
							range6.Merge().Style.Font.SetBold().Font.FontSize = 11;
							range6.Merge().Style.Font.FontColor = XLColor.Black;
							range6.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
							//    worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
							//      worksheet.Cell(1, 3).MergedRange();
							worksheet.Cell(currentRow, 1).Value = "S No:";
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 2).Value = "Item Code";
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
							worksheet.Column("A").Width = 10;
							worksheet.Column("B").Width = 20;
							worksheet.Column("C").Width = 40;
							worksheet.Column("D").Width = 15;
							worksheet.Column("E").Width = 15;
							worksheet.Column("F").Width = 15;
							worksheet.Column("G").Width = 15;
							worksheet.Column("H").Width = 15;
							worksheet.Column("i").Width = 15;
							worksheet.Cell(currentRow, 3).Value = "ProductName";
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 4).Value = "Return QTY";
							worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 5).Value = "Return Price";
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 6).Value = "Gross";
							worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 7).Value = "Dis%";
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 8).Value = "Dis Amount";
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
							worksheet.Cell(currentRow, 9).Value = "Amount";
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
							CultureInfo siLk = new CultureInfo("si-LK");
							int count = 1;
							foreach (var sale in prIQId)
							{
								currentRow++;
								worksheet.Cell(currentRow, 1).Value = count;
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Value = sale.ItemCode.ToString();
								worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Value = sale.ProductName;
								worksheet.Cell(currentRow, 4).Value = sale.ReturnQTY;
								//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
								//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
								//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
								worksheet.Cell(currentRow, 5).Value = sale.ReturnPrice;
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
								worksheet.Cell(currentRow, 6).Value = sale.StockValue;
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 7).Value = sale.Dis;
								worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Value = sale.DisAmount;
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 9).Value = sale.StockValue;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								count++;
							}
							using (var stream = new MemoryStream())
							{
								workbook.SaveAs(stream);
								var content = stream.ToArray();
								return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PurchaseReturnDetails.xlsx");
							}
						};
					}
					var data3 = from ph in _context.PurchaseReturnHeaders
								join pd in _context.PurchaseReturnDetails on ph.PurchaseReturnHeaderId equals pd.PurchaseReturnHeaderId
								where ph.BranchLocationId == branchLocationId && pd.CreateDate >= userParams.FromDate && pd.CreateDate <= userParams.ToDate
								group pd by new { pd.PriceId, pd.ProductId, pd.ProductExpireDate } into g
								select new { g.Key.ProductId, g.Key.PriceId, g.Key.ProductExpireDate, Sum = g.Sum(x => x.ReturnQuantity) };
					List<PurchaseReturnReportDetail> prIQ = new List<PurchaseReturnReportDetail>();
					foreach (var item in data3)
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						prIQ.Add(new PurchaseReturnReportDetail { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, ReturnQTY = item.Sum, ReturnPrice = price.ModifiedUnitPrice, SaleValue = item.Sum * price.SalePrice, StockValue = item.Sum * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, DisAmount = item.Sum * price.DiscountPrice, Dis = price.DiscountPrice });
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Purchase Return");
						var currentRow = 9;
						var range = worksheet.Range("A1:H1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:H2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:H3");
						range2.Value = "Purchase Return Details";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:H4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						var range4 = worksheet.Range("A5:B5");
						range4.Value = "PRN NO:  ";
						range4.Merge().Style.Font.SetBold().Font.FontSize = 11;
						range4.Merge().Style.Font.FontColor = XLColor.Black;
						range4.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
						var range5 = worksheet.Range("A6:B6");
						range5.Value = "PRN Description:  ";
						range5.Merge().Style.Font.SetBold().Font.FontSize = 11;
						range5.Merge().Style.Font.FontColor = XLColor.Black;
						range5.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
						var range6 = worksheet.Range("A7:B7");
						range6.Value = "PRN Date:  ";
						range6.Merge().Style.Font.SetBold().Font.FontSize = 11;
						range6.Merge().Style.Font.FontColor = XLColor.Black;
						range6.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
						//  worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						//  worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Item Code";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Cell(currentRow, 3).Value = "ProductName";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "QTY";
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Value = "SalePrice";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "SaleValue";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Cost";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "Stock Value";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in prIQ)
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.ItemCode.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.ProductName;
							worksheet.Cell(currentRow, 4).Value = sale.ReturnQTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 5).Value = sale.CashPrice;
							worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 6).Value = sale.SaleValue;
							worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 7).Value = sale.ReturnPrice;
							worksheet.Cell(currentRow, 8).Value = sale.StockValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PurchaseReturnDetails.xlsx");
						}
					};
				case "Supplier List":
					var supplierList = await _context.Suppliers.Where(s => s.BranchLocationId == userParams.Id).Include(t => t.Title).Include(t => t.City).Include(t => t.District).Include(t => t.Provinces).ToListAsync();
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Suppliers List");
						var currentRow = 6;
						var range = worksheet.Range("A1:M1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:M2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:M3");
						range2.Value = "Suppliers List";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:M4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")}";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Supplier Name:";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 3).Value = "Address:";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Title";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 20;
						worksheet.Column("D").Width = 20;
						worksheet.Column("E").Width = 40;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Cell(currentRow, 5).Value = "First Name";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "Last Name";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Email";
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Value = "Phone";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "Mobile";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "Fax";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 11).Value = "City";
						worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 12).Value = "District";
						worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 13).Value = "Province";
						worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in supplierList.OrderBy(d => d.SupplierName))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.SupplierName.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.Address1;
							worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 4).Value = sale.Title?.TitleName;
							worksheet.Cell(currentRow, 5).Value = sale.FirstName;
							worksheet.Cell(currentRow, 6).Value = sale.LastName;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 7).Value = sale.Email;
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = sale.Phone;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Value = sale.Mobile;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 10).Value = sale.Fax;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 11).Value = sale.City?.CityNameEn;
							worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 12).Value = sale.District?.DistrictNameEn;
							worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 13).Value = sale.Provinces?.ProvinceNameEn;
							worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SuppliersList.xlsx");
						}
					};
				case "Sales Details By Customer":
					var data88 = from sd in _context.SaleDetails
								 join sh in _context.SaleHeaders on sd.SaleHeaderId equals sh.SaleHeaderId
								 where sh.Paid == userParams.Status && sh.EmployeeCode == userParams.Id.ToString() && sh.BranchLocationId == branchLocationId && sd.BillDate.Date >= userParams.FromDate.Date && sd.BillDate.Date <= userParams.ToDate.Date
								 select new { sd.ProductId, sd.PriceId, sd.Quantity, sd.ReturnQuantity, sd.ProductExpireDate, sd.SaleHeaderId, sd.CreateDate, sd.Paid, sh.EmployeeCode,sh.PaymentId };
					List<SaleReportDetailByCustomer> sale78IQ = new List<SaleReportDetailByCustomer>();
					foreach (var item in data88.OrderBy(d => d.SaleHeaderId))
					{
						var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == item.ProductId);
						var price = await _context.Prices.SingleOrDefaultAsync(p => p.PriceId == item.PriceId);
						sale78IQ.Add(new SaleReportDetailByCustomer { ItemCode = product.Barcode, ProductName = product.ProductName, CashPrice = price.SalePrice, QTY = item.Quantity, ReturnQTY = item.ReturnQuantity, Cost = price.ModifiedUnitPrice, SaleValue = (item.Quantity - item.ReturnQuantity) * price.SalePrice, StockValue = (item.Quantity) * price.ModifiedUnitPrice, ProductExpireDate = item.ProductExpireDate, SaleHeaderId = item.SaleHeaderId, CreateDate = item.CreateDate, Paid = item.Paid,PaymentType=item.PaymentId==1?"Credit":"Cash" });
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("Sales Details By Customer");
						var currentRow = 6;
						var range = worksheet.Range("A1:K1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:K2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:K3");
						range2.Value = "Sales Details By Employee No :" + userParams.Id.ToString();
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:K4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Bill Number:";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 3).Value = "Bill Date:";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 4).Value = "Item Code";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 20;
						worksheet.Column("D").Width = 20;
						worksheet.Column("E").Width = 40;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Column("K").Width = 15;
						worksheet.Cell(currentRow, 5).Value = "Item Name";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "Quantity";
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 7).Value = "Return Quantity";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "paid";
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Value = "Payment Type";
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Value = "Price";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 11).Value = "Total Amount";
						worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
						worksheet.Cell(sale78IQ.Count + 7, 11).Value = sale78IQ.Sum(s => s.SaleValue);
						worksheet.Cell(sale78IQ.Count + 7, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var sale in sale78IQ.OrderBy(d => d.SaleHeaderId))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = sale.SaleHeaderId.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Number);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 3).Value = sale.CreateDate;
							worksheet.Cell(currentRow, 3).SetDataType(XLDataType.DateTime);
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 4).Value = sale.ItemCode;
							worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 5).Value = sale.ProductName;
							worksheet.Cell(currentRow, 6).Value = sale.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							// worksheet.Cell(currentRow, 7).Value = stock.UnitPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 7).Value = sale.ReturnQTY;
							worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = sale.Paid;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Value = sale.PaymentType;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 10).Value = sale.CashPrice;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 11).Value = sale.SaleValue;
							worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesDetailsByCustomer.xlsx");
						}
					};
				case "Stock Transfer":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var getStockTransferByDateRange = "sp_GetStockTransferByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("Locationid", branchLocationId, DbType.Int32);
							parameters.Add("Cfdate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("Ctdate", userParams.ToDate.Date, DbType.Date);
							var getexcelStockTransfers = (await connection.QueryAsync<GetStockTransferByDateRangeDto>(getStockTransferByDateRange, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							using (var workbook = new XLWorkbook())
							{
								var worksheet = workbook.Worksheets.Add("Stock Transfer");
								var currentRow = 6;
								var range = worksheet.Range("A1:R1");
								range.Value = $"{branch.BranchName}";
								range.Merge().Style.Font.SetBold().Font.FontSize = 18;
								range.Merge().Style.Font.FontColor = XLColor.Black;
								range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range.Style.Font.Bold = true;
								var range3 = worksheet.Range("A2:R2");
								range3.Value = $"No: {branch.Address}.";
								range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
								range3.Merge().Style.Font.FontColor = XLColor.Black;
								range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range3.Style.Font.Bold = true;
								var range2 = worksheet.Range("A3:R3");
								range2.Value = "Stock Transfer :" + userParams.Id.ToString();
								range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
								range2.Merge().Style.Font.FontColor = XLColor.White;
								range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
								range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								var range1 = worksheet.Range("A4:R4");
								range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
								range1.Merge().Style.Font.FontColor = XLColor.Black;
								range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range1.Style.Font.Bold = true;
								worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
								worksheet.Cell(1, 3).MergedRange();
								worksheet.Cell(currentRow, 1).Value = "S No:";
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 2).Value = "DocNo";
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 3).Value = "Stock Transfer Reference No";
								worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 4).Value = "Product Code";
								worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
								worksheet.Column("A").Width = 10;
								worksheet.Column("B").Width = 20;
								worksheet.Column("C").Width = 20;
								worksheet.Column("D").Width = 20;
								worksheet.Column("E").Width = 20;
								worksheet.Column("F").Width = 15;
								worksheet.Column("G").Width = 15;
								worksheet.Column("H").Width = 15;
								worksheet.Column("I").Width = 15;
								worksheet.Column("J").Width = 15;
								worksheet.Column("K").Width = 15;
								worksheet.Column("L").Width = 15;
								worksheet.Column("M").Width = 15;
								worksheet.Column("N").Width = 15;
								worksheet.Column("O").Width = 15;
								worksheet.Column("P").Width = 15;
								worksheet.Column("Q").Width = 15;
								worksheet.Column("R").Width = 15;
								worksheet.Cell(currentRow, 5).Value = "Product Name";
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 6).Value = "Transfer Branch";
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 7).Value = "Transfer Date";
								worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 8).Value = "Transfer Unit Price";
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Value = "Transfer Selling Price";
								worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = "Transfer Qty";
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 11).Value = "Expire Date";
								worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 12).Value = "Receive Branch";
								worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 13).Value = "Receive Date";
								worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 14).Value = "Receive Unit Price";
								worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 14).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 15).Value = "Receive Selling Price";
								worksheet.Cell(currentRow, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 15).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 16).Value = "Receive Qty";
								worksheet.Cell(currentRow, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 16).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 17).Value = "Payment Type";
								worksheet.Cell(currentRow, 17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 17).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 18).Value = "Balance";
								worksheet.Cell(currentRow, 18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 18).Style.Font.Bold = true;
								CultureInfo siLk = new CultureInfo("si-LK");
								int count = 1;
								foreach (var sale in getexcelStockTransfers.OrderBy(d => d.StockTransferDetailId))
								{
									currentRow++;
									worksheet.Cell(currentRow, 1).Value = count;
									worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 2).Value = sale.cDocNo.ToString();
									worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 3).Value = sale.StockTransferHeaderReferenceNo.ToString();
									worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 4).Value = sale.ProductCode.ToString();
									worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 5).Value = sale.ProductName;
									worksheet.Cell(currentRow, 5).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 6).Value = sale.TransferBranch;
									worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 7).Value = sale.TransferDate;
									worksheet.Cell(currentRow, 7).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 8).Value = sale.TransferUnitPrice;
									worksheet.Cell(currentRow, 8).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 9).Value = sale.TransferSellingPrice;
									worksheet.Cell(currentRow, 9).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 10).Value = sale.TransferQty;
									worksheet.Cell(currentRow, 10).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 11).Value = sale.ProductExpireDate;
									worksheet.Cell(currentRow, 11).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 12).Value = sale.ReceiveBranch;
									worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 13).Value = sale.ReceiveDate;
									worksheet.Cell(currentRow, 13).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 14).Value = sale.ReceiveUnitPrice;
									worksheet.Cell(currentRow, 14).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 15).Value = sale.ReceiveSellingprice;
									worksheet.Cell(currentRow, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 15).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 16).Value = sale.ReceiveQty;
									worksheet.Cell(currentRow, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 16).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 17).Value = sale.PaymentType;
									worksheet.Cell(currentRow, 17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 18).Value = sale.Balance;
									worksheet.Cell(currentRow, 18).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									count++;
								}
								using (var stream = new MemoryStream())
								{
									workbook.SaveAs(stream);
									var content = stream.ToArray();
									return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockTransferReport.xlsx");
								}
							};
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Item Wise Bin Card":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetTransactionsByItemCode";
							var parameters = new DynamicParameters();
							parameters.Add("Locationid", branchLocationId, DbType.Int32);
							parameters.Add("ProductId", userParams.Id, DbType.Int32);
							parameters.Add("Cfdate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("Ctdate", userParams.ToDate.Date, DbType.Date);
	    					var transactionExcelDetails = (await connection.QueryAsync<GetBinByItemCodeDto>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							using (var workbook = new XLWorkbook())
							{
								var worksheet = workbook.Worksheets.Add("Item Wise Bin Card");
								var currentRow = 6;
								var range = worksheet.Range("A1:R1");
								range.Value = $"{branch.BranchName}";
								range.Merge().Style.Font.SetBold().Font.FontSize = 18;
								range.Merge().Style.Font.FontColor = XLColor.Black;
								range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range.Style.Font.Bold = true;
								var range3 = worksheet.Range("A2:R2");
								range3.Value = $"No: {branch.Address}.";
								range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
								range3.Merge().Style.Font.FontColor = XLColor.Black;
								range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range3.Style.Font.Bold = true;
								var range2 = worksheet.Range("A3:R3");
								range2.Value = "Item Wise Bin Card :" + userParams.Id.ToString();
								range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
								range2.Merge().Style.Font.FontColor = XLColor.White;
								range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
								range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								var range1 = worksheet.Range("A4:R4");
								range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
								range1.Merge().Style.Font.FontColor = XLColor.Black;
								range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range1.Style.Font.Bold = true;
								worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
								worksheet.Cell(1, 3).MergedRange();
								worksheet.Cell(currentRow, 1).Value = "S No:";
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 2).Value = "TransactionDetailId";
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 3).Value = "cExtRefNum";
								worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 4).Value = "Barcode";
								worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
								worksheet.Column("A").Width = 10;
								worksheet.Column("B").Width = 20;
								worksheet.Column("C").Width = 20;
								worksheet.Column("D").Width = 20;
								worksheet.Column("E").Width = 20;
								worksheet.Column("F").Width = 15;
								worksheet.Column("G").Width = 15;
								worksheet.Column("H").Width = 15;
								worksheet.Column("I").Width = 15;
								worksheet.Column("J").Width = 15;
								worksheet.Column("K").Width = 15;
								worksheet.Column("L").Width = 15;
								worksheet.Column("M").Width = 15;
								worksheet.Column("N").Width = 15;
								worksheet.Column("O").Width = 15;
								worksheet.Column("P").Width = 15;
								worksheet.Column("Q").Width = 15;
								worksheet.Column("R").Width = 15;
								worksheet.Cell(currentRow, 5).Value = "ProductName";
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 6).Value = "Created Date";
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 7).Value = "dQuantity";
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 8).Value = "dIssueQuantity";
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Value = "dNetQuantity";
								worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = "dCostPrice";
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 11).Value = "dSalePrice";
								worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 12).Value = "dIN";
								worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 13).Value = "dOUT";
								worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 14).Value = "ProductExpireDate";
								worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 14).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 15).Value = "SaleEmployeeCode";
								worksheet.Cell(currentRow, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 15).Style.Font.Bold = true;

								CultureInfo siLk = new CultureInfo("si-LK");
								int count = 1;
								foreach (var sale in transactionExcelDetails.OrderBy(d => d.TransactionDetailId))
								{
									currentRow++;
									worksheet.Cell(currentRow, 1).Value = count;
									worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 2).Value = sale.TransactionDetailId.ToString();
									worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 3).Value = sale.ProductCode.ToString();
									worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 4).Value = sale.ProductName;
									worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 5).Value = sale.ReceivedDate;
									worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 6).Value = sale.ReceivedUnitPrice;
									worksheet.Cell(currentRow, 6).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 7).Value = sale.UnitSellingprice;
									worksheet.Cell(currentRow, 7).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 8).Value = sale.ReceiveQty;
									worksheet.Cell(currentRow, 9).Value = sale.ProductExpireDate;
									worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 10).Value = sale.EmployeeNo;
									worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 11).Value = sale.IssuedDate;
									worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 12).Value = sale.IssuedUnitPrice;
									worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 13).Value = sale.IssuedSoldPrice;
									worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 14).Value = sale.IssuedQty;
									worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 15).Value = sale.Balance;
									worksheet.Cell(currentRow, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
											count++;
								}
								using (var stream = new MemoryStream())
								{
									workbook.SaveAs(stream);
									var content = stream.ToArray();
									return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemWiseBinCardReport.xlsx");
								}
							};
						}
					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Product Sales":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetItemWiseSaleDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("productId", userParams.Id, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var dateWiseProductSales = (await connection.QueryAsync<SaleReportItemWiseDetail>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							using (var workbook = new XLWorkbook())
							{
								var worksheet = workbook.Worksheets.Add("Date Wise Product Sales");
								var currentRow = 6;
								var range = worksheet.Range("A1:N1");
								range.Value = $"{branch.BranchName}";
								range.Merge().Style.Font.SetBold().Font.FontSize = 18;
								range.Merge().Style.Font.FontColor = XLColor.Black;
								range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range.Style.Font.Bold = true;
								var range3 = worksheet.Range("A2:N2");
								range3.Value = $"No: {branch.Address}.";
								range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
								range3.Merge().Style.Font.FontColor = XLColor.Black;
								range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range3.Style.Font.Bold = true;
								var range2 = worksheet.Range("A3:N3");
								range2.Value = "Date Wise Product Sales :" + userParams.Id.ToString();
								range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
								range2.Merge().Style.Font.FontColor = XLColor.White;
								range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
								range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								var range1 = worksheet.Range("A4:N4");
								range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
								range1.Merge().Style.Font.FontColor = XLColor.Black;
								range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range1.Style.Font.Bold = true;
								worksheet.Column("A").Width = 10;
								worksheet.Column("B").Width = 20;
								worksheet.Column("C").Width = 20;
								worksheet.Column("D").Width = 20;
								worksheet.Column("E").Width = 20;
								worksheet.Column("F").Width = 15;
								worksheet.Column("G").Width = 15;
								worksheet.Column("H").Width = 15;
								worksheet.Column("I").Width = 15;
								worksheet.Column("J").Width = 15;
								worksheet.Column("K").Width = 15;
								worksheet.Column("L").Width = 15;
								worksheet.Column("M").Width = 15;
								worksheet.Column("N").Width = 15;
								worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
								worksheet.Cell(1, 3).MergedRange();
								worksheet.Cell(currentRow, 1).Value = "S No:";
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 2).Value = "Location";
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 3).Value = "Bill#";
								worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 4).Value = "BillDate";
								worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 5).Value = "EPFNo";
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 6).Value = "FullName";
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 7).Value = "ItemCode";
								worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 8).Value = "ProductName";
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Value = "QTY";
								worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = "ReturnQty";
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 11).Value = "SalePrice";
								worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 12).Value = "MRP";
								worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 13).Value = "ProductExpireDate";
								worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 14).Value = "SaleValue";
								worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 14).Style.Font.Bold = true;
								worksheet.Cell(dateWiseProductSales.Count + 7, 14).Value = dateWiseProductSales.Sum(s => s.SaleValue);
								worksheet.Cell(dateWiseProductSales.Count + 7, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

								CultureInfo siLk = new CultureInfo("si-LK");
								int count = 1;
								foreach (var sale in dateWiseProductSales.OrderBy(d => d.SaleHeaderId))
								{
									currentRow++;
									worksheet.Cell(currentRow, 1).Value = count;
									worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 2).Value = sale.Location.ToString();
									worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 3).Value = sale.SaleHeaderId.ToString();
									worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 4).Value = sale.BillDate;
									worksheet.Cell(currentRow, 4).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 5).Value = sale.EPFNo;
									worksheet.Cell(currentRow, 5).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 6).Value = sale.FullName;
									worksheet.Cell(currentRow, 6).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 7).Value = sale.ItemCode;
									worksheet.Cell(currentRow, 7).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 8).Value = sale.ProductName;
									worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 9).Value = sale.ProductExpireDate;
									worksheet.Cell(currentRow, 9).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 10).Value = sale.QTY;
									worksheet.Cell(currentRow, 10).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 11).Value = sale.ReturnQty;
									worksheet.Cell(currentRow, 11).SetDataType(XLDataType.Number);

									worksheet.Cell(currentRow, 12).Value = sale.SalePrice;
									worksheet.Cell(currentRow, 12).SetDataType(XLDataType.Number);

									worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 13).Value = sale.MRP;
									worksheet.Cell(currentRow, 13).SetDataType(XLDataType.Number);

									worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 14).Value = sale.SaleValue;
									worksheet.Cell(currentRow, 14).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

									count++;
								}
								using (var stream = new MemoryStream())
								{
									workbook.SaveAs(stream);
									var content = stream.ToArray();
									return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DateWiseProductSales.xlsx");
								}
							};
						}
					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Receipt Summary":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetReciptDetailsByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var getReceiptReportDetails = (await connection.QueryAsync<ReciptDetailsByDateRange>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();

							using (var workbook = new XLWorkbook())
							{
								var worksheet = workbook.Worksheets.Add("Date Wise Receipt Summary");
								var currentRow = 6;
								var range = worksheet.Range("A1:K1");
								range.Value = $"{branch.BranchName}";
								range.Merge().Style.Font.SetBold().Font.FontSize = 18;
								range.Merge().Style.Font.FontColor = XLColor.Black;
								range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range.Style.Font.Bold = true;
								var range3 = worksheet.Range("A2:K2");
								range3.Value = $"No: {branch.Address}.";
								range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
								range3.Merge().Style.Font.FontColor = XLColor.Black;
								range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range3.Style.Font.Bold = true;
								var range2 = worksheet.Range("A3:K3");
								range2.Value = "Date Wise Receipt Summary :" + userParams.Id.ToString();
								range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
								range2.Merge().Style.Font.FontColor = XLColor.White;
								range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
								range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								var range1 = worksheet.Range("A4:K4");
								range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
								range1.Merge().Style.Font.FontColor = XLColor.Black;
								range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range1.Style.Font.Bold = true;
								worksheet.Column("A").Width = 10;
								worksheet.Column("B").Width = 20;
								worksheet.Column("C").Width = 20;
								worksheet.Column("D").Width = 20;
								worksheet.Column("E").Width = 15;
								worksheet.Column("F").Width = 35;
								worksheet.Column("G").Width = 20;
								worksheet.Column("H").Width = 20;
								worksheet.Column("I").Width = 15;
								worksheet.Column("J").Width = 15;
								worksheet.Column("K").Width = 15;
								worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
								worksheet.Cell(1, 3).MergedRange();
								worksheet.Cell(currentRow, 1).Value = "S No:";
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 2).Value = "Location";
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 3).Value = "PO#";
								worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 4).Value = "GRN#";
								worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 5).Value = "GRN Date";
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 6).Value = "Supplier Name";
								worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 7).Value = "Supplier Invoice Number";
								worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 8).Value = "Supplier Invoice Date";
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Value = "Cost Amount";
								worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = "Discount Amount";
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 11).Value = "Total Amount";
								worksheet.Cell(getReceiptReportDetails.Count + 7, 11).Value = getReceiptReportDetails.Sum(s => s.TotalCost);
								worksheet.Cell(getReceiptReportDetails.Count + 7, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

								CultureInfo siLk = new CultureInfo("si-LK");
								int count = 1;
								foreach (var sale in getReceiptReportDetails.OrderBy(d => d.PurchaseOrderHeaderId))
								{
									currentRow++;
									worksheet.Cell(currentRow, 1).Value = count;
									worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 2).Value = sale.BranchCode.ToString();
									worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 3).Value = sale.PurchaseOrderReferenceNo.ToString();
									worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 4).Value = sale.GoodReceptNoteReferenceNo;
									worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 5).Value = sale.CreateDate;
									worksheet.Cell(currentRow, 5).SetDataType(XLDataType.DateTime);
									worksheet.Cell(currentRow, 6).Value = sale.SupplierName;
									worksheet.Cell(currentRow, 6).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 7).Value = sale.SupplierInvoiceId;
									worksheet.Cell(currentRow, 7).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 8).Value = sale.SupplierInvoiceDate;
									worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 9).Value = sale.TotalBillAmount;
									worksheet.Cell(currentRow, 9).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 10).Value = sale.TotalDiscountAmount;
									worksheet.Cell(currentRow, 10).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 11).Value = sale.TotalCost;
									worksheet.Cell(currentRow, 11).SetDataType(XLDataType.Number);
							 
									count++;
								}
								using (var stream = new MemoryStream())
								{
									workbook.SaveAs(stream);
									var content = stream.ToArray();
									return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DateWiseReceiptSummary.xlsx");
								}
							};
						}
					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Date Wise Lunch Packet Summary":
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_GetItemSalesSummaryByDateRange";
							var parameters = new DynamicParameters();
							parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("productId", userParams.Id, DbType.Int32);
							parameters.Add("frmDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("toDate", userParams.ToDate.Date, DbType.Date);
							var lunchPackSalesSummary = (await connection.QueryAsync<LunchPackSalesSummary>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();

							using (var workbook = new XLWorkbook())
							{
								var worksheet = workbook.Worksheets.Add("Date Wise Lunch Packet Summary");
								var currentRow = 6;
								var range = worksheet.Range("A1:K1");
								range.Value = $"{branch.BranchName}";
								range.Merge().Style.Font.SetBold().Font.FontSize = 18;
								range.Merge().Style.Font.FontColor = XLColor.Black;
								range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range.Style.Font.Bold = true;
								var range3 = worksheet.Range("A2:K2");
								range3.Value = $"No: {branch.Address}.";
								range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
								range3.Merge().Style.Font.FontColor = XLColor.Black;
								range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range3.Style.Font.Bold = true;
								var range2 = worksheet.Range("A3:K3");
								range2.Value = "Date Wise Lunch Packet Summary :" + userParams.Id.ToString();
								range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
								range2.Merge().Style.Font.FontColor = XLColor.White;
								range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
								range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								var range1 = worksheet.Range("A4:K4");
								range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")}  ";
								range1.Merge().Style.Font.FontColor = XLColor.Black;
								range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								range1.Style.Font.Bold = true;
								worksheet.Column("A").Width = 10;
								worksheet.Column("B").Width = 20;
								worksheet.Column("C").Width = 20;
								worksheet.Column("D").Width = 20;
								worksheet.Column("E").Width = 20;
								worksheet.Column("F").Width = 15;
								worksheet.Column("G").Width = 15;
								worksheet.Column("H").Width = 15;
								worksheet.Column("I").Width = 15;
                                worksheet.Column("J").Width = 15;
                                worksheet.Column("K").Width = 15;
                                worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
								worksheet.Cell(1, 3).MergedRange();
								worksheet.Cell(currentRow, 1).Value = "S No:";
								worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 2).Value = "Epf#";
								worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 3).Value = "Number#";
								worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 4).Value = "FullName";
								worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 5).Value = "Scheme";
								worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 5).Style.Font.Bold = true;

                                worksheet.Cell(currentRow, 6).Value = "Job Title";
                                worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                worksheet.Cell(currentRow, 6).Style.Font.Bold = true;

                                worksheet.Cell(currentRow, 7).Value = "Department";
                                worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                worksheet.Cell(currentRow, 7).Style.Font.Bold = true;

                                worksheet.Cell(currentRow, 8).Value = "ProductName";
								worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Value = "QTY";
								worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Value = "SalePrice";
								worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
								worksheet.Cell(currentRow, 11).Value = "SaleValue";
								worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
								 worksheet.Cell(lunchPackSalesSummary.Count + 7, 10).Value = lunchPackSalesSummary.Sum(s => s.SalePrice*s.Total);
								worksheet.Cell(lunchPackSalesSummary.Count + 7, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

								CultureInfo siLk = new CultureInfo("si-LK");
								int count = 1;
								foreach (var sale in lunchPackSalesSummary.OrderBy(d => d.Number))
								{
									currentRow++;
									worksheet.Cell(currentRow, 1).Value = count;
									worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 2).Value = sale.Customer_ID;
									worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 3).Value = sale.Number;
									worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 4).Value = sale.FullName;
									worksheet.Cell(currentRow, 4).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 5).Value = sale.Scheme;
									worksheet.Cell(currentRow, 5).SetDataType(XLDataType.Text);


                                    worksheet.Cell(currentRow, 6).Value = sale.JobTitle;
                                    worksheet.Cell(currentRow, 6).SetDataType(XLDataType.Text);

                                    worksheet.Cell(currentRow, 7).Value = sale.hrODescription;
                                    worksheet.Cell(currentRow, 7).SetDataType(XLDataType.Text);

                                    worksheet.Cell(currentRow, 8).Value = sale.ProductName;
									worksheet.Cell(currentRow, 8).SetDataType(XLDataType.Text);
									worksheet.Cell(currentRow, 9).Value = sale.Total;
									worksheet.Cell(currentRow, 9).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
									worksheet.Cell(currentRow, 10).Value = sale.SalePrice;
									worksheet.Cell(currentRow, 10).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 11).Value = sale.Total* sale.SalePrice;
									worksheet.Cell(currentRow, 11).SetDataType(XLDataType.Number);
									worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
								   count++;
								}
								using (var stream = new MemoryStream())
								{
									workbook.SaveAs(stream);
									var content = stream.ToArray();
									return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DateWiseLunchPacketSummary.xlsx");
								}
							};
						}
					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
				case "Item Wise Moving Nonmoving Stock":
					List<StockNonMovement> stockIQ8 = new List<StockNonMovement>();
					try
					{
						using (var connection = _dpContext.CreateConnection())
						{
							var procedureName1 = "sp_NonMovementStock";
							var parameters = new DynamicParameters();
							parameters.Add("@branchLocationId", branchLocationId, DbType.Int32);
							parameters.Add("@fDate", userParams.FromDate.Date, DbType.Date);
							parameters.Add("@tDate", userParams.ToDate.Date, DbType.Date);
							stockIQ8 = (await connection.QueryAsync<StockNonMovement>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
							.ConfigureAwait(false)).AsList();
							//return Ok(stockIQ.Where(c => c.QTY > 0));
						}

					}
					catch (Exception ex)
					{
						//log error
						return StatusCode(500, ex.Message);
					}
					using (var workbook = new XLWorkbook())
					{
						var worksheet = workbook.Worksheets.Add("ItemWiseMovingNonmovingStock");
						var currentRow = 6;
						var range = worksheet.Range("A1:J1");
						range.Value = $"{branch.BranchName}";
						range.Merge().Style.Font.SetBold().Font.FontSize = 18;
						range.Merge().Style.Font.FontColor = XLColor.Black;
						range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range.Style.Font.Bold = true;
						var range3 = worksheet.Range("A2:J2");
						range3.Value = $"No: {branch.Address}.";
						range3.Merge().Style.Font.SetBold().Font.FontSize = 16;
						range3.Merge().Style.Font.FontColor = XLColor.Black;
						range3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range3.Style.Font.Bold = true;
						var range2 = worksheet.Range("A3:J3");
						range2.Value = "ItemWiseMovingNonmovingStock";
						range2.Merge().Style.Font.SetBold().Font.FontSize = 15;
						range2.Merge().Style.Font.FontColor = XLColor.White;
						range2.Style.Fill.BackgroundColor = XLColor.DodgerBlue;
						range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						var range1 = worksheet.Range("A4:J4");
						range1.Value = $"Month Of {userParams.ToDate.ToString("MMMM")} : {userParams.FromDate.ToString("dd MMMM yyyy")} - {userParams.ToDate.ToString("dd MMMM yyyy")} ";
						range1.Merge().Style.Font.FontColor = XLColor.Black;
						range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						range1.Style.Font.Bold = true;
						worksheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.AirForceBlue;
						worksheet.Cell(1, 3).MergedRange();
						worksheet.Cell(currentRow, 1).Value = "S No:";
						worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 2).Value = "Supplier Name";
						worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 3).Value = "Item Code";
						worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
						worksheet.Column("A").Width = 10;
						worksheet.Column("B").Width = 20;
						worksheet.Column("C").Width = 40;
						worksheet.Column("D").Width = 15;
						worksheet.Column("E").Width = 15;
						worksheet.Column("F").Width = 15;
						worksheet.Column("G").Width = 15;
						worksheet.Column("H").Width = 15;
						worksheet.Column("I").Width = 15;
						worksheet.Column("J").Width = 15;
						worksheet.Cell(currentRow, 4).Value = "ProductName";
						worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 5).Value = "ProductExpireDate";
						worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Value = "QTY";
						worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Value = "Cost";
						worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 8).Value = "CostValue";
						worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 9).Value = "UnitPrice";
						worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
						worksheet.Cell(currentRow, 10).Value = "SaleValue";
						worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
						worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
						CultureInfo siLk = new CultureInfo("si-LK");
						int count = 1;
						foreach (var stock in stockIQ8.Where(c => c.QTY > 0))
						{
							currentRow++;
							worksheet.Cell(currentRow, 1).Value = count;
							worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 2).Value = stock.SupplierName.ToString();
							worksheet.Cell(currentRow, 2).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//worksheet.Range("B"+7+":B"+15).Column(1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);;
							worksheet.Cell(currentRow, 3).Value = stock.Barcode.ToString();
							worksheet.Cell(currentRow, 3).SetDataType(XLDataType.Text);
							worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 4).Value = stock.ProductName;
							worksheet.Cell(currentRow, 5).Value = stock.ProductExpireDate;
							worksheet.Cell(currentRow, 6).Value = stock.QTY;
							//   worksheet.Cell(currentRow, 5).Value = stock.Cost.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Value = stock.CostValue.ToString("C", siLk);
							//    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 7).Value = stock.dCostPrice.ToString("C", siLk);
							worksheet.Cell(currentRow, 8).Value = stock.tValue;
							worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							worksheet.Cell(currentRow, 9).Value = stock.dSalePrice;
							worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							//  worksheet.Cell(currentRow, 8).Value = stock.SaleValue.ToString("C", siLk);
							worksheet.Cell(currentRow, 10).Value = stock.dSalePrice*stock.QTY;
							worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
							count++;
						}
						using (var stream = new MemoryStream())
						{
							workbook.SaveAs(stream);
							var content = stream.ToArray();
							return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemWiseMovingNonmovingStock.xlsx");
						}
					};	
				case "Order Details By ID":
					break;
			}
			return NoContent();
		}
	}
	public class CustomerDetail
	{
		[Display(Name = "Customer Number")]
		public string CustomerNumber { get; set; }
		public Decimal Total { get; set; }
		[Display(Name = "Customer Id")]
		public string CustomerId { get; set; }
		[Display(Name = "Full Name")]
		public string FullName { get; set; }
		[Display(Name = "Basic Salarry")]
		public Decimal BasicSalary { get; set; }

	}
	public class ShopStockDetail
	{
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Quantity")]
		public decimal QTY { get; set; }
		[Display(Name = "Cost")]
		public Decimal Cost { get; set; }
		[Display(Name = "MaximumRetailPrice")]
		public Decimal MaximumRetailPrice { get; set; }
		[Display(Name = "Cost Value")]
		public Decimal CostValue { get; set; }
		[Display(Name = "Unit Price")]
		public Decimal UnitPrice { get; set; }
		[Display(Name = "Cost Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
	}
	public class StockNonMovement
	{
		public string  Barcode { get; set; }
		public string  ProductName { get; set; }
		public string  UOM { get; set; }
		public decimal tValue { get; set; }
		public string  SupplierName { get; set; }
		public decimal dSalePrice { get; set; }
		public decimal dCostPrice { get; set; }
		public decimal QTY { get; set; }
		public DateTime ProductExpireDate { get; set; }
	}
	public class SaleReportDetail
	{
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Cash Price")]
		public Decimal CashPrice { get; set; }
        [Display(Name = "Quantity")]
		public decimal QTY { get; set; }
		[Display(Name = "Sale Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Cost")]
		public Decimal Cost { get; set; }
		[Display(Name = "MaximumRetailPrice")]
		public Decimal MaximumRetailPrice { get; set; }
		[Display(Name = "Stock Value")]
		public Decimal StockValue { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
		public Int64 SaleHeaderId { get; set; }
		public DateTime CreateDate { get; set; }
		public string PaymentType { get; set; } /*https://codingstatus.com/sql-case-statement/*/
	}
	public class SaleReportItemWiseDetail
	{
		public string EPFNo { get; set; }
		public string Location { get; set; }
		public string FullName { get; set; }
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Cash Price")]
		public Decimal CashPrice { get; set; }
		[Display(Name = "Quantity")]
		public decimal QTY { get; set; }
		[Display(Name = "Return Quantity")]
		public decimal ReturnQty { get; set; }
		[Display(Name = "Sale Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Sale Price")]
		public Decimal SalePrice { get; set; }
		[Display(Name = "Retail Price")]
		public Decimal MRP { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
		public Int64 SaleHeaderId { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime BillDate { get; set; }
		public string PaymentType { get; set; }

	}
	public class ReciptDetailsByDateRange
	{
		public Int64 GoodReceptNoteHeaderId { get; set; }
		public string SupplierName { get; set; }
		public string SupplierInvoiceId { get; set; }
		public DateTime SupplierInvoiceDate { get; set; }
		public DateTime CreateDate { get; set; }
		public decimal TotalCost { get; set; }
		public decimal FreeItemsTotalCost { get; set; }
		public decimal TotalDiscountAmount { get; set; }
		public string BranchCode { get; set; }
		public int PurchaseOrderHeaderId { get; set; }
		public decimal TotalBillAmount { get; set; }
		public string GoodReceptNoteReferenceNo { get; set; }
		public string PurchaseOrderReferenceNo { get; set; }

	}

	public class SaleReportDetailByCustomer
	{
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "Paid")]
		public bool Paid { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Cash Price")]
		public Decimal CashPrice { get; set; }
		[Display(Name = "Quantity")]
		public decimal QTY { get; set; }
		[Display(Name = "Return Quantity")]
		public decimal ReturnQTY { get; set; }
		[Display(Name = "Sale Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Cost")]
		public Decimal Cost { get; set; }
		[Display(Name = "Stock Value")]
		public Decimal StockValue { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
		public Int64 SaleHeaderId { get; set; }
		public DateTime CreateDate { get; set; }
		public string BranchCode { get; set; }
		public string PaymentType { get; set; }

	}
	public class PurchaseReturnReportDetail
	{
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Cash Price")]
		public Decimal CashPrice { get; set; }
		[Display(Name = "Return QTY")]
		public decimal ReturnQTY { get; set; }
		[Display(Name = "Sale Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Return Price")]
		public Decimal ReturnPrice { get; set; }
		[Display(Name = "Stock Value")]
		public Decimal StockValue { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
		[Display(Name = "Dis Amount")]
		public Decimal DisAmount { get; set; }
		[Display(Name = "Dis%")]
		public Decimal Dis { get; set; }
	}
	public class CustomerReturnReportDetail
	{
		[Display(Name = "Item Code")]
		public string ItemCode { get; set; }
		[Display(Name = "ProductName")]
		public string ProductName { get; set; }
		[Display(Name = "Cash Price")]
		public Decimal CashPrice { get; set; }
		[Display(Name = "Return QTY")]
		public decimal ReturnQTY { get; set; }
		[Display(Name = "Sale QTY")]
		public decimal SaleQuantity { get; set; }

		[Display(Name = "Sale Value")]
		public Decimal SaleValue { get; set; }
		[Display(Name = "Return Price")]
		public Decimal ReturnPrice { get; set; }
		[Display(Name = "Sale Price")]
		public Decimal SalePrice { get; set; }
		[Display(Name = "Stock Value")]
		public Decimal StockValue { get; set; }
		[Display(Name = "Product Expire Date")]
		public DateTime ProductExpireDate { get; set; }
		[Display(Name = "Dis Amount")]
		public Decimal DisAmount { get; set; }
		[Display(Name = "Dis%")]
		public Decimal Dis { get; set; }

		public string PaymentType { get; set; }
	}
	public class LunchPackSalesSummary
	{
		public string ProductName { get; set; }
		[Display(Name = "Sale Price")]
		public Decimal SalePrice { get; set; }
		public int Total { get; set; }
		public int Customer_ID { get; set; }
		public string FullName { get; set; }
		public string Scheme { get; set; }
		public string Number { get; set; }
		public string JobTitle { get; set; }

		public string hrODescription { get; set; }


    }
	public class SaleSummaryReportDetail
	{
		public Int64 SaleHeaderId { get; set; }
		public DateTime OrderDate { get; set; }
		public bool Paid { get; set; }
		public DateTime PaymentDate { get; set; }
		public string EmployeeCode { get; set; }
		public Decimal TotalAmount { get; set; }
		public Decimal CustomerReturnAmount { get; set; }
		public string BranchCode { get; set; }
		public string PaymentType { get; set; }
	}
	public class PurchaseOrderListReport
    {
		public DateTime DeliveryDate { get; set; }
		public DateTime POCreatedDate { get; set; }
		public Decimal UnitCost { get; set; }
		public Decimal UnitCostWithOrOutVAT { get; set; }
		public Decimal MaximumRetailPrice { get; set; }
		public Decimal SalePrice { get; set; }
		public Decimal Total { get; set; }
		public Decimal POQuantity { get; set; }
		public Decimal GoodReceiptQuantity { get; set; }
		public string ProductName { get; set; }
		public string ProductCode  { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public string SupplierName { get; set; }
		public string PONumber { get; set; }
		public string  GoodReceptNoteNumber { get; set; }
		public string Status { get; set; }
		public Decimal VATPrecentage { get; set; }
		public Decimal PricePrecentage { get; set; }
		public Decimal dSalePrice { get; set; }
		public Decimal dCostPrice { get; set; }

		
	}
}