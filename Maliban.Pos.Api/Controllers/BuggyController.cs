using System;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Errors;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Maliban.Pos.Api.Controllers
{
  [AllowAnonymous]
  public class BuggyController : BaseController
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
        public BuggyController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IOptions<SmtpSettings> smtpSettings, IRepositoryWrapper repoWrapper) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
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

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
      var thing = _context.Products.Find(Convert.ToInt64(42));

      if (thing == null)
      {
        return NotFound(new ApiResponse(404));
      }

      return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
      var thing = _context.Products.Find(Convert.ToInt64(42));

      var thingToReturn = thing.ToString();

      return Ok();
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
      return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
      return Ok();
    }
  }
}
