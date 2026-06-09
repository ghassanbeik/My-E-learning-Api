

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Domain.Shared;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class DiscussionRepository : Repository<Discussion>, IDiscussionRepository
    {
        public DiscussionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Discussion?> GetWithRepliesAsync(Guid discussionId, CancellationToken ct = default)
            => await _dbSet
                .Include(d => d.User)
                .Include(d => d.Replies.Where(r => r.ParentReplyId == null))
                    .ThenInclude(r => r.User)
                .Include(d => d.Replies.Where(r => r.ParentReplyId == null))
                    .ThenInclude(r => r.ChildReplies).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(d => d.Id == discussionId, ct);

        public async Task<PagedResult<Discussion>> GetByCourseAsync(Guid courseId, int page, int pageSize, DiscussionType? type = null, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(d => d.User)
                .Where(d => d.CourseId == courseId);

            if (type.HasValue) query = query.Where(d => d.Type == type);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(d => d.IsPinned)
                .ThenByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Discussion> { Items = items, TotalCount = total, PageNumber = page, PageSize = pageSize };
        }

        public async Task<PagedResult<Discussion>> GetByLessonAsync(Guid lessonId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbSet.Include(d => d.User).Where(d => d.LessonId == lessonId);
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Discussion> { Items = items, TotalCount = total, PageNumber = page, PageSize = pageSize };
        }

        public async Task IncrementReplyCountAsync(Guid discussionId, CancellationToken ct = default)
        {
            var discussion = await _dbSet.FindAsync(new object[] { discussionId }, ct);
            if (discussion != null) discussion.ReplyCount++;
        }

        public async Task IncrementUpvoteAsync(Guid discussionId, CancellationToken ct = default)
        {
            var discussion = await _dbSet.FindAsync(new object[] { discussionId }, ct);
            if (discussion != null) discussion.UpvoteCount++;
        }

        public async Task<IEnumerable<Discussion>> GetPinnedAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(d => d.User)
                .Where(d => d.CourseId == courseId && d.IsPinned)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
