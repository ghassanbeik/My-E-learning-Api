
using FluentValidation;

namespace Horizon.Application.Features.Auth.UpdateProfile
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Dto.FirstName).MaximumLength(100).When(x => x.Dto.FirstName != null);
            RuleFor(x => x.Dto.LastName).MaximumLength(100).When(x => x.Dto.LastName != null);
            RuleFor(x => x.Dto.Bio).MaximumLength(2000).When(x => x.Dto.Bio != null);
            RuleFor(x => x.Dto.Headline).MaximumLength(200).When(x => x.Dto.Headline != null);
            RuleFor(x => x.Dto.Website).MaximumLength(300).Must(url =>
                url == null || Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Invalid website URL.");
        }
    }
}
