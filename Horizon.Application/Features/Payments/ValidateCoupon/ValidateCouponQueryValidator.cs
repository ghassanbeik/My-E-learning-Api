
using FluentValidation;

namespace Horizon.Application.Features.Payments.ValidateCoupon
{
    public class ValidateCouponQueryValidator : AbstractValidator<ValidateCouponQuery>
    {
        public ValidateCouponQueryValidator()
        {
            RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Dto.CourseId).NotEmpty();
        }
    }
}
