using API.Infrastructure.Database;
using API.Resource.Course.Lesson.Contract;
using Microsoft.EntityFrameworkCore;

namespace API.Resource.Course.Lesson
{
    public class LessonResource : ILessonResource
    {
        private readonly DatabaseContext _dbContext;
        public LessonResource(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Contract.Lesson>> GetLessonsByCourseId(Guid id)
        {
            return await _dbContext.Lessons.Where(l => l.CourseId == id).ToListAsync();
        }

        public async Task<Contract.Lesson?> GetLessonById(Guid id)
        {
            return await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task MarkLessonAsComplete(Guid userId, Guid lessonId)
        {
            var enrollmentLesson = new UserCompletedLessons
            {
                UserId = userId,
                LessonId = lessonId
            };

            _dbContext.UserCompletedLessons.Add(enrollmentLesson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsLessonCompleted(Guid userId, Guid lessonId)
        {
            return await _dbContext.UserCompletedLessons
            .AnyAsync(el => el.UserId == userId && el.LessonId == lessonId);
        }

        public async Task<IEnumerable<Contract.Lesson>> GetCompletedLessonForUserByCourseId(Guid userId, Guid courseId)
        {
            return await _dbContext.UserCompletedLessons
                .Where(el => el.UserId == userId && el.Lesson.CourseId == courseId)
                .Select(el => new Contract.Lesson
                {
                    Id = el.Lesson.Id,
                    Title = el.Lesson.Title,
                    Content = el.Lesson.Content,
                    LessonNumber = el.Lesson.LessonNumber,
                    VideoUrl = el.Lesson.VideoUrl,
                    FileUrl = el.Lesson.FileUrl,
                    CourseId = el.Lesson.CourseId

                }).ToListAsync();
        }

        public async Task<Contract.Lesson> AddLesson(Contract.Lesson lesson)
        {
            await _dbContext.Lessons.AddAsync(lesson);
            await _dbContext.SaveChangesAsync();

            return lesson;
        }

        public async Task DeleteLesson(Guid id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return;
            }

            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();
        }
    }
}
