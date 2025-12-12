using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDDD.Extensions;

public static class GlobalExceptionHandlerException
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception               = exceptionHandlerFeature?.Error;

                if (exception is DomainException domainException)
                {
                    context.Response.StatusCode  = domainException.StatusCode;
                    context.Response.ContentType = "application/json";

                    var problemDetails = new ProblemDetails()
                    {
                        Status   = domainException.StatusCode,
                        Title    = domainException.ErrorCode,
                        Detail   = domainException.Message,
                        Instance = context.Request.Path
                    };

                    if (domainException is ValidationException validationException)
                    {
                        problemDetails.Extensions["errors"] = validationException.Errors;
                    }

                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
                else
                {
                    context.Response.StatusCode  = 500;
                    context.Response.ContentType = "application/json";

                    var problemDetails = new ProblemDetails
                    {
                        Status   = 500,
                        Title    = "Internal Server Error",
                        Detail   = exception?.Message,
                        Instance = context.Request.Path
                    };

                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}