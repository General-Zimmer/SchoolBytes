using System.Collections.Generic;

namespace SchoolBytes.Models
{
    public class Teacher
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }

        public int Id { get; set; }
    }
}