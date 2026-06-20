using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.JWTServices;
using Horizon.Domain.Interfaces.Services.PasswordHasher;
using MediatR;

namespace Horizon.Application.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;
        private readonly IEventBus _eventBus;

        public LoginHandler(IUnitOfWork uow, IPasswordHasher hasher, IJwtService jwt, IEventBus eventBus)
        {
            _uow = uow; _hasher = hasher; _jwt = jwt; _eventBus = eventBus;
        }

        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Dto.Email, ct);
            if (user == null || !_hasher.Verify(request.Dto.Password, user.PasswordHash))
                return Result<AuthResponseDto>.Unauthorized("Invalid email or password.");

            if (!user.IsActive)
                return Result<AuthResponseDto>.Forbidden("Account is deactivated.");

            var roles        = (await _uow.UserRoles.GetUserRoleNamesAsync(user.Id, ct)).ToList();
            var accessToken  = _jwt.GenerateAccessToken(user, roles);
            var refreshToken = _jwt.GenerateRefreshToken();

            await _uow.Sessions.RevokeAllUserSessionsAsync(user.Id, ct);
            await _uow.Sessions.AddAsync(new Session
            {
                UserId       = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow.AddDays(7),
            }, ct);

            user.LastLoginAt = DateTime.UtcNow;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new UserLoggedInEvent
            {
                UserId = user.Id,
                Email  = user.Email,
            }, ct);

            return Result<AuthResponseDto>.Success(new AuthResponseDto(
                accessToken,
                refreshToken,
                _jwt.GetAccessTokenExpiry(),          // ← was DateTime.UtcNow.AddHours(1)
                new UserDto(user.Id, user.FirstName, user.LastName, user.FullName,
                            user.Email, user.AvatarUrl, user.Headline, user.IsEmailVerified, roles)));
        }
    }
}
