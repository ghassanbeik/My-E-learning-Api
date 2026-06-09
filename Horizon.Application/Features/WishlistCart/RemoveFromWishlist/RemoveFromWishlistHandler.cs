

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.RemoveFromWishlist
{
    public class RemoveFromWishlistHandler : IRequestHandler<RemoveFromWishlistCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public RemoveFromWishlistHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(RemoveFromWishlistCommand request, CancellationToken ct)
        {
            var removed = await _uow.Wishlists.RemoveAsync(request.UserId, request.CourseId, ct);
            if (!removed) return Result.NotFound("Item not found in wishlist.");
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
