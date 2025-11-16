using DocumentManagement.Core.Exceptions;
using FluentValidation;
using Serilog;

namespace DocumentManagement.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage });

            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                errors
            });

            return;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception");

            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (status, message) = ex switch
        {
            AuthorizationException => (StatusCodes.Status403Forbidden, ex.Message),
            NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
            AppValidationException => (StatusCodes.Status400BadRequest, ex.Message),

            // FluentValidation
            ValidationException fvEx =>
                (StatusCodes.Status400BadRequest,
                 string.Join("; ", fvEx.Errors.Select(e => e.ErrorMessage))),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        context.Response.StatusCode = status;

        return context.Response.WriteAsJsonAsync(new
        {
            success = false,
            error = message
        });
    }
}