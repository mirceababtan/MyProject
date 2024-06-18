namespace API.Manager.Course.Contract
{
    public class EnrollRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
    }
}
