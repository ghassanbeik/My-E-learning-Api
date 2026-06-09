
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Reviews.CreateReviewResponse
{
    public record CreateReviewResponseCommand(Guid ReviewId, Guid InstructorId, CreateReviewResponseDto Dto) : IRequest<Result<ReviewResponseDto>>;

}
