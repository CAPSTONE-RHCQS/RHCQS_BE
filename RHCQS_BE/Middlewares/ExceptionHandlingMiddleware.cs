using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RHCQS_BusinessObjects;
using System;
using System.Net;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppConstant.MessageError messageError)
        {
            _logger.LogError($"Error with status code: {messageError.Code}, Message: {messageError.Message}");

            context.Response.StatusCode = messageError.Code;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(new ErrorResponse
            {
                StatusCode = messageError.Code,
                Error = messageError.Message,
                TimeStamp = DateTime.UtcNow
            }.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex}");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Error = "Internal Server Error",
                TimeStamp = DateTime.UtcNow
            }.ToString());
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
