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
using System.Text;


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
            string filePath = @"C:\Users\jakob\source\repos\General-Zimmer\SchoolBytes\SchoolBytes\SchoolBytes\App_Data\ConnectionCredentials.json";
            StreamReader credJson = new StreamReader(filePath); //HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json")
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

        public static List<Participant> GetAbsentees()
        {
            //Finder courseModules afholdt inden for seneste 2 months
            List<CourseModule> recentCMs = getDBContext().courseModules.Where(cm => DateTime.Compare(DateTime.Now.AddMonths(-2), cm.StartTime)<0).ToList();
                
            List<Registration> registrations = recentCMs.SelectMany(cm => cm.Registrations).ToList();

            //Finder participants der har 3 eller flere udeblivelser indenfor seneste 2 months
            List<Participant> absentess = getDBContext().participants.Where(p => registrations.Count(r => r.participant.Id == p.Id && !r.Attendance) > 2).ToList();

            return getDBContext().participants.Where(p => p.Id % 2 == 0).ToList();
        }

        public static string CheckNotificationsTest()
        {
            List<Participant> absentees = DBConnection.GetAbsentees();

            string resultString = "The following students are absent: " + System.Environment.NewLine;
            resultString += string.Join(", " + System.Environment.NewLine, absentees);

            if (absentees.Count() == 0) resultString = "";

            return resultString;
        }
    }
}