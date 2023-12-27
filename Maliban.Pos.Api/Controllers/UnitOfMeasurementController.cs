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
    public class UnitOfMeasurementController : BaseController
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
        public UnitOfMeasurementController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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
        /// Get UnitOfMeasurements 
        /// </summary>
        /// <param name="getallunitOfMeasurements"></param>
        [HttpGet("getallunitOfMeasurements", Name = "GetAllUnitOfMeasurements")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllUnitOfMeasurements(bool status, [FromQuery] UserParams userParams)
        {
            var unitOfMeasurements = await _repoWrapper.UnitOfMeasurement.GetUnitOfMeasurementsAsync(userParams);
            var unitOfMeasurementToReturn = _mapper.Map<IEnumerable<UnitOfMeasurementListDto>>(unitOfMeasurements);
            if (unitOfMeasurementToReturn.Count() == 0)
            {
                return BadRequest("Don't have any products.");
            }
            switch (status)
            {
                case true:
                    var resulttrue = unitOfMeasurementToReturn.Where(p => p.IsDeleted = true);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = unitOfMeasurementToReturn.Where(p => p.IsDeleted = false);
                    return Ok(resultfalse);
                    /*        default:
                              Response.AddPagination(unitOfMeasurements.CurrentPage, unitOfMeasurements.PageSize, unitOfMeasurements.TotalCount, unitOfMeasurements.TotalPages);
                              return Ok(unitOfMeasurementToReturn);*/
            }

        }

        /// <summary>
        /// Get Product By BarCode 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpGet("{id}", Name = "GetUnitOfMeasurementByUnitOfMeasurementId")]
        public async Task<IActionResult> GetUnitOfMeasurementByUnitOfMeasurementId(int? id)
        {
            if (await _repoWrapper.UnitOfMeasurement.AnyAsync(b => b.UnitOfMeasurementId == id && b.IsDeleted == false))
            {
                var unitOfMeasurement = await _repoWrapper.UnitOfMeasurement.FindByCondition(b => b.UnitOfMeasurementId == id).SingleOrDefaultAsync<UnitOfMeasurement>();
                if (unitOfMeasurement != null)
                {
                    return Ok(unitOfMeasurement);
                }
            }
            return BadRequest("You've unable to fetching unitOfMeasurement details by the given id");
        }
        /// <summary>
        /// Delete UnitOfMeasurement By id 
        /// </summary>
        /// <param name="{id}"></param>
        [HttpDelete("{id}", Name = "DeleteUnitOfMeasurementByUnitOfMeasurementId")]
        public async Task<IActionResult> DeleteUnitOfMeasurementByUnitOfMeasurementId(int? id)
        {
            if (id != null && await _repoWrapper.UnitOfMeasurement.AnyAsync(p => p.UnitOfMeasurementId == id && p.IsDeleted == false))
            {
                var unitOfMeasurement = await _repoWrapper.UnitOfMeasurement.FindByCondition(p => p.UnitOfMeasurementId == id).SingleOrDefaultAsync();
                unitOfMeasurement.IsDeleted = true;
                _repoWrapper.UnitOfMeasurement.Update(unitOfMeasurement);
                await _repoWrapper.SaveAllAsync();
                return Ok("Successfully Deleted.");
            }
            return NotFound("UnitOfMeasurement doesn't exists or deleted.Please check the id.");
        }
        /// <summary>
        /// Create UnitOfMeasurement.
        /// </summary> 
        /// <param name="createunitOfMeasurement"></param> 
        [HttpPost("createunitOfMeasurement")]
        public async Task<IActionResult> CreateUnitOfMeasurement([FromForm] CreateUnitOfMeasurementDto createUnitOfMeasurementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createUnitOfMeasurement = _mapper.Map<UnitOfMeasurement>(createUnitOfMeasurementDto);
            if (await _repoWrapper.UnitOfMeasurement.FindByCondition(b => b.UOM == createUnitOfMeasurementDto.UOM).AnyAsync(s => s.UOM == createUnitOfMeasurementDto.UOM))
            {
                return Conflict(new
                {
                    ErrorCode = 409,
                    ErrorMessage = $"Duplicate unitOfMeasurement name exists.The unitOfMeasurement is {createUnitOfMeasurementDto.UOM}"
                });
            }
            if (createUnitOfMeasurement != null)
            {
                var saved = false;
                // add unitOfMeasurement
                await _repoWrapper.UnitOfMeasurement.CreateAsync(createUnitOfMeasurement);
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
                            if (entry.Entity is UnitOfMeasurement)
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
                    return Ok(createUnitOfMeasurement);
                    // return CreatedAtRoute("GetProductByVenderProductId", new { venderproductId = createProduct.VenderProductId }, _mapper.Map<Product>(createProduct));
                }
            }
            return BadRequest("You've unable to create unitOfMeasurement.Please try again.");
        }

        /// <summary>
        /// Update UnitOfMeasurement.
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnitOfMeasurement(int? id, [FromForm] UpdateUnitOfMeasurementDto updateUnitOfMeasurementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var unitOfMeasurement = await _repoWrapper.UnitOfMeasurement.FindByCondition(b => b.UnitOfMeasurementId == id).FirstOrDefaultAsync<UnitOfMeasurement>();
            if (updateUnitOfMeasurementDto.UOM == null)
            {
                updateUnitOfMeasurementDto.UOM = unitOfMeasurement.UOM;
            }
            if (updateUnitOfMeasurementDto.Description == null)
            {
                updateUnitOfMeasurementDto.Description = unitOfMeasurement.Description;
            }
            unitOfMeasurement.UOM = updateUnitOfMeasurementDto.UOM;
            unitOfMeasurement.Description = updateUnitOfMeasurementDto.Description;
            unitOfMeasurement.ModifiedDate = updateUnitOfMeasurementDto.ModifiedDate;
            unitOfMeasurement.ModifiedBy = updateUnitOfMeasurementDto.ModifiedBy;

            //check UnitOfMeasurementName update
            //var isUnitOfMeasurementModified = _context.Entry(unitOfMeasurement).Property("UnitOfMeasurementName").IsModified;

            if (await _repoWrapper.UnitOfMeasurement.AnyAsync(b => b.UnitOfMeasurementId == id && b.IsDeleted == false))
            {
                _repoWrapper.UnitOfMeasurement.Update(unitOfMeasurement);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) when (!_repoWrapper.UnitOfMeasurement.UnitOfMeasurement.Any(b => b.UnitOfMeasurementId == id && b.IsDeleted == false))
                {
                    //return BadRequest(e);
                    return Conflict(ex);
                }
                return Ok(unitOfMeasurement);
            }
            return BadRequest("there is no unitOfMeasurement to update details.");
        }

    }
}