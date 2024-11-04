using SchoolBytes.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public class FoodModule : IModule
{
    public int Capacity { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    [DataType(DataType.Time)]
    public DateTime StartTime { get; set; }
    [DataType(DataType.Time)]
    public DateTime EndTime { get; set; }
    public CourseModule CourseModule { get; set; }
    public Course Course { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public Teacher Teacher { get; set; }

    
}