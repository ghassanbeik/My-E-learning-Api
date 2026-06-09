

namespace Horizon.Application.DTOs
{
    public record QuizDto(
         Guid Id,
         Guid LessonId,
         string Title,
         string? Instructions,
         int TimeLimitMinutes,
         int PassingScore,
         int MaxAttempts,
         bool ShuffleQuestions,
         bool ShowCorrectAnswers,
         int QuestionCount);

    public record QuizDetailDto(
        Guid Id,
        Guid LessonId,
        string Title,
        string? Instructions,
        int TimeLimitMinutes,
        int PassingScore,
        int MaxAttempts,
        bool ShuffleQuestions,
        bool ShowCorrectAnswers,
        List<QuestionDto> Questions);

    public record QuestionDto(
        Guid Id,
        string Text,
        string? Explanation,
        int Points,
        int DisplayOrder,
        List<AnswerOptionDto> AnswerOptions);

    public record AnswerOptionDto(
        Guid Id,
        string Text,
        bool? IsCorrect,
        int DisplayOrder);

    public record SubmitQuizDto(List<QuizAnswerSubmitDto> Answers);

    public record QuizAnswerSubmitDto(Guid QuestionId, Guid SelectedAnswerId);

    public record QuizAttemptDto(
        Guid Id,
        Guid QuizId,
        int Score,
        int MaxScore,
        bool IsPassed,
        int AttemptNumber,
        DateTime? StartedAt,
        DateTime? CompletedAt,
        List<QuizAnswerResultDto> Answers);

    public record QuizAnswerResultDto(
        Guid QuestionId,
        string QuestionText,
        Guid SelectedAnswerId,
        string SelectedAnswerText,
        bool IsCorrect,
        int PointsEarned,
        string? Explanation,
        string? CorrectAnswerText);

    public record CreateQuizDto(
        Guid LessonId,
        string Title,
        string? Instructions,
        int TimeLimitMinutes,
        int PassingScore,
        int MaxAttempts,
        bool ShuffleQuestions,
        bool ShowCorrectAnswers,
        List<CreateQuestionDto> Questions);

    public record CreateQuestionDto(
        string Text,
        string? Explanation,
        int Points,
        int DisplayOrder,
        List<CreateAnswerOptionDto> AnswerOptions);

    public record CreateAnswerOptionDto(string Text, bool IsCorrect, int DisplayOrder);
}
