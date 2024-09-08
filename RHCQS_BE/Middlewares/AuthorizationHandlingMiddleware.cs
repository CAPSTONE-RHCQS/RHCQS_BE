using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace RHCQS_BE;
public class AuthorizationHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationHandlingMiddleware> _logger;

    public AuthorizationHandlingMiddleware(RequestDelegate next, ILogger<AuthorizationHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Unauthorized access. You do not have permission to access this resource.");
        }
    }
}