using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MPService.Common.Entities;
using MPService.Common.Exceptions;
using System.Text.Json;

namespace MPService.Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (NotFoundException nfEx)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await HandleExceptionAsync(context, nfEx.ErrorCode, nfEx.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HandleExceptionAsync(context, "INTERNAL_SERVER_ERROR", ex.Message, 500);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string code, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";

            var error = new ErrorResponse
            {
                Status = statusCode,
                ErrorCode = code,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(error);
            await context.Response.WriteAsync(json);
        }

    }
}
