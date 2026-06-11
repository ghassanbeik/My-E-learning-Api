

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Categories.CreateTag
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Result<TagDto>>
    {
        private readonly IUnitOfWork _uow;
        public CreateTagHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<TagDto>> Handle(CreateTagCommand request, CancellationToken ct)
        {
            var exists = await _uow.Tags.GetByNameAsync(request.Dto.Name, ct);
            if (exists != null)
                return Result<TagDto>.Conflict("Tag name already exists.");

            var tag = new Tag { Name = request.Dto.Name, Description = request.Dto.Description };
            await _uow.Tags.AddAsync(tag, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<TagDto>.Success(new TagDto(tag.Id, tag.Name, tag.Description, 0), 201);
        }
    }
}
