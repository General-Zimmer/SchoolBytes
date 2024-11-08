using SchoolBytes.Models;
using System;

public class CourseModule : IModule
{
    public string Name { get; set; }
    public Teacher Teacher { get; set; }
    public FoodModule FoodModule { get; set; }
    public DateTime Date { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime StartTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime EndTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int Capacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Course Course { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int Id { get; set; }
}