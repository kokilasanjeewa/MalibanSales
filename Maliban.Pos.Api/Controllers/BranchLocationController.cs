using System;
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
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Maliban.Pos.Api.Controllers
{
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class BranchLocationController : BaseController
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
        public BranchLocationController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get BranchLocations 
    /// </summary>
    /// <param name="getallbranchlocations"></param>
    [HttpGet("getallbranchlocations", Name = "GetAllBranchLocations")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllBranchLocations(bool status, [FromQuery] UserParams userParams)
    {
      var branchlocations = await _repoWrapper.BranchLocation.GetBranchLocationsAsync(userParams);
      var branchlocationToReturn = _mapper.Map<IEnumerable<BranchLocationListDto>>(branchlocations);
      if (branchlocationToReturn.Count() == 0)
      {
        return BadRequest("Don't have any products.");
      }
      switch (status)
      {
        case true:
          var resulttrue = branchlocationToReturn.Where(p => p.IsDeleted = true);
          return Ok(resulttrue);
        case false:
          var resultfalse = branchlocationToReturn.Where(p => p.IsDeleted = false);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(branchlocations.CurrentPage, branchlocations.PageSize, branchlocations.TotalCount, branchlocations.TotalPages);
          return Ok(branchlocationToReturn);*/
      }

    }

    /// <summary>
    /// Get Product By BarCode 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetBranchLocationByBranchLocationId")]
    public async Task<IActionResult> GetBranchLocationByBranchLocationId(int? id)
    {
      if (await _repoWrapper.BranchLocation.AnyAsync(b => b.BranchLocationId == id && b.IsDeleted == false))
      {
        var branchlocation = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == id).SingleOrDefaultAsync<BranchLocation>();
        if (branchlocation != null)
        {
          return Ok(branchlocation);
        }
      }
      return BadRequest("You've unable to fetching branchlocation details by the given id");
    }
    /// <summary>
    /// Delete BranchLocation By id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpDelete("{id}", Name = "DeleteBranchLocationByBranchLocationId")]
    public async Task<IActionResult> DeleteBranchLocationByBranchLocationId(int? id)
    {
      if (id != null && await _repoWrapper.BranchLocation.AnyAsync(p => p.BranchLocationId == id && p.IsDeleted == false))
      {
        var branchlocation = await _repoWrapper.BranchLocation.FindByCondition(p => p.BranchLocationId == id).SingleOrDefaultAsync();
        branchlocation.IsDeleted = true;
        _repoWrapper.BranchLocation.Update(branchlocation);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("BranchLocation doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create BranchLocation.
    /// </summary> 
    /// <param name="createbranchlocation"></param> 
    [HttpPost("createbranchlocation")]
    public async Task<IActionResult> CreateBranchLocation([FromForm] CreateBranchLocationDto createBranchLocationDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createBranchLocation = _mapper.Map<BranchLocation>(createBranchLocationDto);
      if (await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchCode == createBranchLocationDto.BranchCode).AnyAsync(s => s.BranchCode == createBranchLocationDto.BranchCode))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate branchlocation name exists.The branchlocation is {createBranchLocationDto.BranchCode}"
        });
      }
      if (createBranchLocation != null)
      {
        var saved = false;
        // add branchlocation
        await _repoWrapper.BranchLocation.CreateAsync(createBranchLocation);
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
              if (entry.Entity is BranchLocation)
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
          return Ok(createBranchLocation);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create branchlocation.Please try again.");
    }

    /// <summary>
    /// Update BranchLocation.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranchLocation(int? id, [FromForm] UpdateLocationDto updateLocationDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var branchlocation = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == id).FirstOrDefaultAsync<BranchLocation>();
      if (updateLocationDto.BranchCode == null)
      {
        updateLocationDto.BranchCode = branchlocation.BranchCode;
      }
      if (updateLocationDto.BranchName == null)
      {
        updateLocationDto.BranchName = branchlocation.BranchName;
      }
      branchlocation.BranchCode = updateLocationDto.BranchCode;
      branchlocation.BranchName = updateLocationDto.BranchName;
      branchlocation.ModifiedDate = updateLocationDto.ModifiedDate;
      branchlocation.ModifiedBy = updateLocationDto.ModifiedBy;

      //check BranchLocationName update
      //var isBranchLocationModified = _context.Entry(branchlocation).Property("BranchLocationName").IsModified;

      if (await _repoWrapper.BranchLocation.AnyAsync(b => b.BranchLocationId == id && b.IsDeleted == false))
      {
        _repoWrapper.BranchLocation.Update(branchlocation);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.BranchLocation.BranchLocation.Any(b => b.BranchLocationId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(branchlocation);
      }
      return BadRequest("there is no branchlocation to update details.");
    }
  }
}