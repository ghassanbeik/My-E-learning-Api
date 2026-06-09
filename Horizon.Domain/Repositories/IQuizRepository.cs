

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<Quiz?> GetWithQuestionsAsync(Guid quizId, CancellationToken ct = default);
        Task<IEnumerable<Quiz>> GetByLessonAsync(Guid lessonId, CancellationToken ct = default);
    }
}
