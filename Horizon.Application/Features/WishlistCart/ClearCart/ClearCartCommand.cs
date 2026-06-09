

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.ClearCart
{
    public record ClearCartCommand(Guid UserId) : IRequest<Result>;

}
