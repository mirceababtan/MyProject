using CourseResourceContract = API.Resource.Course.Contract;

namespace API.Resource.Course.Lesson.Contract
{
    public class Lesson
    {
        public required Guid Id { get; set; }
        public required Guid CourseId { get; set; }
        public CourseResourceContract.Course? Course { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required int LessonNumber { get; set; }
        public string? VideoUrl { get; set; }

        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserCompletedLessons> CompletedByUsers { get; set; }
    }
}
