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
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class StoreController : BaseController
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
        public StoreController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get Stores 
    /// </summary>
    /// <param name="getallstores"></param>
    [HttpGet("getallstores", Name = "GetAllStores")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllStores(bool status, [FromQuery] UserParams userParams)
    {
      var stores = await _repoWrapper.Store.GetStoresAsync(userParams);
      var storesToReturn = _mapper.Map<IEnumerable<StoreListDto>>(stores);
      if (storesToReturn.Count() == 0)
      {
        return BadRequest("Don't have any stores.");
      }
      switch (status)
      {
        case true:
          var resulttrue = storesToReturn.Where(p => p.IsDeleted = true);
          return Ok(resulttrue);
        case false:
          var resultfalse = storesToReturn.Where(p => p.IsDeleted = false);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(stores.CurrentPage, stores.PageSize, stores.TotalCount, stores.TotalPages);
          return Ok(storesToReturn);*/
      }
    }
    /// <summary>
    /// Get Store By Id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetStoreById")]
    public async Task<IActionResult> GetStoreById(int? id)
    {
      if (await _repoWrapper.Store.AnyAsync(b => b.StoreId == id && b.IsDeleted == false))
      {
        var store = await _repoWrapper.Store.FindByCondition(b => b.StoreId == id && b.IsDeleted == false).SingleOrDefaultAsync<Store>();
        if (store != null)
        {
          return Ok(store);
        }
      }
      return BadRequest("You've unable to fetching store details by the given id");
    }
    /// <summary>
    /// Delete Store By id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpDelete("{id}", Name = "DeleteStoreByStoreId")]
    public async Task<IActionResult> DeleteStoreByStoreId(int? id)
    {
      if (id != null && await _repoWrapper.Store.AnyAsync(p => p.StoreId == id && p.IsDeleted == false))
      {
        var store = await _repoWrapper.Store.FindByCondition(p => p.StoreId == id).SingleOrDefaultAsync<Store>();
        store.IsDeleted = true;
        _repoWrapper.Store.Update(store);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("Store doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create Store.
    /// </summary> 
    /// <param name="createbrand"></param> 
    [HttpPost("createbrand")]
    public async Task<IActionResult> CreateStore([FromForm] CreateStoreDto createStoreDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createStore = _mapper.Map<Store>(createStoreDto);
      if (await _repoWrapper.Store.FindByCondition(b => b.StoreName == createStoreDto.StoreName).AnyAsync(s => s.StoreName == createStoreDto.StoreName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate brand name exists.The brand is {createStoreDto.StoreName}"
        });
      }
      if (createStore != null)
      {
        var saved = false;
        // add brand
        await _repoWrapper.Store.CreateAsync(createStore);
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
              if (entry.Entity is Store)
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
          return Ok(createStore);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create brand.Please try again.");
    }
    /// <summary>
    /// Update Store.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStore(int? id, [FromForm] UpdateStoreDto updateStoreDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var store = await _repoWrapper.Store.FindByCondition(b => b.StoreId == id).FirstOrDefaultAsync<Store>();
      if (updateStoreDto.StoreName == null)
      {
        updateStoreDto.StoreName = store.StoreName;
      }
      if (updateStoreDto.StoreDescription == null)
      {
        updateStoreDto.StoreDescription = store.StoreDescription;
      }
      store.StoreName = updateStoreDto.StoreName;
      store.StoreDescription = updateStoreDto.StoreDescription;
      store.ModifiedDate = updateStoreDto.ModifiedDate;
      //check StoreName update
      //var isStoreModified = _context.Entry(brand).Property("StoreName").IsModified;

      if (await _repoWrapper.Store.AnyAsync(b => b.StoreId == id && b.IsDeleted == false))
      {
        _repoWrapper.Store.Update(store);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Store.Store.Any(b => b.StoreId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(store);
      }
      return BadRequest("there is no brand to update details.");
    }

  }
}