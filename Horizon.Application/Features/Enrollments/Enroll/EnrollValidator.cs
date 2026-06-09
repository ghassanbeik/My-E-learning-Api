

using FluentValidation;

namespace Horizon.Application.Features.Enrollments.Enroll
{
    public class EnrollValidator : AbstractValidator<EnrollCommand>
    {
        public EnrollValidator()
        {
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.CouponCode).MaximumLength(50).When(x => x.CouponCode != null);
        }
    }
}
