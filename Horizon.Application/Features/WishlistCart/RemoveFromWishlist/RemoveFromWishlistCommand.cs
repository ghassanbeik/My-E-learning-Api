

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.RemoveFromWishlist
{
    public record RemoveFromWishlistCommand(Guid UserId, Guid CourseId) : IRequest<Result>;

}
