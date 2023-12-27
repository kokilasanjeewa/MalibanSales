using Maliban.Pos.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Maliban.Pos.Api.Middleware
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
        [System.Obsolete]
        private readonly IHostingEnvironment _env;

        [System.Obsolete]
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostingEnvironment env)
    {
      _next = next;
      _logger = logger;
      _env = env;

    }
  }
}