
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.PaymentEventHandlers
{
    public class PaymentCompletedEventHandler : IDomainEventHandler<PaymentCompletedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _uow;

        public PaymentCompletedEventHandler(
            IEmailService email,
            INotificationService notifications,
            IEventBus eventBus,
            IUnitOfWork uow)
        {
            _email = email;
            _notifications = notifications;
            _eventBus = eventBus;
            _uow = uow;
        }

        public async Task HandleAsync(PaymentCompletedEvent e, CancellationToken ct = default)
        {
            // Send payment receipt email
            await _email.SendPaymentReceiptAsync(e.UserEmail, e.UserName, new PaymentReceiptData
            {
                TransactionId = e.TransactionId,
                CourseTitle = e.CourseTitle,
                Amount = e.Amount,
                Currency = e.Currency,
                PaidAt = e.OccurredAt,
                PaymentMethod = e.PaymentMethod,
            }, ct);

            // In-app notification
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.UserId,
                Title = "Payment successful",
                Message = $"Payment of {e.Currency} {e.Amount:F2} for '{e.CourseTitle}' confirmed.",
                Type = Domain.Enums.NotificationType.PaymentReceived,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
            }, ct);

            // Fire enrollment event
            var student = await _uow.Users.GetByIdAsync(e.UserId, ct);
            if (student == null) return;

            var enrollment = await _uow.Enrollments.GetByStudentAndCourseAsync(e.UserId, e.CourseId, ct);
            if (enrollment == null) return;

            await _eventBus.PublishAsync(new StudentEnrolledEvent
            {
                EnrollmentId = enrollment.Id,
                StudentId = e.UserId,
                CourseId = e.CourseId,
                InstructorId = e.InstructorId,
                StudentEmail = e.UserEmail,
                StudentName = e.UserName,
                CourseTitle = e.CourseTitle,
                AmountPaid = e.Amount,
            }, ct);
        }
    }

    
}
