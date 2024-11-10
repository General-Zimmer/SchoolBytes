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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CourseModule>()
                .HasOne(cm => cm.Course)
                .WithMany(c => c.CoursesModules)
                .HasForeignKey(cm => cm.Id)
                .OnDelete(DeleteBehavior.Cascade);  // Enable cascade delete
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
            StreamReader credJson = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json"));//File.ReadAllText("C:\\Users\\Duff\\Desktop\\SchoolBytes\\SchoolBytes\\SchoolBytes\\App_Data\\ConnectionCredentials.json"); 
            return (string)JObject.Parse(credJson.ReadToEnd())["credentials"];
        }
    }
}