using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.JWTServices;
using Horizon.Domain.Interfaces.Services.PasswordHasher;
using MediatR;

namespace Horizon.Application.Features.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;
        private readonly IEventBus _eventBus;

        public RegisterHandler(IUnitOfWork uow, IPasswordHasher hasher, IJwtService jwt, IEventBus eventBus)
        {
            _uow = uow; _hasher = hasher; _jwt = jwt; _eventBus = eventBus;
        }

        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct)
        {
            if (await _uow.Users.EmailExistsAsync(request.Dto.Email, ct))
                return Result<AuthResponseDto>.Conflict("Email already registered.");

            // ── Role restriction ──────────────────────────────────────────────
            // Public self-registration is restricted to the "Student" role only.
            // - Instructors are promoted to the role by an Admin after vetting.
            // - Admin accounts are seeded at startup and cannot be self-created.
            // Without this guard, anyone could POST { role: "Admin" } and gain
            // elevated access on first sign-up.
            const string publicRole = "Student";
            if (!string.Equals(request.Dto.Role, publicRole, StringComparison.OrdinalIgnoreCase))
                return Result<AuthResponseDto>.Forbidden(
                    "Public registration is only available for the Student role.");

            var role = await _uow.Roles.GetByNameAsync(publicRole, ct);
            if (role == null) return Result<AuthResponseDto>.Failure("Role configuration error.");

            var user = new UserInfo
            {
                Email        = request.Dto.Email.ToLower(),
                PasswordHash = _hasher.Hash(request.Dto.Password),
                FirstName    = request.Dto.FirstName,
                LastName     = request.Dto.LastName,
            };

            await _uow.Users.AddAsync(user, ct);
            await _uow.UserRoles.AssignRoleAsync(user.Id, role.Id, ct);

            var refreshToken = _jwt.GenerateRefreshToken();
            await _uow.Sessions.AddAsync(new Session
            {
                UserId       = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow.AddDays(7),
            }, ct);

            await _uow.SaveChangesAsync(ct);

            var roles       = new List<string> { publicRole };
            var accessToken = _jwt.GenerateAccessToken(user, roles);

            await _eventBus.PublishAsync(new UserRegisteredEvent
            {
                UserId   = user.Id,
                Email    = user.Email,
                FullName = user.FullName,
                Role     = publicRole,
            }, ct);

            return Result<AuthResponseDto>.Success(new AuthResponseDto(
                accessToken,
                refreshToken,
                _jwt.GetAccessTokenExpiry(),          // ← was DateTime.UtcNow.AddHours(1)
                MapUser(user, roles)), 201);
        }

        private static UserDto MapUser(UserInfo u, List<string> roles) => new(
            u.Id, u.FirstName, u.LastName, u.FullName,
            u.Email, u.AvatarUrl, u.Headline, u.IsEmailVerified, roles);
    }
}
