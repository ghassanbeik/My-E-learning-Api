
using FluentValidation;

namespace Horizon.Application.Features.Enrollments.Unenroll
{
    public class UnenrollValidator : AbstractValidator<UnenrollCommand>
    {
        public UnenrollValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
