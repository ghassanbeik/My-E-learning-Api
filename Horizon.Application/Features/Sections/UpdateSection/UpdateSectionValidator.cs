

using FluentValidation;

namespace Horizon.Application.Features.Sections.UpdateSection
{
    public class UpdateSectionValidator : AbstractValidator<UpdateSectionCommand>
    {
        public UpdateSectionValidator()
        {
            RuleFor(x => x.Dto.Title).MaximumLength(200).When(x => x.Dto.Title != null);
        }
    }
}
