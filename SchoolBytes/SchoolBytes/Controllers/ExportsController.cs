using IronXL;
using SchoolBytes.Models;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
namespace SchoolBytes.Controllers
{
    public class ExportsController : Controller
    {

        [HttpGet]
        [Route("report")]
        public ActionResult DownloadReport(Participant selectedParticipant = null, Course selectedCourse = null)
        {
            ExportsBuilder builder = new ExportsBuilder();
            if (selectedParticipant.Id != 0)
            {
                builder.ForParticipant(selectedParticipant);
            }
            if (selectedCourse.Id != 0)
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