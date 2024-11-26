using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
        [JsonIgnore]
        public virtual List<Registration> Registrations { get; set; } = new List<Registration>();
        [JsonIgnore]
        public virtual LinkedList<WaitRegistration> Waitlist { get; set; } = new LinkedList<WaitRegistration>();
        public DateTime Date { get ; set ; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; } = 0;
        [JsonIgnore]
        public virtual Course Course { get; set; }
        public string Location { get; set; }
        [JsonIgnore]
        public int MaxCapacity { get; set; }


    }
}