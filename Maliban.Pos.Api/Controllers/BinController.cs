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
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class BinController : BaseController
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
        public BinController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /// Get Bins 
        /// </summary>
        /// <param name="getallbins"></param>
        [HttpGet("getallbins", Name = "GetAllBins")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllBins(bool status, [FromQuery] UserParams userParams)
        {
            var bins = await _repoWrapper.Bin.GetBinsAsync(userParams);
            var binsToReturn = _mapper.Map<IEnumerable<BinListDto>>(bins);
            if (binsToReturn.Count() == 0)
            {
                return BadRequest("Don't have any bins.");
            }
            switch (status)
            {
                case true:
                    var resulttrue = binsToReturn.Where(p => p.IsDeleted = true);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = binsToReturn.Where(p => p.IsDeleted = false);
                    return Ok(resultfalse);
                    /*        default:
                              Response.AddPagination(bins.CurrentPage, bins.PageSize, bins.TotalCount, bins.TotalPages);
                              return Ok(binsToReturn);*/
            }
        }
        /// <summary>
        /// Get Bin By Id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetBinById")]
        public async Task<IActionResult> GetBinById(int? id)
        {
            if (await _repoWrapper.Bin.AnyAsync(b => b.BinId == id && b.IsDeleted == false))
            {
                var bin = await _repoWrapper.Bin.FindByCondition(b => b.BinId == id && b.IsDeleted == false).SingleOrDefaultAsync<Bin>();
                if (bin != null)
                {
                    return Ok(bin);
                }
            }
            return BadRequest("You've unable to fetching bin details by the given id");
        }
        /// <summary>
        /// Delete Bin By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteBinByBinId")]
        public async Task<IActionResult> DeleteBinByBinId(int? id)
        {
            if (id != null && await _repoWrapper.Bin.AnyAsync(p => p.BinId == id && p.IsDeleted == false))
            {
                var bin = await _repoWrapper.Bin.FindByCondition(p => p.BinId == id).SingleOrDefaultAsync<Bin>();
                bin.IsDeleted = true;
                _repoWrapper.Bin.Update(bin);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("Bin doesn't exists or deleted.Please check the id.");
        }
        /// <summary>
        /// Create Bin.
        /// </summary> 
        /// <param name="createbn"></param> 
        [HttpPost("createbin")]
        public async Task<IActionResult> CreateBin([FromForm] CreateBinDto createBinDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createBin = _mapper.Map<Bin>(createBinDto);
            if (await _repoWrapper.Bin.FindByCondition(b => b.BinNumber == createBinDto.BinNumber).AnyAsync(s => s.BinNumber == createBinDto.BinNumber))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"Duplicate brand name exists.The brand is {createBinDto.BinNumber}"
                });
            }
            if (createBin != null)
            {
                var saved = false;
                // add brand
                await _repoWrapper.Bin.CreateAsync(createBin);
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
                            if (entry.Entity is Bin)
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
                    return Ok(createBin);
                    // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
                }
            }
            return BadRequest("You've unable to create brand.Please try again.");
        }

        /// <summary>
        /// Update Bin.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBin(int? id, [FromForm] UpdateBinDto updateBinDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bin = await _repoWrapper.Bin.FindByCondition(b => b.BinId == id).FirstOrDefaultAsync<Bin>();
            if (updateBinDto.BinNumber == null)
            {
                updateBinDto.BinNumber = bin.BinNumber;
            }
            if (updateBinDto.BinDescription == null)
            {
                updateBinDto.BinDescription = bin.BinDescription;
            }
            if (updateBinDto.ModifiedDate == null)
            {
                updateBinDto.ModifiedDate = bin.ModifiedDate;
            }
            if (updateBinDto.ModifiedBy == null)
            {
                updateBinDto.ModifiedBy = bin.ModifiedBy;
            }
            bin.BinNumber = updateBinDto.BinNumber;
            bin.BinDescription = updateBinDto.BinDescription;
            bin.ModifiedDate = updateBinDto.ModifiedDate;
            bin.ModifiedBy = updateBinDto.ModifiedBy;
            //check BinName update
            //var isBinModified = _context.Entry(brand).Property("BinName").IsModified;

            if (await _repoWrapper.Bin.AnyAsync(b => b.BinId == id && b.IsDeleted == false))
            {
                _repoWrapper.Bin.Update(bin);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.Bin.Bin.Any(b => b.BinId == id))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(bin);
            }
            return BadRequest("there is no brand to update details.");
        }

    }
}