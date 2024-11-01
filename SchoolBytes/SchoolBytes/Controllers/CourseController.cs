using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {



        // GET: Course
        public ActionResult Index()
        {
            return View();
        }


        //CRUD METODER

        

        [HttpPost]
        public ActionResult addCourse()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult removeCourse()
        {
            return View();
        }

        [Route("hold/{id}")]
        public ActionResult getCourse(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult updateCourse()
        {
            return View();
        }

    }
}