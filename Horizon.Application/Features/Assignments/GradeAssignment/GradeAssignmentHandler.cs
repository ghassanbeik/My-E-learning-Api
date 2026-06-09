

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using MediatR;

namespace Horizon.Application.Features.Assignments.GradeAssignment
{
    public class GradeAssignmentHandler : IRequestHandler<GradeAssignmentCommand, Result<AssignmentSubmissionDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly INotificationService _notifications;

        public GradeAssignmentHandler(IUnitOfWork uow, INotificationService notifications)
        {
            _uow = uow;
            _notifications = notifications;
        }

        public async Task<Result<AssignmentSubmissionDto>> Handle(GradeAssignmentCommand request, CancellationToken ct)
        {
            var submission = await _uow.AssignmentSubmissions.GetByIdAsync(request.SubmissionId, ct);
            if (submission == null) return Result<AssignmentSubmissionDto>.NotFound("Submission not found.");

            var assignment = await _uow.Assignments.GetByIdAsync(submission.AssignmentId, ct);
            if (assignment == null) return Result<AssignmentSubmissionDto>.NotFound("Assignment not found.");

            if (request.Dto.Score < 0 || request.Dto.Score > assignment.TotalPoints)
                return Result<AssignmentSubmissionDto>.Failure($"Score must be between 0 and {assignment.TotalPoints}.");

            submission.Score = request.Dto.Score;
            submission.Feedback = request.Dto.Feedback;
            submission.IsGraded = true;
            submission.GradedAt = DateTime.UtcNow;
            submission.GradedById = request.InstructorId;
            await _uow.AssignmentSubmissions.UpdateAsync(submission);
            await _uow.SaveChangesAsync(ct);

            var instructor = await _uow.Users.GetByIdAsync(request.InstructorId, ct);
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = submission.StudentId,
                Title = "Assignment graded",
                Message = $"Your assignment has been graded. Score: {request.Dto.Score}/{assignment.TotalPoints}.",
                Type = NotificationType.AssignmentGraded,
                Channel = NotificationChannel.InApp,
                SenderName = instructor?.FullName,
                SenderId = request.InstructorId,
            }, ct);

            var student = await _uow.Users.GetByIdAsync(submission.StudentId, ct);
            return Result<AssignmentSubmissionDto>.Success(new AssignmentSubmissionDto(
                submission.Id, submission.AssignmentId, submission.StudentId,
                student?.FullName ?? string.Empty, submission.SubmissionText,
                submission.FileUrl, submission.FileName, submission.SubmittedAt,
                submission.Score, submission.Feedback, submission.IsGraded,
                instructor?.FullName, submission.IsLate));
        }
    }

}
