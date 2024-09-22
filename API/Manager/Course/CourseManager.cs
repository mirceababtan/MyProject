using API.Manager.Course.Contract;
using API.Resource.Course.Contract;
using API.Resource.Course.Lesson.Contract;
using AutoMapper;
using Azure.Core;
using System.Transactions;

namespace API.Manager.Course
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseResource _courseResource;
        private readonly ILessonResource _lessonResource;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CourseManager(ICourseResource courseResource,
                            IMapper mapper,
                            ILessonResource lessonResource,
                            IWebHostEnvironment hostingEnvironment)
        {
            _courseResource = courseResource;
            _mapper = mapper;
            _lessonResource = lessonResource;
            _hostingEnvironment = hostingEnvironment;
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

        public async Task<bool> IsCourseCompleted(Guid userId, Guid courseId)
        {
            var course = await _courseResource.GetCourseByIdAsync(courseId);
            var completedLessons = await _lessonResource.GetCompletedLessonForUserByCourseId(userId, courseId);

            if (course.LessonCount == completedLessons.Count())
                return true;
            return false;
        }

        public async Task<object> AddCourse(CourseData courseData)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var courseToInsert = new Resource.Course.Contract.Course()
                {
                    Id = Guid.NewGuid(),
                    Title = courseData.Title,
                    Description = courseData.Description,
                    LessonCount = courseData.Lessons.Count,
                    InstructorId = courseData.InstructorId,
                    CreatedAt = DateTime.UtcNow,
                    ImageUrl = courseData.ImageUrl,
                };

                if (!string.IsNullOrEmpty(courseData.ImageUrl))
                {
                    var base64Data = courseData.ImageUrl.Split(',')[1];
                    var imageBytes = Convert.FromBase64String(base64Data);

                    var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = courseToInsert.Id.ToString() + ".png";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                    courseToInsert.ImageUrl = filePath;
                }

                await _courseResource.AddCourse(courseToInsert);

                var lessonsToInsert = courseData.Lessons.Select(lesson => new Resource.Course.Lesson.Contract.Lesson()
                {
                    Id = Guid.NewGuid(),
                    CourseId = courseToInsert.Id,
                    LessonNumber = lesson.LessonNumber,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    VideoUrl = lesson.VideoUrl,
                    FileUrl = lesson.FileUrl,
                    CreatedAt = DateTime.UtcNow,
                }).ToList();

                foreach (var lesson in lessonsToInsert)
                {
                    await _lessonResource.AddLesson(lesson);
                }

                transaction.Complete();

                return new
                {
                    Course = courseToInsert,
                    Lessons = lessonsToInsert
                };
            }
        }

        public async Task<object?> DeleteCourse(Guid id)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var course = await _courseResource.GetCourseByIdAsync(id);
                if (course == null)
                {
                    return null;
                }

                var lessons = await _lessonResource.GetLessonsByCourseId(id);
                foreach (var lesson in lessons)
                {
                    await _lessonResource.DeleteLesson(lesson.Id);
                }

                await _courseResource.DeleteCourse(id);

                transaction.Complete();

                return new
                {
                    CourseId = id,
                    Message = "Course and associated lessons have been deleted successfully."
                };
            }
        }

    }
}
