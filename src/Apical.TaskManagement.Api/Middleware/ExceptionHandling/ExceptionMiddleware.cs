using System.Net;
using FluentValidation;
using Serilog;

namespace Apical.TaskManagement.Api.Middleware.ExceptionHandling;

public class ExceptionMiddleware
{
    // https://stackoverflow.com/a/38935583/856692
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestDelegate> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<RequestDelegate> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        if (exception is ValidationException) code = HttpStatusCode.BadRequest;
        context.Response.StatusCode = (int)code;

        string result = "An error has occurred";

        return context.Response.WriteAsync(result);
    }
}
