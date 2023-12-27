using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Maliban.Pos.Api.Filters
{
  public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      string currentUserRole = Convert.ToString(context.HttpContext.Session.GetString("UserRole"));

      if (!string.IsNullOrEmpty(currentUserRole))
      {
        if (currentUserRole != "Administrator")
        {
          context.Result = new RedirectToRouteResult
      (
      new RouteValueDictionary(new
      {
        action = "Error",
        controller = "Error"
      }));

        }
        else
        {
          context.Result = new RedirectToRouteResult
     (
     new RouteValueDictionary(new
     {
       action = "Error",
       controller = "Error"
     }));

        }
      }
      else
      {
        context.Result = new RedirectToRouteResult
        (
        new RouteValueDictionary(new
        {
          action = "Error",
          controller = "Error"
        }));

      }
    }
  }
}