using System;
using System.Collections.Generic;

public class Course
{

    public string Name { get; set; }
    public string Description { get; set; }
    public Teacher Teacher { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public List<CourseModule> Courses { get; set; } = new List<CourseModule>();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public int Id { get; set; }

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
    public Course(string name, string description, Teacher teacher, DateTime startDate, DateTime endDate, int maxCapacity, int id)
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