

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class DiscussionReplyRepository : Repository<DiscussionReply>, IDiscussionReplyRepository
    {
        public DiscussionReplyRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<DiscussionReply>> GetByDiscussionAsync(Guid discussionId, CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.User)
                .Where(r => r.DiscussionId == discussionId && r.ParentReplyId == null)
                .OrderBy(r => r.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<DiscussionReply>> GetChildRepliesAsync(Guid parentReplyId, CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.User)
                .Where(r => r.ParentReplyId == parentReplyId)
                .OrderBy(r => r.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
