using IronXL;
using SchoolBytes.Models;
using System;
using System.IO;
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
            if (selectedParticipant != null)
            {
                builder.ForParticipant(selectedParticipant);
            }
            if (selectedCourse != null)
            {
                builder.ForClass(selectedCourse.Name);
            }

            Exports data = builder.Build();
            WorkBook wb = data.ConvertToXls();

            using (var memStream = wb.ToStream())
            {

                memStream.Seek(0, SeekOrigin.Begin);

                return File(memStream, "application/vnd.ms-excel", $"Attendance-${DateTime.Now.Date}.xls");
            }
            
        }
    }
}