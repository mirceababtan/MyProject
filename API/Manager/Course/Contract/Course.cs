namespace API.Manager.Course.Contract
{
    public class Course
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Guid InstructorId { get; set; }
        public int LessonCount { get; set; }
        public required DateTime CreatedAt { get; set; }

        public string? ImageUrl { get; set; }
    }
}
