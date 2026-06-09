

using FluentValidation;

namespace Horizon.Application.Features.Sections.CreateSection
{
    public class CreateSectionValidator : AbstractValidator<CreateSectionCommand>
    {
        public CreateSectionValidator()
        {
            RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo(0);
        }
    }
}
