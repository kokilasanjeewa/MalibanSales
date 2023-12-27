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
  public class StoreAreaController : BaseController
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
        public StoreAreaController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get StoreAreas 
    /// </summary>
    /// <param name="getallstoreareas"></param>
    [HttpGet("getallstoreareas", Name = "GetAllStoreAreass")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllStoreAreass(bool status, [FromQuery] UserParams userParams)
    {
      var storeareas = await _repoWrapper.StoreArea.GetStoreAreasAsync(userParams);
      var storeareasToReturn = _mapper.Map<IEnumerable<StoreAreaListDto>>(storeareas);
      if (storeareasToReturn.Count() == 0)
      {
        return BadRequest("Don't have any storeareas.");
      }
      switch (status)
      {
        case true:
          var resulttrue = storeareasToReturn.Where(p => p.IsDeleted = true);
          return Ok(resulttrue);
        case false:
          var resultfalse = storeareasToReturn.Where(p => p.IsDeleted = false);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(storeareas.CurrentPage, storeareas.PageSize, storeareas.TotalCount, storeareas.TotalPages);
          return Ok(storeareasToReturn);*/
      }
    }
    /// <summary>
    /// Get StoreArea By Id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetStoreAreaById")]
    public async Task<IActionResult> GetStoreAreaById(int? id)
    {
      if (await _repoWrapper.StoreArea.AnyAsync(b => b.StoreAreaId == id && b.IsDeleted == false))
      {
        var storearea = await _repoWrapper.StoreArea.FindByCondition(b => b.StoreAreaId == id && b.IsDeleted == false).SingleOrDefaultAsync<StoreArea>();
        if (storearea != null)
        {
          return Ok(storearea);
        }
      }
      return BadRequest("You've unable to fetching storearea details by the given id");
    }
    /// <summary>
    /// Delete StoreArea By id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpDelete("{id}", Name = "DeleteStoreAreaByStoreAreaId")]
    public async Task<IActionResult> DeleteStoreAreaByStoreAreaId(int? id)
    {
      if (id != null && await _repoWrapper.StoreArea.AnyAsync(p => p.StoreAreaId == id && p.IsDeleted == false))
      {
        var storearea = await _repoWrapper.StoreArea.FindByCondition(p => p.StoreAreaId == id).SingleOrDefaultAsync<StoreArea>();
        storearea.IsDeleted = true;
        _repoWrapper.StoreArea.Update(storearea);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("StoreArea doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create StoreArea.
    /// </summary> 
    /// <param name="createbrand"></param> 
    [HttpPost("createbrand")]
    public async Task<IActionResult> CreateStoreArea([FromForm] CreateStoreAreaDto createStoreAreaDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createStoreArea = _mapper.Map<StoreArea>(createStoreAreaDto);
      if (await _repoWrapper.StoreArea.FindByCondition(b => b.StoreAreaName == createStoreAreaDto.StoreAreaName).AnyAsync(s => s.StoreAreaName == createStoreAreaDto.StoreAreaName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate brand name exists.The brand is {createStoreAreaDto.StoreAreaName}"
        });
      }
      if (createStoreArea != null)
      {
        var saved = false;
        // add brand
        await _repoWrapper.StoreArea.CreateAsync(createStoreArea);
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
              if (entry.Entity is StoreArea)
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
          return Ok(createStoreArea);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create brand.Please try again.");
    }

    /// <summary>
    /// Update StoreArea.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStoreArea(int? id, [FromForm] UpdateStoreAreaDto updateStoreAreaDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var storearea = await _repoWrapper.StoreArea.FindByCondition(b => b.StoreAreaId == id).FirstOrDefaultAsync<StoreArea>();
      if (updateStoreAreaDto.StoreAreaName == null)
      {
        updateStoreAreaDto.StoreAreaName = storearea.StoreAreaName;
      }
      if (updateStoreAreaDto.StoreAreaDescription == null)
      {
        updateStoreAreaDto.StoreAreaDescription = storearea.StoreAreaDescription;
      }
      storearea.StoreAreaName = updateStoreAreaDto.StoreAreaName;
      storearea.StoreAreaDescription = updateStoreAreaDto.StoreAreaDescription;
      storearea.ModifiedDate = updateStoreAreaDto.ModifiedDate;
      //check StoreAreaName update
      //var isStoreAreaModified = _context.Entry(brand).Property("StoreAreaName").IsModified;

      if (await _repoWrapper.StoreArea.AnyAsync(b => b.StoreAreaId == id && b.IsDeleted == false))
      {
        _repoWrapper.StoreArea.Update(storearea);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.StoreArea.StoreArea.Any(b => b.StoreAreaId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(storearea);
      }
      return BadRequest("there is no brand to update details.");
    }

  }
}