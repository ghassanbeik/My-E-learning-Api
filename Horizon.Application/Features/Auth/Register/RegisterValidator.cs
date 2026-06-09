

using FluentValidation;

namespace Horizon.Application.Features.Auth.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Dto.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Dto.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(100)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.Dto.Role).Must(r => new[] { "Student", "Instructor" }.Contains(r))
                .WithMessage("Role must be Student or Instructor.");
        }
    }
}
