

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class AnswerOptionRepository
    : Repository<AnswerOption>, IAnswerOptionRepository
    {
        public AnswerOptionRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
