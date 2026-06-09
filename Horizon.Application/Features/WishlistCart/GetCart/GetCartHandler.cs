

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.GetCart
{
    public class GetCartHandler : IRequestHandler<GetCartQuery, Result<CartSummaryDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetCartHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CartSummaryDto>> Handle(GetCartQuery request, CancellationToken ct)
        {
            var items = await _uow.CartItems.GetByUserAsync(request.UserId, ct);
            var dtos = items.Select(ci => new CartItemDto(
                ci.Id, ci.CourseId, ci.Course.Title, ci.Course.ThumbnailUrl,
                ci.Course.Instructor.FullName, ci.Course.Price, ci.Course.DiscountPrice,
                ci.Course.CurrentPrice, ci.CouponCode, ci.DiscountAmount, ci.AddedAt)).ToList();

            var subTotal = dtos.Sum(i => i.OriginalPrice);
            var discount = dtos.Sum(i => i.DiscountAmount ?? (i.OriginalPrice - i.CurrentPrice));
            var total = Math.Max(0, subTotal - discount);

            return Result<CartSummaryDto>.Success(new CartSummaryDto(dtos, subTotal, discount, total, "USD"));
        }
    }
}
