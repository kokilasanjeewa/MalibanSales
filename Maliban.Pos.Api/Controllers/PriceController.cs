using System.Collections;
using System.Linq;
using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Maliban.Pos.Api.Helpers;
using System.Collections.Generic;

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  // [AllowAnonymous]
  public class PriceController : BaseController
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
        public PriceController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get Student List Details
    /// </summary>
    /// <remarks>
    /// Note that the key is a GUID and not an integer.
    ///  
    ///     POST /Todo
    ///     {
    ///        "key": "0e7ad584-7788-4ab1-95a6-ca0a5b444cbb",
    ///        "name": "Item1",
    ///        "isComplete": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="{barcode}"></param>
    /// <returns>A fetched price</returns>
    /// <response code="200">Returns the fetched item</response>
    /// <response code="404">If the item is null</response> 
    [HttpGet("{productId}", Name = "GetPriceByBarcode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetPriceByBarcode(int? productId)
    {
      var price = await _repoWrapper.Price.FindByCondition(x => x.Product.ProductId.Equals(productId) && x.Active == true).AsNoTracking().FirstOrDefaultAsync<Price>();
      if (price != null)
      {
        return Ok(price);

      }
      return NotFound($"You've unable to find the price by the given barcode {productId}.");
    }
    /// <summary>
    /// Get Student List Details
    /// </summary>
    /// <remarks>
    /// Note that the key is a GUID and not an integer.
    ///  
    ///     POST /Todo
    ///     {
    ///        "key": "0e7ad584-7788-4ab1-95a6-ca0a5b444cbb",
    ///        "name": "Item1",
    ///        "isComplete": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="{productId}"></param>
    /// <returns>A fetched price</returns>
    /// <response code="200">Returns the fetched item</response>
    /// <response code="404">If the item is null</response> 
    [HttpDelete("{productId}", Name = "DeletePriceByBarcode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> DeletePriceByBarcode(int? productId)
    {
      if (await _repoWrapper.Price.AnyAsync(p => p.Product.ProductId == productId))
      {
        await _repoWrapper.Price.RemovePriceAsync(productId);
        await _repoWrapper.SaveAllAsync();
        return Ok($"Successfully deleted the price by the given product {productId}.");
      }
      return NotFound($"You've unable to delete the price by the given barcode {productId}.");
    }
    /// <summary>
    /// Update Price By Barcode
    /// </summary>
    /// <remarks>
    /// Note that the key is a GUID and not an integer.
    ///  
    ///     Put /Price
    ///     {
    ///        "key": "0e7ad584-7788-4ab1-95a6-ca0a5b444cbb",
    ///        "name": "Item1",
    ///        "isComplete": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="{barcode}">product barcode</param>
    /// <returns>A updated price</returns>
    /// <response code="200">Returns the fetched item</response>
    /// <response code="404">If the item is null</response> 
    [HttpPut("{productId}", Name = "UpdatePriceByBarcode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> UpdatePriceByBarcode(int productId, [FromForm] UpdatePriceDto updatePriceDto)
    {
      var updatePrice = _mapper.Map<Price>(updatePriceDto);
      var price = await _repoWrapper.Price.Price.Where(p => p.Product.ProductId == productId).FirstOrDefaultAsync<Price>();
      price.UnitPrice = updatePrice.UnitPrice;
      if (await _repoWrapper.Price.AnyAsync(p => p.Product.ProductId == productId))
      {
        _repoWrapper.Price.Update(price);
        try
        {
          await _repoWrapper.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Price.Price.Any(p => p.Product.ProductId == productId))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(price);
      }
      return BadRequest("You've unable to update product details.");
    }
    /// <summary>
    /// Create Member.
    /// </summary> 
    /// <param name="createprice"></param> 
    [HttpPost("createprice")]
    [ModelValidationAttribute]
    public async Task<IActionResult> CreatePrice([FromForm] CreatePriceDto createPriceDto)
    {
      var createPrice = _mapper.Map<Price>(createPriceDto);
      var currentPrice = await _repoWrapper.Price.Price.Where(p => p.Product.ProductId == createPriceDto.ProductId && p.Active == true).FirstOrDefaultAsync<Price>();

      if (createPrice != null)
      {
        var saved = false;
        if (currentPrice != null)
        {
          currentPrice.Active = false;
          _repoWrapper.Price.Update(currentPrice);
          try
          {
            await _repoWrapper.SaveAllAsync();
          }
          catch (DbUpdateException ex)
          {
            return BadRequest(ex.Message);
          }
        }
        await _repoWrapper.Price.CreateAsync(createPrice);
        while (!saved)
        {
          try
          {
            // Attempt to save changes to the database
            await _repoWrapper.SaveAllAsync();
            saved = true;
          }
          catch (DbUpdateConcurrencyException e)
          {
            foreach (var entry in e.Entries)
            {
              if (entry.Entity is Price)
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
          return Ok(createPrice);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }

      }
      return BadRequest(ModelState);
    }
    /// <summary>
    /// Create Member.
    /// </summary> 
    /// <param name="supplierprice/createsupplierprice"></param> 
    [HttpPost("supplierprice/createsupplierprice")]
    [ModelValidationAttribute]
    public async Task<IActionResult> CreateSupplierPrice([FromForm] CreateSupplierPriceDto createSupplierPriceDto)
    {
      var createSupplierPrice = _mapper.Map<SupplierPrice>(createSupplierPriceDto);
      var currentSupplierPrice = await _context.SupplierPrices.Where(p => p.ProductId == createSupplierPrice.ProductId && p.BranchLocationId == createSupplierPrice.BranchLocationId && p.SupplierId == createSupplierPrice.SupplierId && p.Active == true && p.UnitPrice == createSupplierPrice.UnitPrice && p.MaximumRetailPrice == createSupplierPrice.MaximumRetailPrice && p.Profit== createSupplierPrice.Profit).FirstOrDefaultAsync<SupplierPrice>();

      if (createSupplierPrice != null)
      {
        var saved = false;
        if (currentSupplierPrice != null)
        {
          currentSupplierPrice.CreateDate = DateTime.Now;
          currentSupplierPrice.ModifiedBy = createSupplierPrice.CreatedBy;
          currentSupplierPrice.ModifiedDate = DateTime.Now;
          _context.SupplierPrices.Update(currentSupplierPrice);
          try
          {
            await _repoWrapper.SaveAllAsync();
          }
          catch (DbUpdateException ex)
          {
            return BadRequest(ex.Message);
          }
          return Ok(createSupplierPrice);
        }
        await _context.SupplierPrices.AddAsync(createSupplierPrice);
        while (!saved)
        {
          try
          {
            // Attempt to save changes to the database
            await _repoWrapper.SaveAllAsync();
            saved = true;
          }
          catch (DbUpdateConcurrencyException e)
          {
            foreach (var entry in e.Entries)
            {
              if (entry.Entity is SupplierPrice)
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
          return Ok(createSupplierPrice);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }

      }
      return BadRequest(ModelState);
    }
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallsupplierprices", Name = "GetAllSupplierPrices")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllSupplierPrices(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
    {
      var supplierprices = await _repoWrapper.SupplierPrice.GetSupplierPricesAsync(userParams, BranchLocationId);
      var supplierPriceList = _mapper.Map<IEnumerable<SupplierPriceListDto>>(supplierprices);
      // Headers get 
      Request.Headers.TryGetValue("BranchLocationId", out var traceValue);
      // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
      if (supplierprices.Count() == 0)
      {
        return NoContent();
      }
      switch (status)
      {
        case true:
          var resulttrue = supplierPriceList.Where(p => p.Active == true);
          Response.AddPagination(supplierprices.CurrentPage, supplierprices.PageSize, supplierprices.TotalCount, supplierprices.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = supplierPriceList.Where(p => p.Active == false);
          Response.AddPagination(supplierprices.CurrentPage, supplierprices.PageSize, supplierprices.TotalCount, supplierprices.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(supplierprices.CurrentPage, supplierprices.PageSize, supplierprices.TotalCount, supplierprices.TotalPages);
          return Ok(supplierPriceList);*/
      }

    }
       // [Authorize(Policy = "AdminAccess")]
       [AllowAnonymous]
        [HttpGet("getallsupplierpricesuppliers", Name = "GetAllSupplierPriceSuppliers")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllSupplierPriceSuppliers(bool status,int branchLocationId)
        {
            var supplierprices = _context.SupplierPrices.Where(sp => sp.IsDeleted == status && sp.BranchLocationId == branchLocationId)
                .GroupBy(sp => new { sp.SupplierId,sp.Supplier.SupplierName }).Select(sp => new { SupplierId = sp.Key.SupplierId, SupplierName = sp.Key.SupplierName });

            // Headers get 
            Request.Headers.TryGetValue("BranchLocationId", out var traceValue);
           // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierPriceSupplierDto>>(supplierprices.AsEnumerable());
            if (supplierprices.Count() == 0)
            {
                return NoContent();
            }
            return Ok(await supplierprices.ToListAsync());
        }
        /// </remarks>
        /// <param name="SupplierPrice/{supplierId}/{branchLocationId}"></param>
        /// <returns>A fetched price</returns>
        /// <response code="200">Returns the fetched item</response>
        /// <response code="404">If the item is null</response> 
        [HttpGet("SupplierPrice/{supplierId}/{branchLocationId}", Name = "GetProductPriceBySupplierId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetProductPriceBySupplierId(int? supplierId, int? branchLocationId)
    {
      var supplierprice = await _context.SupplierPrices.Where(x => x.SupplierId.Equals(supplierId) && x.Active == true && x.BranchLocationId.Equals(branchLocationId)).Include(p => p.Product).Include(p => p.BranchLocation).Include(p => p.Supplier).Select(sp => new { sp.ProductId, sp.Product, sp.PriceDate, sp.UnitPrice, sp.MaximumRetailPrice, sp.CreateDate,sp.Profit }).OrderByDescending(sp => sp.CreateDate).ToListAsync();
      // var supplierPriceList = _mapper.Map<IEnumerable<SupplierPriceProductListDto>>(supplierprice).ToList();
      if (supplierprice.Count() > 0)
      {
        return Ok(supplierprice);

      }
      return NotFound($"You've unable to find the supplierproduct price by the given supplierId {supplierId}.");
    }
        /// </remarks>
    /// <param name="SupplierPrice/{supplierId}/{branchLocationId}/{productId}"></param>
    /// <returns>A fetched price</returns>
    /// <response code="200">Returns the fetched item</response>
    /// <response code="404">If the item is null</response> 
    [HttpGet("SupplierPrice/{supplierId}/{branchLocationId}/{productId}", Name = "GetProductPriceBySupplierProductId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetProductPriceBySupplierProductId(int? supplierId, int? branchLocationId,int? productId)
    {
      var supplierprice = await _context.SupplierPrices.Where(x => x.SupplierId.Equals(supplierId) && x.Active == true && x.BranchLocationId.Equals(branchLocationId) && x.ProductId==productId).Include(p => p.Product).Include(p => p.BranchLocation).Include(p => p.Supplier).Select(sp => new { sp.ProductId, sp.Product, sp.PriceDate, sp.UnitPrice, sp.MaximumRetailPrice, sp.CreateDate,sp.SupplierPriceId,sp.Profit }).OrderByDescending(sp => sp.CreateDate).ToListAsync();
      // var supplierPriceList = _mapper.Map<IEnumerable<SupplierPriceProductListDto>>(supplierprice).ToList();
      if (supplierprice.Count() > 0)
      {
        return Ok(supplierprice);

      }
      return NotFound($"You've unable to find the supplierproduct price by the given supplierId {supplierId}.");
    }
    /// </remarks>
    /// <param name="supplierprice/deletesupplierprice"></param>
    /// <returns>A fetched price</returns>
    /// <response code="200">Returns the fetched item</response>
    /// <response code="404">If the item is null</response> 
    [HttpPost("supplierprice/deletesupplierprice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> DeleteSupplierPrice([FromForm] DeleteSupplierPriceDto deleteSupplierPriceDto)
    {
      var supplierPrice = await _context.SupplierPrices.Where(p => p.ProductId == deleteSupplierPriceDto.ProductId && p.SupplierId == deleteSupplierPriceDto.SupplierId && p.Active == true && p.BranchLocationId == deleteSupplierPriceDto.BranchLocationId).FirstOrDefaultAsync<SupplierPrice>();
      if (supplierPrice != null)
      {
        supplierPrice.Active = false;
        supplierPrice.IsDeleted = true;
        _context.SupplierPrices.Update(supplierPrice);
        await _context.SaveChangesAsync();
        return Ok(supplierPrice);
      }
      return NotFound($"You've unable to delete the price by the given barcode {deleteSupplierPriceDto.SupplierId}.");
    }
  }
}