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
  public class RackController : BaseController
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
        public RackController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get Racks 
    /// </summary>
    /// <param name="getallracks"></param>
    [HttpGet("getallracks", Name = "GetAllRacks")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllRackss(bool status, [FromQuery] UserParams userParams)
    {
      var racks = await _repoWrapper.Rack.GetRacksAsync(userParams);
      var racksToReturn = _mapper.Map<IEnumerable<RackListDto>>(racks);
      if (racksToReturn.Count() == 0)
      {
        return BadRequest("Don't have any racks.");
      }
      switch (status)
      {
        case true:
          var resulttrue = racksToReturn.Where(p => p.IsDeleted = true);
          return Ok(resulttrue);
        case false:
          var resultfalse = racksToReturn.Where(p => p.IsDeleted = false);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(racks.CurrentPage, racks.PageSize, racks.TotalCount, racks.TotalPages);
          return Ok(racksToReturn);*/
      }
    }
    /// <summary>
    /// Get Rack By Id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetRackById")]
    public async Task<IActionResult> GetRackById(int? id)
    {
      if (await _repoWrapper.Rack.AnyAsync(b => b.RackId == id && b.IsDeleted == false))
      {
        var rack = await _repoWrapper.Rack.FindByCondition(b => b.RackId == id && b.IsDeleted == false).SingleOrDefaultAsync<Rack>();
        if (rack != null)
        {
          return Ok(rack);
        }
      }
      return BadRequest("You've unable to fetching rack details by the given id");
    }
    /// <summary>
    /// Delete Rack By id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpDelete("{id}", Name = "DeleteRackByRackId")]
    public async Task<IActionResult> DeleteRackByRackId(int? id)
    {
      if (id != null && await _repoWrapper.Rack.AnyAsync(p => p.RackId == id && p.IsDeleted == false))
      {
        var rack = await _repoWrapper.Rack.FindByCondition(p => p.RackId == id).SingleOrDefaultAsync<Rack>();
        rack.IsDeleted = true;
        _repoWrapper.Rack.Update(rack);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("Rack doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create Rack.
    /// </summary> 
    /// <param name="createbrand"></param> 
    [HttpPost("createbrand")]
    public async Task<IActionResult> CreateRack([FromForm] CreateRackDto createRackDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createRack = _mapper.Map<Rack>(createRackDto);
      if (await _repoWrapper.Rack.FindByCondition(b => b.RackName == createRackDto.RackName).AnyAsync(s => s.RackName == createRackDto.RackName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate brand name exists.The brand is {createRackDto.RackName}"
        });
      }
      if (createRack != null)
      {
        var saved = false;
        // add brand
        await _repoWrapper.Rack.CreateAsync(createRack);
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
              if (entry.Entity is Rack)
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
          return Ok(createRack);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create brand.Please try again.");
    }

    /// <summary>
    /// Update Rack.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRack(int? id, [FromForm] UpdateRackDto updateRackDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var rack = await _repoWrapper.Rack.FindByCondition(b => b.RackId == id).FirstOrDefaultAsync<Rack>();
      if (updateRackDto.RackName == null)
      {
        updateRackDto.RackName = rack.RackName;
      }
      if (updateRackDto.RackDescription == null)
      {
        updateRackDto.RackDescription = rack.RackDescription;
      }
      rack.RackName = updateRackDto.RackName;
      rack.RackDescription = updateRackDto.RackDescription;
      rack.ModifiedDate = updateRackDto.ModifiedDate;
      //check RackName update
      //var isRackModified = _context.Entry(brand).Property("RackName").IsModified;

      if (await _repoWrapper.Rack.AnyAsync(b => b.RackId == id && b.IsDeleted == false))
      {
        _repoWrapper.Rack.Update(rack);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Rack.Rack.Any(b => b.RackId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(rack);
      }
      return BadRequest("there is no brand to update details.");
    }

  }
}