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
    public class CompanyMasterSettingController : BaseController
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
        public CompanyMasterSettingController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /// <param name="getallcompanymastersetting"></param>
        [HttpGet("getallcompanymastersetting", Name = "GetAllCompanyMasterSetting")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllCompanyMasterSetting(bool status, [FromQuery] UserParams userParams)
        {
            var companyMasters = await _repoWrapper.CompanyMasterSetting.GetCalculationMastersAsync(userParams, status);
            var companyMasterReturn = _mapper.Map<IEnumerable<CompanyMasterSettingListDto>>(companyMasters);
            if (companyMasterReturn.Count() == 0)
            {
                return BadRequest("Don't have any products.");
            }
            switch (status)
            {
                case true:
                    var resulttrue = companyMasterReturn.Where(p => p.Active == true);
                    Response.AddPagination(companyMasters.CurrentPage, companyMasters.PageSize, companyMasters.TotalCount, companyMasters.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = companyMasterReturn.Where(p => p.Active == false);
                    Response.AddPagination(companyMasters.CurrentPage, companyMasters.PageSize, companyMasters.TotalCount, companyMasters.TotalPages);
                    return Ok(resultfalse);
                    /*        default:
                              Response.AddPagination(companyMasters.CurrentPage, companyMasters.PageSize, companyMasters.TotalCount, companyMasters.TotalPages);
                              return Ok(companyMasterReturn);*/
            }

        }

        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetCompanyMasterSetting")]
        public async Task<IActionResult> GetCompanyMasterSetting(int? id)
        {
            if (await _repoWrapper.CompanyMasterSetting.AnyAsync(b => b.CalculationMasterId == id && b.IsDeleted == false))
            {
                var companyMaster = await _repoWrapper.CompanyMasterSetting.FindByCondition(b => b.CalculationMasterId == id).Include(b => b.BranchLocation).SingleOrDefaultAsync<CalculationMaster>();
                if (companyMaster != null)
                {
                    return Ok(companyMaster);
                }
            }
            return BadRequest("You've been unable to fetch company master setting details by the given id");
        }
        /// <summary>
        /// Delete Category By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteCompanyMasterSetting")]
        public async Task<IActionResult> DeleteCompanyMasterSetting(int? id)
        {
            if (id != null && await _repoWrapper.CompanyMasterSetting.AnyAsync(p => p.CalculationMasterId == id && p.IsDeleted == false))
            {
                var companyMaster = await _repoWrapper.CompanyMasterSetting.FindByCondition(p => p.CalculationMasterId == id).SingleOrDefaultAsync();
                companyMaster.IsDeleted = true;
                companyMaster.Active = false;
                _repoWrapper.CompanyMasterSetting.Update(companyMaster);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("The company master setting doesn't exists or has been deleted.Please check the id.");
        }
        /// <summary>
        /// Create Category.
        /// </summary> 
        /// <param name="createcompanymastersetting"></param> 
        [HttpPost("createcompanymastersetting")]
        public async Task<IActionResult> CreateCompanyMasterSetting([FromForm] CompanyMasterSettingDto companyMasterSettingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyMasterSetting = _mapper.Map<CalculationMaster>(companyMasterSettingDto);
            if (await _repoWrapper.CompanyMasterSetting.FindByCondition(b => b.BranchLocationId == companyMasterSettingDto.BranchLocationId).AnyAsync(s => s.BranchLocationId == companyMasterSettingDto.BranchLocationId))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"{companyMasterSettingDto.BranchLocationId} is Duplicate Company Master Setting."
                });
            }
            if (companyMasterSetting != null)
            {
                var saved = false;
                // add CalculationMaster
                await _repoWrapper.CompanyMasterSetting.CreateAsync(companyMasterSetting);
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
                    return Ok(companyMasterSetting);
                    // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
                }
            }
            return BadRequest("You've been unable to create Company Master Setting.Please try again.");
        }

        /// <summary>
        /// Update Category.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyMasterSetting(int? id, [FromForm] UpdateCompanyMasterSettingDto updateCompanyMasterSettingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyMasterSetting = await _repoWrapper.CompanyMasterSetting.FindByCondition(b => b.CalculationMasterId == id).SingleOrDefaultAsync<CalculationMaster>();
            _mapper.Map(updateCompanyMasterSettingDto, companyMasterSetting);
            /*  var updateCategory = _mapper.Map<Category>(new Category() {
                CategoryName = updateCategoryDto.CategoryName,
                CategoryDescription =,
                ModifiedDate = DateTime.Now;
              } );*/
            //check CategoryName update
            //var isCategoryModified = _context.Entry(category).Property("CategoryName").IsModified;
            if (await _repoWrapper.CompanyMasterSetting.AnyAsync(b => b.CalculationMasterId == id && b.IsDeleted == false && b.Active == true))
            {
                _repoWrapper.CompanyMasterSetting.Update(companyMasterSetting);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.CompanyMasterSetting.CalculationMaster.Any(b => b.CalculationMasterId == id && b.IsDeleted == false))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(companyMasterSetting);
            }
            return BadRequest("There is no company master setup to update details.");
        }

    }
}