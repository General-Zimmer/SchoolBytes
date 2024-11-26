using System.IO;
using System.Web;
using Gherkin.CucumberMessages.Types;
using Microsoft.EntityFrameworkCore;
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
using Gherkin.CucumberMessages.Types;


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
            string filePath = @"C:\Users\Duff\Desktop\SchoolBytes\SchoolBytes\SchoolBytes\App_Data\ConnectionCredentials.json";
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

        public void CancelModule(CourseModule courseModule) 
        {
            if (courseModule.StartTime > DateTime.Now)  // cannot cancel a courseModule that's from the past!
            {
                courseModule.IsCancelled = true;

                // if there's a related Food Module, then that gets cancelled too
                if (courseModule.FoodModule != null)
                {
                    courseModule.FoodModule.IsCancelled = true;
                    self.Update(courseModule.FoodModule);
                }

                // if there are any participants in the course
                if (courseModule.Capacity > 0)
                {
                    // notify all participants
                    MailService service = new MailService("host");
                    service.ClassCanceledNotification(courseModule);
                }

                self.Update(courseModule);
                self.SaveChanges();
            }
        }
    }
}