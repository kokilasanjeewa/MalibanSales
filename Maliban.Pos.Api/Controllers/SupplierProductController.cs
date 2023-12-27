using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
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
using Maliban.Pos.Api.Helper;
using System.Collections.Generic;
using Maliban.Pos.Api.Helpers;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dapper;
using System.Data;

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class SupplierProductController : BaseController
  {
    private readonly SmtpSettings _smtpSettings;
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly DapperContext _dpContext;
    private readonly ILog logger;
    private readonly IRepositoryWrapper _repoWrapper;

        [Obsolete]
        public SupplierProductController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper,DapperContext dpContext) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
    {
      _config = config;
      _userManager = userManager;
      _signinManager = signinManager;
      _mapper = mapper;
      _context = context;
      this.logger = logger;
      _smtpSettings = smtpSettings.Value;
      _repoWrapper = repoWrapper;
      _dpContext = dpContext;
    }
   /// <summary>
    /// Get Product By SupplierId 
    /// </summary>
    /// <param name="FilterProduct/{supplierid}/{branchLocationId}"></param>
    [HttpGet("FilterProduct/{supplierid}/{branchLocationId}", Name = "GetProductFilterBySupplierId")]
    public async Task<IActionResult> GetProductFilterBySupplierId(int? supplierid, int? branchLocationId)
    {
          try
            {
                using (var connection = _dpContext.CreateConnection())
                {
                    var procedureName1 = "sp_GetProductBySupplierFilterByProduct";
                    var parameters = new DynamicParameters();
                    parameters.Add("supplierId", supplierid, DbType.Int32);
                    parameters.Add("branchLocationId", branchLocationId, DbType.Int32);
                    var companies = (await connection.QueryAsync<GetProductFilterBySupplierId>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
                    .ConfigureAwait(false)).AsList();
                    return Ok(companies.ToList());
                }

            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
    }
    /// <summary>
    /// Get Product By BarCode 
    /// </summary>
    /// <param name="{supplierid}/{branchLocationId}"></param>
    [HttpGet("{supplierid}/{branchLocationId}", Name = "GetProductBySupplierId")]
    public async Task<IActionResult> GetProductBySupplierId(int? supplierid, int? branchLocationId)
    {
      if (await _repoWrapper.SupplierProduct.AnyAsync(c => c.SupplierId == supplierid && c.BranchLocationId == branchLocationId))
      {
        var products = await _repoWrapper.SupplierProduct.FindByCondition(c => c.SupplierId == supplierid && c.BranchLocationId == branchLocationId).Select(s => new { s.Supplier, s.Product, s.BranchLocation }).AsNoTracking().ToListAsync();
        //var products = _repoWrapper.Product.FindByCondition(x => x.Barcode.Equals(barcode));
        if (products != null)
        {
          return Ok(products);
        }

      }
      if (!await _repoWrapper.SupplierProduct.AnyAsync(c => c.SupplierId == supplierid && c.BranchLocationId == branchLocationId))
      {
        return NoContent();
      }
      return BadRequest("You've You've unable to fetching products by the given  supplierid");
    }
    /// <summary>
    /// Create Member.
    /// </summary> 
    /// <param name="createsupplierproduct"></param> 
    [HttpPost("createsupplierproduct")]
    [ModelValidationAttribute]
    public async Task<IActionResult> CreateSupplierProduct([FromForm] CreateSupplierProductDto createSupplierProductDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createSupplierProduct = _mapper.Map<SupplierProduct>(createSupplierProductDto);
      if (await _repoWrapper.SupplierProduct.SupplierProduct.AnyAsync(p => p.SupplierId == createSupplierProduct.SupplierId && p.ProductId == createSupplierProduct.ProductId && p.BranchLocationId == createSupplierProduct.BranchLocationId))
      {
        var product = await _context.Products.FindAsync(createSupplierProduct.ProductId);
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"You've already entered product.The product code is {product.Barcode}"
        });
      }
      if (createSupplierProduct != null)
      {
        var product = await _context.Products.FindAsync(createSupplierProduct.ProductId);
        var supplier = await _context.Suppliers.FindAsync(createSupplierProduct.SupplierId);
        var branch = await _context.BranchLocations.FindAsync(createSupplierProduct.BranchLocationId);

        createSupplierProduct.Product = product;
        createSupplierProduct.Supplier = supplier;
        createSupplierProduct.BranchLocation = branch;

        var saved = false;
        await _repoWrapper.SupplierProduct.CreateAsync(createSupplierProduct);
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
              if (entry.Entity is Product)
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
          return Ok(createSupplierProduct);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've You've unable to create supplier product.Please try again.");
    }
    /// <summary>
    /// Delete SupplierProduct 
    /// </summary>
    /// <param name="deletesupplierproduct"></param>
    [HttpPost("deletesupplierproduct")]
    [ModelValidationAttribute]
    public async Task<IActionResult> DeleteSupplierProduct([FromForm] DeleteSupplierProductDto deleteSupplierProductDto)
    {
      var deleteSupplierProduct = _mapper.Map<SupplierProduct>(deleteSupplierProductDto);
      if (await _repoWrapper.SupplierProduct.SupplierProduct.AnyAsync(p => p.SupplierId == deleteSupplierProduct.SupplierId && p.ProductId == deleteSupplierProduct.ProductId && p.BranchLocationId == deleteSupplierProduct.BranchLocationId))
      {
        _repoWrapper.SupplierProduct.Delete(deleteSupplierProduct);
        await _repoWrapper.SaveAllAsync();
        return Ok(deleteSupplierProduct);
      }
      return BadRequest($"You've You've unable to delete product details by the given  BarCode {deleteSupplierProduct} ");
    }
    /*
    /// <summary>
    /// Update Product.
    /// </summary> 
    /// <param name="{barcode}">Update Product</param> 

    [HttpPut("{barcode}")]
    [ModelValidationAttribute]
    public async Task<IActionResult> UpdateProduct(string barcode, [FromForm] UpdateProductDto updateProductDto)
    {
      var updateProduct = _mapper.Map<Product>(updateProductDto);
      var product = await _repoWrapper.Product.Product.Where(p => p.Barcode == barcode).FirstOrDefaultAsync<Product>();
      product.Barcode = updateProduct.Barcode;
      if (await _repoWrapper.VerifyBarCode(barcode))
      {
        _repoWrapper.Product.Update(product);
        try
        {
          await _repoWrapper.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.VerifyBarCode(barcode).Wait(0))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(product);
      }
      return BadRequest("You've unable to update product details.");
    }
*/
  }
}