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
    public class CustomerMasterSettingController : BaseController
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
        public CustomerMasterSettingController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /// <param name="getallcustomermastersetting"></param>
        [HttpGet("getallcustomermastersetting", Name = "GetAllCustomerMasterSetting")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllCustomerMasterSetting(bool status, [FromQuery] UserParams userParams)
        {
            var categorys = await _repoWrapper.CustomerMasterSetting.GetCustomerMasterSettingsAsync(userParams, status);
            var categoryToReturn = _mapper.Map<IEnumerable<CustomerMasterSettingListDto>>(categorys);
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
/*                default:
                    Response.AddPagination(categorys.CurrentPage, categorys.PageSize, categorys.TotalCount, categorys.TotalPages);
                    return Ok(categoryToReturn);*/
            }

        }

        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetCustomerMasterSetting")]
        public async Task<IActionResult> GetCustomerMasterSetting(int? id)
        {
            if (await _repoWrapper.CustomerMasterSetting.AnyAsync(b => b.CustomerMasterSettingId == id && b.IsDeleted == false))
            {
                var category = await _repoWrapper.CustomerMasterSetting.FindByCondition(b => b.CustomerMasterSettingId == id).Include(b => b.BranchLocation).SingleOrDefaultAsync<CustomerMasterSetting>();
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
        [HttpDelete("{id}", Name = "DeleteCustomerMasterSetting")]
        public async Task<IActionResult> DeleteCustomerMasterSetting(int? id)
        {
            if (id != null && await _repoWrapper.CustomerMasterSetting.AnyAsync(p => p.CustomerMasterSettingId == id && p.IsDeleted == false))
            {
                var category = await _repoWrapper.CustomerMasterSetting.FindByCondition(p => p.CustomerMasterSettingId == id).SingleOrDefaultAsync();
                category.IsDeleted = true;
                category.Active = false;
                _repoWrapper.CustomerMasterSetting.Update(category);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("Category doesn't exists or deleted.Please check the id.");
        }
        /// <summary>
        /// Create Category.
        /// </summary> 
        /// <param name="createcustomermastersetting"></param> 
        [HttpPost("createcustomermastersetting")]
        public async Task<IActionResult> CreateCustomerMasterSetting([FromForm] CreateCustomerMasterSettingDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createCategory = _mapper.Map<CustomerMasterSetting>(createCategoryDto);
            var customerCategory = (CustomerCategoryCode)createCategoryDto.CustomerCategoryCode;
            createCategory.CustomerCategory = customerCategory.ToString();
            if (await _repoWrapper.CustomerMasterSetting.FindByCondition(b => b.BranchLocationId == createCategoryDto.BranchLocationId && b.CustomerCategoryCode == createCategoryDto.CustomerCategoryCode).AnyAsync(b => b.BranchLocationId == createCategoryDto.BranchLocationId && b.CustomerCategoryCode == createCategoryDto.CustomerCategoryCode))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"Customer master setup already exists."
                });
            }
            if (createCategory != null)
            {
                var saved = false;
                await _repoWrapper.CustomerMasterSetting.CreateAsync(createCategory);
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
            return BadRequest("You've been unable to create customer master setting.Please try again.");
        }

        /// <summary>
        /// Update Category.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerMasterSetting(int? id, [FromForm] UpdateCustomerMasterSettingDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = await _repoWrapper.CustomerMasterSetting.FindByCondition(b => b.CustomerMasterSettingId == id).SingleOrDefaultAsync<CustomerMasterSetting>();
               _mapper.Map(updateCategoryDto, category);
            /*  var updateCategory = _mapper.Map<Category>(new Category() {
                CategoryName = updateCategoryDto.CategoryName,
                CategoryDescription =,
                ModifiedDate = DateTime.Now;
              } );*/
            //check CategoryName update
            //var isCategoryModified = _context.Entry(category).Property("CategoryName").IsModified;
            if (await _repoWrapper.CustomerMasterSetting.AnyAsync(b => b.CustomerMasterSettingId == id && b.IsDeleted == false && b.Active == true))
            {
                var customerCategory = (CustomerCategoryCode)updateCategoryDto.CustomerCategoryCode;
                category.CustomerCategory = customerCategory.ToString();
                _repoWrapper.CustomerMasterSetting.Update(category);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.CustomerMasterSetting.CustomerMasterSetting.Any(b => b.CustomerMasterSettingId == id && b.IsDeleted == false))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(category);
            }
            return BadRequest("There is no update customer master setting.");
        }

    }
}