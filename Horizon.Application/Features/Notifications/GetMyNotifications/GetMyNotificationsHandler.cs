

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetMyNotifications
{
    public class GetMyNotificationsHandler : IRequestHandler<GetMyNotificationsQuery, Result<PagedResponse<NotificationDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetMyNotificationsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<NotificationDto>>> Handle(GetMyNotificationsQuery request, CancellationToken ct)
        {
            var result = await _uow.Notifications.GetPagedByRecipientAsync(request.UserId, request.Page, request.PageSize, ct);
            var items = result.Items.Select(n => new NotificationDto(
                n.Id, n.Title, n.Message, n.Type.ToString(), n.Channel.ToString(),
                n.Status.ToString(), n.Status == NotificationStatus.Unread,
                n.ActionUrl, n.ImageUrl, n.RelatedEntityId, n.RelatedEntityType,
                n.ReadAt, n.CreatedAt));

            return Result<PagedResponse<NotificationDto>>.Success(
                PagedResponse<NotificationDto>.From(items, result.TotalCount, result.PageSize, result.PageSize));
        }
    }
}
