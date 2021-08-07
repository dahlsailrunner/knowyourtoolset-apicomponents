# KnowYourToolset.ApiComponents
_[![NuGet version](https://img.shields.io/nuget/v/KnowYourToolset.ApiComponents.svg?style=flat&label=nuget%3A%20KnowYourToolset.ApiComponents)](https://www.nuget.org/packages/KnowYourToolset.ApiComponents)_

# Getting Started
The features in this `ApiComponents` package are:

* **[ProblemDetails](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-5.0) middleware** that can be used by API projects and easily return a customizable `ProblemDetails` 
object* when errors (unhandled exceptions) occur

### Using the ProblemDetails middleware
To use the middleware for exception handling, simply call the `UseProblemDetailsHandler` extension method in the `Configure` method in `Startup.cs`.

You can call `UseProblemDetailsHandler` with no parameters to use defaults, and it will return a base-level `ProblemDetails` object
with a variety of its properties filled in whenever an unhandled exception occurs.  

If you want to add some additional context to the `ProblemDetails` response or use something 
other than `500-InternalServerError` for the HTTP status, just use the `AddResponseDetails` option 
and provide a delegate method with your own logic (the `CustomizeResponse` method 
in the example below).

To do that, your `Configure` method might end up looking something like this:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseProblemDetailsHandler(options => options.AddResponseDetails = CustomizeResponse);

    //... more code here ...
}

private HttpStatusCode CustomizeResponse(HttpContext httpContext, Exception exception, ProblemDetails problemDetails)
{
    if (exception is ApplicationException appEx)
    {
        problemDetails.Detail = appEx.Message;
        return HttpStatusCode.BadRequest;
    }
    return HttpStatusCode.InternalServerError; 
}
```

** Sample:** A sample API that uses this is the [KnowYourToolset.BackEnd template](https://github.com/dahlsailrunner/knowyourtoolset-templates/tree/main/templates/KnowYourToolset.BackEnd) in the `KnowYourToolset.Templates` package. 
You can create a project using this template if you want to experiment with it.

# Contributing
This is meant to be very simple and minimalistic features that can support ASP.NET Core+ API projects.

Feel free to use this code as a starter for things you might want to do in the same direction for your 
projects, or if you feel that something belongs in here, use the following workflow:

* Fork the repo
* Create a branch
* Make your changes (and test them!) in the branch you created 
* Commit your changes 
* Submit a Pull Request

