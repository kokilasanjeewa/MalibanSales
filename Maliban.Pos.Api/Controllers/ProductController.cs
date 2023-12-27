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

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
 // [Authorize(Policy = "AdminAccess")]
//[AllowAnonymous]
  [Authorize]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class ProductController : BaseController
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
        public ProductController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get Products 
    /// </summary>
    /// <param name="getallproducts"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpGet("getallproducts", Name = "GetAllProducts")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllProducts(bool status, [FromQuery] UserParams userParams)
    {
      var products = await _repoWrapper.Product.GetProductsAsync(userParams);
      var productToReturn = _mapper.Map<IEnumerable<ProductListDto>>(products);
      if (productToReturn.Count() == 0)
      {
        return BadRequest("Don't have any products.");
      }
      switch (status)
      {
        case true:
          var resulttrue = productToReturn.Where(p => p.Active == true);
          Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = productToReturn.Where(p => p.Active == false);
          Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
          return Ok(productToReturn);*/
      }

    }
    /// <summary>
    /// Get Products 
    /// </summary>
    /// <param name="getallproducts1"></param>
    [HttpGet("getallproducts1")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllProducts1(bool status, [FromQuery] UserParams userParams)
    {
      var products = await _repoWrapper.Product.GetProductsAsync(userParams);
      var productToReturn = _mapper.Map<IEnumerable<ProductListDto>>(products);
      if (productToReturn.Count() > 0)
      {
        return BadRequest("Don't have any products.");
      }
      switch (status)
      {
        case true:
          productToReturn.Where(p => p.IsDeleted = true);
          return Ok(productToReturn);
        case false:
          productToReturn.Where(p => p.IsDeleted = false);
          return Ok(productToReturn);
/*        default:
          Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
          return Ok(productToReturn);*/
      }
    }
    /// <summary>
    /// Get Product By BarCode 
    /// </summary>
    /// <param name="{barcode}"></param>
    [HttpGet("{barcode}", Name = "GetProductByBarCode")]
    public async Task<IActionResult> GetProductByBarCode(string barcode)
    {
      if (await _repoWrapper.VerifyBarCode(barcode))
      {
        var products = await _repoWrapper.Product.GetProductAsync(barcode);
        //var products = _repoWrapper.Product.FindByCondition(x => x.Barcode.Equals(barcode));
        if (products != null)
        {
          return Ok(products);
        }
      }
      return BadRequest("product code not found.");
    }
        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{barcode}"></param>
        [HttpGet("ProductName/{productName}", Name = "GetProductByName")]
        public async Task<IActionResult> GetProductByName(string productName)
        {
                var products = await _repoWrapper.Product.GetProductsByNameAsync(productName);
                //var products = _repoWrapper.Product.FindByCondition(x => x.Barcode.Equals(barcode));
                if (products != null)
                {
                    return Ok(products);
                }
            
            return BadRequest("You've You've unable to fetching product details by the given  product barcode");
        }
        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="Id/{id}"></param>
        [HttpGet("Id/{id}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById(int? id)
    {
      if (await _repoWrapper.Product.AnyAsync(p => p.ProductId == id))
      {
        var products = await _repoWrapper.Product.FindByCondition(p => p.ProductId == id).Include(v => v.Brand).
        Include(v => v.Vendor).Include(c => c.Category).Include(c => c.SubCategory).Include(c => c.UnitOfMeasurement).FirstOrDefaultAsync();
        //var products = _repoWrapper.Product.FindByCondition(x => x.Barcode.Equals(barcode));
        if (products != null)
        {
          return Ok(products);
        }
      }
      return BadRequest("You've You've unable to fetching product details by the given  id");
    }
    /// <summary>
    /// Delete Product By BarCode 
    /// </summary>
    /// <param name="{barcode}"></param>
    [HttpDelete("{barcode}", Name = "DeleteProductByBarCode")]
    public async Task<IActionResult> DeleteProductByBarCode(string barcode)
    {
      if (await _repoWrapper.VerifyBarCode(barcode))
      {
        var products = await _repoWrapper.Product.RemoveProductAsync(barcode);
        await _repoWrapper.SaveAllAsync();
        return Ok(products);
      }
      return BadRequest($"You've unable to delete product details by the given  product barcode {barcode} ");
    }
    /// <summary>
    /// Create Member.
    /// </summary> 
    /// <param name="createproduct"></param> 
    [HttpPost("createproduct")]
    [ModelValidationAttribute]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
    {
      var createProduct = _mapper.Map<Product>(createProductDto);
      createProduct.Vendor = await _context.Vendors.FindAsync(createProductDto.VendorId);
      if (await _repoWrapper.Product.Product.AnyAsync(p => p.Barcode == createProduct.Barcode))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"You've duplicate product exists.The product name is {createProduct.ProductName}"
        });
      }
      if (createProduct != null)
      {
        var saved = false;
        await _repoWrapper.Product.CreateAsync(createProduct);
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
          return Ok(createProduct);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've You've unable to create product.Please try again.");
    }
    /*   /// <summary>
      /// Update Product.
      /// </summary> 
      /// <param name="{barcode}">Update Product</param> 
      //https://code-maze.com/net-core-web-development-part14/
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
      } */
    /// <summary>
    /// Update Product.
    /// </summary> 
    /// <param name="{id}">Update Product</param> 
    //https://code-maze.com/net-core-web-development-part14/
    [HttpPut("{id}")]
    [ModelValidationAttribute]
    public async Task<IActionResult> UpdateProduct(Int64 id, [FromForm] UpdateProductDto updateProductDto)
    {
      var entity = await _context.Products.Where(s => s.ProductId == id).SingleAsync<Product>();
      if (entity == null)
      {
        return BadRequest("You've You've unable to find record with id '" + id + "'.");
      }
      // Update changes if any of the properties have been modified.
      _context.Entry(entity).CurrentValues.SetValues(updateProductDto);
      _context.Entry(entity).State = EntityState.Modified;
      try
      {

        if (_context.Entry(entity).Properties.Any(property => property.IsModified))
        {
          await _context.SaveChangesAsync();
        }
      }
      catch (DbUpdateConcurrencyException ex)
      {
        //return BadRequest(e);
        return Conflict(ex);
      }
      return Ok(entity);
     /*  var product = await _repoWrapper.Product.FindByCondition(p => p.ProductId == id).FirstOrDefaultAsync<Product>();
      product.Barcode = updateProductDto.Barcode;
      if (await _repoWrapper.Product.AnyAsync(p => p.ProductId == id))
      {
        _mapper.Map(updateProductDto, product);
        _repoWrapper.Product.Update(product);
        try
        {
          await _repoWrapper.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Product.AnyAsync(p => p.ProductId == id).Wait(0))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(product);
      }
      return BadRequest("You've unable to update product details."); */
    }

  }
}