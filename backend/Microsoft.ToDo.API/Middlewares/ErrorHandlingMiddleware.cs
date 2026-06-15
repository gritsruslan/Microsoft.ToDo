using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.ToDo.Application.Exceptions;

namespace Microsoft.ToDo.API.Middlewares;

internal sealed class ErrorHandlingMiddleware(
    RequestDelegate next,
    ProblemDetailsFactory problemDetailsFactory,
    ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            ProblemDetails problemDetails;

            switch (ex)
            {
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext, (int) domainException.ErrorCode, domainException.Message);
                    break;
                case ValidationException validationException:
                {
                    var errors = validationException.Errors.Select(e => e.ErrorMessage).ToArray();
                    
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext,
                        StatusCodes.Status400BadRequest,
                        "Validation failed"
                    );

                    problemDetails.Extensions["errors"] = errors;
                    break;
                }
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(
                        httpContext, 
                        StatusCodes.Status500InternalServerError, 
                        "Something went wrong! Please contact us.");
                
                    logger.LogError(ex, "Unhandled exception occured!");
                    break;
            }
            
            httpContext.Response.StatusCode = problemDetails.Status ?? 
                                              StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}