using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]

    public class MenuController : BaseController
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly DapperContext _dpContext;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        //   private Sale cart;

        // private IDistributedCache cache;

        [Obsolete]
        public MenuController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper, IDistributedCache cache, DapperContext dpContext) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
            _repoWrapper = repoWrapper;
            //  cart = cartService;
            _dpContext = dpContext;
            //  this.cache = cache;
        }
        [HttpGet("get/menu")]
        public async Task<IActionResult> GetAllMenus()
        {
            var menu = await _context.MainMenus.ToListAsync();
            return Ok(menu);
        }
        [HttpGet("get/menu/{id}")]
        public async Task<IActionResult> GetAllSubMenuById(int id)
        {
            var menu = await _context.Submenus.Where(x => x.MainMenuId == id).ToListAsync();
            return Ok(menu);
        }
        [HttpGet("get/submenu/{id}")]
        public async Task<IActionResult> GetAllSubMenuItemById(int id)
        {
            var menu = await _context.SubmenuItems.Where(x => x.SubMenuId == id).ToListAsync();
            return Ok(menu);
        }
        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var query = "SELECT * FROM RolePermissions";
                using var connection = _dpContext.CreateConnection();
                var companies = await connection.QueryAsync<RolePermission>(query);
                return Ok(companies.ToList());

            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary> 
        [HttpGet("{id}", Name = "GetMenuByUserId")]
        //https://code-maze.com/net-core-web-development-part14/
        public async Task<IActionResult> GetMenuByUserId(int id)
        {
            try
            {
                using var connection = _dpContext.CreateConnection();
                var procedureName1 = "sp_GetUserPermissionById";
                var param = new { id };
                var companies = (await connection.QueryAsync<RolePermissionCustom>(procedureName1, param, commandType: CommandType.StoredProcedure)
                .ConfigureAwait(false)).AsList();
                return Ok(companies.ToList());

            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromForm] MenuForCreationDto company)
        {
            try
            {
                /*              var query = "INSERT INTO RolePermissions ([Allow],[SubmenuId],[SubmenuItemId],[ApplicationRoleId],[ApplicationUserId],[AccessControllerName],[MainMenuId]) VALUES (@Allow, @SubmenuId,@SubmenuItemId,@ApplicationRoleId,@ApplicationUserId,@AccessControllerName,@MainMenuId)" +
                   "SELECT CAST(SCOPE_IDENTITY() as int)"; */
                var user = await _userManager.Users.Where(u => u.Id == company.ApplicationUserId).FirstOrDefaultAsync();
                var userRoles = await _userManager.GetRolesAsync(user);
                string AccessControllerName = _context.SubmenuItems.Find(company.SubmenuItemId).Name.ToString().Replace(" ", "");
                //  string author = company.Role.FirstOrDefault();
                //  string[] authorsList = author.Split(",");
                foreach (string item in userRoles)
                {
                    company.ApplicationRoleId = _context.Roles.Where(r => r.Name == item).FirstOrDefault().Id;
                }
                var procedureName1 = "sp_CreateUserPermissionById";
                // var procedureName1 ="Orders_Insert";
                var parameters = new DynamicParameters();
                parameters.Add("applicationUserId", company.ApplicationUserId, DbType.Int32);
                parameters.Add("applicationRoleId", company.ApplicationRoleId, DbType.Int32);
                parameters.Add("accessControllerName", "CanAccess" + AccessControllerName, DbType.String);
                parameters.Add("allow", company.Allow, DbType.Boolean);
                //   parameters.Add("Deny", company.Deny, DbType.Boolean);
                parameters.Add("mainMenuId", company.MainMenuId, DbType.Int32);
                parameters.Add("submenuId", company.SubmenuId, DbType.Int32);
                parameters.Add("submenuItemId", company.SubmenuItemId, DbType.Int32);

                using var connection = _dpContext.CreateConnection();
                var id = await connection.QuerySingleAsync<int>(procedureName1, parameters, commandType: CommandType.StoredProcedure);
                var createdCompany = new RolePermission
                {
                    ApplicationUserId = company.ApplicationUserId,
                    ApplicationRoleId = company.ApplicationRoleId,
                    AccessControllerName = company.AccessControllerName,
                    Allow = company.Allow,
                    MainMenuId = company.MainMenuId,
                    SubmenuId = company.SubmenuId
                };
                return CreatedAtRoute("GetMenuByUserId", new { id = createdCompany.ApplicationUserId }, createdCompany);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}", Name = "DeleteCompany")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            /* var p = await _context.RolePermissions.FindAsync(id);
            var r = _context.RolePermissions.Remove(p);
            return Ok(p); */
            try
            {
                var procedureName1 = "sp_DeleteUserPermissionById";
                var parameters = new DynamicParameters();
                parameters.Add("id", id, DbType.Int32);
                using var connection = _dpContext.CreateConnection();
                var result = await connection.QuerySingleAsync<int>(procedureName1, parameters, commandType: CommandType.StoredProcedure);
                return Ok(id);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromForm] MenuForUpdateDto company)
        {
            try
            {
                var query = "UPDATE [dbo].[RolePermissions] SET [Allow] =@Allow  WHERE AccessControllerName=@AccessControllerName and ApplicationUserId=@ApplicationUserId";
                var parameters = new DynamicParameters();
                parameters.Add("ApplicationUserId", company.ApplicationUserId, DbType.Int32);
                parameters.Add("AccessControllerName", company.AccessControllerName, DbType.String);
                parameters.Add("Allow", company.Allow, DbType.Boolean);
                using var connection = _dpContext.CreateConnection();
                var id = await connection.ExecuteAsync(query, parameters);
                var createdCompany = new RolePermission
                {
                    ApplicationUserId = company.ApplicationUserId,
                    AccessControllerName = company.AccessControllerName,
                    Allow = company.Allow,
                };
                return CreatedAtRoute("GetMenuByUserId", new { id = createdCompany.ApplicationUserId }, createdCompany);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
    public class RolePermissionCustom
    {
        public int Id { get; set; }
        public bool Allow { get; set; }
        public string MainMenu { get; set; }
        public string Submenu { get; set; }
        public string SubmenuItem { get; set; }
        public string AccessControllerName { get; set; }
        public int ApplicationRoleId { get; set; }
        public int ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}