﻿namespace API.Manager.Course.Contract
{
    public class CoursePreview
    {
        public Guid Id { get; set; }
        public  string Title { get; set; }
        public  string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
