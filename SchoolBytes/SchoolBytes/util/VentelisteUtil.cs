using SchoolBytes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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

            // the phone number is not on the wait list
            if (notOnTheList) 
            {
                WaitRegistration newWaitRegistation = new WaitRegistration(participant, courseModule, DateTime.Now);
                    courseModule.Waitlist.AddLast(newWaitRegistation);
                    dBConnection.SaveChangesV2();
            }
        }

        // copied this one from the Controller so we can test it - do NOT use outside of the test!!!
        public static void testScenario3(CourseModule courseModule, Participant participant) 
        {
            if (courseModule.Capacity < courseModule.MaxCapacity)
            {
                if (DBConnection.IsEligibleToSubscribe(participant))
                {
                    Registration registration = new Registration(participant, courseModule);
                    courseModule.Capacity += 1;
                    dBConnection.UpdateSub(registration, courseModule);
                }
                else
                {
                    return;
                }
            }
            else
            {
                AddToWaitlist(courseModule, participant);
            }
        }

        public static void Unsub(CourseModule courseModule, Participant participant)
        {
            Registration registration = courseModule.Registrations.Where(r => r.participant.PhoneNumber == participant.PhoneNumber).First();
            if (registration != null)
            {
                courseModule.Registrations.Remove(registration);

                // move the first person from the waitlist (if any) to the course
                if (courseModule.Waitlist.First != null) 
                {
                    courseModule.Registrations.Add(new Registration(courseModule.Waitlist.First.Value.participant, courseModule));
                    courseModule.Waitlist.RemoveFirst();
                }

                dBConnection.Update(courseModule);
                dBConnection.SaveChanges();
            }
            else
            {
                // No registration with that phone number found
                return;
            }
        }
    }
}