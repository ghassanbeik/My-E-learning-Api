

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.ValidateCoupon
{
    public class ValidateCouponHandler : IRequestHandler<ValidateCouponQuery, Result<ValidateCouponResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        public ValidateCouponHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<ValidateCouponResponseDto>> Handle(ValidateCouponQuery request, CancellationToken ct)
        {
            var coupon = await _uow.Coupons.GetByCodeAsync(request.Dto.Code, ct);
            if (coupon == null)
                return Result<ValidateCouponResponseDto>.Success(new ValidateCouponResponseDto(false, null, null, null, null, "Coupon not found."));

            var isValid = await _uow.Coupons.IsValidAsync(request.Dto.Code, request.Dto.CourseId, ct);
            if (!isValid)
                return Result<ValidateCouponResponseDto>.Success(new ValidateCouponResponseDto(false, coupon.Code, null, null, null, "Coupon is not valid or has expired."));

            var hasUsed = await _uow.Coupons.HasUserUsedAsync(request.Dto.Code, request.UserId, ct);
            if (hasUsed)
                return Result<ValidateCouponResponseDto>.Success(new ValidateCouponResponseDto(false, coupon.Code, null, null, null, "You have already used this coupon."));

            return Result<ValidateCouponResponseDto>.Success(new ValidateCouponResponseDto(
                true, coupon.Code, coupon.Type.ToString(), coupon.Value, coupon.MaxDiscountAmount, "Coupon is valid!"));
        }
    }
}
