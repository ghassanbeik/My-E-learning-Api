

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.CreateTag
{
    public record CreateTagCommand(CreateTagDto Dto) : IRequest<Result<TagDto>>;

}
