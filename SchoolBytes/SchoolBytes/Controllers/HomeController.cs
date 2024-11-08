using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SchoolBytes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolBytes.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBConnection dBConnection = DBConnection.getDBContext();
            dBConnection.Add(new Course());
            dBConnection.SaveChanges();
            Debug.Print("yeet");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CourseOverview(int? selectedCourseId = null)
        {
            var courses = new List<Course>
            {
                new Course { Name = "Holdnavn 1", Description = "Description for Holdnavn 1",
                    Teacher = new Teacher { Name = "Underviser 1" }, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(30), MaxCapacity = 25, Id = 1 },
                new Course { Name = "Holdnavn 2", Description = "Description for Holdnavn 2",
            Teacher = new Teacher { Name = "Underviser 2" }, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(60), MaxCapacity = 10, Id = 2 }
            };

            ViewBag.SelectedCourseId = selectedCourseId;
            
            return View(courses);
        }
    }
}