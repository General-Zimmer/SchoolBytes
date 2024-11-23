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
            // Logic so that you can't add duplicates to the waitlist
            LinkedList<WaitRegistration> existingWaitRegistrations = courseModule.Waitlist;
            bool notOnTheList = true;

            foreach (WaitRegistration registration in existingWaitRegistrations) 
            {
                if (registration.participant.PhoneNumber == participant.PhoneNumber) 
                { 
                    notOnTheList = false;
                    break;
                }
            }

            if (notOnTheList) 
            {
                // hvis courseModule er fuld booket
                if (courseModule.Capacity == courseModule.MaxCapacity) 
                {
                    WaitRegistration newWaitRegistation = new WaitRegistration(participant, courseModule, DateTime.Now);
                    courseModule.Waitlist.AddLast(newWaitRegistation);
                    dBConnection.SaveChangesV2();
                }
            }
        }

    }
}