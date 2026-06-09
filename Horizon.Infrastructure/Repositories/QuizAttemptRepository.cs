
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class QuizAttemptRepository : Repository<QuizAttempt>, IQuizAttemptRepository
    {
        public QuizAttemptRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<QuizAttempt>> GetByStudentAndQuizAsync(Guid studentId, Guid quizId, CancellationToken ct = default)
            => await _dbSet
                .Where(a => a.StudentId == studentId && a.QuizId == quizId)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<QuizAttempt?> GetWithAnswersAsync(Guid attemptId, CancellationToken ct = default)
            => await _dbSet
                .Include(a => a.Answers).ThenInclude(a => a.Question)
                .Include(a => a.Answers).ThenInclude(a => a.SelectedAnswer)
                .FirstOrDefaultAsync(a => a.Id == attemptId, ct);

        public async Task<int> GetAttemptCountAsync(Guid studentId, Guid quizId, CancellationToken ct = default)
            => await _dbSet.CountAsync(a => a.StudentId == studentId && a.QuizId == quizId, ct);

        public async Task<QuizAttempt?> GetBestAttemptAsync(Guid studentId, Guid quizId, CancellationToken ct = default)
            => await _dbSet
                .Where(a => a.StudentId == studentId && a.QuizId == quizId)
                .OrderByDescending(a => a.Score)
                .FirstOrDefaultAsync(ct);
    }
}
