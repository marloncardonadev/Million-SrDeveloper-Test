using FluentValidation;
using Million.RealEstate.Backend.Domain.Common;
using System.Net;

namespace Million.RealEstate.Backend.Api.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        object response;

        switch (exception)
        {
            case DomainException domainEx:
                statusCode = HttpStatusCode.BadRequest;
                response = new { success = false, message = domainEx.Message };
                break;

            case ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                response = new
                {
                    success = false,
                    message = "Validation failed",
                    errors = validationEx.Errors.Select(e => e.ErrorMessage)
                };
                break;

            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response = new { success = false, message = exception.Message };
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                response = new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    detail = exception.Message
                };
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
