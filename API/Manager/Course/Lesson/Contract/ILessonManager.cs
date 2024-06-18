namespace API.Manager.Course.Lesson.Contract
{
    public interface ILessonManager
    {
        Task<IEnumerable<LessonPreview>> GetLessonPreviewsByCourseId(Guid id);

        Task<Lesson?> GetLessonById(Guid id);
    }
}
