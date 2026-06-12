

using FluentValidation;

namespace Horizon.Application.Features.Courses.UnpublishCourse
{
    public class UnpublishCourseValidator : AbstractValidator<UnpublishCourseCommand>
    {
        public UnpublishCourseValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
