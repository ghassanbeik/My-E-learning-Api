

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.GetActiveCoupons
{
    public record GetActiveCouponsQuery() : IRequest<Result<List<CouponDto>>>;

}
