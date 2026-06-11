

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.MoveToCart
{
    public class MoveToCartHandler : IRequestHandler<MoveToCartCommand, Result<CartItemDto>>
    {
        private readonly IUnitOfWork _uow;
        public MoveToCartHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CartItemDto>> Handle(MoveToCartCommand request, CancellationToken ct)
        {
            await _uow.Wishlists.RemoveAsync(request.UserId, request.CourseId, ct);

            if (!await _uow.CartItems.ExistsAsync(request.UserId, request.CourseId, ct))
            {
                var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
                if (course == null) return Result<CartItemDto>.NotFound("Course not found.");

                var cartItem = new Domain.Entities.CartItem
                { UserId = request.UserId, CourseId = request.CourseId };
                await _uow.CartItems.AddAsync(cartItem, ct);
                await _uow.SaveChangesAsync(ct);

                return Result<CartItemDto>.Success(new CartItemDto(
                    cartItem.Id, course.Id, course.Title, course.ThumbnailUrl,
                    course.Instructor?.FullName ?? string.Empty, course.Price,
                    course.DiscountPrice, course.CurrentPrice, null, null, cartItem.AddedAt));
            }

            await _uow.SaveChangesAsync(ct);
            return Result<CartItemDto>.Failure("Course already in cart.");
        }
    }
}
