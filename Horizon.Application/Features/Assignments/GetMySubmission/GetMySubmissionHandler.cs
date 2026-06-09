

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Assignments.GetMySubmission
{
    public class GetMySubmissionHandler
        : IRequestHandler<GetMySubmissionQuery, Result<AssignmentSubmissionDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetMySubmissionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<AssignmentSubmissionDto>> Handle(
            GetMySubmissionQuery request, CancellationToken ct)
        {
            var submission = await _uow.AssignmentSubmissions
                .GetByStudentAndAssignmentAsync(request.StudentId, request.AssignmentId, ct);

            if (submission == null)
                return Result<AssignmentSubmissionDto>.NotFound("No submission found for this assignment.");

            var student = await _uow.Users.GetByIdAsync(submission.StudentId, ct);
            var gradedBy = submission.GradedById.HasValue
                ? await _uow.Users.GetByIdAsync(submission.GradedById.Value, ct)
                : null;

            return Result<AssignmentSubmissionDto>.Success(new AssignmentSubmissionDto(
                submission.Id, submission.AssignmentId, submission.StudentId,
                student?.FullName ?? string.Empty,
                submission.SubmissionText, submission.FileUrl, submission.FileName,
                submission.SubmittedAt, submission.Score, submission.Feedback,
                submission.IsGraded, gradedBy?.FullName, submission.IsLate));
        }
    }
}
