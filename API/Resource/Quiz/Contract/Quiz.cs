using LessonResourceContract =  API.Resource.Course.Lesson.Contract;

namespace API.Resource.Quiz.Contract
{
    public class Quiz
    {
        public required Guid Id { get; set; }
        public required Guid LessonId { get; set; }
        public LessonResourceContract.Lesson? Lesson { get; set; }
        public required string Title { get; set; }
        public int QuestionCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
