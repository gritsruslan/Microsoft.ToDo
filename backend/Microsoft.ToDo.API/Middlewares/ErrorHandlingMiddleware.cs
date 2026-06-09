using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.ToDo.Application.Exceptions;

namespace Microsoft.ToDo.API.Middlewares;

public sealed class ErrorHandlingMiddleware(
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
                    var modelStateDictionary = new ModelStateDictionary();
                    
                    foreach (var error in validationException.Errors)
                    {
                        modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                        httpContext,
                        modelStateDictionary,
                        StatusCodes.Status400BadRequest,
                        "Validation failed");

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