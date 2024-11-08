using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolBytes.Models
{
    public class FoodModule : IModule
    {
        public int Capacity { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public Course Course { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
    }
}