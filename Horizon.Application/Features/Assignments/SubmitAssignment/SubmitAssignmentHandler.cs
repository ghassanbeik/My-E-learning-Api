

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Assignments.SubmitAssignment
{
    public class SubmitAssignmentHandler : IRequestHandler<SubmitAssignmentCommand, Result<AssignmentSubmissionDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public SubmitAssignmentHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<AssignmentSubmissionDto>> Handle(SubmitAssignmentCommand request, CancellationToken ct)
        {
            var assignment = await _uow.Assignments.GetByIdAsync(request.AssignmentId, ct);
            if (assignment == null) return Result<AssignmentSubmissionDto>.NotFound("Assignment not found.");

            var existing = await _uow.AssignmentSubmissions.GetByStudentAndAssignmentAsync(request.StudentId, request.AssignmentId, ct);

            var isLate = assignment.DueDate.HasValue && DateTime.UtcNow > assignment.DueDate;
            if (isLate && !assignment.AllowLateSubmission)
                return Result<AssignmentSubmissionDto>.Failure("Submission deadline has passed.");

            var student = await _uow.Users.GetByIdAsync(request.StudentId, ct);

            if (existing != null)
            {
                existing.SubmissionText = request.Dto.SubmissionText;
                existing.FileUrl = request.Dto.FileUrl;
                existing.FileName = request.Dto.FileName;
                existing.SubmittedAt = DateTime.UtcNow;
                existing.IsGraded = false;
                existing.AttemptNumber++;
                existing.IsLate = isLate;
                await _uow.AssignmentSubmissions.UpdateAsync(existing);
            }
            else
            {
                existing = new AssignmentSubmission
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = request.StudentId,
                    SubmissionText = request.Dto.SubmissionText,
                    FileUrl = request.Dto.FileUrl,
                    FileName = request.Dto.FileName,
                    SubmittedAt = DateTime.UtcNow,
                    IsLate = isLate,
                };
                await _uow.AssignmentSubmissions.AddAsync(existing, ct);
            }

            await _uow.SaveChangesAsync(ct);

            return Result<AssignmentSubmissionDto>.Success(new AssignmentSubmissionDto(
                existing.Id, existing.AssignmentId, existing.StudentId,
                student?.FullName ?? string.Empty, existing.SubmissionText,
                existing.FileUrl, existing.FileName, existing.SubmittedAt,
                existing.Score, existing.Feedback, existing.IsGraded, null, existing.IsLate));
        }
    }

}
