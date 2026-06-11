
using FluentValidation;

namespace Horizon.Application.Features.Payments.RequestRefund
{
    public class RequestRefundValidator : AbstractValidator<RequestRefundCommand>
    {
        public RequestRefundValidator()
        {
            RuleFor(x => x.Dto.PaymentId).NotEmpty();
            RuleFor(x => x.Dto.Reason).NotEmpty().MinimumLength(10).MaximumLength(1000);
        }
    }
}
