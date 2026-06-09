

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Events.DiscussionEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.CreateReply
{
    public class CreateReplyHandler : IRequestHandler<CreateReplyCommand, Result<DiscussionReplyDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public CreateReplyHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<DiscussionReplyDto>> Handle(CreateReplyCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result<DiscussionReplyDto>.NotFound("Discussion not found.");

            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<DiscussionReplyDto>.NotFound("User not found.");

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(request.UserId, ct)).ToList();
            var isInstructorReply = roles.Contains("Instructor") || roles.Contains("Admin");

            var reply = new DiscussionReply
            {
                DiscussionId = request.DiscussionId,
                UserId = request.UserId,
                Content = request.Dto.Content,
                ParentReplyId = request.Dto.ParentReplyId,
                IsInstructorAnswer = isInstructorReply,
            };

            await _uow.DiscussionReplies.AddAsync(reply, ct);
            await _uow.Discussions.IncrementReplyCountAsync(request.DiscussionId, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new DiscussionRepliedEvent
            {
                ReplyId = reply.Id,
                DiscussionId = request.DiscussionId,
                DiscussionAuthorId = discussion.UserId,
                ReplierId = request.UserId,
                ReplierName = user.FullName,
                IsInstructorReply = isInstructorReply,
            }, ct);

            return Result<DiscussionReplyDto>.Success(new DiscussionReplyDto(
                reply.Id, request.DiscussionId, user.Id, user.FullName, user.AvatarUrl,
                reply.Content, reply.ParentReplyId, reply.IsInstructorAnswer, 0, new(), reply.CreatedAt), 201);
        }
    }

}
