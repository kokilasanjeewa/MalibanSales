using System.IO.Compression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Runtime.InteropServices;
using Dapper;
using System.Data;

namespace Maliban.Pos.Api.Controllers
{
	[Route("api/[controller]")]
	[AllowAnonymous]
	[ApiVersion("1.0")]
	[ApiVersion("1.1")]
	public class CommonController : BaseController
	{
		private readonly IConfiguration _config;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signinManager;
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly ILog logger;
		private readonly IRepositoryWrapper _repoWrapper;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnviroment;
		private readonly ISqlDataAccess _db;

        [Obsolete]
        public CommonController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IRepositoryWrapper repoWrapper, ISqlDataAccess db) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
		{
			_config = config;
			_userManager = userManager;
			_signinManager = signinManager;
			_mapper = mapper;
			_context = context;
			this.logger = logger;
			_repoWrapper = repoWrapper;
			_hostingEnviroment = hostingEnviroment;
			_db = db;

		}
		// report/purchasereturn
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallprn"></param>
		[HttpGet("getallprn", Name = "GetAllPrn")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllPrn(bool status, int? branchLocationId, DateTime fromDate, DateTime toDate)
		{
			var grns = _context.PurchaseReturnHeaders.Where(b => b.IsDeleted == status &&
			b.BranchLocationId == branchLocationId && b.CreateDate.Date >= fromDate && b.CreateDate.Date <= toDate).ToList();
			var grnToReturn = _mapper.Map<IEnumerable<PrnListDto>>(grns);
			if (grnToReturn.Count() == 0)
			{
				return NotFound("Don't have any Grn.");
			}
			return Ok(grnToReturn);
		}
		/// <summary>
		/// Get All Scheme
		/// </summary>
		/// <param name="getallscheme"></param>
		[HttpGet("getallscheme", Name = "GetAllScheme")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllScheme(bool status)
		{
			var paymentMethods = _context.MI_Employer_Employees.Select(c => new { c.Scheme, c.SchemeCode }).Distinct().ToList();
			if (paymentMethods.Count() == 0)
			{
				return NotFound("Don't have any payment method.");
			}
			return Ok(paymentMethods);
		}
		// report/purchasereturn
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallgrn"></param>
		[HttpGet("getallgrn", Name = "GetAllGrn")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllGrn(bool status, int? branchLocationId, DateTime fromDate, DateTime toDate)
		{
			var grns = _context.GoodReceptNoteHeaders.Where(b => b.IsDeleted == status &&
			b.BranchLocationId == branchLocationId && b.CreateDate.Date >= fromDate && b.CreateDate.Date <= toDate).ToList();
			var grnToReturn = _mapper.Map<IEnumerable<GRNListDto>>(grns);
			if (grnToReturn.Count() == 0)
			{
				return NotFound("Don't have any Grn.");
			}
			return Ok(grnToReturn);
		}
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallsalereturn"></param>
		[HttpGet("getallsalereturn", Name = "GetAllSaleReturn")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllSaleReturn(bool status, int? branchLocationId, DateTime fromDate, DateTime toDate)
		{
			var grns = _context.CustomerReturnHeaders.Where(b => b.IsDeleted == status && b.BranchLocationId == branchLocationId && b.CreateDate >= fromDate && b.CreateDate <= toDate).Select(c => new { CustomerReturnHeaderId = c.CustomerReturnHeaderId }).ToList();
			if (grns.Count() == 0)
			{
				return NotFound("Don't have any Grn.");
			}
			return Ok(grns);
		}
        /// <summary>
        /// Get All Card Type 
        /// </summary>
        /// <param name="getallcardtype"></param>
        [HttpGet("getallcardtype", Name = "GetAllCardType")]
        [MapToApiVersion("1.1")]
        public IActionResult GetAllCardType(bool status, int? branchLocationId)
        {
            var grns = _context.CardtMethods.Where(b => b.IsDeleted == status && b.BranchLocationId == branchLocationId).Select(c => new { ServiceCharge = c.ServiceCharge, CardName = c.CardName, CardMethodId = c.CardMethodId }).ToList();
            if (grns.Count() == 0)
            {
                return NotFound("Don't have any Card Type.");
            }
            return Ok(grns);
        }
        /// <summary>
        /// Get All Branches 
        /// </summary>
        /// <param name="getallstockadjustment"></param>
        [HttpGet("getallstockadjustment", Name = "GetAllStockAdjustment")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllStockAdjustment(bool status, int? branchLocationId, DateTime fromDate, DateTime toDate)
		{
			var grns = _context.StockAdjustmentHeaders.Where(b => b.IsDeleted == status && b.BranchLocationId == branchLocationId && b.CreateDate >= fromDate && b.CreateDate <= toDate).Select(s => new { stockAdjustmentHeaderId = s.StockAdjustmentHeaderId }).ToList();
			if (grns.Count() == 0)
			{
				return NotFound("Don't have any Grn.");
			}
			return Ok(grns);
		}
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallbranch"></param>
		[HttpGet("getallbranch", Name = "GetAllBranch")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllBranch(bool status)
		{
			var branchlocations = _context.BranchLocations.Where(b => b.IsDeleted == status).ToList();
			var branchlocationToReturn = _mapper.Map<IEnumerable<BranchLocationListDto>>(branchlocations);
			if (branchlocationToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(branchlocationToReturn);
		}
		/// <summary>
		/// Get All Factory Wise Sale
		/// </summary>
		/// <param name="getallfactorywisesale"></param>
		[HttpGet("getallfactorywisesale", Name = "GetAllFactoryWiseSale")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllFactoryWiseSale(bool status)
		{
			var getallfactorywisesale = _db.LoadData<FactoryWiseSale, dynamic>("dbo.sp_GetAllFactoryWiseSale", new { xYear = DateTime.Now.Date.Year, xMonth = DateTime.Now.Date.Month, xChartName= "getallfactorywisesale" }).Result.ToList();

			/*            var getallfactorywisesale = await (from sh in _context.SaleHeaders.Where(sh => sh.OrderDate.Date.Year == DateTime.Now.Date.Year && sh.OrderDate.Date.Month == DateTime.Now.Date.Month)
									join b in _context.BranchLocations
									on sh.BranchLocationId equals b.BranchLocationId
									group sh by new {b.BranchCode} into sagroup
								   select new { sagroup.Key.BranchCode, SaleAmount= sagroup.Sum(x => x.TotalAmount-x.CustomerReturnAmount) }).ToListAsync();*/
			if (getallfactorywisesale.Count() == 0)
			{
				return NotFound("Don't have any sales.");
			}
			return Ok(getallfactorywisesale);
		}
		/// <summary>
		/// Get All Factory Wise Sale
		/// </summary>
		/// <param name="topsellingproduct"></param>
		[HttpGet("topsellingproduct", Name = "TopSellingProduct")]
		[MapToApiVersion("1.1")]
		public IActionResult TopSellingProduct(bool status)
		{
			var getallfactorywisesale = _db.LoadData<TopSellingProduct, dynamic>("dbo.sp_GetAllFactoryWiseSale", new { xYear = DateTime.Now.Date.Year, xMonth = DateTime.Now.Date.Month, xChartName = "topsellingproduct" }).Result.ToList();

			/*            var getallfactorywisesale = await (from sh in _context.SaleHeaders.Where(sh => sh.OrderDate.Date.Year == DateTime.Now.Date.Year && sh.OrderDate.Date.Month == DateTime.Now.Date.Month)
									join b in _context.BranchLocations
									on sh.BranchLocationId equals b.BranchLocationId
									group sh by new {b.BranchCode} into sagroup
								   select new { sagroup.Key.BranchCode, SaleAmount= sagroup.Sum(x => x.TotalAmount-x.CustomerReturnAmount) }).ToListAsync();*/
			if (getallfactorywisesale.Count() == 0)
			{
				return NotFound("Don't have any sales.");
			}
			return Ok(getallfactorywisesale);
		}
		/// <summary>
		/// Get All Factory Wise Sale
		/// </summary>
		/// <param name="dquantityvsdissueingquantity"></param>
		[HttpGet("dquantityvsdissueingquantity", Name = "DQuantityVsDissueingQuantity")]
		[MapToApiVersion("1.1")]
		public IActionResult DQuantityVsDissueingQuantity(bool status)
		{
			var getallfactorywisesale = _db.LoadData<DQuantityError, dynamic>("dbo.sp_GetAllFactoryWiseSale", new { xYear = DateTime.Now.Date.Year, xMonth = DateTime.Now.Date.Month, xChartName = "dquantityvsdissueingquantity" }).Result.ToList();

			/*            var getallfactorywisesale = await (from sh in _context.SaleHeaders.Where(sh => sh.OrderDate.Date.Year == DateTime.Now.Date.Year && sh.OrderDate.Date.Month == DateTime.Now.Date.Month)
									join b in _context.BranchLocations
									on sh.BranchLocationId equals b.BranchLocationId
									group sh by new {b.BranchCode} into sagroup
								   select new { sagroup.Key.BranchCode, SaleAmount= sagroup.Sum(x => x.TotalAmount-x.CustomerReturnAmount) }).ToListAsync();*/
			if (getallfactorywisesale.Count() == 0)
			{
				return NotFound("Don't have any error.");
			}
			return Ok(getallfactorywisesale);
		}
		
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallreturntype"></param>
		[HttpGet("getallreturntype", Name = "GetAllReturnType")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllReturnType(bool status)
		{
			var branchlocations = _context.ReturnReasons.Where(b => b.IsDeleted == status).ToList();
			var branchlocationToReturn = _mapper.Map<IEnumerable<ReturnReasonListDto>>(branchlocations);
			if (branchlocationToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(branchlocationToReturn);
		}
		/// <summary>
		/// Get All Role 
		/// </summary>
		/// <param name="getallrole"></param>
		[HttpGet("getallrole", Name = "GetAllRole")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllRole(bool status, [Optional] int code)
		{
			var user = await _userManager.Users.Where(u => u.Id == code).FirstOrDefaultAsync();
			if (user == null) return NotFound("Could not find user");
			var userRoles = await _userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
			{
				if (role == "Administrator")
				{

					var roless = _context.Roles.Where(b => b.IsActive == !status).Select(r => new { r.Name, r.Id, r.IsActive, r.CreatedDate, r.Description }).ToList();
					if (roless.Count() == 0)
					{
						return NotFound("Don't have any role.");
					}
					return Ok(roless.Where(r => r.Name != "Administrator" && r.Name != "GlobalAdministrator"));
				}
				if (role == "GlobalAdministrator")
				{

					var roless = _context.Roles.Where(b => b.IsActive == !status).Select(r => new { r.Name, r.Id, r.IsActive, r.CreatedDate, r.Description }).ToList();
					if (roless.Count() == 0)
					{
						return NotFound("Don't have any role.");
					}
					return Ok(roless);
				}
			}
			var roles = _context.Roles.Where(b => b.IsActive == !status).Select(r => new { r.Name, r.Id, r.IsActive, r.CreatedDate, r.Description }).ToList();
			if (roles.Count() == 0)
			{
				return NotFound("Don't have any role.");
			}
			return Ok(roles.Where(r => r.Name == "User"));
		}
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getallreport"></param>
		[HttpGet("getallreport", Name = "GetAllRepot")]
		[MapToApiVersion("1.1")]
		public ActionResult<IEnumerable<ReportListDto>> GetAllReport(bool status)
		{
			var branchlocations = _context.Reports.Where(b => b.IsDeleted == status).ToList();
			var branchlocationToReturn = _mapper.Map<IEnumerable<ReportListDto>>(branchlocations);
			if (branchlocationToReturn.Count() == 0)
			{
				return NotFound("Don't have any reports.");
			}

			return Ok(branchlocationToReturn);
		}
		/// <summary>
		/// Get All Branches 
		/// </summary>
		/// <param name="getbranch"></param>
		[HttpGet("getbranch", Name = "GetBranch")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetBranch(bool status, int? branchLocationId)
		{
			var branchlocations = await _context.BranchLocations.Where(b => b.IsDeleted == status && b.BranchLocationId == branchLocationId).ToListAsync();
			if (branchlocations == null)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(branchlocations);
		}
		/// <summary>
		/// Get All PO's 
		/// </summary>
		/// <param name="getallpo"></param>
		[HttpGet("getallpo", Name = "GetAllPO")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllPO(bool status, int? branchLocationId, [Optional] DateTime fromDate, [Optional] DateTime toDate)
		{
			var poList = _context.PurchaseOrderHeaders.Where(b => b.IsProcess == false && b.IsComfirm == status && b.BranchLocationId == branchLocationId).Select(p => new { p.PurchaseOrderHeaderId, p.BranchLocation, p.PurchaseAmount }).ToList();
			if (poList.Count() == 0)
			{
				return NotFound("Don't have any po.");
			}
			return Ok(poList);
		}
		/// <summary>
		/// Get All PO's 
		/// </summary>
		/// <param name="getallpo"></param>
		[HttpGet("getallconfirmpo", Name = "GetAllConfirmPO")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllConfirmPO(bool status, int? branchLocationId, [Optional] DateTime fromDate, [Optional] DateTime toDate)
		{
			var poList = _context.PurchaseOrderHeaders.Where(b => b.IsProcess == false && b.IsComfirm == true && b.IsApprove == true && b.IsDeleted == false && b.BranchLocationId == branchLocationId).Select(p => new { p.PurchaseOrderHeaderId, p.BranchLocation, p.PurchaseAmount }).ToList();
			if (poList.Count() == 0)
			{
				return NotFound("Don't have any po.");
			}
			return Ok(poList);
		}
		/// <summary>
		/// Get All PO's 
		/// </summary>
		/// <param name="getallpurchaserequest"></param>
		[HttpGet("getallpurchaserequest", Name = "GetAllPurchaseRequest")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllPurchaseRequest(bool status, int? branchLocationId)
		{
			var prList = from pr in _context.PurchaseRequestDetails.Where(y => y.IsComfirm == status).OrderBy(z => z.ProductId) join p in _context.Products on pr.ProductId equals p.ProductId group pr by new { pr.ProductId, p.Barcode, p.ProductName, pr.PurchaseRequestHeader.BranchLocation.BranchCode, pr.PurchaseRequestHeader.BranchLocation.BranchLocationId, pr.PurchaseRequestHeader.PurchaseRequestHeaderId, pr.CreateDate, } into g select new { g.Key.PurchaseRequestHeaderId, g.Key.BranchCode, g.Key.BranchLocationId, g.Key.ProductId, g.Key.Barcode, g.Key.ProductName, g.Key.CreateDate, Quantity = g.Sum(a => a.Quantity) };

			if (prList.Count() == 0)
			{
				return NotFound("Don't have any purchase request.");
			}
			return Ok(prList.Where(p => p.BranchLocationId == branchLocationId));
		}
		/// <summary>
		/// Get All District 
		/// </summary>
		/// <param name="getalldistrict"></param>
		[HttpGet("getalldistrict", Name = "GetAllDistrict")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllDistrict(bool status, [FromQuery] string Code)
		{
			if (!string.IsNullOrEmpty(Code))
			{
				var province = await _context.Provinces.SingleAsync(p => p.ProvinceId == int.Parse(Code));
				var district1 = _context.Districts.Where(d => d.IsDeleted == status && d.ProvinceCode == province.ProvinceCode).ToList();
				var districtToReturn = _mapper.Map<IEnumerable<DistrictListDto>>(district1);
				return Ok(districtToReturn);
			}
			var district = _context.Districts.Where(d => d.IsDeleted == status).ToList();
			var districtToReturn1 = _mapper.Map<IEnumerable<DistrictListDto>>(district);
			if (district.Count() == 0)
			{
				return NotFound("Don't have any district.");
			}
			return Ok(districtToReturn1);
		}
		/// <summary>
		/// Get All Cities 
		/// </summary>
		/// <param name="getallcities"></param>
		[HttpGet("getallcities", Name = "GetAllCities")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllCities(bool status, [FromQuery] string Code)
		{
			if (!string.IsNullOrEmpty(Code))
			{
				var district = await _context.Districts.SingleAsync(p => p.DistrictId == int.Parse(Code));
				var cities1 = _context.Cities.Where(d => d.IsDeleted == status && d.DistrictCode == district.DistrictCode).ToList();
				var citiesToReturn1 = _mapper.Map<IEnumerable<CityListDto>>(cities1);
				return Ok(citiesToReturn1);
			}

			var cities = _context.Cities.Where(d => d.IsDeleted == status).ToList();
			var citiesToReturn = _mapper.Map<IEnumerable<CityListDto>>(cities);
			if (cities.Count() == 0)
			{
				return NotFound("Don't have any district.");
			}
			return Ok(citiesToReturn);
		}
		/// <summary>
		/// Get All Provinces 
		/// </summary>
		/// <param name="getallprovinces"></param>
		[HttpGet("getallprovinces", Name = "GetAllProvinces")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllProvinces(bool status)
		{
			var provinces = _context.Provinces.Where(d => d.IsDeleted == status).ToList();
			var provincesToReturn = _mapper.Map<IEnumerable<ProvinceListDto>>(provinces);
			if (provinces.Count() == 0)
			{
				return NotFound("Don't have any district.");
			}
			return Ok(provincesToReturn);
		}
		/// <summary>
		/// Get AllCountries 
		/// </summary>
		/// <param name="getallcountries"></param>
		[HttpGet("getallcountries", Name = "GetAllCountries")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllCountries(bool status)
		{
			var countries = _context.Countries.Where(d => d.IsDeleted == status).ToList();
			var countriesToReturn = _mapper.Map<IEnumerable<ProvinceListDto>>(countries);
			if (countries.Count() == 0)
			{
				return NotFound("Don't have any district.");
			}
			return Ok(countriesToReturn);
		}
		/// <summary>
		/// Get All Titles
		/// </summary>
		/// <param name="getalltitle"></param>
		[HttpGet("getalltitle", Name = "GetAllTitle")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllTitle(bool status)
		{
			var titles = _context.Titles.Where(d => d.IsDeleted == status).Select(c => new { c.TitleId, c.TitleName }).ToList();
			if (titles.Count() == 0)
			{
				return NotFound("Don't have any titles.");
			}
			return Ok(titles);
		}
		/// <summary>
		/// Get All TypeofGoods
		/// </summary>
		/// <param name="getalltypeofgoods"></param>
		[HttpGet("getalltypeofgoods", Name = "GetAllTypeofGoods")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllTypeofGoods(bool status)
		{
			var typeofGoods = _context.TypeofGoods.Where(d => d.IsDeleted == status).Select(c => new { c.TypeofGoodId, c.TypeofGoods }).ToList();
			if (typeofGoods.Count() == 0)
			{
				return NotFound("Don't have any TypeofGoods.");
			}
			return Ok(typeofGoods);
		}
		/// <summary>
		/// Get All TypeofGoods
		/// </summary>
		/// <param name="getallproduct"></param>
		[HttpGet("getallproduct", Name = "getallproduct")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllProduct(bool status)
		{
			var products = await _context.Products.Where(d => d.IsDeleted == status).Select(c => new { c.ProductId, c.ProductName, c.Barcode }).ToListAsync();
			if (products.Count() == 0)
			{
				return NotFound("Don't have any products.");
			}
			return Ok(products);
		}
		/// <summary>
		/// Get All Stocks
		/// </summary>
		/// <param name="getallstock"></param>
		[HttpGet("getallstock", Name = "getallstock")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllStock(bool status)
		{
			var stocks = await _context.ShopStocks.Where(d => d.Active == status).
			Include(c => c.Product).
			Select(c => new { c.ProductId, c.Product.ProductName, c.Product.Barcode, c.UnitsInStock }).ToListAsync();
			if (stocks.Count() == 0)
			{
				return NotFound("Don't have any stock.");
			}
			return Ok(stocks);
		}
		/// <summary>
		/// Get Stock Details 
		/// </summary>
		/// <param name="getallstockdetail"></param>
		//[ResponseCache(Duration = 600)]
		[HttpGet("getallstockdetail", Name = "GetAllStockDetail")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllStockDetails(bool status, int branchLocationId, [Optional] string barCode)
		{
			var stockList = new List<CartItemList>();
			if (!String.IsNullOrWhiteSpace(barCode))
			{
				var shopstockdetails = await _context.TransactionDetails.Include(p => p.Price).Where(s => s.IsDeleted == status && s.BranchLocationId == branchLocationId && s.Barcode == barCode).OrderBy(t => t.TransactionDetailId).GroupBy(t => new { t.ProductId, t.PriceId, t.dSalePrice, t.Barcode, t.ProductName, t.dCostPrice, ProductExpireDate = t.ProductExpireDate.Date }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dQuantity - t.dIssueQuantity), dSalePrice = p.Key.dSalePrice, Barcode = p.Key.Barcode, ProductName = p.Key.ProductName, dCostPrice = p.Key.dCostPrice, PriceId = p.Key.PriceId, ProductExpireDate = p.Key.ProductExpireDate }).Where(c => c.dNetQuantity > 0).ToListAsync();
				return Ok(shopstockdetails);
			}
			var results = await _db.LoadData<CartItemList, dynamic>("sp_GetAllStockDetails", new { branchLocationId = branchLocationId, status = status });
			stockList = results.ToList();

			if (stockList.Count() == 0)
			{
				return NotFound("Don't have any stock.");
			}
			return Ok(stockList.Where(s => s.dNetQuantity > 0));
		}
		/// <summary>
		/// Get Stock Details 
		/// </summary>
		/// <param name="getallcustomerdetail"></param>
		//[ResponseCache(Duration = 600)]
		[HttpGet("getallcustomerdetail", Name = "GetAllCustomerDetail")]
		[MapToApiVersion("1.1")]
		public async Task<IActionResult> GetAllCustomerDetails(bool status, int branchLocationId, [Optional] string barCode)
		{
			/*       if (!String.IsNullOrWhiteSpace(barCode))
				  {
					var shopstockdetails = await _context.TransactionDetails.Include(p => p.Price).Where(s => s.IsDeleted == status && s.BranchLocationId == branchLocationId && s.Barcode == barCode).GroupBy(t => new { t.ProductId, t.PriceId, t.dSalePrice, t.Barcode, t.ProductName, t.dCostPrice }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dNetQuantity), dSalePrice = p.Key.dSalePrice, Barcode = p.Key.Barcode, ProductName = p.Key.ProductName, dCostPrice = p.Key.dCostPrice, PriceId = p.Key.PriceId }).ToListAsync();
					return Ok(shopstockdetails.Where(c => c.dNetQuantity > 0));
				  } */
			//  var cachedPostsJson = await _cache.GetStringAsync("shopstocks");
			var shopstocks = await _context.MI_Employer_Employees.Where(s => s.Deleted == Convert.ToInt32(status)).Select(t => new { t.Number, t.FullName, t.CreditLimit, t.CreditBalance, t.EPFNo }).ToListAsync();
			/*      if (cachedPostsJson == null)
				 {
				   var postsJson = JsonSerializer.Serialize(shopstocks);
				   await _cache.SetStringAsync("shopstocks", postsJson);
				   return Ok(shopstocks);
				 } */
			// var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
			if (shopstocks.Count() == 0)
			{
				return NotFound("Don't have any employee.");
			}
			// var cachedPosts = JsonSerializer.Deserialize<List<ShopStockDetail>>(cachedPostsJson);
			return Ok(shopstocks);
		}
		/// <summary>
		/// Get All Payment Methods
		/// </summary>
		/// <param name="getallpaymentmethod"></param>
		[HttpGet("getallpaymentmethod", Name = "GetAllPaymentMethod")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllPaymentMethod(bool status)
		{
			var paymentMethods = _context.PaymentMethods.Where(d => d.IsDeleted == status).Select(c => new { c.MethodType, c.PaymentMethodId }).ToList();
			if (paymentMethods.Count() == 0)
			{
				return NotFound("Don't have any payment method.");
			}
			return Ok(paymentMethods);
		}
		/// <summary>
		/// Get All Payment Methods
		/// </summary>
		/// <param name="getalladjustmenttype"></param>
		[HttpGet("getalladjustmenttype", Name = "GetAllAdjustmentType")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllAdjustmentType(bool status)
		{
			var adjustmenttypes = _context.AdjustmentTypes.Where(d => d.IsDeleted == status).Select(c => new { c.AdjustmentName, c.AdjustmentTypeId }).ToList();
			if (adjustmenttypes.Count() == 0)
			{
				return NotFound("Don't have any adjustment type.");
			}
			return Ok(adjustmenttypes);
		}
		/// <summary>
		/// Get All Payment Methods
		/// </summary>
		/// <param name="getallpurchasereturn"></param>
		[HttpGet("getallpurchasereturn", Name = "getallpurchasereturn")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllPurchaseReturn(bool status, int? branchLocationId)
		{
			var purchasereturns = _context.PurchaseReturnHeaders.Where(d => d.IsDeleted == status && d.BranchLocationId == branchLocationId).Select(c => new { c.PurchaseReturnHeaderId, c.BranchLocationId, c.SupplierId, c.PurchaseReturnAmount, c.CreateDate }).ToList();
			if (purchasereturns.Count() == 0)
			{
				return NotFound("Don't have any adjustment type.");
			}
			return Ok(purchasereturns);
		}
		/// <summary>
		/// Get All Vendors 
		/// </summary>
		/// <param name="getallvendor"></param>
		[HttpGet("getallvendor", Name = "GetAllVendor")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllVendor(bool status)
		{
			var vendors = _context.Vendors.Where(b => b.IsDeleted == status).ToList();
			//  var vendorsToReturn = _mapper.Map<IEnumerable<VendorsDto>>(vendors);
			if (vendors.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(vendors);
		}
		/// <summary>
		/// Get All Suppliers 
		/// </summary>
		/// <param name="getallsupplier"></param>
		[HttpGet("getallsupplier", Name = "GetAllSupplier")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllSupplier(bool status, int? branchLocationId)
		{
			var getsuppliers = _context.Suppliers.Where(b => b.IsDeleted == status && b.BranchLocationId == branchLocationId).ToList();
			var getsuppliersToReturn = _mapper.Map<IEnumerable<SuppliersDto>>(getsuppliers);
			if (getsuppliersToReturn.Count() == 0)
			{
				return NotFound("Don't have any suppliers.");
			}
			return Ok(getsuppliersToReturn.OrderBy(x => x.SupplierName));
		}
		/// <summary>
		/// Get All Category 
		/// </summary>
		/// <param name="getallcategory"></param>
		[HttpGet("getallcategory", Name = "GetAllCategory")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllCategory(bool status)
		{
			var categories = _context.Categories.Where(b => b.IsDeleted == status).ToList();
			var categoriesToReturn = _mapper.Map<IEnumerable<CategoriesDto>>(categories);
			if (categoriesToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(categoriesToReturn);
		}
		/// <summary>
		/// Get All SubCategory 
		/// </summary>
		/// <param name="getallsubcategory"></param>
		[HttpGet("getallsubcategory", Name = "GetAllSubCategory")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllSubCategory(bool status)
		{
			var subCategories = _context.SubCategories.Where(b => b.IsDeleted == status).ToList();
			var subCategoriesToReturn = _mapper.Map<IEnumerable<SubCategoriesDto>>(subCategories);
			if (subCategoriesToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(subCategoriesToReturn);
		}
		/// <summary>
		/// Get All Unit Of Measurement 
		/// </summary>
		/// <param name="getalluOm"></param>
		[HttpGet("getalluOm", Name = "GetAllUOM")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllUOM(bool status)
		{
			var unitOfMeasurements = _context.UnitOfMeasurements.Where(b => b.IsDeleted == status).ToList();
			var unitOfMeasurementsToReturn = _mapper.Map<IEnumerable<UomsDto>>(unitOfMeasurements);
			if (unitOfMeasurementsToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(unitOfMeasurementsToReturn);
		}
		/// <summary>
		/// Get All Brand 
		/// </summary>
		/// <param name="getallbrand"></param>
		[HttpGet("getallbrand", Name = "GetAllBrand")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllBrand(bool status)
		{
			var brands = _context.Brands.Where(b => b.IsDeleted == status).ToList();
			var brandsToReturn = _mapper.Map<IEnumerable<BrandsDto>>(brands);
			if (brandsToReturn.Count() == 0)
			{
				return NotFound("Don't have any branches.");
			}
			return Ok(brandsToReturn);
		}
		/// <summary>
		/// Get All Brand 
		/// </summary>
		/// <param name="getalldepartment"></param>
		[HttpGet("getalldepartment", Name = "getalldepartment")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllDepartment(bool status)
		{
			var departments = _context.Departments.Where(b => b.IsDeleted == status).ToList();
			var departmentsToReturn = _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
			if (departmentsToReturn.Count() == 0)
			{
				return NotFound("Don't have any department.");
			}
			return Ok(departmentsToReturn);
		}
		/// <summary>
		/// Get All Section 
		/// </summary>
		/// <param name="getallsection"></param>
		[HttpGet("getallsection", Name = "getallsection")]
		[MapToApiVersion("1.1")]
		public IActionResult GetAllSection(bool status)
		{
			var sections = _context.Sections.Where(b => b.IsDeleted == status).ToList();
			var sectionsToReturn = _mapper.Map<IEnumerable<SectionListDto>>(sections);
			if (sectionsToReturn.Count() == 0)
			{
				return NotFound("Don't have any section.");
			}
			return Ok(sectionsToReturn);
		}
		/// <summary>
		/// Create Title.
		/// </summary> 
		/// <param name="createtitle"></param> 
		[HttpPost("createtitle")]
		public async Task<IActionResult> CreateTitle([FromForm] CreateTitleDto createTitleDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var createTitle = _mapper.Map<Title>(createTitleDto);
			if (await _context.Titles.AnyAsync(t => t.TitleName == createTitleDto.TitleName))
			{
				return Conflict(new
				{
					ErrorCode = 409,
					ErrorMessage = $"Duplicate Title name exists.The Title Name is {createTitleDto.TitleName}"
				});
			}
			if (createTitle != null)
			{
				var saved = false;
				// add brand
				await _context.Titles.AddAsync(createTitle);
				while (!saved)
				{
					try
					{
						// Attempt to save changes to the database
						await _context.SaveChangesAsync();
						saved = true;
					}
					catch (DbUpdateConcurrencyException e)
					{
						foreach (var entry in e.Entries)
						{
							if (entry.Entity is Vendor)
							{
								var proposedValues = entry.CurrentValues;
								var databaseValues = entry.GetDatabaseValues();

								foreach (var property in proposedValues.Properties)
								{
									var proposedValue = proposedValues[property];
									var databaseValue = databaseValues[property];

									// TODO: decide which value should be written to database
									// proposedValues[property] = <value to be saved>;
								}
								// Refresh original values to bypass next concurrency check
								entry.OriginalValues.SetValues(databaseValues);
							}
							else
							{
								throw new NotSupportedException(
									"Don't know how to handle concurrency conflicts for "
									+ entry.Metadata.Name);
							}
							return BadRequest(e.Entries);
						}
					}
					return Ok(createTitle);
					// return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
				}
			}
			return BadRequest("You've unable to create title.Please try again.");
		}
		/// <summary>
		/// Create Title.
		/// </summary> 
		/// <param name="createtypeofgoods"></param> 
		[HttpPost("createtypeofgoods")]
		public async Task<IActionResult> CreateTypeOfGoods([FromForm] CreateTypeofGoodDto createTypeofGoodDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var createTypeofGoods = _mapper.Map<TypeofGood>(createTypeofGoodDto);
			if (await _context.TypeofGoods.AnyAsync(t => t.TypeofGoods == createTypeofGoodDto.TypeofGoods))
			{
				return Conflict(new
				{
					ErrorCode = 409,
					ErrorMessage = $"Duplicate TypeOfGoods name exists.The TypeOfGoods is {createTypeofGoodDto.TypeofGoods}"
				});
			}
			if (createTypeofGoods != null)
			{
				var saved = false;
				// add brand
				await _context.TypeofGoods.AddAsync(createTypeofGoods);
				while (!saved)
				{
					try
					{
						// Attempt to save changes to the database
						await _context.SaveChangesAsync();
						saved = true;
					}
					catch (DbUpdateConcurrencyException e)
					{
						foreach (var entry in e.Entries)
						{
							if (entry.Entity is Vendor)
							{
								var proposedValues = entry.CurrentValues;
								var databaseValues = entry.GetDatabaseValues();

								foreach (var property in proposedValues.Properties)
								{
									var proposedValue = proposedValues[property];
									var databaseValue = databaseValues[property];

									// TODO: decide which value should be written to database
									// proposedValues[property] = <value to be saved>;
								}
								// Refresh original values to bypass next concurrency check
								entry.OriginalValues.SetValues(databaseValues);
							}
							else
							{
								throw new NotSupportedException(
									"Don't know how to handle concurrency conflicts for "
									+ entry.Metadata.Name);
							}
							return BadRequest(e.Entries);
						}
					}
					return Ok(createTypeofGoods);
					// return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
				}
			}
			return BadRequest("You've unable to create type of goods.Please try again.");
		}
		/// <summary>
		/// Create Title.
		/// </summary> 
		/// <param name="createpaymentmethod"></param> 
		[HttpPost("createpaymentmethod")]
		public async Task<IActionResult> CreatePaymentMethod([FromForm] CreatePaymentMethodDto createPaymentMethodDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var createPaymentMethod = _mapper.Map<PaymentMethod>(createPaymentMethodDto);
			if (await _context.PaymentMethods.AnyAsync(t => t.MethodType == createPaymentMethodDto.MethodType))
			{
				return Conflict(new
				{
					ErrorCode = 409,
					ErrorMessage = $"Duplicate TypeOfGoods name exists.The TypeOfGoods is {createPaymentMethodDto.MethodType}"
				});
			}
			if (createPaymentMethod != null)
			{
				var saved = false;
				// add brand
				await _context.PaymentMethods.AddAsync(createPaymentMethod);
				while (!saved)
				{
					try
					{
						// Attempt to save changes to the database
						await _context.SaveChangesAsync();
						saved = true;
					}
					catch (DbUpdateConcurrencyException e)
					{
						foreach (var entry in e.Entries)
						{
							if (entry.Entity is Vendor)
							{
								var proposedValues = entry.CurrentValues;
								var databaseValues = entry.GetDatabaseValues();

								foreach (var property in proposedValues.Properties)
								{
									var proposedValue = proposedValues[property];
									var databaseValue = databaseValues[property];

									// TODO: decide which value should be written to database
									// proposedValues[property] = <value to be saved>;
								}
								// Refresh original values to bypass next concurrency check
								entry.OriginalValues.SetValues(databaseValues);
							}
							else
							{
								throw new NotSupportedException(
									"Don't know how to handle concurrency conflicts for "
									+ entry.Metadata.Name);
							}
							return BadRequest(e.Entries);
						}
					}
					return Ok(createPaymentMethod);
					// return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
				}
			}
			return BadRequest("You've unable to create type of goods.Please try again.");
		}
		/// <summary>
		/// Create Title.
		/// </summary> 
		/// <param name="createpayment"></param> 
		[HttpPost("createpayment")]
		public async Task<IActionResult> CreatePayment([FromForm] CreatePaymentDto createPaymentDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var createPayment = _mapper.Map<Payment>(createPaymentDto);
			if (await _context.Payments.AnyAsync(t => t.PaymentType == createPaymentDto.PaymentType))
			{
				return Conflict(new
				{
					ErrorCode = 409,
					ErrorMessage = $"Duplicate Payment name exists.The payment name is {createPaymentDto.PaymentType}"
				});
			}
			if (createPayment != null)
			{
				var saved = false;
				// add brand
				await _context.Payments.AddAsync(createPayment);
				while (!saved)
				{
					try
					{
						// Attempt to save changes to the database
						await _context.SaveChangesAsync();
						saved = true;
					}
					catch (DbUpdateConcurrencyException e)
					{
						foreach (var entry in e.Entries)
						{
							if (entry.Entity is Vendor)
							{
								var proposedValues = entry.CurrentValues;
								var databaseValues = entry.GetDatabaseValues();

								foreach (var property in proposedValues.Properties)
								{
									var proposedValue = proposedValues[property];
									var databaseValue = databaseValues[property];

									// TODO: decide which value should be written to database
									// proposedValues[property] = <value to be saved>;
								}
								// Refresh original values to bypass next concurrency check
								entry.OriginalValues.SetValues(databaseValues);
							}
							else
							{
								throw new NotSupportedException(
									"Don't know how to handle concurrency conflicts for "
									+ entry.Metadata.Name);
							}
							return BadRequest(e.Entries);
						}
					}
					return Ok(createPayment);
					// return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
				}
			}
			return BadRequest("You've unable to create type of payment.Please try again.");
		}

		/*   [HttpPut("copyMasterData/{id}")]
		  public async Task<IActionResult> CopyMasterData(int id, bool isCopiedData)
		  {
			try
			{
			  var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"MasterData\\masterdata.json"}");
			  var JSON = System.IO.File.ReadAllText(folderDetails);
			  dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);
			  var response = 0;
			  foreach (var item in jsonObj["M203structureKind"])
			  {
				var m203StructureKind = new M203structureKind
				{
				  Code = item["Code"].ToString(),
				  Description = item["Description"].ToString(),
				  AgencyId = id,
				  IsActive = true,
				  IsDeleted = false,
				  AddedBy = User.GetUserId(),
				  ModifiedBy = User.GetUserId()
				AddedOn = DateTime.Now,
				  ModifiedOn = DateTime.Now
				};
				response = await _m203structkindService.AddAsync(m203StructureKind);
			  }
			  n response > 0 ? Ok(new { message = "Agency details added successfully." }) : StatusCode(500, "An error occured while adding agency details. Please try again...");
			}
			catch (Exception ex)
			{
			  return StatusCode(500, ResponseMessages.GetErrorMessage(ResponseMessages.MessageType.Add, ex.Message));
			}
		  } */
	}
	public class FactoryWiseSale
	{
		public string branchCode { get; set; }
		public decimal saleAmount { get; set; }
	}
	public class TopSellingProduct
	{
		public string productName { get; set; }
		public decimal productSale { get; set; }
	}
	public class DQuantityError
	{
		public decimal quantity { get; set; }
		public string barCode { get; set; }
	}
} 