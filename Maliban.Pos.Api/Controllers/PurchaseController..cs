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
 // [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class PurchaseController : BaseController
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
    private PurchaseRequest cart;

        // private IDistributedCache cache;

        [Obsolete]
        public PurchaseController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, PurchaseRequest cartService, IDistributedCache cache) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
      //  this.cache = cache;
    }
    #endregion
    /// <summary>
    /// Get Stocks 
    /// </summary>
    /// <param name="getallpurchaserequestheaders"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallpurchaserequestheaders", Name = "GetAllPurchaseRequestHeaders")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllPurchaseRequestHeaders(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
    {
      var purchaseRequestHeaders = await _repoWrapper.PurchaseRequestHeader.GetPurchaseRequestHeadersAsync(userParams, BranchLocationId);
      // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
         //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
      if (purchaseRequestHeaders.Count() == 0)
      {
        return NoContent();
      }
      switch (status)
      {
        case true:
          var resulttrue = purchaseRequestHeaders.Where(p => p.Active = true);
          Response.AddPagination(purchaseRequestHeaders.CurrentPage, purchaseRequestHeaders.PageSize, purchaseRequestHeaders.TotalCount, purchaseRequestHeaders.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = purchaseRequestHeaders.Where(p => p.Active = false);
          Response.AddPagination(purchaseRequestHeaders.CurrentPage, purchaseRequestHeaders.PageSize, purchaseRequestHeaders.TotalCount, purchaseRequestHeaders.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(purchaseRequestHeaders.CurrentPage, purchaseRequestHeaders.PageSize, purchaseRequestHeaders.TotalCount, purchaseRequestHeaders.TotalPages);
          return Ok(purchaseRequestHeaders);*/
      } 
    }
    /// <summary>
    /// Get Stocks 
    /// </summary>
    /// <param name="getallpurchaserequestdetails"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallpurchaserequestdetails", Name = "GetAllPurchaseRequestDetails")]
    [MapToApiVersion("1.1")]
    public  IActionResult GetAllPurchaseRequestDetails(bool status, [FromQuery] UserParams userParams, int purchaseRequestHeaderId)
    {
      var purchaseRequestDetails = _repoWrapper.PurchaseRequestDetail.GetPurchaseRequestDetailsAsync(userParams, purchaseRequestHeaderId);
      // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
         //   var purchaseRequestHeaders = from c in _context.PurchaseRequestDetails select new{c.ProductId,c.Product};
      if (purchaseRequestDetails.Count() == 0)
      {
        return NoContent();
      }
      switch (status)
      {
        case true:
          var resulttrue = purchaseRequestDetails.Where(p => p.Active = true);
          Response.AddPagination(purchaseRequestDetails.CurrentPage, purchaseRequestDetails.PageSize, purchaseRequestDetails.TotalCount, purchaseRequestDetails.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = purchaseRequestDetails.Where(p => p.Active = false);
          Response.AddPagination(purchaseRequestDetails.CurrentPage, purchaseRequestDetails.PageSize, purchaseRequestDetails.TotalCount, purchaseRequestDetails.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(purchaseRequestDetails.CurrentPage, purchaseRequestDetails.PageSize, purchaseRequestDetails.TotalCount, purchaseRequestDetails.TotalPages);
          return Ok(purchaseRequestDetails);*/
      } 
    }
    /// <summary>
    ///  Create SaleDetailDto.
    /// </summary> 
    /// <param name="createpurchaserequestitemdetails"></param> 
    [HttpPost("createpurchaserequestitemdetails")]
    public async Task<IActionResult> CreatePurchaseRequestItemDetails([FromForm] CreatePurchaseRequestDetailsDto createSaleItemDetailsDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      if (await _repoWrapper.VerifyBarCode(createSaleItemDetailsDto.barCode))
      {
        var product = await _repoWrapper.Product.GetProductAsync(createSaleItemDetailsDto.barCode);
        cart.AddItem(product, createSaleItemDetailsDto.quantity);
        SaveCart(cart);
      }
      else
      {
        return NotFound("Barcode doesn't exists.");
      }
      PurchaseRequest cart1 = GetSale();
      // object to string
      string value = JsonConvert.SerializeObject(cart1);
      // string set to cache the name is cart1
      //   cache.SetString("cart1", value);
      // retrun the cart1 object
      return Ok(cart1);
    }
      /// <summary>
    /// Remove SaleDetailDto.
    /// </summary> 
    /// <param name="removepurchaserequestitemdetails"></param> 
    [Authorize(Policy = "AdminAccess")]
    [HttpPost("removepurchaserequestitemdetails")]
    public async Task<IActionResult> RemovePurchaseRequestItemDetails([FromForm] RemoveSaleDetailDto removeSaleDetailDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      if (await _repoWrapper.VerifyBarCode(removeSaleDetailDto.barCode))
      {
        var product = await _repoWrapper.Product.GetProductAsync(removeSaleDetailDto.barCode);
        cart.RemoveLine(product);
        SaveCart(cart);
      }
      else
      {
        return NotFound("Barcode doesn't exists.");
      }
      PurchaseRequest cart1 = GetSale();
      // object to string
      string value = JsonConvert.SerializeObject(cart1);
      // string set to cache the name is cart1
      //    cache.SetString("cart1", value);
      // retrun the cart1 object
      return Ok(cart1);
    }
    /// <summary>
    /// You can call checkout method.Please make sure purchaserequest details already exists in the list
    /// </summary> 
    /// <param name="Check/createpurchaserequestheader"></param> 
    [HttpPost("Check/createpurchaserequestheader")]
    public async Task<IActionResult> CreatePurchaseRequestHeader([FromForm] CreatePurchaseRequestHeaderDto createPurchaseRequestHeaderDto)
    {
      int result;
      if (!ModelState.IsValid)
      {
        BadRequest(ModelState);
      }
      PurchaseRequest cart1 = GetSale();
      var saleitem = _mapper.Map<PurchaseRequestHeader>(createPurchaseRequestHeaderDto);
      saleitem.PurchaseRequestDetails = cart1.Lines;
      if (saleitem != null && cart1.Lines.Count > 0)
      {
        _context.AttachRange(saleitem.PurchaseRequestDetails);
        await _context.PurchaseRequestHeaders.AddAsync(saleitem);

        try
        {
          // Attempt to save changes to the database
          result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
          return BadRequest(e);
        }
        if (result > 0)
        {
          cart.Clear();
        }
        return Ok(cart.Lines);
        // retrun the cart1 object
        // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
      }
      //get session data
     // return BadRequest("Please check the order details.");
        return NoContent();
    }
    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<PurchaseRequest>> Get()
    {
      PurchaseRequest cart1 = GetSale();
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
      PurchaseRequest cart2 = GetSale();
      return Ok(cart2);
    }
    /// <summary>
    /// Test .
    /// </summary> 
    /// <param name="clear"></param> 
    [HttpGet("clear")]
    public ActionResult Clear()
    {
      PurchaseRequest cart2 = GetSale();
      if (cart2.Lines.Count > 0)
      {
        HttpContext.Session.Clear();
        return Ok("Clear Cart Successfully.");
      }
      return NoContent();
    }
    #region initial method
    private PurchaseRequest GetSale()
    {
      //get object in the session storage 
      PurchaseRequest cart = HttpContext.Session.GetJson<PurchaseRequest>("PurchaseRequest") ?? new PurchaseRequest();
      return cart;
    }
    private void SaveCart(PurchaseRequest cart)
    {
      // store object in session
      HttpContext.Session.SetJson("PurchaseRequest", cart);
    }

    #endregion
  }

  static class Extensions
  {
    public static bool HasMoreThanThreeElements<T>(this IEnumerable<T> enumerable)
    {
      return enumerable.Count() > 3;
    }
  }
}