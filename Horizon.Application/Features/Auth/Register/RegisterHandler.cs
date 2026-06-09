

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
            _uow = uow;
            _hasher = hasher;
            _jwt = jwt;
            _eventBus = eventBus;
        }

        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct)
        {
            if (await _uow.Users.EmailExistsAsync(request.Dto.Email, ct))
                return Result<AuthResponseDto>.Conflict("Email already registered.");

            var role = await _uow.Roles.GetByNameAsync(request.Dto.Role, ct);
            if (role == null) return Result<AuthResponseDto>.Failure("Invalid role.");

            var user = new UserInfo
            {
                Email = request.Dto.Email.ToLower(),
                PasswordHash = _hasher.Hash(request.Dto.Password),
                FirstName = request.Dto.FirstName,
                LastName = request.Dto.LastName,
            };

            await _uow.Users.AddAsync(user, ct);
            await _uow.UserRoles.AssignRoleAsync(user.Id, role.Id, ct);

            var refreshToken = _jwt.GenerateRefreshToken();
            var expiry = int.Parse("7");
            await _uow.Sessions.AddAsync(new Session
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(expiry),
            }, ct);

            await _uow.SaveChangesAsync(ct);

            var roles = new List<string> { request.Dto.Role };
            var accessToken = _jwt.GenerateAccessToken(user, roles);

            await _eventBus.PublishAsync(new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = request.Dto.Role,
            }, ct);

            return Result<AuthResponseDto>.Success(new AuthResponseDto(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddHours(1),
                MapUser(user, roles)), 201);
        }

        private static UserDto MapUser(UserInfo user, List<string> roles) => new(
            user.Id, user.FirstName, user.LastName, user.FullName,
            user.Email, user.AvatarUrl, user.Headline, user.IsEmailVerified, roles);
    }

}
