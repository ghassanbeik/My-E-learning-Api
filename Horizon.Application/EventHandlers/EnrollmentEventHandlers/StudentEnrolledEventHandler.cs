

using Horizon.Domain.Events.CertificateEvents;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.EnrollmentEventHandlers
{
    public class StudentEnrolledEventHandler : IDomainEventHandler<StudentEnrolledEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;
        private readonly ICacheService _cache;

        public StudentEnrolledEventHandler(IEmailService email, INotificationService notifications, ICacheService cache)
        {
            _email = email;
            _notifications = notifications;
            _cache = cache;
        }

        public async Task HandleAsync(StudentEnrolledEvent e, CancellationToken ct = default)
        {
            // Email confirmation to student
            await _email.SendEnrollmentConfirmationAsync(e.StudentEmail, e.StudentName, e.CourseTitle, ct);

            // In-app notification to student
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.StudentId,
                Title = "Enrollment confirmed!",
                Message = $"You are now enrolled in '{e.CourseTitle}'. Start learning!",
                Type = Domain.Enums.NotificationType.NewEnrollment,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
            }, ct);

            // Notify instructor
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "New student enrolled",
                Message = $"{e.StudentName} enrolled in '{e.CourseTitle}'.",
                Type = Domain.Enums.NotificationType.NewEnrollment,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
                SenderName = e.StudentName,
                SenderId = e.StudentId,
            }, ct);

            // Invalidate course cache
            await _cache.RemoveAsync(CacheKeys.Course(e.CourseId), ct);
        }
    }

   

   
}
