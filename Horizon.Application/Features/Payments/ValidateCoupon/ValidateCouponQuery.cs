

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.ValidateCoupon
{
    public record ValidateCouponQuery(Guid UserId, ValidateCouponDto Dto) : IRequest<Result<ValidateCouponResponseDto>>;

}
