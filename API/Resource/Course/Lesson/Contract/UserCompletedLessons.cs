namespace API.Resource.Course.Lesson.Contract
{
    public class UserCompletedLessons
    {
        public Guid UserId { get; set; }

        public Guid LessonId { get; set; }

        public Resource.User.Contract.User User { get; set; }

        public Resource.Course.Lesson.Contract.Lesson Lesson { get; set; }

    }

}
