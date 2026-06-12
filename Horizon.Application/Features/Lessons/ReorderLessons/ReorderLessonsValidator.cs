

using FluentValidation;

namespace Horizon.Application.Features.Lessons.ReorderLessons
{
    public class ReorderLessonsValidator : AbstractValidator<ReorderLessonsCommand>
    {
        public ReorderLessonsValidator()
        {
            RuleFor(x => x.SectionId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.Orders).NotEmpty()
                .WithMessage("At least one lesson order entry is required.");
        }
    }
}
