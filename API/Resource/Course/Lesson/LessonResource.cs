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
    }
}
