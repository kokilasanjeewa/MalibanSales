using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Repository;
//email confugaration
using MimeKit;
using System.Net.Http;
using MailKit.Net.Smtp;
using MailKit;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using MailKit.Security;
using Maliban.Pos.Api.Service;
using Microsoft.Extensions.Options;
using Maliban.Pos.Api.Filters;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.Features;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : BaseController
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
        public AuthController(IConfiguration config, IRepositoryWrapper repoWrapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _smtpSettings = smtpSettings.Value;
         }
        /// <summary>
        /// User Registration  a specific username and password.
        /// </summary> 
        /// <param name="register"></param> 
        [HttpPost("register")]
        [AllowAnonymous]
        //[SetAuthor("Test Value")]   filter value then add to Header please see Headers controller or method you can apply

        public async Task<IActionResult> Register([FromForm] UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<ApplicationUser>(userForRegisterDto);
            if (await UserExists(userToCreate.UserName))
            {
                //ModelState.AddModelError("emailAddress", "This email already exists");
                //return BadRequest(ModelState);
                return BadRequest(error: "Sorry, the username allready exists.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);
            var err = result.Errors.ToList();
            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userToReturn.UserName);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var changePasswordUrl = Request.Headers["http://localhost:4200/#/authentication/resetpassword"];
                // new commit
                var urlBulder = new UriBuilder("https://192.168.1.72:5001/authentication/confirmemail");
                var query = HttpUtility.ParseQueryString(urlBulder.Query);
                query["token"] = token;
                query["userId"] = user.Id.ToString();
                query["username"] = user.UserName.ToString();
                urlBulder.Query = query.ToString();
                var urlString = urlBulder.ToString().Replace("4200", "4200/#");
                var emailBody = $"Click on this link to confirm your registration. <br>{urlString}";
                //message details
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Maliban Point Of Sale", _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(user.UserName, user.Email));
                message.Subject = "Email confirmation notice";
                message.Body = new TextPart("plain") { Text = emailBody };
                //email configaration here
                using (var client = new SmtpClient())
                {
                    client.MessageSent += (sender, args) =>
                    {
                        //args.Message
                    };
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.Auto);
                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                //return Ok("Plase check your email for reset password.");
                return Ok(userToReturn);
                //return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);

            }
            logger.Error(result.Errors.ToString());
            return BadRequest(err);
        }
        /// <summary>
        /// User forgot password  a specific username.
        /// </summary> 
        /// <param name="changepassword"></param> 
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromForm] UserForChangePasswordDto userForChangePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userForChangePasswordDto.UserId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetpassword = await _userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(token), userForChangePasswordDto.Password);
            if (resetpassword.Succeeded)
            {
                return Ok("Password Reset Successfully.");
            }
            return BadRequest(resetpassword.Errors);
        }
        /// <summary>
        /// User confirm email with a specific username and password.
        /// </summary> 
        /// <param name="confirmemail"></param>    
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromForm] UserForConfirmEmailDto userForConfirmEmailDto)
        {
            var user = await _userManager.FindByIdAsync(userForConfirmEmailDto.UserId);
            var confirm = await _userManager.ConfirmEmailAsync(user, Uri.UnescapeDataString(userForConfirmEmailDto.Token));
            if (confirm.Succeeded)
            {
                return Ok();
            }
            return Unauthorized();
        }
        /// <summary>
        /// User forgot password  a specific username.
        /// </summary> 
        /// <param name="sendconfirmemail"></param> 
        [HttpPost("sendconfirmemail")]
        public async Task<IActionResult> SendConfirmEmail([FromForm] UserForSendConfirmEmailDto userForSendConfirmEmailDto)
        {
            //var userToReset = _mapper.Map<ApplicationUser>(userForResetPasswordDto);
            var user = await _userManager.FindByNameAsync(userForSendConfirmEmailDto.UserName);
            if (await UserExists(user.UserName))
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var changePasswordUrl = Request.Headers["http://localhost:4200/#/authentication/resetpassword"];
                var urlBulder = new UriBuilder("http://localhost:4200/#/authentication/confirmemail");
                var query = HttpUtility.ParseQueryString(urlBulder.Query);
                query["token"] = token;
                query["userId"] = user.Id.ToString();
                urlBulder.Query = query.ToString();
                var urlString = urlBulder.ToString();
                var emailBody = $"Click on link to change password <br>{urlString}";
                //message details
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Kokila Sanjeewa", "kokila.sanjeewa@maliban.com"));
                message.To.Add(new MailboxAddress("Kokila Sanjeewa", "kokila.sanjeewa@gmail.com"));
                message.Subject = "Reset password recover email";
                message.Body = new TextPart("plain") { Text = emailBody };
                //email configaration here
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("mail.maliban.com", 25, false);
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("kokila.sanjeewa@maliban.com", "MaLiB750");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return Ok("Plase check your email for reset password.");
            }
            return Unauthorized();
        }
        /// <summary>
        /// User Login  a specific username and password.
        /// </summary> 
        /// <param name="login"></param>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserForLogInDto userForLogInDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var user = await _userManager.Users.Where(u => u.UserName==userForLogInDto.UserName && u.Active==true).FirstOrDefaultAsync();
            var passwordverify = await _userManager.CheckPasswordAsync(user, userForLogInDto.Password);
            if (user == null && !passwordverify)
            {
                ModelState.AddModelError("UserName and Password", "Sorry,we couldn't find an account with that username.Can we help you recover your username or password? ");
                return BadRequest(ModelState);
            }
            if (user == null)
            {
                ModelState.AddModelError("UserName", "Sorry,we couldn't find an account with that username.Can we help you recover your username? ");
                return BadRequest(ModelState);
            }
            if (!passwordverify)
            {
                return BadRequest("Sorry, that password isn't right.Try again!");
            }
            // check the user logging activity
            /*await _userManager.UpdateSecurityStampAsync(user);
            var loggedinuser = await _signinManager.PasswordSignInAsync(user, userForLogInDto.Password, isPersistent: false, lockoutOnFailure: false);
            if (loggedinuser.Succeeded)
            {
              return BadRequest("User already logged in.");
            }*/
            var result = await _signinManager.CheckPasswordSignInAsync(user, userForLogInDto.Password, false);
            if (result.Succeeded)
            {
                var appUser = _mapper.Map<UserForListDto>(user);
                user.LastLoginDate = DateTime.Now;
                user.LastConnectedDate = DateTime.Now;
                // Retreive server/local IP address
                var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                var remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
                var LocalIPAddr = feature?.LocalIpAddress?.ToString();
                user.Address = remoteIp;
                await _userManager.UpdateSecurityStampAsync(user);
                await _userManager.UpdateAsync(user);
                //    _httpContextAccessor.HttpContext.Session.SetString("UserRole", "Administrator");
                return Ok(new { token = GenerateJwtToken(user).Result, appUser });
            }
            return Unauthorized();
        }
        [HttpPost("create-roles/{userId}")]
        public async Task<IActionResult> CreateRoles(int userId, [FromForm] RoleAddDto roles)
        {   // var user = await _userManager.FindByNameAsync(roles.UserName);
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return NotFound("Could not find user");
            var userRoles = await _userManager.GetRolesAsync(user);
            string[] selectedRoles = roles.RoleNames.Take(1).FirstOrDefault().Split(',', ' ');
            selectedRoles ??= new string[] { };
            var createRole = selectedRoles.Except(userRoles);
            var result = await _userManager.AddToRolesAsync(user, createRole);
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            if (userRoles == null)
            {
                var result1 = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!result1.Succeeded) return BadRequest("Failed to add to roles");
            }
            //result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            //if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }
        [Authorize(Policy = "RequiredUser"), HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = (from userRole in u.UserRoles join role in _context.Roles on userRole.RoleId equals role.Id select role.Name).ToList(),
                    
                }).ToListAsync();
            return Ok(users);
        }
        [Authorize(Policy = "RequiredUser"), HttpGet("user-with-roles/{userId}")]
        public async Task<IActionResult> GetUserWithRoles(int userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null) return NotFound("Could not find user");
            var roles = await _context.UserRoles.Where(r => r.UserId == userId).Select(u => new {u.UserId,u.ApplicationRole.Id,u.ApplicationRole.Name,u.ApplicationRole.CreatedDate, u.ApplicationRole.IsActive,u.ApplicationRole.Description }).ToListAsync();
            return Ok(roles);
        }
        [HttpPost("edit-roles/{username}")]
        public async Task<IActionResult> EditRoles(string username, [FromForm] RoleEditDto roles)
        {

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roles.RoleNames;
            selectedRoles ??= new string[] { };
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
     {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim("SecurityStamp",user.SecurityStamp),
            new Claim("BranchLocationId",user.BranchLocationId.ToString()),
            new Claim("MasterSettings",user.BranchLocationId.ToString()),
            //new Claim(ClaimTypes.Email,user.Email),
      };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var permissions = await _context.RolePermissions.Where(r => r.ApplicationUserId == user.Id).Include(r => r.ApplicationRole).ToListAsync();
            // List<Role> Role = new List<Role>();
            List<Permission> Permission = new List<Permission>();
            foreach (var role in roles)
            {
                foreach (var permission in permissions.Where(p => p.ApplicationRole.Name == role))
                {
                    Permission.Add(new Permission
                    {
                        Id = permission.Id,
                        AccessControllerName = permission.AccessControllerName,
                        Allow = permission.Allow,
                        //  Deny = permission.Deny,
                        MainMenu = permission.MainMenuId,
                        Submenu = permission.SubmenuId,
                    });
                }
            }
            /*       foreach (var role in roles)
                  {
                      foreach (var permission in permissions.Where(c => c.ApplicationRole.Name == role))
                      {
                          Permission.Add(new Permission
                          {
                              Id = permission.Id,
                              AccessControllerName = permission.AccessControllerName,
                              Allow = permission.Allow,
                              Deny = permission.Deny,
                              MainMenu = permission.MainMenu,
                              Submenu = permission.Submenu,
                          });
                      }
                      Role.Add(new Role { roleName = role, permission = Permission });
                  } */
            string mySerializedClass = JsonConvert.SerializeObject(Permission);
            claims.Add(new Claim("rolePermission", mySerializedClass));

            //map branchlocations
            var branches = await _context.UserBranchLocations.Where(u => u.Id == user.Id).Select(u => new { u.BranchLocationId }).ToListAsync();
            int[] branchArray = new int[branches.Count];
            int index = 0;
            foreach (var item in branches)
            {
                branchArray[index] = item.BranchLocationId;
                index++;
            }
            string mySerializedBranch = JsonConvert.SerializeObject(branchArray);
            claims.Add(new Claim("MapBranchLocations", mySerializedBranch));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            /*       var encryptingCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512); */

            var credts = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //https://dev.to/thisdotmedia/angular-and-the-rest-authentication-with-jwt-5em0
            /*     SecurityTokenDescriptor options =>
                     issuer: "https://localhost:44342",
                     audience: "https://localhost:44342",
                     claims: new List<Claim>(),
                     expires: DateTime.Now.AddMinutes(5),
                     signingCredentials: signinCredentials */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = _config["JwtToken:Issuer"],
                Audience = _config["JwtToken:Issuer"],
                SigningCredentials = credts,
                //   EncryptingCredentials = encryptingCredentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private async Task<bool> UserExists(string username)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == username))
            {
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// User log out .
        /// </summary> 
        /// <param name="logout"></param> 
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return Ok("Logout successfully.");
        }
        /// <summary>
        /// Email Verify .
        /// </summary> 
        /// <param name="{username}"></param> 
        [HttpGet("verifyemail/{username}")]
        public async Task<IActionResult> Verifyemail(string username)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == username))
            {
                return BadRequest(false);
            }
            return Ok(true);

        }

        /// <summary>
        /// Get Users 
        /// </summary>
        /// <param name="getallusers"></param>
        //  [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
        [Authorize(Policy = "AdminAccess")]
        [HttpGet("getallusers", Name = "GetAllUsers")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllUsers(bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var users = await _repoWrapper.User.GetUsersAsync(userParams, BranchLocationId);
            // Headers get 
            Request.Headers.TryGetValue("BranchLocationId", out var traceValue);
           if (users.Count() == 0)
            {
                return NoContent();
            }
                switch (status)
                {
                    case true:
                        var resulttrue = users.Where(p => p.Active == true);
                        Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
                        return Ok(resulttrue);
                    case false:
                        var resultfalse = users.Where(p => p.Active == false);
                        Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
                        return Ok(resultfalse);
                }
          }
     //   private async Task<bool> UserNicExists(string id) => await _userManager.Users.AnyAsync(u => u.NationId == id);

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Permission
    {
        public int Id { get; set; }
        public bool Allow { get; set; }
        //  public bool Deny { get; set; }
        public int MainMenu { get; set; }
        public int Submenu { get; set; }
        public string AccessControllerName { get; set; }

    }
    public class Role
    {
        public string roleName { get; set; }
        public List<Permission> permission { get; set; }
    }

    public class Root
    {
        public List<Role> Role { get; set; }
    }
}

