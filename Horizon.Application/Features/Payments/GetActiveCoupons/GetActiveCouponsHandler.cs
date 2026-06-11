

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.GetActiveCoupons
{
    public class GetActiveCouponsHandler : IRequestHandler<GetActiveCouponsQuery, Result<List<CouponDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetActiveCouponsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<CouponDto>>> Handle(GetActiveCouponsQuery request, CancellationToken ct)
        {
            var coupons = await _uow.Coupons.GetActiveAsync(ct);
            return Result<List<CouponDto>>.Success(coupons.Select(c => new CouponDto(
                c.Id, c.Code, c.Description, c.Type.ToString(),
                c.Value, c.MaxDiscountAmount, c.MinOrderAmount, c.ExpiryDate,
                c.MaxUses, c.CurrentUses, c.IsActive, true)).ToList());
        }
    }
}
