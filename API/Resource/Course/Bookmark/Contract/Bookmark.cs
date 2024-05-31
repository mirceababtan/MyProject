using UserResourceContract = API.Resource.User.Contract;
using LessonResourceContract = API.Resource.Course.Lesson.Contract;

namespace API.Resource.Course.Bookmark.Contract
{
    public class Bookmark
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public UserResourceContract.User? User { get; set; }
        public required Guid LessonId { get; set; }
        public LessonResourceContract.Lesson? Lesson { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
