using SchoolBytes.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace SchoolBytes.util
{
    public static class RemoveParticipantFromCourse
    {
        public static void DeleteParticipant(int participantId, int courseId, DBConnection dBConnection)
        {
            Course course = dBConnection.courses.Find(courseId);
            Participant participant = dBConnection.participants.Find(participantId);

            if (course == null)
            {
                Console.WriteLine("course not found");
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "course not found");
            }
            else if (participant == null)
            {
                Console.WriteLine("participant not found");
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "participant not found");
            }
            else if (!course.Participants.Contains(participant))
            {
                Console.WriteLine("participant not associated with course");
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "participant not associatied with course");
            }
            else
            {
                course.Participants.Remove(participant);
                dBConnection.SaveChanges();

                //return new HttpStatusCodeResult(HttpStatusCode.Redirect, "redireting to CourseOverview");
            }
        }
    }
}