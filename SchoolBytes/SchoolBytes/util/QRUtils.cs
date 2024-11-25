
using SchoolBytes.Models;
using System.Net;
using System.Web.Mvc;

namespace SchoolBytes.util 
{
    public static class QRUtils
    {
        private static DBConnection dbConnection = DBConnection.getDBContext();
        public static HttpStatusCodeResult RegistrationCheckIn(string tlfNummer, int moduleId)
        {
            CourseModule courseModule = dbConnection.courseModules.Find(moduleId);
            Registration registration = null;
            foreach (var reg in courseModule.Registrations)
            {
                if (reg.participant.PhoneNumber == tlfNummer)
                {
                    registration = reg;
                    break;
                }
            }
            if (registration != null)
            {
                if (registration.Attendance == true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Fremmøde allerede registreret.");
                }
                registration.Attendance = true;
                dbConnection.UpdateSub(registration, courseModule);
                dbConnection.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Fremmøde registreret.");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Tilmelding ikke fundet.");
            }
        }
    }
}