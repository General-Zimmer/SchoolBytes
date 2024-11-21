using System;
using System.Collections.Generic;
using System.Linq;
using SchoolBytes.DTO;

namespace SchoolBytes.Models
{
    public class Course
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual List<CourseModule> CoursesModules { get; set; } = new List<CourseModule>();
        public virtual List<Participant> Participants { get; set; } = new List<Participant>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxCapacity { get; set; }
        public int Id { get; set; }

        public Course()
        {

        }

        public Course(string name, string description, DateTime startDate, DateTime endDate, int maxCapacity, int id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            MaxCapacity = maxCapacity;
            Id = id;
        }

        // +Teacher
        public Course(string name, string description, Teacher teacher, DateTime startDate, DateTime endDate,
            int maxCapacity, int id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            MaxCapacity = maxCapacity;
            Id = id;
            Teacher = teacher;
        }

      
        
        private static DateTime GetDayForWeekday(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysToAdd);
        }
    }
}