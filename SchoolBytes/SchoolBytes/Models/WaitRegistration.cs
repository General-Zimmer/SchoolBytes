using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolBytes.Models
{

    public class WaitRegistration
    {
        public int Id { get; set; }
        public virtual Participant participant { get; set; }
        public virtual CourseModule CourseModule { get; set; }
        public DateTime Timestamp { get; set; }

        public WaitRegistration()
        {
        }

        public WaitRegistration(Participant newParticipant, CourseModule courseModule)
        {
            this.participant = newParticipant;
            this.CourseModule = courseModule;
        }

        public WaitRegistration(Participant newParticipant, CourseModule courseModule, DateTime timestamp)
        {
            this.participant = newParticipant;
            this.CourseModule = courseModule;
            this.Timestamp = timestamp;
        }
    }
}