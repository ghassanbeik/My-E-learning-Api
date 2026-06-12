

using FluentValidation;

namespace Horizon.Application.Features.Courses.PublishCourse
{
    public class PublishCourseValidator : AbstractValidator<PublishCourseCommand>
    {
        public PublishCourseValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
