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
  [Route("api/[controller]")]
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class VendorController : BaseController
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
        public VendorController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get Vendors 
    /// </summary>
    /// <param name="getallvendors"></param>
    [HttpGet("getallvendors", Name = "GetAllVendors")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllVendorss(bool status, [FromQuery] UserParams userParams)
    {
      var vendors = await _repoWrapper.Vendor.GetVendorsAsync(userParams);
      var vendorsToReturn = _mapper.Map<IEnumerable<VendorListDto>>(vendors);
      if (vendorsToReturn.Count() == 0)
      {
        return BadRequest("Don't have any vendors.");
      }
      switch (status)
      {
        case true:
          var resulttrue = vendorsToReturn.Where(p => p.IsDeleted = true);
          return Ok(resulttrue);
        case false:
          var resultfalse = vendorsToReturn.Where(p => p.IsDeleted = false);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(vendors.CurrentPage, vendors.PageSize, vendors.TotalCount, vendors.TotalPages);
          return Ok(vendorsToReturn);*/
      }
    }
    /// <summary>
    /// Get Vendor By Id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetVendorById")]
    public async Task<IActionResult> GetVendorById(int? id)
    {
      if (await _repoWrapper.Vendor.AnyAsync(b => b.VendorId == id && b.IsDeleted == false))
      {
        var vendor = await _repoWrapper.Vendor.FindByCondition(b => b.VendorId == id && b.IsDeleted == false).SingleOrDefaultAsync<Vendor>();
        if (vendor != null)
        {
          return Ok(vendor);
        }
      }
      return BadRequest("You've unable to fetching vendor details by the given id");
    }
    /// <summary>
    /// Delete Vendor By id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpDelete("{id}", Name = "DeleteVendorByVendorId")]
    public async Task<IActionResult> DeleteVendorByVendorId(int? id)
    {
      if (id != null && await _repoWrapper.Vendor.AnyAsync(p => p.VendorId == id && p.IsDeleted == false))
      {
        var vendor = await _repoWrapper.Vendor.FindByCondition(p => p.VendorId == id).SingleOrDefaultAsync<Vendor>();
        vendor.IsDeleted = true;
        _repoWrapper.Vendor.Update(vendor);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("Vendor doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create Vendor.
    /// </summary> 
    /// <param name="createbrand"></param> 
    [HttpPost("createbrand")]
    public async Task<IActionResult> CreateVendor([FromForm] CreateVendorDto createVendorDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createVendor = _mapper.Map<Vendor>(createVendorDto);
      if (await _repoWrapper.Vendor.FindByCondition(b => b.VendorName == createVendorDto.VendorName).AnyAsync(s => s.VendorName == createVendorDto.VendorName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate brand name exists.The brand is {createVendorDto.VendorName}"
        });
      }
      if (createVendor != null)
      {
        var saved = false;
        // add brand
        await _repoWrapper.Vendor.CreateAsync(createVendor);
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
          return Ok(createVendor);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create brand.Please try again.");
    }

    /// <summary>
    /// Update Vendor.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendor(int? id, [FromForm] UpdateVendorDto updateVendorDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var vendor = await _repoWrapper.Vendor.FindByCondition(b => b.VendorId == id).FirstOrDefaultAsync<Vendor>();
      if (updateVendorDto.VendorName == null)
      {
        updateVendorDto.VendorName = vendor.VendorName;
      }
      if (updateVendorDto.VendorDescription == null)
      {
        updateVendorDto.VendorDescription = vendor.VendorDescription;
      }
      vendor.VendorName = updateVendorDto.VendorName;
      vendor.VendorDescription = updateVendorDto.VendorDescription;
      vendor.ModifiedBy = updateVendorDto.ModifiedBy;
      //check VendorName update
      //var isVendorModified = _context.Entry(brand).Property("VendorName").IsModified;

      if (await _repoWrapper.Vendor.AnyAsync(b => b.VendorId == id && b.IsDeleted == false))
      {
        _repoWrapper.Vendor.Update(vendor);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Vendor.Vendors.Any(b => b.VendorId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(vendor);
      }
      return BadRequest("there is no brand to update details.");
    }
  }
}