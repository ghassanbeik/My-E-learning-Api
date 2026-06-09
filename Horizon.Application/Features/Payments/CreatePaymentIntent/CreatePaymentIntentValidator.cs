

using FluentValidation;

namespace Horizon.Application.Features.Payments.CreatePaymentIntent
{
    public class CreatePaymentIntentValidator : AbstractValidator<CreatePaymentIntentCommand>
    {
        public CreatePaymentIntentValidator()
        {
            RuleFor(x => x.Dto.CourseId).NotEmpty();
            RuleFor(x => x.Dto.CouponCode).MaximumLength(50).When(x => x.Dto.CouponCode != null);
        }
    }
}
