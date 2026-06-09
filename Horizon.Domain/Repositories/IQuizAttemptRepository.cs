

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IQuizAttemptRepository : IRepository<QuizAttempt>
    {
        Task<IEnumerable<QuizAttempt>> GetByStudentAndQuizAsync(Guid studentId, Guid quizId, CancellationToken ct = default);
        Task<QuizAttempt?> GetWithAnswersAsync(Guid attemptId, CancellationToken ct = default);
        Task<int> GetAttemptCountAsync(Guid studentId, Guid quizId, CancellationToken ct = default);
        Task<QuizAttempt?> GetBestAttemptAsync(Guid studentId, Guid quizId, CancellationToken ct = default);
    }
}
