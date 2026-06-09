

using FluentValidation;

namespace Horizon.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Dto.Token).NotEmpty();
            RuleFor(x => x.Dto.NewPassword).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Dto.ConfirmPassword).Equal(x => x.Dto.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }
}
