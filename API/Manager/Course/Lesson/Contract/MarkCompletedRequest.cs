namespace API.Manager.Course.Lesson.Contract
{
    public class MarkCompletedRequest
    {
        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }
    }
}
