﻿namespace API.Resource.Course.Contract
{
    public class Enrollment
    {
        public Guid UserId { get; set; }
        public User.Contract.User User { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

    }
}
