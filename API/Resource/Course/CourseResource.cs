using API.Infrastructure.Database;
using API.Resource.Course.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resource.Course
{
    public class CourseResource : ICourseResource
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CourseResource(DatabaseContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<Contract.Course>> GetAllCoursesAsync()
        {
            var courses = await _dbContext.Courses.ToListAsync();
            return await ConvertImageUrlsToBase64(courses);
        }

        public async Task<Contract.Course> AddCourse(Contract.Course course)
        {
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            return course;
        }

        public async Task<Contract.Course?> GetCourseByIdAsync(Guid id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course != null)
            {
                course.ImageUrl = await ConvertImageUrlToBase64(course.ImageUrl);
            }
            return course;
        }

        public async Task<Contract.Course?> GetCourseByInstructorId(Guid id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.InstructorId == id);
            if (course != null)
            {
                course.ImageUrl = await ConvertImageUrlToBase64(course.ImageUrl);
            }
            return course;
        }

        public async Task<IEnumerable<Contract.Course?>> GetEnrolledCoursesByUserId(Guid userId)
        {
            var courses = await _dbContext.Enrollments
                .Where(e => e.UserId == userId)
                .Select(e => e.Course)
                .ToListAsync();

            return await ConvertImageUrlsToBase64(courses);
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

            return existingEnrollment != null;
        }

        private async Task<string?> ConvertImageUrlToBase64(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, imageUrl);

            if (File.Exists(filePath))
            {
                var imageBytes = await File.ReadAllBytesAsync(filePath);
                var base64String = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64String}";
            }

            return null;
        }

        private async Task<IEnumerable<Contract.Course>> ConvertImageUrlsToBase64(IEnumerable<Contract.Course> courses)
        {
            foreach (var course in courses)
            {
                course.ImageUrl = await ConvertImageUrlToBase64(course.ImageUrl);
            }
            return courses;
        }

        public async Task DeleteCourse(Guid id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return;
            }

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }

    }
}
