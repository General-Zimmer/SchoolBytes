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

namespace SchoolBytes.Controllers
{
    public class QRController : Controller
    {
        DBConnection dbConnection = DBConnection.getDBContext();

        readonly QRGeneratorService generatorService = new QRGeneratorService();
        [HttpGet]
        public ActionResult GenerateQR()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            string fullUrl = baseUrl + "/QR/Index";
            Bitmap bitmap = generatorService.GenerateBitmap(fullUrl);
            String base64String;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            ViewBag.Message = "På denne side kan du genere en ny QR kode.\n" +
                "Når en ny kode er lavet virker de tidligere ikke.";
            ViewBag.Title = "QR Kode Oprettelse";
            ViewBag.ImageData = base64String;
            return View();
        }

        [HttpGet]
        [Route("QR/Index")]
        public ActionResult Index()
        {
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
        [Route("QR/Index/RegistrationCheckIn")]
        public ActionResult RegistrationCheckIn()
        {
            System.Diagnostics.Debug.WriteLine("Fremmøde Check In");
            return RedirectToAction("Index", "QR"); // Redirects to QR/Index
        }
    }
}