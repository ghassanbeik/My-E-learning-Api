
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.RemoveFromCart
{
    public record RemoveFromCartCommand(Guid UserId, Guid CourseId) : IRequest<Result>;

}
