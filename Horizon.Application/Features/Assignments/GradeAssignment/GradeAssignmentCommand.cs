

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Assignments.GradeAssignment
{
    public record GradeAssignmentCommand(Guid SubmissionId, Guid InstructorId, GradeAssignmentDto Dto) : IRequest<Result<AssignmentSubmissionDto>>;

}
