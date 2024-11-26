using System;
using System.Collections.Generic;
using SchoolBytes.Models;

namespace SchoolBytes.DTO
{
    public class CourseDTO
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public Teacher Teacher { get; set; }
        public List<Participant> Participants { get; set; } = new List<Participant>();
        public List<CourseModule> CoursesModules { get; set; } = new List<CourseModule>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxCapacity { get; set; }
        public int Id { get; set; }
        
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public CourseDTO()
        {
        
        }
    
        public CourseDTO(string name, string description, DateTime startDate, DateTime endDate, int maxCapacity, int id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            MaxCapacity = maxCapacity;
            Id = id;
        }
    
        // +Teacher
        public CourseDTO(string name, string description, Teacher teacher, DateTime startDate, DateTime endDate, int maxCapacity, int id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            MaxCapacity = maxCapacity;
            Id = id;
            Teacher = teacher;
        }

    }
}