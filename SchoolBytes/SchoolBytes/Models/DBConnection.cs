using System.IO;
using System.Web;
using Gherkin.CucumberMessages.Types;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;


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
    }
}