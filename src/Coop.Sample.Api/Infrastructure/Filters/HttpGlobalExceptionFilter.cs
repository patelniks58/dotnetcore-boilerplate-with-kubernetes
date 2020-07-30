using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coop.Sample.Api.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var jsonErrorResponse = new ErrorResponse
            {
                Messages = new[] { $"Internal Server Error: {context.Exception.Message}" },
                Status = StatusCodes.Status500InternalServerError
            };

            if (_env.IsDevelopment())
            {
                jsonErrorResponse.Exception = context.Exception.ToString();
            }

            context.Result = new ObjectResult(jsonErrorResponse) { StatusCode = jsonErrorResponse.Status };
            context.ExceptionHandled = true;
        }
    }
}
