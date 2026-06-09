

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.RequestRefund
{
    public record RequestRefundCommand(Guid UserId, RefundRequestDto Dto) : IRequest<Result<RefundRequestResponseDto>>;

}
