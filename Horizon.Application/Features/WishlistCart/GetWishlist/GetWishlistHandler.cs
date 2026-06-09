

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.WishlistCart.GetWishlis;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.GetWishlist
{
    public class GetWishlistHandler : IRequestHandler<GetWishlistQuery, Result<List<WishlistDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetWishlistHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<WishlistDto>>> Handle(GetWishlistQuery request, CancellationToken ct)
        {
            var items = await _uow.Wishlists.GetByUserAsync(request.UserId, ct);
            return Result<List<WishlistDto>>.Success(items.Select(w => new WishlistDto(
                w.Id, w.CourseId, w.Course.Title, w.Course.ThumbnailUrl,
                w.Course.Instructor.FullName, w.Course.CurrentPrice,
                w.Course.AverageRating, w.AddedAt)).ToList());
        }
    }
}
