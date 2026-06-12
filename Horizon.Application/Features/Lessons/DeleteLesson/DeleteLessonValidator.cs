

using FluentValidation;

namespace Horizon.Application.Features.Lessons.DeleteLesson
{
    public class DeleteLessonValidator : AbstractValidator<DeleteLessonCommand>
    {
        public DeleteLessonValidator()
        {
            RuleFor(x => x.LessonId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
