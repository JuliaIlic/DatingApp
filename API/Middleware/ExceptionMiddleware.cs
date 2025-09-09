using Microsoft.Extensions.Hosting;
using API.Middleware;
using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

// public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
// IHostEnvironment env)

// {
//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             await next(context);
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, ex.Message);
//             context.Response.ContentType = "application/json";
//             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//             var response = env.IsDevelopment()
//                 ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
//                 : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

//             var options = new JsonSerializerOptions
//             {
//                 PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//             };
//             var json = JsonSerializer.Serialize(response, options);
//             await context.Response.WriteAsync(json);
//         }
//     }
// }

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
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

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                 var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
            else
            {
                _logger.LogWarning("Response already started, cannot modify headers.");
            }
        }
    }
}
