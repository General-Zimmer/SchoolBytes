using SchoolBytes.Models;
using System.Collections.Generic;

public class Teacher
{
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
    public int Id { get; set; }
}