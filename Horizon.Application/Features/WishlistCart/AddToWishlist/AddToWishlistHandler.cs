

using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.AddToWishlist
{
    public class AddToWishlistHandler : IRequestHandler<AddToWishlistCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public AddToWishlistHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(AddToWishlistCommand request, CancellationToken ct)
        {
            if (await _uow.Wishlists.ExistsAsync(request.UserId, request.CourseId, ct))
                return Result.Conflict("Course already in wishlist.");
            if (await _uow.Enrollments.IsEnrolledAsync(request.UserId, request.CourseId, ct))
                return Result.Conflict("Already enrolled in this course.");

            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");

            await _uow.Wishlists.AddAsync(new Wishlist { UserId = request.UserId, CourseId = request.CourseId }, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
