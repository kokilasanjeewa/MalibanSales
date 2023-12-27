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

namespace Maliban.Pos.Api.Controllers
{
  [Route("api/[controller]")]
  [AllowAnonymous]
  [ApiVersion("1.0")]
  [ApiVersion("1.1")]
  public class SubCategoryController : BaseController
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
        public SubCategoryController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
    /// Get SubCategorys 
    /// </summary>
    /// <param name="getallsubcategorys"></param>
    [HttpGet("getallsubcategorys", Name = "GetAllSubCategorys")]
    [MapToApiVersion("1.1")]
    public async Task<IActionResult> GetAllSubCategorys(bool status, [FromQuery] UserParams userParams)
    {
      var subcategorys = await _repoWrapper.SubCategory.GetSubCategoriesAsync(userParams, status);
      var subcategoryToReturn = _mapper.Map<IEnumerable<SubCategoryListDto>>(subcategorys);
      if (subcategoryToReturn.Count() == 0)
      {
        return BadRequest("Don't have any products.");
      }
      switch (status)
      {
        case true:
          var resulttrue = subcategoryToReturn.Where(p => p.Active == true);
          Response.AddPagination(subcategorys.CurrentPage, subcategorys.PageSize, subcategorys.TotalCount, subcategorys.TotalPages);
          return Ok(resulttrue);
        case false:
          var resultfalse = subcategoryToReturn.Where(p => p.Active == false);
          Response.AddPagination(subcategorys.CurrentPage, subcategorys.PageSize, subcategorys.TotalCount, subcategorys.TotalPages);
          return Ok(resultfalse);
/*        default:
          Response.AddPagination(subcategorys.CurrentPage, subcategorys.PageSize, subcategorys.TotalCount, subcategorys.TotalPages);
          return Ok(subcategoryToReturn);*/
      }

    }

    /// <summary>
    /// Get Product By BarCode 
    /// </summary>
    /// <param name="{id}"></param>
    [HttpGet("{id}", Name = "GetSubCategoryBySubCategoryId")]
    public async Task<IActionResult> GetSubCategoryBySubCategoryId(int? id)
    {
      if (await _repoWrapper.SubCategory.AnyAsync(b => b.SubCategoryId == id && b.IsDeleted == false))
      {
        var subCategory = await _repoWrapper.SubCategory.FindByCondition(b => b.SubCategoryId == id).SingleOrDefaultAsync<SubCategory>();
        if (subCategory != null)
        {
          return Ok(subCategory);
        }
      }
      return BadRequest("You've unable to fetching subCategory details by the given id");
    }
        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{categoryId}"></param>
        [HttpGet("SubCategories/{categoryId}", Name = "GetSubCategoriesByCategoryId")]
        public async Task<IActionResult> GetSubCategoriesByCategoryId(int? categoryId)
        {
            if (await _repoWrapper.SubCategory.AnyAsync(b => b.CategoryId == categoryId && b.IsDeleted == false))
            {
                var subCategories = await _repoWrapper.SubCategory.FindByCondition(b => b.CategoryId == categoryId).ToListAsync<SubCategory>();
                if (subCategories.Count >0)
                {
                    return Ok(subCategories);
                }
            }
            return BadRequest("You've unable to fetching subCategory details by the given id");
        }
        /// <summary>
        /// Delete SubCategory By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteSubCategoryBySubCategoryId")]
    public async Task<IActionResult> DeleteSubCategoryBySubCategoryId(int? id)
    {
      if (id != null && await _repoWrapper.SubCategory.AnyAsync(p => p.SubCategoryId == id && p.IsDeleted == false))
      {
        var subCategory = await _repoWrapper.SubCategory.FindByCondition(p => p.SubCategoryId == id).SingleOrDefaultAsync();
        subCategory.IsDeleted = true;
        subCategory.Active = false;
        _repoWrapper.SubCategory.Update(subCategory);
        await _repoWrapper.SaveAllAsync();
        return Ok("Successfully Deleted.");
      }
      return NotFound("SubCategory doesn't exists or deleted.Please check the id.");
    }
    /// <summary>
    /// Create SubCategory.
    /// </summary> 
    /// <param name="createsubcategory"></param> 
    [HttpPost("createsubcategory")]
        [Obsolete]
        public async Task<IActionResult> CreateSubCategory([FromForm] CreateSubCategoryDto createSubCategoryDto, [FromForm] CreatePhotoUploadDto createPhotoUploadDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createSubCategory = _mapper.Map<SubCategory>(createSubCategoryDto);
      CreatePhotoDto createPhotoDto = new CreatePhotoDto
      {
        CreatedBy = createSubCategory.CreatedBy,
        Description = createPhotoUploadDto.PhotoDescription,
        IsMain = true,
      };
      IFormFile uploadedAttachment = createPhotoUploadDto.Files;
      string uniqueFileName = null;
      if (uploadedAttachment != null)
      {
        string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "uploads");
        //string uploadsFolder = Path.Combine("http://scaf.lk:100/", "uploads/");
        uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedAttachment.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        await uploadedAttachment.CopyToAsync(new FileStream(filePath, FileMode.Create));
        createPhotoDto.AttachmentPath = filePath;
        createPhotoDto.Length = uploadedAttachment.Length;
        createPhotoDto.ContentType = uploadedAttachment.ContentType;
        createPhotoDto.FileName = uniqueFileName;
        createPhotoDto.Url = "https://scaf.lk:" + _config.GetSection("ImageDownload:Port").Value + "/uploads/" + uniqueFileName;
      }
      var createPhoto = _mapper.Map<Photo>(createPhotoDto);
      if (await _repoWrapper.SubCategory.FindByCondition(b => b.SubCategoryName == createSubCategoryDto.SubCategoryName).AnyAsync(s => s.SubCategoryName == createSubCategoryDto.SubCategoryName))
      {
        return Conflict(new
        {
          ErrorCode = 409,
          ErrorMessage = $"Duplicate subCategory name exists.The duplicate subCategory name is {createSubCategoryDto.SubCategoryName}"
        });
      }
      if (createSubCategory != null)
      {
        var saved = false;
        // add subCategory
        if (createPhoto != null && uploadedAttachment != null)
        {
          await _context.Photos.AddAsync(createPhoto);
          try
          {
            await _context.SaveChangesAsync();
          }
          catch (DbUpdateException e)
          {
            return BadRequest(e);
          }
        }
        await _repoWrapper.SubCategory.CreateAsync(createSubCategory);
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
              if (entry.Entity is SubCategory)
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
          return Ok(createSubCategory);
          // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
        }
      }
      return BadRequest("You've unable to create subCategory.Please try again.");
    }

    /// <summary>
    /// Update SubCategory.
    /// </summary> 
    /// <param name="{id}"></param> 
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubCategory(int? id, [FromForm] UpdateSubCategoryDto updateSubCategoryDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var subCategory = await _repoWrapper.SubCategory.FindByCondition(b => b.SubCategoryId == id).SingleOrDefaultAsync<SubCategory>();
      //_mapper.Map(updateSubCategoryDto, subCategory);
      /*  var updateSubCategory = _mapper.Map<SubCategory>(new SubCategory() {
          SubCategoryName = updateSubCategoryDto.SubCategoryName,
          SubCategoryDescription =,
          ModifiedDate = DateTime.Now;
        } );*/
      //check SubCategoryName update
      //var isSubCategoryModified = _context.Entry(subCategory).Property("SubCategoryName").IsModified;
      if (await _repoWrapper.SubCategory.AnyAsync(b => b.SubCategoryId == id && b.IsDeleted == false && b.Active == true))
      {
        _repoWrapper.SubCategory.Update(subCategory);
        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.SubCategory.SubCategory.Any(b => b.SubCategoryId == id && b.IsDeleted == false))
        {
          //return BadRequest(e);
          return Conflict(ex);
        }
        return Ok(subCategory);
      }
      return BadRequest("there is no subCategory to update details.");
    } 
 }
}