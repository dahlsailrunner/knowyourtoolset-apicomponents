using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace KnowYourToolset.ApiComponents.Middleware
{
    public class ProblemDetailsOptions
    {
        public ResponseDetailHandler AddResponseDetails { get; set; }
    }
    public delegate HttpStatusCode ResponseDetailHandler(HttpContext httpContext, Exception exception, ProblemDetails problemDetails);

    public class ProblemDetailsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsOptions _options;

        public ProblemDetailsMiddleware(ProblemDetailsOptions options, RequestDelegate next)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _options);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ProblemDetailsOptions opts)
        {
            var error = new ProblemDetails
            { 
                Instance = Guid.NewGuid().ToString(),
                Title = "An error occurred in the API.  Please use the instance and contact our support team if the problem persists."
            };

            var statusCode = opts.AddResponseDetails?.Invoke(context, exception, error);
            statusCode ??= HttpStatusCode.InternalServerError;

            Log.ForContext("ErrorId", error.Instance)
                .Error(exception, "An exception was caught in the API request pipeline");

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
