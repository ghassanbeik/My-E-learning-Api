

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.CreateCoupon
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponCommand, Result<CouponDto>>
    {
        private readonly IUnitOfWork _uow;
        public CreateCouponHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CouponDto>> Handle(CreateCouponCommand request, CancellationToken ct)
        {
            var existing = await _uow.Coupons.GetByCodeAsync(request.Dto.Code, ct);
            if (existing != null)
                return Result<CouponDto>.Conflict("Coupon code already exists.");

            var coupon = new Coupon
            {
                Code = request.Dto.Code.ToUpper(),
                Description = request.Dto.Description,
                Type = Enum.Parse<CouponType>(request.Dto.Type),
                Value = request.Dto.Value,
                MaxDiscountAmount = request.Dto.MaxDiscountAmount,
                MinOrderAmount = request.Dto.MinOrderAmount,
                StartDate = request.Dto.StartDate,
                ExpiryDate = request.Dto.ExpiryDate,
                MaxUses = request.Dto.MaxUses,
                MaxUsesPerUser = request.Dto.MaxUsesPerUser,
                IsActive = true,
            };

            await _uow.Coupons.AddAsync(coupon, ct);
            await _uow.SaveChangesAsync(ct);

            if (request.Dto.CourseIds?.Any() == true)
            {
                foreach (var courseId in request.Dto.CourseIds)
                    await _uow.CouponCourses.AddAsync(
                        new CouponCourse { CouponId = coupon.Id, CourseId = courseId }, ct);
            }

            if (request.Dto.CategoryIds?.Any() == true)
            {
                foreach (var catId in request.Dto.CategoryIds)
                    await _uow.CouponCategories.AddAsync(
                        new CouponCategory { CouponId = coupon.Id, CategoryId = catId }, ct);
            }

            await _uow.SaveChangesAsync(ct);

            return Result<CouponDto>.Success(new CouponDto(
                coupon.Id, coupon.Code, coupon.Description, coupon.Type.ToString(),
                coupon.Value, coupon.MaxDiscountAmount, coupon.MinOrderAmount,
                coupon.ExpiryDate, coupon.MaxUses, coupon.CurrentUses,
                coupon.IsActive, true), 201);
        }
    }
}
