
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Payments.ApproveRefund
{
    public record ApproveRefundCommand(Guid RefundRequestId, Guid AdminId) : IRequest<Result>;

}
