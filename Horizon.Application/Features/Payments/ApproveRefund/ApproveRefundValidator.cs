

using FluentValidation;

namespace Horizon.Application.Features.Payments.ApproveRefund
{
    public class ApproveRefundValidator : AbstractValidator<ApproveRefundCommand>
    {
        public ApproveRefundValidator()
        {
            RuleFor(x => x.RefundRequestId).NotEmpty();
            RuleFor(x => x.AdminId).NotEmpty();
        }
    }
}
