using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Notifications.GetMyNotifications;
using Horizon.Application.Features.Notifications.GetNotificationPreferences;
using Horizon.Application.Features.Notifications.GetUnreadCount;
using Horizon.Application.Features.Notifications.MarkAllRead;
using Horizon.Application.Features.Notifications.MarkNotificationRead;
using Horizon.Application.Features.Notifications.UpdateNotificationPreferences;
using Horizon.Infrastructure.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/notifications")]
    public class NotificationsController : BaseController
    {
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get my notifications</summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<NotificationDto>>), 200)]
        public async Task<IActionResult> GetMy([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetMyNotificationsQuery(UserId, page, pageSize), ct));

        /// <summary>Get unread count</summary>
        [HttpGet("unread-count")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<int>), 200)]
        public async Task<IActionResult> GetUnreadCount(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetUnreadCountQuery(UserId), ct));

        /// <summary>Mark a notification as read</summary>
        [HttpPut("{id:guid}/read")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkRead(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new MarkNotificationReadCommand(id, UserId), ct));

        /// <summary>Mark all notifications as read</summary>
        [HttpPut("read-all")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkAllRead(CancellationToken ct)
            => FromResult(await _mediator.Send(new MarkAllReadCommand(UserId), ct));

        /// <summary>Get notification preferences</summary>
        [HttpGet("preferences")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<NotificationPreferenceDto>>), 200)]
        public async Task<IActionResult> GetPreferences(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetNotificationPreferencesQuery(UserId), ct));

        /// <summary>Update notification preferences</summary>
        [HttpPut("preferences")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdatePreferences([FromBody] List<UpdateNotificationPreferenceDto> prefs, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpdateNotificationPreferencesCommand(UserId, prefs), ct));
    }
}
