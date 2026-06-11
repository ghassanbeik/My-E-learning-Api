

using FluentValidation;

namespace Horizon.Application.Features.Payments.ConfirmPayment
{
    public class ConfirmPaymentValidator : AbstractValidator<ConfirmPaymentCommand>
    {
        public ConfirmPaymentValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.PaymentIntentId).NotEmpty();
        }
    }
}
