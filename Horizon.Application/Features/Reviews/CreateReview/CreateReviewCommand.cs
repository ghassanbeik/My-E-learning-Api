
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Reviews.CreateReview
{
    public record CreateReviewCommand(Guid StudentId, CreateReviewDto Dto) : IRequest<Result<ReviewDto>>;
}
