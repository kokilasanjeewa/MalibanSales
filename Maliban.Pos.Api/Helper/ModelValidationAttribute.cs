using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Maliban.Pos.Api.Helper
{
  public class ModelValidationAttribute : ActionFilterAttribute
  {
     //https://www.yogihosting.com/aspnet-core-filters/
    //model validation actionfilter  
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        context.Result = new BadRequestObjectResult(context.ModelState); // returns 400 with error
      }
    }

  }
}
