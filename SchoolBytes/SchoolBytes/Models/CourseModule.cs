using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public virtual List<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual LinkedList<WaitRegistration> Waitlist { get; set; } = new LinkedList<WaitRegistration>();
        public DateTime Date { get ; set ; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; } = 0;
        public virtual Course Course { get; set; }
        public string Location { get; set; }
        public int MaxCapacity { get; set; }


    }
}