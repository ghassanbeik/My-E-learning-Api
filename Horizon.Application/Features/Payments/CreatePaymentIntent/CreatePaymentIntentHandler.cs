

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using MediatR;

namespace Horizon.Application.Features.Payments.CreatePaymentIntent
{
    public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, Result<PaymentIntentResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPaymentService _payment;

        public CreatePaymentIntentHandler(IUnitOfWork uow, IPaymentService payment) { _uow = uow; _payment = payment; }

        public async Task<Result<PaymentIntentResponseDto>> Handle(CreatePaymentIntentCommand request, CancellationToken ct)
        {
            if (await _uow.Enrollments.IsEnrolledAsync(request.UserId, request.Dto.CourseId, ct))
                return Result<PaymentIntentResponseDto>.Conflict("Already enrolled in this course.");

            var course = await _uow.Courses.GetByIdAsync(request.Dto.CourseId, ct);
            if (course == null) return Result<PaymentIntentResponseDto>.NotFound("Course not found.");

            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<PaymentIntentResponseDto>.NotFound("User not found.");

            decimal amount = course.CurrentPrice;

            if (!string.IsNullOrEmpty(request.Dto.CouponCode))
            {
                var coupon = await _uow.Coupons.GetByCodeAsync(request.Dto.CouponCode, ct);
                if (coupon != null && await _uow.Coupons.IsValidAsync(request.Dto.CouponCode, course.Id, ct))
                {
                    var discount = coupon.Type == CouponType.Percentage
                        ? amount * coupon.Value / 100
                        : coupon.Value;
                    if (coupon.MaxDiscountAmount.HasValue)
                        discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);
                    amount = Math.Max(0, amount - discount);
                }
            }

            if (amount == 0)
                return Result<PaymentIntentResponseDto>.Failure("Use the free enroll endpoint for free courses.");

            var result = await _payment.CreatePaymentIntentAsync(new CreatePaymentIntentRequest
            {
                UserId = request.UserId,
                CourseId = request.Dto.CourseId,
                Amount = amount,
                Currency = course.Currency,
                CustomerEmail = user.Email,
                CouponCode = request.Dto.CouponCode,
            }, ct);

            if (!result.Success) return Result<PaymentIntentResponseDto>.Failure(result.Error ?? "Payment failed.");

            return Result<PaymentIntentResponseDto>.Success(new PaymentIntentResponseDto(
                result.ClientSecret!, result.PaymentIntentId!, amount, course.Currency));
        }
    }
}
