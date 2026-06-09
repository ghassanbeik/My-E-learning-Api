

using FluentValidation;

namespace Horizon.Application.Features.Auth.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Dto.CurrentPassword).NotEmpty();
            RuleFor(x => x.Dto.NewPassword).NotEmpty().MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Must contain uppercase.")
                .Matches("[0-9]").WithMessage("Must contain a number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Must contain a special character.");
            RuleFor(x => x.Dto.ConfirmPassword).Equal(x => x.Dto.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }
}
