
using FluentValidation;

namespace Horizon.Application.Features.Auth.VerifyEmail
{
    public class VerifyEmailValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Token).NotEmpty()
                .WithMessage("Verification token is required.");
        }
    }
}
