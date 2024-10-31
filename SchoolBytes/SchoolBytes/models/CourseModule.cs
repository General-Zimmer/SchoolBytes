using SchoolBytes.Models;
using System;

public class CourseModule : IModule
{
    public string Name { get; set; }
    public Teacher Teacher { get; set; }
    public DateTime Date { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime StartTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime EndTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}