

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.ClearCart
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public ClearCartHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(ClearCartCommand request, CancellationToken ct)
        {
            await _uow.CartItems.ClearCartAsync(request.UserId, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
