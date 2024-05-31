using UserResourceContract = API.Resource.User.Contract;
using LessonResourceContract = API.Resource.Course.Lesson.Contract;

namespace API.Resource.User.UserProgress.Contract
{
    public class UserProgress
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public UserResourceContract.User? User { get; set; }
        public required Guid LessonId { get; set; }
        public LessonResourceContract.Lesson? Lesson { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
