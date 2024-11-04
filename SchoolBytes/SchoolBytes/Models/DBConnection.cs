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

        public DBConnection(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(connectionString);
            }


        }
        public static DBConnection getDBContext()
        {
            StreamReader credJson = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/ConnectionCredentials.json"));
            var yeet = (string)JObject.Parse(credJson.ReadToEnd())["credentials"];
            if (self == null)
            {
                self = new DBConnection(yeet);
            }
            return self;
        }

    }
}