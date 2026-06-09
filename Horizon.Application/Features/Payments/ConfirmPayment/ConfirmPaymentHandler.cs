
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using MediatR;

namespace Horizon.Application.Features.Payments.ConfirmPayment
{
    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Result<PaymentDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPaymentService _payment;
        private readonly IEventBus _eventBus;

        public ConfirmPaymentHandler(IUnitOfWork uow, IPaymentService payment, IEventBus eventBus)
        {
            _uow = uow; _payment = payment; _eventBus = eventBus;
        }

        public async Task<Result<PaymentDto>> Handle(ConfirmPaymentCommand request, CancellationToken ct)
        {
            var result = await _payment.ConfirmPaymentAsync(request.PaymentIntentId, ct);
            if (!result.Success) return Result<PaymentDto>.Failure(result.Error ?? "Payment confirmation failed.");

            var course = await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct);
            if (course == null) return Result<PaymentDto>.NotFound("Course not found.");
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<PaymentDto>.NotFound("User not found.");

            var platformFee = result.Amount * 0.15m;
            var instructorEarning = result.Amount - platformFee;

            var payment = new Payment
            {
                UserId = request.UserId,
                TransactionId = result.TransactionId!,
                PaymentMethod = "Stripe",
                Amount = result.Amount,
                Currency = course.Currency,
                Status = PaymentStatus.Completed,
                PaidAt = DateTime.UtcNow,
                PlatformFee = platformFee,
                InstructorEarnings = instructorEarning,
            };

            await _uow.Payments.AddAsync(payment, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new PaymentCompletedEvent
            {
                PaymentId = payment.Id,
                UserId = request.UserId,
                CourseId = request.CourseId,
                InstructorId = course.InstructorId,
                UserEmail = user.Email,
                UserName = user.FullName,
                CourseTitle = course.Title,
                TransactionId = result.TransactionId!,
                Amount = result.Amount,
                Currency = course.Currency,
                PaymentMethod = "Stripe",
            }, ct);

            return Result<PaymentDto>.Success(new PaymentDto(
                payment.Id, null, payment.TransactionId, payment.PaymentMethod,
                payment.Amount, payment.Currency, payment.Status.ToString(),
                payment.PaidAt, null, null));
        }
    }
}
