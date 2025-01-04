using IronXL;
using Microsoft.Ajax.Utilities;
using SchoolBytes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Windows.Documents;
namespace SchoolBytes.Controllers
{
    public class ExportsController : Controller
    {

        private static DBConnection _connection = DBConnection.getDBContext();

        [Route("index")]
        public ActionResult Index()
        {
            ViewBag.Courses = _connection.courses.ToList();
            ViewBag.Participants = _connection.participants.ToList();


            return View();
        }

        [HttpPost]
        [Route("report/download")]
        public ActionResult DownloadReport(FormCollection request)
        {
            var selectedCourseId = Int32.Parse(request[1]);
            var selectedParticipantId = Int32.Parse(request[0]);

            var selectedCourse = _connection.courses.Find(selectedParticipantId);
            var selectedParticipant = _connection.participants.Find(selectedParticipantId);

            ExportsBuilder builder = new ExportsBuilder();
            if (selectedParticipant != null)
            {
                builder.ForParticipant(selectedParticipant);
            }
            if (selectedCourse != null)
            {
                builder.ForClass(selectedCourse.Name);
            } else
            {
                builder.ForClass("All");
            }
            Exports data = builder.Build();
            if (data == null) return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            WorkBook wb = data.ConvertToXls();
            var memStream = wb.ToStream();
            memStream.Seek(0, SeekOrigin.Begin);

            // Return the file as a downloadable response
            return File(memStream, "application/vnd.ms-excel", $"Attendance-{DateTime.Now:yyyy-MM-dd}.xls");
        }

    }
    
}