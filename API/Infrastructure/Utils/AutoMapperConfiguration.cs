using AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<API.Resource.Course.Contract.Course,API.Manager.Course.Contract.Course>();
        CreateMap<API.Resource.Course.Contract.Course, API.Manager.Course.Contract.CoursePreview>();
        CreateMap<API.Resource.Course.Lesson.Contract.Lesson,API.Manager.Course.Lesson.Contract.LessonPreview>();
        CreateMap<API.Resource.Course.Lesson.Contract.Lesson,API.Manager.Course.Lesson.Contract.Lesson>();
    }
}
