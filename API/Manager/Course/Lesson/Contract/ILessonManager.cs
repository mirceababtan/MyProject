namespace API.Manager.Course.Lesson.Contract
{
    public interface ILessonManager
    {
        Task<IEnumerable<LessonPreview>> GetLessonPreviewsByCourseId(Guid id);

        Task<IEnumerable<Contract.Lesson>> GetLessonByCourseId(Guid courseId);

        Task<Lesson?> GetLessonById(Guid id);

        Task MarkLessonAsComplete(Guid userId,Guid lessonId);

        Task<bool> IsLessonCompleted(Guid userId,Guid lessonId);

        Task<IEnumerable<Lesson>> GetCompletedLessonForUserByCourseId(Guid userId, Guid courseId);
    }
}
