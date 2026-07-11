using DAL.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [HttpGet("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        ProblemDetails problemDetails = exception switch
        {
            DataAccessException ex => new ProblemDetails
            {
                Type = "https://api.myshipping.com/problems/database-error",
                Title = "Database Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Instance = HttpContext.Request.Path
            },

            _ => new ProblemDetails
            {
                Type = "https://yourapi.com/probs/internal-server-error",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred.",
                Instance = HttpContext.Request.Path
            }
        };

        return StatusCode(problemDetails.Status!.Value, problemDetails);
    }

    [HttpGet("/error-development")]
    public IActionResult ErrorDevelopment(
        [FromServices] IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return NotFound();

        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problemDetails = new ProblemDetails
        {
            Type = "about:blank",
            Title = exception?.GetType().Name,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception?.Message,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["stackTrace"] = exception?.StackTrace;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        return StatusCode(problemDetails.Status!.Value, problemDetails);
    }
}