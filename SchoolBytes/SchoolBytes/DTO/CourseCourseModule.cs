using System;
using System.Collections.Generic;
using SchoolBytes.Models;

namespace SchoolBytes.DTO
{
    public class CourseCourseModule
    {
        public List<Course> Courses { get; set; } = new List<Course>();
        public int SelectedModuleId { get; set; }
    }
}