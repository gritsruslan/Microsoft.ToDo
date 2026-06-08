using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

            if (ex is DomainException domainException)
            {
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    httpContext, (int) domainException.ErrorCode, domainException.Message);
            }
            else
            {
                problemDetails = problemDetailsFactory.CreateProblemDetails(
                    httpContext, 
                    StatusCodes.Status500InternalServerError, 
                    "Unhandled error occured! Please contact us.");
                
                logger.LogError(ex, "Unhandled exception occured!");
            }
            
            httpContext.Response.StatusCode = problemDetails.Status ?? 
                                              StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}