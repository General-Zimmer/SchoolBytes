using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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
            var course = dBConnection.courses.Include(c => c.CoursesModules).ToList().Where(c  => c.Id == id).First();
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            ViewBag.teachers = dBConnection.teachers.ToList();
            return View(course.CoursesModules.ToList()); // Passes only the course modules to the view
        }

        // POST: api/course/{courseid}/update/{moduleid} (Update course module)
        [HttpPost]
        [Route("course/{courseId}/update/{moduleId}/{teacherId}")]
        public ActionResult Update(int courseId, int moduleId, int teacherId, CourseModule updatedCourseModule)
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

            Debug.Print("TEEEEEEEEEEEEEEEEEEST: " + fm.Name);

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


            //TODO: need to find a better way of doing this - fetching the teacherID in this way is not very elegant!!
            
            Teacher updatedTeacher = dBConnection.teachers.ToList().Where(t => t.Id == teacherId).FirstOrDefault();
            module.Teacher = updatedTeacher;
            
       
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