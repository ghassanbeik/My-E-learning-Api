
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.JWTServices;
using MediatR;

namespace Horizon.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwt;

        public RefreshTokenHandler(IUnitOfWork uow, IJwtService jwt) { _uow = uow; _jwt = jwt; }

        public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var session = await _uow.Sessions.GetByRefreshTokenAsync(request.Dto.RefreshToken, ct);
            if (session == null || !session.IsActive)
                return Result<AuthResponseDto>.Unauthorized("Invalid or expired refresh token.");

            var user = await _uow.Users.GetByIdAsync(session.UserId, ct);
            if (user == null) return Result<AuthResponseDto>.Unauthorized();

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(user.Id, ct)).ToList();
            var accessToken = _jwt.GenerateAccessToken(user, roles);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _uow.Sessions.RevokeSessionAsync(request.Dto.RefreshToken, string.Empty, ct);
            await _uow.Sessions.AddAsync(new Session
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            }, ct);

            await _uow.SaveChangesAsync(ct);

            return Result<AuthResponseDto>.Success(new AuthResponseDto(
                accessToken, refreshToken, DateTime.UtcNow.AddHours(1),
                new UserDto(user.Id, user.FirstName, user.LastName, user.FullName,
                            user.Email, user.AvatarUrl, user.Headline, user.IsEmailVerified, roles)));
        }
    }

}
