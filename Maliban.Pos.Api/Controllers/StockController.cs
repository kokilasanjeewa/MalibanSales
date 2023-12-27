using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class StockController : BaseController
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
        public StockController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// <param name="getallstocks"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallstocks", Name = "GetAllStocks")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllStocks(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
    {
      var shopstocks = await _repoWrapper.ShopStock.GetShopStocksAsync(userParams, BranchLocationId);

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
/*        default:
          Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
          return Ok(shopstocks);*/
      }

    }
    /// <summary>
    /// Get Stocks 
    /// </summary>
    /// <param name="getallstockdetails"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallstockdetails", Name = "GetAllStockDetails")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllStockDetails(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
    {
      var shopstocks = await _repoWrapper.ShopStock.GetStockDetailsAsync(userParams, BranchLocationId);

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
/*        default:
          Response.AddPagination(shopstocks.CurrentPage, shopstocks.PageSize, shopstocks.TotalCount, shopstocks.TotalPages);
          return Ok(shopstocks);*/
      }

    }

  }
}