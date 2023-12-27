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
    public class CategoryController : BaseController
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
        public CategoryController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /// Get Categorys 
        /// </summary>
        /// <param name="getallcategorys"></param>
        [HttpGet("getallcategorys", Name = "GetAllCategorys")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllCategorys(bool status, [FromQuery] UserParams userParams)
        {
            var categorys = await _repoWrapper.Category.GetCategoriesAsync(userParams, status);
            var categoryToReturn = _mapper.Map<IEnumerable<CategoryListDto>>(categorys);
            if (categoryToReturn.Count() == 0)
            {
                return BadRequest("Don't have any products.");
            }
            switch (status)
            {
                case true:
                    var resulttrue = categoryToReturn.Where(p => p.Active == true);
                    Response.AddPagination(categorys.CurrentPage, categorys.PageSize, categorys.TotalCount, categorys.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = categoryToReturn.Where(p => p.Active == false);
                    Response.AddPagination(categorys.CurrentPage, categorys.PageSize, categorys.TotalCount, categorys.TotalPages);
                    return Ok(resultfalse);
                    /*        default:
                              Response.AddPagination(categorys.CurrentPage, categorys.PageSize, categorys.TotalCount, categorys.TotalPages);
                              return Ok(categoryToReturn);*/
            }

        }

        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetCategoryByCategoryId")]
        public async Task<IActionResult> GetCategoryByCategoryId(int? id)
        {
            if (await _repoWrapper.Category.AnyAsync(b => b.CategoryId == id && b.IsDeleted == false))
            {
                var category = await _repoWrapper.Category.FindByCondition(b => b.CategoryId == id).Include(b => b.Photo).SingleOrDefaultAsync<Category>();
                if (category != null)
                {
                    return Ok(category);
                }
            }
            return BadRequest("You've unable to fetching category details by the given id");
        }
        /// <summary>
        /// Delete Category By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteCategoryByCategoryId")]
        public async Task<IActionResult> DeleteCategoryByCategoryId(int? id)
        {
            if (id != null && await _repoWrapper.Category.AnyAsync(p => p.CategoryId == id && p.IsDeleted == false))
            {
                var category = await _repoWrapper.Category.FindByCondition(p => p.CategoryId == id).SingleOrDefaultAsync();
                var photo = await _repoWrapper.Photo.FindByCondition(p => p.PhotoId == category.PhotoId).SingleOrDefaultAsync();
                category.IsDeleted = true;
                category.Active = false;
                photo.Active = false;
                photo.IsDeleted = true;
                _repoWrapper.Category.Update(category);
                _repoWrapper.Photo.Update(photo);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("Category doesn't exists or deleted.Please check the id.");
        }
        /// <summary>
        /// Create Category.
        /// </summary> 
        /// <param name="createcategory"></param> 
        [HttpPost("createcategory")]
        [Obsolete]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto createCategoryDto, [FromForm] CreatePhotoUploadDto createPhotoUploadDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createCategory = _mapper.Map<Category>(createCategoryDto);
            CreatePhotoDto createPhotoDto = new CreatePhotoDto
            {
                CreatedBy = createCategory.CreatedBy,
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
            if (await _repoWrapper.Category.FindByCondition(b => b.CategoryName == createCategoryDto.CategoryName).AnyAsync(s => s.CategoryName == createCategoryDto.CategoryName))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"Duplicate category name exists.The duplicate category name is {createCategoryDto.CategoryName}"
                });
            }
            if (createCategory != null)
            {
                var saved = false;
                // add category
                if (createPhoto != null && uploadedAttachment != null)
                {
                    await _context.Photos.AddAsync(createPhoto);
                    try
                    {
                        await _context.SaveChangesAsync();
                        createCategory.PhotoId = createPhoto.PhotoId;
                    }
                    catch (DbUpdateException e)
                    {
                        return BadRequest(e);
                    }
                }
                await _repoWrapper.Category.CreateAsync(createCategory);
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
                            if (entry.Entity is Category)
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
                    return Ok(createCategory);
                    // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
                }
            }
            return BadRequest("You've unable to create category.Please try again.");
        }

        /// <summary>
        /// Update Category.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int? id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _repoWrapper.Category.FindByCondition(b => b.CategoryId == id).SingleOrDefaultAsync<Category>();
            _mapper.Map(updateCategoryDto, category);
            /*  var updateCategory = _mapper.Map<Category>(new Category() {
                CategoryName = updateCategoryDto.CategoryName,
                CategoryDescription =,
                ModifiedDate = DateTime.Now;
              } );*/
            //check CategoryName update
            //var isCategoryModified = _context.Entry(category).Property("CategoryName").IsModified;
            if (await _repoWrapper.Category.AnyAsync(b => b.CategoryId == id && b.IsDeleted == false && b.Active == true))
            {
                _repoWrapper.Category.Update(category);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Category.Category.Any(b => b.CategoryId == id && b.IsDeleted == false))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(category);
            }
            return BadRequest("there is no category to update details.");
        }







    }
}