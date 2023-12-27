using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Maliban.Pos.Api.Helper
{
  public class SetAuthorAttribute : Attribute, IResultFilter
  {
    string _value;
    public SetAuthorAttribute(string value)
    {
      _value = value;
    }
    public void OnResultExecuted(ResultExecutedContext context)
    {
      //throw new NotImplementedException();
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
      context.HttpContext.Response.Headers.Add("Author", new string[] { _value });

    }
  }
}