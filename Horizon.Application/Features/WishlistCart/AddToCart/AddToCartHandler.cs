

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.AddToCart
{
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Result<CartItemDto>>
    {
        private readonly IUnitOfWork _uow;
        public AddToCartHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CartItemDto>> Handle(AddToCartCommand request, CancellationToken ct)
        {
            if (await _uow.CartItems.ExistsAsync(request.UserId, request.CourseId, ct))
                return Result<CartItemDto>.Conflict("Course already in cart.");
            if (await _uow.Enrollments.IsEnrolledAsync(request.UserId, request.CourseId, ct))
                return Result<CartItemDto>.Conflict("Already enrolled in this course.");

            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result<CartItemDto>.NotFound("Course not found.");

            decimal? discountAmount = null;
            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var coupon = await _uow.Coupons.GetByCodeAsync(request.CouponCode, ct);
                if (coupon != null && await _uow.Coupons.IsValidAsync(request.CouponCode, course.Id, ct))
                {
                    discountAmount = coupon.Type == CouponType.Percentage
                        ? course.CurrentPrice * coupon.Value / 100
                        : coupon.Value;
                    if (coupon.MaxDiscountAmount.HasValue)
                        discountAmount = Math.Min(discountAmount.Value, coupon.MaxDiscountAmount.Value);
                }
            }

            var item = new CartItem
            {
                UserId = request.UserId,
                CourseId = request.CourseId,
                CouponCode = request.CouponCode,
                DiscountAmount = discountAmount,
            };

            await _uow.CartItems.AddAsync(item, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<CartItemDto>.Success(new CartItemDto(
                item.Id, course.Id, course.Title, course.ThumbnailUrl,
                course.Instructor.FullName, course.Price, course.DiscountPrice,
                course.CurrentPrice, request.CouponCode, discountAmount, item.AddedAt), 201);
        }
    }

}
