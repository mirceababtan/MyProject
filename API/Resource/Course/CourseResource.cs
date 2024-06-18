using API.Infrastructure.Database;
using API.Resource.Course.Contract;
using Microsoft.EntityFrameworkCore;

namespace API.Resource.Course
{
    public class CourseResource : ICourseResource
    {
        private readonly DatabaseContext _dbContext;
        public CourseResource(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Contract.Course>> GetAllCoursesAsync()
        {
            return await _dbContext.Courses.ToListAsync();
        }

        public async Task<Contract.Course?> GetCourseByIdAsync(Guid id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
            return course;
        }

        public async Task<Contract.Course?> GetCourseByInstructorId(Guid id)
        {
            return await _dbContext.Courses.FirstOrDefaultAsync(c => c.InstructorId == id);
        }

        public async Task<IEnumerable<Contract.Course?>> GetEnrolledCoursesByUserId(Guid userId)
        {
            return await _dbContext.Enrollments
            .Where(e => e.UserId == userId)
            .Select(e => e.Course)
            .ToListAsync();
        }

        public async Task EnrollUserToCourse(Guid userId, Guid courseId)
        {

            var existingEnrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                return;
            }

            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
            };

            _dbContext.Enrollments.Add(enrollment);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UnenrollUserFromCourse(Guid userId, Guid courseId)
        {

            var enrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment == null)
            {
                return;
            }

            _dbContext.Enrollments.Remove(enrollment);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserEnrolled(Guid userId, Guid courseId)
        {
            var existingEnrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                return true;
            }

            return false;
        }
    }
}
