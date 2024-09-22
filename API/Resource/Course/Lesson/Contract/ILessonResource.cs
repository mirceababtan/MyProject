namespace API.Resource.Course.Lesson.Contract
{
    public interface ILessonResource
    {
        Task<IEnumerable<Lesson>> GetLessonsByCourseId(Guid id);
        Task<Lesson?> GetLessonById(Guid id);

        Task MarkLessonAsComplete(Guid userId, Guid lessonId);

        Task<bool> IsLessonCompleted(Guid userId,Guid lessonId);

        Task<IEnumerable<Lesson>> GetCompletedLessonForUserByCourseId(Guid userId,Guid courseId);
        Task<Lesson> AddLesson(Lesson lesson);
        Task DeleteLesson(Guid id);
    }
}
