using Newtonsoft.Json;
using System;

namespace SchoolBytes.Models
{
    public class CourseModule : IModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        public virtual FoodModule FoodModule { get; set; }
        public DateTime Date { get ; set ; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
        public string Location { get; set; }
    }
}