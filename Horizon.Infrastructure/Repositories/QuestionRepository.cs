

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class QuestionRepository
    : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
