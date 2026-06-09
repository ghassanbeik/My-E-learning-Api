

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Assignments.GetAssignmentSubmissions
{
    public class GetAssignmentSubmissionsHandler
         : IRequestHandler<GetAssignmentSubmissionsQuery, Result<List<AssignmentSubmissionDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetAssignmentSubmissionsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<AssignmentSubmissionDto>>> Handle(
            GetAssignmentSubmissionsQuery request, CancellationToken ct)
        {
            var assignment = await _uow.Assignments.GetByIdAsync(request.AssignmentId, ct);
            if (assignment == null)
                return Result<List<AssignmentSubmissionDto>>.NotFound("Assignment not found.");

            var lesson = await _uow.Lessons.GetByIdAsync(assignment.LessonId, ct);
            var section = lesson != null ? await _uow.Sections.GetByIdAsync(lesson.SectionId, ct) : null;
            var course = section != null ? await _uow.Courses.GetByIdAsync(section.CourseId, ct) : null;

            if (course?.InstructorId != request.InstructorId)
                return Result<List<AssignmentSubmissionDto>>.Forbidden("You do not own this course.");

            var submissions = await _uow.AssignmentSubmissions.GetByAssignmentAsync(request.AssignmentId, ct);

            var dtos = new List<AssignmentSubmissionDto>();
            foreach (var s in submissions)
            {
                var student = await _uow.Users.GetByIdAsync(s.StudentId, ct);
                var gradedBy = s.GradedById.HasValue
                    ? await _uow.Users.GetByIdAsync(s.GradedById.Value, ct)
                    : null;

                dtos.Add(new AssignmentSubmissionDto(
                    s.Id, s.AssignmentId, s.StudentId,
                    student?.FullName ?? string.Empty,
                    s.SubmissionText, s.FileUrl, s.FileName,
                    s.SubmittedAt, s.Score, s.Feedback,
                    s.IsGraded, gradedBy?.FullName, s.IsLate));
            }

            return Result<List<AssignmentSubmissionDto>>.Success(dtos);
        }
    }
}
