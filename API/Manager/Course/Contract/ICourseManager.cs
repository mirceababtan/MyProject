namespace API.Manager.Course.Contract
{
    public interface ICourseManager
    {
        Task<IEnumerable<Contract.Course>> GetAllCoursesAsync();
        Task<Contract.Course?> GetCourseByIdAsync(Guid id);

        Task<Contract.Course?> GetCourseByInstructorIdAsync(Guid id);

        Task<object> AddCourse(CourseData courseData);

        Task<IEnumerable<Contract.CoursePreview>> GetAllCoursesPreviewAsync();

        Task<IEnumerable<Contract.Course?>> GetEnrolledCoursesByUserId(Guid userId);


        Task EnrollUserToCourse(Guid userId, Guid courseId);

        Task UnenrollUserFromCourse(Guid userId, Guid courseId);

        Task<bool> IsUserEnrolled(Guid userId,Guid courseId);

        Task<bool> IsCourseCompleted(Guid userId,Guid courseId);
        Task<object?> DeleteCourse(Guid id);
    }
}
