using API.Manager.Course.Lesson.Contract;
using API.Resource.Course.Lesson.Contract;
using AutoMapper;

namespace API.Manager.Course.Lesson
{
    public class LessonManager : ILessonManager
    {
        private readonly ILessonResource _lessonResource;
        private readonly IMapper _mapper;

        public LessonManager(ILessonResource lessonResource, IMapper mapper)
        {
            _lessonResource = lessonResource;
            _mapper = mapper;
        }

        public async Task<Contract.Lesson?> GetLessonById(Guid id)
        {
            return _mapper.Map<Contract.Lesson>(await _lessonResource.GetLessonById(id));
        }

        public async Task<IEnumerable<LessonPreview>> GetLessonPreviewsByCourseId(Guid id)
        {
            return _mapper.Map<Contract.LessonPreview[]>(await _lessonResource.GetLessonsByCourseId(id));
        }
        public async Task<IEnumerable<Contract.Lesson>> GetLessonByCourseId(Guid courseId)
        {
            return _mapper.Map<Contract.Lesson[]>(await _lessonResource.GetLessonsByCourseId(courseId));
        }

        public async Task MarkLessonAsComplete(Guid userId, Guid lessonId)
        {
            await _lessonResource.MarkLessonAsComplete(userId, lessonId);
        }
        public async Task<bool> IsLessonCompleted(Guid userId, Guid lessonId)
        {
            return await _lessonResource.IsLessonCompleted(userId, lessonId);
        }


        public async Task<IEnumerable<Contract.Lesson>> GetCompletedLessonForUserByCourseId(Guid userId, Guid courseId)
        {
            return _mapper.Map<IEnumerable<Contract.Lesson>>(await _lessonResource.GetCompletedLessonForUserByCourseId(userId,courseId));
        }

    }
}
