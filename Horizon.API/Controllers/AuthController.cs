using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Auth.ChangePassword;
using Horizon.Application.Features.Auth.ForgotPassword;
using Horizon.Application.Features.Auth.GetCurrentUser;
using Horizon.Application.Features.Auth.Login;
using Horizon.Application.Features.Auth.Logout;
using Horizon.Application.Features.Auth.RefreshToken;
using Horizon.Application.Features.Auth.Register;
using Horizon.Application.Features.Auth.ResetPassword;
using Horizon.Application.Features.Auth.UpdateProfile;
using Horizon.Application.Features.Auth.UploadAvatar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Horizon.API.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>Register a new user</summary>
    [HttpPost("register")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new RegisterCommand(dto), ct));

    /// <summary>Login with email and password</summary>
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new LoginCommand(dto), ct));

    /// <summary>Refresh access token</summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 401)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new RefreshTokenCommand(dto), ct));

    /// <summary>Logout and revoke refresh token</summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout([FromBody] string refreshToken, CancellationToken ct)
        => FromResult(await _mediator.Send(new LogoutCommand(refreshToken), ct));

    /// <summary>Change password</summary>
    [HttpPut("change-password")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ChangePasswordCommand(UserId, dto), ct));

    /// <summary>Request password reset email</summary>
    [HttpPost("forgot-password")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ForgotPassword([FromBody] string email, CancellationToken ct)
        => FromResult(await _mediator.Send(new ForgotPasswordCommand(email), ct));

    /// <summary>Reset password with token</summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ResetPasswordCommand(dto), ct));

    /// <summary>Get current user profile</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
    public async Task<IActionResult> Me(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetCurrentUserQuery(UserId), ct));

    /// <summary>Update profile</summary>
    [HttpPut("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new UpdateProfileCommand(UserId, dto), ct));

    /// <summary>Upload avatar</summary>
    [HttpPost("avatar")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse.Failure("No file uploaded."));

        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(file.ContentType))
            return BadRequest(ApiResponse.Failure("Only JPEG, PNG, and WebP images are allowed."));

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest(ApiResponse.Failure("File size must not exceed 5MB."));

        return FromResult(await _mediator.Send(
            new UploadAvatarCommand(UserId, file.OpenReadStream(), file.FileName, file.ContentType), ct));
    }
}
