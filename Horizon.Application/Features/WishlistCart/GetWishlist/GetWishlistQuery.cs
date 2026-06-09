

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.GetWishlis
{
    public record GetWishlistQuery(Guid UserId) : IRequest<Result<List<WishlistDto>>>;

}
