

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.AddToWishlist
{
    public record AddToWishlistCommand(Guid UserId, Guid CourseId) : IRequest<Result>;
}
