﻿

using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace cloudscribe.Web.Common.Middleware
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Handling request: " + context.Request.Path);

            try
            {
                //This is how you call the next module in the Http pipeline.
                //The code above this call executes during the request.
                //The code below this call executes during the response.

                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                _logger.LogError("can middleware invoke exceptions be swallowed?", ex);
            }
            

            _logger.LogInformation("Finished handling request.");
        }
    }

    public static class RequestLoggerExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggerMiddleware>();
        }
    }
}
