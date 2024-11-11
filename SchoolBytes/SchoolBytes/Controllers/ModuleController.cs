using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    public class ModuleController : Controller
    {
        DBConnection dBConnection = DBConnection.getDBContext();
               

        // GET: course/{id}/modules (Get course modules by course ID)
        [HttpGet]
        [Route("course/{id}/moduleOverview")]
        public ActionResult ModuleOverview(int id)
        {
            var course = dBConnection.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            return View(course.CoursesModules.ToList()); // Passes only the course modules to the view
        }

        // POST: api/course/{courseid}/update/{moduleid} (Update course module)
        [HttpPost]
        [Route("course/{courseId}/update/{moduleId}")]
        public ActionResult Update(int courseId, int moduleId, CourseModule updatedCourseModule)
        {
            //updatedCourse module already has all this info, do we really need course id and module id? It's in updatedCourseModule
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
       
            module.Name = updatedCourseModule.Name;

            FoodModule fm = updatedCourseModule.FoodModule;
            //FoodModule code
            if (fm!= null && fm.Name != "")
            {
                fm.Course = course;
                fm.Date = updatedCourseModule.Date;
                fm.Capacity = updatedCourseModule.Capacity;
                fm.Teacher = updatedCourseModule.Teacher;
                module.FoodModule = fm;
                dBConnection.Add(fm);
            }


            //TODO: Make this use ModelState again, but need to fix frontend for that as it sends a string not a teacher obj so there can be no mapping
            //Easy solution would be to queury for all teachers, then make dropdown where you can choose teacher's name. Then have the value for the select option be the teachers id
            //and set it that way. Won't fix modelstate, but we can just avoid modelstate anyway as we're not posting a form here.
            module.Teacher = updatedCourseModule.Teacher;
            
       
            module.Date = updatedCourseModule.Date;
            module.StartTime = updatedCourseModule.StartTime;
            module.EndTime = updatedCourseModule.EndTime;
            module.Capacity = updatedCourseModule.Capacity;
            module.Location = updatedCourseModule.Location;
            dBConnection.Update(module);
            dBConnection.SaveChanges();

           /* if (ModelState.IsValid)
            {
               

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }*/

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult setFoodModule(FoodModule fm)
        {
      

            Debug.Print("TEEEEEEEEEEEEEEEST  " + fm.Name);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}