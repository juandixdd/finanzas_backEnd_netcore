using BaseBackend.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace BaseBackend.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger
    )
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
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode statusCode;
        string message = ex.Message;

        switch (ex)
        {
            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                break;

            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred";
                break;
        }

        var response = new
        {
            success = false,
            status = (int)statusCode,
            error = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}