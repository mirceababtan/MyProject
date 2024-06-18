using API.Manager.Course.Contract;
using API.Manager.Course.Lesson.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseManager _courseManager;
        private readonly ILessonManager _lessonManager;

        public CourseController(ICourseManager courseManager, ILessonManager lessonManager)
        {
            _courseManager = courseManager;
            _lessonManager = lessonManager;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _courseManager.GetAllCoursesAsync());
        }

        [AllowAnonymous]
        [HttpGet("PreviewAll")]
        public async Task<IActionResult> GetAllCoursePreview()
        {
            return Ok(await _courseManager.GetAllCoursesPreviewAsync());
        }
        [AllowAnonymous]
        [HttpGet("GetCourseById")]
        public async Task<IActionResult> GetCourseById([FromQuery] Guid id)
        {
            return Ok(await _courseManager.GetCourseByIdAsync(id));
        }

        [AllowAnonymous]
        [HttpGet("LessonPreviewsAll")]
        public async Task<IActionResult> GetLessonPreviews([FromQuery] Guid id)
        {
            return Ok(await _lessonManager.GetLessonPreviewsByCourseId(id));
        }

        [HttpGet("GetEnrolledById")]
        public async Task<IActionResult> GetEnrolledCoursesByUserId([FromQuery] Guid id)
        {
            return Ok(await _courseManager.GetEnrolledCoursesByUserId(id));
        }

        [HttpPost("EnrollUserToCourse")]
        public async Task<IActionResult> EnrollUserToCourse([FromBody] EnrollRequest enrollRequest)
        {
            await _courseManager.EnrollUserToCourse(enrollRequest.UserId, enrollRequest.CourseId);
            return Ok();
        }

        [HttpDelete("UnenrollUserFromCourse")]
        public async Task<IActionResult> UnrollUserFromCourse([FromQuery] Guid userId, Guid courseId)
        {
            await _courseManager.UnenrollUserFromCourse(userId, courseId);
            return Ok();
        }

        [HttpGet("IsUserEnrolled")]
        public async Task<IActionResult> IsUserEnrolled([FromQuery] Guid userId, Guid courseId)
        {
            bool isEnrolled = await _courseManager.IsUserEnrolled(userId, courseId);
            return Ok(new { isEnrolled });
        }

        [HttpGet("GetLessonById")]
        public async Task<IActionResult> GetLessonById([FromQuery] Guid id)
        {
            return Ok(await _lessonManager.GetLessonById(id));
        }
    }
}
