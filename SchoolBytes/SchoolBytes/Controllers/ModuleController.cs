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
            var course = courses.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            return View(course.Courses); // Passes only the course modules to the view
        }

        // POST: api/course/{courseid}/update/{moduleid} (Update course module)
        [HttpPost]
        [Route("course/{courseId}/update/{moduleId}")]
        public ActionResult Update(int courseId, int moduleId, CourseModule updatedCourseModule)
        {
            var course = courses.FirstOrDefault(x => x.Id == courseId);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            var module = course.Courses.FirstOrDefault(y => y.Id == moduleId);
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

                return RedirectToAction("ModuleOverview");
            }

            Debug.WriteLine($"CourseId: {courseId}, ModuleId: {moduleId}");

            return View(course.Courses);
        }

        // DELETE: api/course/{id}/delete/{moduleId} (Remove course module)
        [HttpPost]
        [Route("course/{courseId}/delete/{moduleId}")]
        public ActionResult Delete(int courseId, int moduleId)
        {
            var course = courses.SingleOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            var module = course.Courses.FirstOrDefault(y => y.Id == moduleId);
            if (module == null)
            {
                return HttpNotFound("Course module not found");
            }

            Debug.WriteLine($"CourseId: {courseId}, ModuleId: {moduleId}");

            course.Courses.Remove(module);
            return RedirectToAction("CourseOverview");
        }
    }
}