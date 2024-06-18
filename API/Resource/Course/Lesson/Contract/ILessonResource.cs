namespace API.Resource.Course.Lesson.Contract
{
    public interface ILessonResource
    {
        Task<IEnumerable<Lesson>> GetLessonsByCourseId(Guid id);
        Task<Lesson?> GetLessonById(Guid id);
    }
}
