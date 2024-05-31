using QuestionResourceContract = API.Resource.Quiz.Question.Contract;

namespace API.Resource.Quiz.Option
{
    public class Option
    {
        public required Guid Id { get; set; }
        public required Guid QuestionId { get; set; }
        public QuestionResourceContract.Question? Question { get; set; }
        public required string OptionText { get; set; }
        public required bool IsCorrect { get; set; }
    }
}
