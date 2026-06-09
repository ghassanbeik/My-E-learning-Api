
using FluentValidation;

namespace Horizon.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.Dto.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");
            RuleFor(x => x.Dto.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
