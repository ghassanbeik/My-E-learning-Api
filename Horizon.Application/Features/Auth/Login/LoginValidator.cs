

using FluentValidation;

namespace Horizon.Application.Features.Auth.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Dto.Password).NotEmpty();
        }
    }
}
