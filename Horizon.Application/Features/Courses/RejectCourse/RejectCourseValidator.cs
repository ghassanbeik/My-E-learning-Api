
using FluentValidation;

namespace Horizon.Application.Features.Courses.RejectCourse
{
    public class RejectCourseValidator : AbstractValidator<RejectCourseCommand>
    {
        public RejectCourseValidator()
        {
            RuleFor(x => x.Reason).NotEmpty().MinimumLength(10).MaximumLength(1000)
                .WithMessage("Rejection reason must be between 10 and 1000 characters.");
        }
    }
}
