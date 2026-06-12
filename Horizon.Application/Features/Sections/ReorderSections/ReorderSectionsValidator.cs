
using FluentValidation;

namespace Horizon.Application.Features.Sections.ReorderSections
{
    public class ReorderSectionsValidator : AbstractValidator<ReorderSectionsCommand>
    {
        public ReorderSectionsValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.Orders).NotEmpty()
                .WithMessage("At least one section order entry is required.");
        }
    }
}
