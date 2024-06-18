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
    }
}
