using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
        [JsonIgnore]
        public virtual Course Course { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
        public virtual List<Participant> Participants{ get; set; } = new List<Participant>();

        public int Id { get; set; }
        public bool IsCancelled { get; set; } = false;
    }
}