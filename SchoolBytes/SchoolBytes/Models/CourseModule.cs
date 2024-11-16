using System;
using System.Collections.Generic;

namespace SchoolBytes.Models
{
    public class CourseModule : IModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual FoodModule FoodModule { get; set; }
        public virtual List<Participant> Participants { get; set; } = new List<Participant>();
        public virtual Queue<Participant> Waitlist { get; set; } = new Queue<Participant>();
        public DateTime Date { get ; set ; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public virtual Course Course { get; set; }
        public string Location { get; set; }
        public int MaxCapacity { get; set; }
    }
}