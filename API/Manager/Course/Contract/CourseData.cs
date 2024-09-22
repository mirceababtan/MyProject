using API.Manager.Course.Lesson.Contract;

namespace API.Manager.Course.Contract
{
    public class CourseData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }

        public Guid InstructorId { get; set; }
        public string ImageUrl { get; set; }

        public List<Lesson.Contract.Lesson> Lessons { get; set; } = [];
    }
}
