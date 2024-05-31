using QuizResourceContract = API.Resource.Quiz.Contract;

namespace API.Resource.Quiz.Question.Contract
{
    public class Question
    {
        public required Guid Id { get; set; }
        public required Guid QuizId { get; set; }
        public QuizResourceContract.Quiz? Quiz { get; set; }
        public required string QuestionText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
