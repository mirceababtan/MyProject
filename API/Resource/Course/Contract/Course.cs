using UserResourceContract =  API.Resource.User.Contract;

namespace API.Resource.Course.Contract
{
    public class Course
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Guid InstructorId { get; set; }
        public UserResourceContract.User? Instructor { get; set; }
        public int LessonCount { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
