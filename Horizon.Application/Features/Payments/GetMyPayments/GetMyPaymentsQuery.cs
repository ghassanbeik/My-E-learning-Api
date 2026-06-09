

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.GetMyPayments
{
    public record GetMyPaymentsQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<PaymentDto>>>;

}
