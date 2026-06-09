using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services;
using Horizon.Domain.Interfaces.Services.CurrentUserServices;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Common;

// ─── API Response Wrapper ─────────────────────────────────────────────────────

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Success(T data, int statusCode = 200, string? message = null)
        => new() { IsSuccess = true, Data = data, StatusCode = statusCode, Message = message };

    public static ApiResponse<T> Failure(string error, int statusCode = 400)
        => new() { IsSuccess = false, Message = error, Errors = new() { error }, StatusCode = statusCode };

    public static ApiResponse<T> Failure(List<string> errors, int statusCode = 400)
        => new() { IsSuccess = false, Errors = errors, Message = errors.FirstOrDefault(), StatusCode = statusCode };
}

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse Success(string? message = null, int statusCode = 200)
        => new() { IsSuccess = true, StatusCode = statusCode, Message = message };

    public static ApiResponse Failure(string error, int statusCode = 400)
        => new() { IsSuccess = false, Message = error, Errors = new() { error }, StatusCode = statusCode };
}

// ─── Base Controller ──────────────────────────────────────────────────────────

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    private ICurrentUserService? _currentUser;

    protected ICurrentUserService CurrentUser =>
        _currentUser ??= HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

    protected Guid UserId => CurrentUser.UserId ?? throw new UnauthorizedAccessException();

    protected IActionResult FromResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return StatusCode(result.StatusCode, ApiResponse<T>.Success(result.Value!, result.StatusCode));

        return StatusCode(result.StatusCode, result.Errors.Any()
            ? ApiResponse<T>.Failure(result.Errors, result.StatusCode)
            : ApiResponse<T>.Failure(result.Error ?? "An error occurred.", result.StatusCode));
    }

    protected IActionResult FromResult(Result result)
    {
        if (result.IsSuccess)
            return StatusCode(result.StatusCode, ApiResponse.Success(statusCode: result.StatusCode));

        return StatusCode(result.StatusCode, result.Errors.Any()
            ? ApiResponse.Failure(string.Join(", ", result.Errors), result.StatusCode)
            : ApiResponse.Failure(result.Error ?? "An error occurred.", result.StatusCode));
    }
}
