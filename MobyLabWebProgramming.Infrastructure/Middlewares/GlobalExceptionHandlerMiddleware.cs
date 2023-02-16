using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MobyLabWebProgramming.Core.Errors;

namespace MobyLabWebProgramming.Infrastructure.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Caught exception in global exception handler!");

            var response = context.Response;
            response.ContentType = "application/json";

            if (ex is ServerException serverException)
            {
                response.StatusCode = (int)serverException.Status;
                await response.WriteAsync(JsonSerializer.Serialize(ErrorMessage.FromException(serverException)));
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonSerializer.Serialize(new ErrorMessage(HttpStatusCode.InternalServerError, "A unexpected error occurred!")));
            }
        }
    }
}
