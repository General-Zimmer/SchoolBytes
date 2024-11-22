using System.Web.Mvc;
using System;
using SchoolBytes.Models;
using QRService;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Data.Common;
using SchoolBytes.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;


namespace SchoolBytes.Controllers
{
    public class QRController : Controller
    {
        // Setup
        DBConnection dbConnection = DBConnection.getDBContext();
        //private readonly IConfiguration _configuration;
        readonly QRGeneratorService generatorService = new QRGeneratorService();
        // Constructor to inject IConfiguration
        /*public QRController(IConfiguration configuration)
        {
            _configuration = configuration;
        }*/

        [HttpGet]
        [Route("QR/Generation")]
        public ActionResult Generation()
        {
            return View();
        }
        [HttpGet]
        [Route("QR/Generation/Generate")]
        public ActionResult GenerateQR()
        {
            string newId = GenerateRandomId();
            SetQrId(newId);
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            string fullUrl = baseUrl + "/QR/Index/" + newId;
            Bitmap bitmap = generatorService.GenerateBitmap(fullUrl);
            String base64String;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            ViewBag.ImageData = base64String;
            return View();
        }

        [HttpGet]
        [Route("QR/Index/{Id}")]
        public ActionResult Index(string Id)
        {
            string currentId = GetQrId();
            if (Id != currentId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Id ikke rigtig.");
            }
            ViewBag.Title = "Fremmøde Tjek Ind";
            return View(dbConnection.courses.ToList());
        }

        // GET: Course
        [HttpGet]
        //[Route("QR/Index/")]
        public ActionResult CourseOverviewQR()
        {
            var courses = dbConnection.courses.ToList();

            return View(courses);  // Send the list of courses to the view
        }

        // Post: Fremmøde
        [HttpPost]
        [Route("QR/Index/RegistrationCheckIn/{tlfNummer}/{moduleId}")]
        public ActionResult RegistrationCheckIn(string tlfNummer, int moduleId)
        {
            CourseModule courseModule = dbConnection.courseModules.Find(moduleId);
            System.Diagnostics.Debug.WriteLine(courseModule);
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
                System.Diagnostics.Debug.WriteLine(registration);
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


        public string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Generates a number between
        }
        public string GetQrId()
        {
            var json = System.IO.File.ReadAllText("appsettings.json");
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
            return config["CurrentQrId"]?.ToString();
        }
        public void SetQrId(string newId)
        {
            var json = System.IO.File.ReadAllText("appsettings.json");
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
            config["CurrentQrId"] = newId;

            // Write updated JSON back to the file
            System.IO.File.WriteAllText("appsettings.json", config.ToString());
        }
    }
}
