using System.ComponentModel;
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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class BrandController : BaseController
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
        public BrandController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /*
        /// <summary>
        /// Get Products 
        /// </summary>
        /// <param name="getallbrands"></param>
        [HttpGet("getallbrands", Name = "GetAllBrands")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllBrands(bool status, [FromQuery] UserParams userParams)
        {
          var products = await _productRepo.GetProductsAsync(userParams);
          var productToReturn = _mapper.Map<IEnumerable<ProductListDto>>(products);
          if (productToReturn.Count() == 0)
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
            default:
              Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
              return Ok(productToReturn);
          }

        }
        */
        /// <summary>
        /// Get Brands 
        /// </summary>
        /// <param name="getallbrands"></param>
        [HttpGet("getallbrands", Name = "GetAllBrands")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllBrands(bool status, [FromQuery] UserParams userParams)
        {
            var brands = await _repoWrapper.Brand.GetBrandsAsync(userParams, status);
            var brandToReturn = _mapper.Map<IEnumerable<BrandListDto>>(brands);
            if (brandToReturn.Count() == 0)
            {
                return BadRequest("Don't have any products.");
            }
            switch (status)
            {
                case true:
                    var resulttrue = brandToReturn.Where(p => p.Active == true);
                    Response.AddPagination(brands.CurrentPage, brands.PageSize, brands.TotalCount, brands.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = brandToReturn.Where(p => p.Active == false);
                    Response.AddPagination(brands.CurrentPage, brands.PageSize, brands.TotalCount, brands.TotalPages);
                    return Ok(resultfalse);
                    /*        default:
                              Response.AddPagination(brands.CurrentPage, brands.PageSize, brands.TotalCount, brands.TotalPages);
                              return Ok(brandToReturn);*/
            }

        }

        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetBrandByBrandId")]
        public async Task<IActionResult> GetBrandByBrandId(int? id)
        {
            if (await _repoWrapper.Brand.AnyAsync(b => b.BrandId == id && b.IsDeleted == false))
            {
                var brand = await _repoWrapper.Brand.FindByCondition(b => b.BrandId == id).Include(b => b.Photo).SingleOrDefaultAsync<Brand>();
                if (brand != null)
                {
                    return Ok(brand);
                }
            }
            return BadRequest("You've unable to fetching brand details by the given id");
        }
        /// <summary>
        /// Delete Brand By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteBrandByBrandId")]
        public async Task<IActionResult> DeleteBrandByBrandId(int? id)
        {
            if (id != null && await _repoWrapper.Brand.AnyAsync(p => p.BrandId == id && p.IsDeleted == false))
            {
                var brand = await _repoWrapper.Brand.FindByCondition(p => p.BrandId == id).SingleOrDefaultAsync();
                var photo = await _repoWrapper.Photo.FindByCondition(p => p.PhotoId == brand.PhotoId).SingleOrDefaultAsync();
                brand.IsDeleted = true;
                brand.Active = false;
                photo.Active = false;
                photo.IsDeleted = true;
                _repoWrapper.Brand.Update(brand);
                _repoWrapper.Photo.Update(photo);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("Brand doesn't exists or deleted.Please check the id.");
        }
        /// <summary>
        /// Create Brand.
        /// </summary> 
        /// <param name="createbrand"></param> 
        [HttpPost("createbrand")]
        [Obsolete]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandDto createBrandDto, [FromForm] CreatePhotoUploadDto createPhotoUploadDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createBrand = _mapper.Map<Brand>(createBrandDto);
            CreatePhotoDto createPhotoDto = new CreatePhotoDto
            {
                CreatedBy = createBrand.CreatedBy,
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
            if (await _repoWrapper.Brand.FindByCondition(b => b.BrandName == createBrandDto.BrandName).AnyAsync(s => s.BrandName == createBrandDto.BrandName))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"Duplicate brand name exists.The duplicate brand name is {createBrandDto.BrandName}"
                });
            }
            if (createBrand != null)
            {
                var saved = false;
                // add brand
                if (createPhoto != null && uploadedAttachment != null)
                {
                    await _context.Photos.AddAsync(createPhoto);
                    try
                    {
                        await _context.SaveChangesAsync();
                        createBrand.PhotoId = createPhoto.PhotoId;
                    }
                    catch (DbUpdateException e)
                    {
                        return BadRequest(e);
                    }
                }
                await _repoWrapper.Brand.CreateAsync(createBrand);
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
                    return Ok(createBrand);
                    // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
                }
            }
            return BadRequest("You've unable to create brand.Please try again.");
        }

        /// <summary>
        /// Update Brand.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int? id, [FromForm] UpdateBrandDto updateBrandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var brand = await _repoWrapper.Brand.FindByCondition(b => b.BrandId == id).SingleOrDefaultAsync<Brand>();
            _mapper.Map(updateBrandDto, brand);
            /*  var updateBrand = _mapper.Map<Brand>(new Brand() {
                BrandName = updateBrandDto.BrandName,
                BrandDescription =,
                ModifiedDate = DateTime.Now;
              } );*/
            //check BrandName update
            //var isBrandModified = _context.Entry(brand).Property("BrandName").IsModified;
            if (await _repoWrapper.Brand.AnyAsync(b => b.BrandId == id && b.IsDeleted == false && b.Active == true))
            {
                _repoWrapper.Brand.Update(brand);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Brand.Brand.Any(b => b.BrandId == id && b.IsDeleted == false))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(brand);
            }
            return BadRequest("there is no brand to update details.");
        }

    }
}