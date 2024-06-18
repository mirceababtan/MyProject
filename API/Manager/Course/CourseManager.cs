using API.Manager.Course.Contract;
using API.Resource.Course.Contract;
using AutoMapper;

namespace API.Manager.Course
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseResource _courseResource;
        private readonly IMapper _mapper;
        public CourseManager(ICourseResource courseResource, IMapper mapper)
        {
            _courseResource = courseResource;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Contract.Course>> GetAllCoursesAsync()
        {
            var courses = await _courseResource.GetAllCoursesAsync();
            return courses.Select(c => _mapper.Map<Contract.Course>(c)).ToArray<Contract.Course>();
        }

        public async Task<IEnumerable<Contract.CoursePreview>> GetAllCoursesPreviewAsync()
        {
            var courses = await _courseResource.GetAllCoursesAsync();
            return courses.Select(c => _mapper.Map<CoursePreview>(c)).ToArray();
        }

        public async Task<Contract.Course?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseResource.GetCourseByIdAsync(id);
            if (course == null)
            {
                return null;
            }

            return _mapper.Map<Contract.Course>(course);
        }

        public Task<Contract.Course?> GetCourseByInstructorIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Contract.Course?>> GetEnrolledCoursesByUserId(Guid userId)
        {
            return _mapper.Map<IEnumerable<Contract.Course>>(await _courseResource.GetEnrolledCoursesByUserId(userId)).ToArray();
        }

        public async Task UnenrollUserFromCourse(Guid userId, Guid courseId)
        {
            await _courseResource.UnenrollUserFromCourse(userId, courseId);
        }

        public async Task EnrollUserToCourse(Guid userId, Guid courseId)
        {
            await _courseResource.EnrollUserToCourse(userId, courseId);
        }

        public async Task<bool> IsUserEnrolled(Guid userId, Guid courseId)
        {
            return await _courseResource.IsUserEnrolled(userId, courseId);
        }
    }
}
