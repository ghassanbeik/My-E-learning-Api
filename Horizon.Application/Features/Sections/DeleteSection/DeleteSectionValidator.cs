

using FluentValidation;

namespace Horizon.Application.Features.Sections.DeleteSection
{
    public class DeleteSectionValidator : AbstractValidator<DeleteSectionCommand>
    {
        public DeleteSectionValidator()
        {
            RuleFor(x => x.SectionId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
