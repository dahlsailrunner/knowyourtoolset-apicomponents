using System;
using Microsoft.AspNetCore.Builder;

namespace KnowYourToolset.ApiComponents.Middleware
{
    public static class ApiMiddlewareExtensions
    {
        public static IApplicationBuilder UseProblemDetailsHandler(this IApplicationBuilder builder,
            Action<ProblemDetailsOptions> configureOptions)
        {
            var options = new ProblemDetailsOptions();
            configureOptions(options);

            return builder.UseMiddleware<ProblemDetailsMiddleware>(options);
        }
        public static IApplicationBuilder UseProblemDetailsHandler(this IApplicationBuilder builder)
        {
            var options = new ProblemDetailsOptions();
            return builder.UseMiddleware<ProblemDetailsMiddleware>(options);
        }

    }
}
