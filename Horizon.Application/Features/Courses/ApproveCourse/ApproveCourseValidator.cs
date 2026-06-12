

using FluentValidation;

namespace Horizon.Application.Features.Courses.ApproveCourse
{
    public class ApproveCourseValidator : AbstractValidator<ApproveCourseCommand>
    {
        public ApproveCourseValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
