using Microsoft.EntityFrameworkCore;
using SchoolBytes.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SchoolBytes.util
{
    public static class DatabaseUtils
    {
        private static DBConnection self = DBConnection.getDBContext();

        public static HttpStatusCodeResult Unsub(int courseId, int moduleId, string tlfNr)
        {
            CourseModule courseModule = self.courseModules.Find(moduleId);

            Registration registration = courseModule.Registrations.Where(r => r.participant.PhoneNumber == tlfNr).First();
            if (registration != null)
            {
                courseModule.Registrations.Remove(registration);

                self.Update(courseModule);
                self.SaveChanges();

                return null;
                //TODO: Maybe take person from waiting list if there is any?
            }
            else
            {
                //placeholder
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No registration found for the couse with the phonenumber.");
            }
        }

        public static HttpStatusCodeResult DeleteParticipant(int participantId, int courseId)
        {
            Course course = self.courses.Find(courseId);
            Participant participant = self.participants.Find(participantId);

            if (course == null)
            {
                return new HttpNotFoundResult("Course not found.");
            }
            else if (participant == null)
            {
                return new HttpNotFoundResult("Participant not found.");
            }
            else if (!course.Participants.Contains(participant))
            {
                return new HttpNotFoundResult("Participant not associated with course.");
            }
            else
            {
                course.Participants.Remove(participant);
                self.SaveChanges();
                return new HttpStatusCodeResult(200); //success

            }
        }
        public static int SaveChangesV2(this DbContext FOK)
        {
            try
            {
                return FOK.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException e)
            {
                //Add your code to inspect the inner exception and/or
                //e.Entries here.
                //Or just use the debugger.
                //Added this catch (after the comments below) to make it more obvious 
                //how this code might help this specific problem
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            Debug.Print("WTF MAAAAAN");
            return 0;
        }
    }
}