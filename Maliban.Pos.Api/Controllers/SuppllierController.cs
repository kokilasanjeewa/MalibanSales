using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Maliban.Pos.Api.Filters;

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
  [Authorize]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class SupplierController : BaseController
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
        private readonly IHostingEnvironment _hostingEnviroment;

        [Obsolete]
        public SupplierController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
    {
      _config = config;
      _userManager = userManager;
      _signinManager = signinManager;
      _mapper = mapper;
      _context = context;
      this.logger = logger;
      _smtpSettings = smtpSettings.Value;
      _repoWrapper = repoWrapper;
      _hostingEnviroment = hostingEnviroment;
    }
    /// <summary>
    /// Get Suppliers 
    /// </summary>
    /// <param name="getallsuppliers"></param>
  //  [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
    [Authorize(Policy = "UserAccess")]
    [HttpGet("getallsuppliers", Name = "GetAllSuppliers")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllSuppliers(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
    {
      var suppliers = await _repoWrapper.Supplier.GetSuppliersAsync(userParams, BranchLocationId);
      // Headers get 
      Request.Headers.TryGetValue("BranchLocationId", out var traceValue);

      // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
      if (suppliers.Count() == 0)
      {
        return NoContent();
      }
      switch (status)
      {
        case true:
          var resulttrue = suppliers.Where(p => p.Active == true);
          Response.AddPagination(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = suppliers.Where(p => p.Active == false);
          Response.AddPagination(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);
          return Ok(suppliers);*/
      }

    }
    /// <summary>
    /// Get Supplier By Id 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetSupplierById")]
    public async Task<IActionResult> GetSupplierById(int? id)
    {
      if (await _repoWrapper.Supplier.AnyAsync(b => b.SupplierId == id && b.IsDeleted == false))
      {
        var supplier = await _repoWrapper.Supplier.GetSupplierAsync(id);
        if (supplier != null)
        {
          return Ok(supplier);
        }
      }
      return NotFound("You've You've unable to fetching the supplier by the given id");
    }
    /// <summary>
    /// Delete Supplier By id 
    /// </summary>
    /// <param name="{id}"></param>
    [Authorize(Policy = "AdminAccess")]
    [HttpDelete("{id}", Name = "DeleteSupplierByBrandId")]
    public async Task<IActionResult> DeleteSupplierById(int? id)
    {
      if (id != null && await _repoWrapper.Supplier.AnyAsync(p => p.SupplierId == id && p.IsDeleted == false))
      {
        var supplier = await _repoWrapper.Supplier.FindByCondition(p => p.SupplierId == id).SingleOrDefaultAsync();
        supplier.IsDeleted = true;
        supplier.Active = false;
        supplier.ModifiedDate = DateTime.Now;
        _repoWrapper.Supplier.Update(supplier);
        await _repoWrapper.SaveAllAsync();
        return Ok(new { message = "Successfully Deleted." });
      }
      return BadRequest("Supplier doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create Supplier.
    /// </summary> 
    /// <param name="createsupplier"></param> 
    [Authorize(Policy = "AdminAccess"), HttpPost("createsupplier")]
        [Obsolete]
        public async Task<IActionResult> CreateSupplier([FromForm] CreateSupplierDto createSupplierDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createSupplier = _mapper.Map<Supplier>(createSupplierDto);
      if (await _repoWrapper.Supplier.FindByCondition(s => s.SupplierName == createSupplierDto.SupplierName && s.BranchLocationId == createSupplierDto.BranchLocationId).AnyAsync(s => s.SupplierName == createSupplierDto.SupplierName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"You've duplicate supplier name exists.The supplier is {createSupplierDto.SupplierName}"
        });
      }
      if (createSupplier != null)
      {
        IFormFile uploadedAttachment = createSupplierDto.Files;
        string uniqueFileName = null;
        ICollection<Photo> photoFiles = new Collection<Photo>();
        Photo photo = new Photo();
        if (createSupplierDto.Files != null)
        {
          string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "uploads");
          uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedAttachment.FileName;
          string filePath = Path.Combine(uploadsFolder, uniqueFileName);
          await uploadedAttachment.CopyToAsync(new FileStream(filePath, FileMode.Create));
          photo.Url = "https://scaf.lk:" + _config.GetSection("ImageDownload:Port").Value + "/uploads/" + uniqueFileName;
          photo.ContentType = uploadedAttachment.ContentType;
          photo.FileName = uploadedAttachment.FileName;
          photo.AttachmentPath = filePath;
          photo.CreatedBy = createSupplierDto.CreatedBy;
          photoFiles.Add(photo);
          createSupplier.Photos = photoFiles;
          createSupplier.CreatedBy = createSupplierDto.CreatedBy;
        }
        var saved = false;
        // add supplier
        await _repoWrapper.Supplier.CreateAsync(createSupplier);
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
              if (entry.Entity is Brand)
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
          return Ok(createSupplier);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've You've unable to create Supplier.Please try again.");
    }

    /// <summary>
    /// Update Supplier.
    /// </summary> 
    /// <param name="{id}"></param> 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int? id, [FromForm] UpdateSupplierDto updateSupplierDto)
    {
      // Check that the record exists.
      var entity = await _context.Suppliers.Where(s => s.SupplierId == id).SingleAsync<Supplier>();

      if (entity == null)
      {
        return BadRequest("You've You've unable to find record with id '" + id + "'.");
      }
      // Update changes if any of the properties have been modified.
      _context.Entry(entity).CurrentValues.SetValues(updateSupplierDto);
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

    }
    private async Task<Supplier> UpdateAsync(int id, Supplier updateEntity)
    {
      // Check that the record exists.
      var entity = await _context.Suppliers.Where(s => s.SupplierId == id).SingleAsync<Supplier>();

      if (entity == null)
      {
        throw new Exception("You've You've unable to find record with id '" + id + "'.");
      }

      // Update changes if any of the properties have been modified.
      _context.Entry(entity).CurrentValues.SetValues(updateEntity);
      _context.Entry(entity).State = EntityState.Modified;

      if (_context.Entry(entity).Properties.Any(property => property.IsModified))
      {
        await _context.SaveChangesAsync();
      }
      return entity;
    }

  }
}
