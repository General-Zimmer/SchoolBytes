using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBytes.Models
{
    internal interface IModule
    {
        string Name { get; set; }
        int Capacity { get; set; }
        DateTime Date { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        Course Course { get; set; }
        Teacher Teacher { get; set; }
        //Consider making location a class or enum so we can have static locations
        string Location { get; set; }
    }
}
