

namespace Horizon.Application.DTOs
{
    public record RegisterDto(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string Role = "Student");

    public record LoginDto(string Email, string Password);

    public record RefreshTokenDto(string AccessToken, string RefreshToken);

    public record ChangePasswordDto(string CurrentPassword, string NewPassword, string ConfirmPassword);

    public record ResetPasswordDto(string Email, string Token, string NewPassword, string ConfirmPassword);

    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);
}
