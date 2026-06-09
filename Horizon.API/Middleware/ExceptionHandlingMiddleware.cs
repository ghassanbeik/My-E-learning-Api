using Horizon.API.Common;
using Horizon.Application.Common;
using System.Net;
using System.Text.Json;

namespace Horizon.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, response) = exception switch
            {
                ValidationException ex => (
                    HttpStatusCode.BadRequest,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Validation failed.",
                        Errors = ex.Errors,
                    }),

                NotFoundException ex => (
                    HttpStatusCode.NotFound,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        Message = ex.Message,
                        Errors = new List<string> { ex.Message },
                    }),

                UnauthorizedException ex => (
                    HttpStatusCode.Unauthorized,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 401,
                        Message = ex.Message,
                        Errors = new List<string> { ex.Message },
                    }),

                ForbiddenException ex => (
                    HttpStatusCode.Forbidden,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 403,
                        Message = ex.Message,
                        Errors = new List<string> { ex.Message },
                    }),

                ConflictException ex => (
                    HttpStatusCode.Conflict,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 409,
                        Message = ex.Message,
                        Errors = new List<string> { ex.Message },
                    }),

                UnauthorizedAccessException ex => (
                    HttpStatusCode.Unauthorized,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 401,
                        Message = "Unauthorized.",
                        Errors = new List<string> { ex.Message },
                    }),

                _ => (
                    HttpStatusCode.InternalServerError,
                    new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "An unexpected error occurred. Please try again later.",
                        Errors = new List<string> { "Internal server error." },
                    })
            };

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            await context.Response.WriteAsync(json);
        }
    }
}
