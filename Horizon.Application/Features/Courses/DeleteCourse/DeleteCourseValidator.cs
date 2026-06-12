

using FluentValidation;

namespace Horizon.Application.Features.Courses.DeleteCourse
{
    public class DeleteCourseValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
