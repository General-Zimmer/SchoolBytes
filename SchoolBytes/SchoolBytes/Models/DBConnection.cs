using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using SchoolBytes.util;


namespace SchoolBytes.Models
{
    public class DBConnection : DbContext
    {
        private static DBConnection self = null;

        public string connectionString;
        public DbSet<Course> courses { get; set; }
        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Participant> participants { get; set; }
        public DbSet<FoodModule> foodModules { get; set; }
        public DbSet<CourseModule> courseModules { get; set; }

        


        public DBConnection(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public DBConnection()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {   
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseMySql(getCredentialsPath());
            }


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseModule>()
                   .HasMany(cm => cm.Registrations)
                   .WithOne(r => r.CourseModule)
                   .HasPrincipalKey(cm => cm.Id) // Specifies the primary key on CourseModule
                   .HasForeignKey(r => r.Id); // Define the foreign key property in Registration


        }

        public static DBConnection getDBContext()
        {

            if (self == null)
            {
                self = new DBConnection(getCredentialsPath());
            }
            return self;
        }



        private static string getCredentialsPath()
        {
            string filePath = @"C:\Users\victo\Source\Repos\General-Zimmer\SchoolBytes\SchoolBytes\SchoolBytes\App_Data\ConnectionCredentials.json";
            //string filePath = HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json");
            StreamReader credJson = new StreamReader(filePath);
            //HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json");
            return (string)JObject.Parse(credJson.ReadToEnd())["credentials"];
        }

        public static bool IsEligibleToSubscribe(Participant participant)
        {
            //Finder aktive courses og tilmelder tilmeldinger fra paticipant
            Console.WriteLine(string.Join(", ", getDBContext().courseModules.Where(cm => DateTime.Compare(DateTime.Now, cm.StartTime) <= 0)));
            int registrations = getDBContext().courseModules
                .Where(cm => DateTime.Compare(DateTime.Now, cm.StartTime)<=0)
                .Sum(cm => cm.Registrations.Count(r => r.participant == participant));

            return registrations<=4;
        }

        public static bool IsParticipantFormatValid(Participant participant)
        {
            string daNumba = participant.PhoneNumber.Trim();
            daNumba = Regex.Replace(daNumba, @"\s+", "");

            if (daNumba[0] == '+')
            {
                try
                {
                    if (daNumba.Substring(1).Length != 10) return false; //Hvis der er indtastet landekode, +45, forventes i alt 10 cifre
                    int testConvert = Int32.Parse(daNumba.Substring(1));
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    int testConvert = Int32.Parse(daNumba);
                }
                catch
                {
                    return false;
                }
            }


            //Tester hvis telefon nummeret er brugt allerede, ved en participant med andet navn

            if (getDBContext().participants.ToList().Exists(p => p.PhoneNumber.Equals(participant.PhoneNumber) && !p.Name.Equals(participant.Name))) return false;


            return true;
        }

        public static bool IsParticipantSubscribedToCourseModule(Participant participant, int CourseModuleID)
        {

            return getDBContext().courseModules.Find(CourseModuleID).Registrations.Exists(r => r.participant == participant);

            //foreach(var modul in getDBContext().courseModules)
            //{
            //    if (modul.Registrations.Exists(r => r.participant == participant))
            //        {
            //        return true;
            //        }
            //}
            //return false;
        }


        public void UpdateSub(Registration registration, CourseModule course)
        {
            self.Update(registration);
            self.Update(course);
            self.SaveChanges();
        }

        public static int GetSubscribeCount(Participant participant)
        {
           return getDBContext().courseModules.Sum(cm => cm.Registrations.Count(r => r.participant == participant));

            
        }

        public static string GetParticipantFromModul(Participant participant)
        {

            bool getParticipant = getDBContext().courseModules.Any(p => p.Id.Equals(participant.Id));

            if (getParticipant)
            {
                return "${participant.Name} er der";
            }
            else
            {
                return "${participant.Name} findes ikke";
            }
        }

        public static int Subscribe(int courseId, int moduleId, Participant participant)
        {
            int resultCode = -1;

            CourseModule courseModule = getDBContext().courseModules.Find(moduleId);

            if (DateTime.Compare(DateTime.Now, courseModule.StartTime) > 0) return 4; //Checks if courseModule date has been passed

            if (courseModule.Capacity <= courseModule.MaxCapacity)
            {
                if (DBConnection.IsEligibleToSubscribe(participant) && DBConnection.IsParticipantFormatValid(participant))
                {
                    Registration registration = new Registration(participant, courseModule);
                    courseModule.Capacity += 1;
                    getDBContext().UpdateSub(registration, courseModule);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                //VENTELISTE LOGIK SKAL IND HER

                getDBContext().Update(courseModule);
                WaitRegistration yeet = new WaitRegistration(participant, courseModule, DateTime.Now);

                courseModule.Waitlist.AddLast(yeet);
                getDBContext().SaveChangesV2();
                return 2;
            }


            return 3;
        }

    }
}