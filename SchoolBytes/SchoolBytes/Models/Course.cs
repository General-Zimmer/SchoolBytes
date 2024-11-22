using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SchoolBytes.DTO;

namespace SchoolBytes.Models
{
    public class Course
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
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

        public Course Create(CourseDTO courseDTO)
        {
            var course = new Course
            {
                Name = courseDTO.Name,
                Description = courseDTO.Description,
                Teacher = courseDTO.Teacher,
                Participants = courseDTO.Participants,
                CoursesModules = courseDTO.CoursesModules,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                MaxCapacity = courseDTO.MaxCapacity,
                Id = courseDTO.Id
            };

            var modules = new List<CourseModule>();
            var activeDays = new List<DayOfWeek>();

            if (courseDTO.Monday)
                activeDays.Add(DayOfWeek.Monday);
            if (courseDTO.Tuesday)
                activeDays.Add(DayOfWeek.Tuesday);
            if (courseDTO.Wednesday)
                activeDays.Add(DayOfWeek.Wednesday);
            if (courseDTO.Thursday)
                activeDays.Add(DayOfWeek.Thursday);
            if (courseDTO.Friday)
                activeDays.Add(DayOfWeek.Friday);
            if (courseDTO.Saturday)
                activeDays.Add(DayOfWeek.Saturday);
            if (courseDTO.Sunday)
                activeDays.Add(DayOfWeek.Sunday);

            var daysCount = activeDays.Count;
            if (daysCount == 0)
            {
                throw new InvalidOperationException("Ingen dage valgt på kursus.");
            }

            var modulesPerDay = courseDTO.numberOfModules / daysCount;
            var remainingModules = courseDTO.numberOfModules % daysCount;
            var currentDate = DateTime.Now;

            foreach (var activeDayDate in activeDays.Select(day => GetDayForWeekday(currentDate, day)))
            {
                for (var i = 0; i < modulesPerDay; i++)
                {
                    modules.Add(new CourseModule()
                    {
                        Name = $"Module {modules.Count + 1}",
                        Date = activeDayDate
                    });
                }

                if (remainingModules > 0)
                {
                    modules.Add(new CourseModule()
                    {
                        Name = $"Module {modules.Count + 1}",
                        Date = activeDayDate
                    });
                    remainingModules--;
                }
            }
            
            course.CoursesModules = modules;
            
            return course;
        }
        
        private static DateTime GetDayForWeekday(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysToAdd);
        }
    }
}