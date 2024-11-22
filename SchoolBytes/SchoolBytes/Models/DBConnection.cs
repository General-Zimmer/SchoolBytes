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
using SchoolBytes.util;
using Microsoft.Ajax.Utilities;


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
            string filePath = @"C:\Users\Kasper\Source\Repos\SchoolBytes\SchoolBytes\SchoolBytes\App_Data\ConnectionCredentials.json";
            //string filePath = HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json");
            StreamReader credJson = new StreamReader(filePath);
            //HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json");
            return (string)JObject.Parse(credJson.ReadToEnd())["credentials"];
        }

        public static bool IsEligibleToSubscribe(Participant participant)
        {
            int registrations = getDBContext().courseModules.Sum(cm => cm.Registrations.Count(r => r.participant == participant));

            if (registrations > 5 )
            {
                return false;
            }

            return true;
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

            bool getParticipant = getDBContext().courseModules.Any(p => p.Name.Equals(participant.Name));

            if (getParticipant)
            {
                return "${participant.Name} er der";
            }
            else
            {
                return "${participant.Name} findes ikke";
            }
        }

        public static void SubscribeTest(int courseId, int moduleId, Participant participant)
        {
            CourseModule courseModule = getDBContext().courseModules.Find(moduleId);

            if (courseModule.Capacity <= courseModule.MaxCapacity)
            {
                if (DBConnection.IsEligibleToSubscribe(participant))
                {

                    Registration registration = new Registration(participant, courseModule);
                    courseModule.Capacity += 1;
                    getDBContext().UpdateSub(registration, courseModule);
                }
                else
                {
                    Console.WriteLine("Du har allerede tilmeldt dig maksimum antal hold.");
                }
            }
            else
            {
                //VENTELISTE LOGIK SKAL IND HER

                getDBContext().Update(courseModule);
                WaitRegistration yeet = new WaitRegistration(participant, courseModule, DateTime.Now);

                courseModule.Waitlist.AddLast(yeet);
                getDBContext().SaveChangesV2();

            }
        }
    }
}