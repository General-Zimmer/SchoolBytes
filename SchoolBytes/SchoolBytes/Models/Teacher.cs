using Newtonsoft.Json;
using SchoolBytes.Models;
using System.Collections.Generic;

public class Teacher
{
    public string Name { get; set; }
    [JsonIgnore]
    public virtual List<Course> Courses { get; set; }
    public int Id { get; set; }
}