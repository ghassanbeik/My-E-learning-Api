

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.CreateCoupon
{
    public record CreateCouponCommand(CreateCouponDto Dto) : IRequest<Result<CouponDto>>;

}
