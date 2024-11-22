using SchoolBytes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolBytes.util
{
    public static class VentelisteUtil
    {
        private static DBConnection dBConnection = DBConnection.getDBContext();
        public static void AddToWaitlist(CourseModule courseModule, Participant participant) 
        {
            dBConnection.Update(courseModule);
            WaitRegistration yeet = new WaitRegistration(participant, courseModule, DateTime.Now);

            courseModule.Waitlist.AddLast(yeet);
            dBConnection.SaveChangesV2();
        }

    }
}