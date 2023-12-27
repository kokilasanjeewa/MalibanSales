using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maliban.Pos.Api.Controllers
{
  [AllowAnonymous]
  public class FallbackController : Controller
  {
    public ActionResult Index()
    {
      string dir =Directory.GetCurrentDirectory();
      return PhysicalFile(Path.Combine(dir,
          "wwwroot", "index.html"), "text/HTML");
    }
  }
}