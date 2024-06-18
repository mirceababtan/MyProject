namespace API.Resource.Course.Contract
{
    public interface ICourseResource
    {
        Task<IEnumerable<Contract.Course>> GetAllCoursesAsync();
        Task<Contract.Course?> GetCourseByIdAsync(Guid id);

        Task<Contract.Course?> GetCourseByInstructorId(Guid id);

        Task<IEnumerable<Contract.Course?>> GetEnrolledCoursesByUserId(Guid userId);

        Task EnrollUserToCourse(Guid userId,Guid courseId);

        Task UnenrollUserFromCourse(Guid userId,Guid courseId);
        Task<bool> IsUserEnrolled(Guid userId,Guid courseId);
    }
}
