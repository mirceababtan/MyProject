namespace API.Manager.Course.Lesson.Contract
{
    public class Lesson
    {
        public  Guid Id { get; set; }
        public  Guid CourseId { get; set; }
        public  string Title { get; set; }
        public  string Content { get; set; }
        public  int LessonNumber { get; set; }

        public string? VideoUrl { get; set; }

        public string? FileUrl { get; set; }
    }
}
