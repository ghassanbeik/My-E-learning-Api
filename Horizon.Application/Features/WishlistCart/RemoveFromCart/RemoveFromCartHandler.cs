

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.RemoveFromCart
{
    public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public RemoveFromCartHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken ct)
        {
            var item = await _uow.CartItems.FirstOrDefaultAsync(
                ci => ci.UserId == request.UserId && ci.CourseId == request.CourseId, ct);
            if (item == null) return Result.NotFound("Item not found in cart.");

            await _uow.CartItems.DeleteAsync(item);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
