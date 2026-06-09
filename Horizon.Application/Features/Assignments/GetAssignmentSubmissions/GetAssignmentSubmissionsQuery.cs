

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Assignments.GetAssignmentSubmissions
{
    public record GetAssignmentSubmissionsQuery(Guid AssignmentId, Guid InstructorId) : IRequest<Result<List<AssignmentSubmissionDto>>>;

}
