using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    public class ModuleController : Controller
    {
        DBConnection dBConnection = DBConnection.getDBContext();
               

        // GET: Module
        public ActionResult Index()
        {
            return View("ModuleOverview");
        }

        // GET: course/{id}/modules (Get course modules by course ID)
        [HttpGet]
        [Route("course/{id}/ModuleOverview")]
        public ActionResult ModuleOverview(int id)
        {
            var course = dBConnection.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            return View(course.CoursesModules); // Passes only the course modules to the view
        }

        // POST: api/course/{courseid}/update/{moduleid} (Update course module)
        [HttpPost]
        [Route("course/{courseId}/update/{moduleId}")]
        public ActionResult Update(int courseId, int moduleId, CourseModule updatedCourseModule)
        {
            var course = dBConnection.courses.Find(courseId);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            var module = course.CoursesModules.Find(m => m.Id == moduleId);
            if (module == null) 
            {
                return HttpNotFound("Course module not found");
            }

            if (ModelState.IsValid)
            {
                module.Name = updatedCourseModule.Name;
                module.Teacher = updatedCourseModule.Teacher;
                module.Date = updatedCourseModule.Date;
                module.StartTime = updatedCourseModule.StartTime;
                module.EndTime = updatedCourseModule.EndTime;
                module.Capacity = updatedCourseModule.Capacity;
                module.Location = updatedCourseModule.Location;
                dBConnection.Update(course);
                dBConnection.SaveChanges();

                return RedirectToAction("ModuleOverview");
            }

            return View(course);
        }

        // DELETE: api/course/{id}/delete/{moduleId} (Remove course module)
        [HttpPost]
        [Route("course/{courseId}/delete/{moduleId}")]
        public ActionResult Delete(int courseId, int moduleId)
        {
            var course = dBConnection.courses.Find(courseId);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            var module = course.CoursesModules.Find(m => m.Id == moduleId);
            if (module == null)
            {
                return HttpNotFound("Course module not found");
            }
            //TODO: dobbelt tjek om den også sletter FK på course
            dBConnection.Remove(module);
            dBConnection.SaveChanges();

            return RedirectToAction("CourseOverview");
        }
    }
}