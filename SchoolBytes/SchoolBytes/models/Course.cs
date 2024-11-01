using System;
using System.Collections.Generic;

public class Course
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Teacher Teacher { get; set; }
    public List<Participant> Participants { get; set; }
    public List<CourseModule> Courses { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }


    

}