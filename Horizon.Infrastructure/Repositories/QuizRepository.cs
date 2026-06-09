

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        public QuizRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Quiz?> GetWithQuestionsAsync(Guid quizId, CancellationToken ct = default)
            => await _dbSet
                .Include(q => q.Questions.OrderBy(q => q.DisplayOrder))
                    .ThenInclude(q => q.AnswerOptions.OrderBy(a => a.DisplayOrder))
                .FirstOrDefaultAsync(q => q.Id == quizId, ct);

        public async Task<IEnumerable<Quiz>> GetByLessonAsync(Guid lessonId, CancellationToken ct = default)
            => await _dbSet.Where(q => q.LessonId == lessonId).AsNoTracking().ToListAsync(ct);
    }

}
