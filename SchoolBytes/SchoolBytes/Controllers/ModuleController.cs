using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Gherkin.CucumberMessages.Types;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolBytes.Models;
using SchoolBytes.util;
using static SchoolBytes.util.DatabaseUtils;

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

        // Get all course modules for a course as a JSON object
        [HttpGet]
        [Route("course/{id}/modules")]
        public ActionResult ModuleList(int id)
    {
        List<CourseModule> courseModules = dBConnection.courses.ToList().Where(c => c.Id == id).First().CoursesModules;
        if (courseModules == null)
        {
            return HttpNotFound("Course not found");
        }
            Dictionary<int, string> dict = new Dictionary<int, string>();

            courseModules.ForEach(cm =>
            {
                if (cm.Date.Date == DateTime.Today)
                {
                    dict.Add(cm.Id, cm.Name);
                }
            });
            return Json(JsonConvert.SerializeObject(dict), JsonRequestBehavior.AllowGet); // Passes the course modules as JSON
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

            
            //FoodModule code
            if (fm!= null && fm.Name != "")
            {
                Debug.Print("TEEEEEEEEEEEEEEEEEEST: " + fm.Name);
                fm.Course = course;
                fm.Date = updatedCourseModule.Date;
                fm.Capacity = updatedCourseModule.Capacity;
                fm.Teacher = updatedCourseModule.Teacher;
                module.FoodModule = fm;
                dBConnection.Add(fm);
            }


            //TODO: need to find a better way of doing this - fetching the teacherID in this way is not very elegant!!

            Teacher updatedTeacher = dBConnection.teachers.ToList().Where(t => t.Id == updatedCourseModule.Teacher.Id).FirstOrDefault();
            module.Teacher = updatedTeacher;

            module.Date = updatedCourseModule.Date;
            module.StartTime = updatedCourseModule.StartTime;
            module.EndTime = updatedCourseModule.EndTime;
            module.MaxCapacity = updatedCourseModule.MaxCapacity;
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

        [HttpGet]
        [Route("Module/SubModalWindow/{courseId}/{ModuleId}")]
        public ActionResult SubModal(int courseId, int ModuleId)
        {
            var course = dBConnection.courses.Find(courseId);
            var module = dBConnection.courseModules.Find(ModuleId);
            ViewBag.CourseId = courseId;
            ViewBag.ModuleId = ModuleId;
            ViewBag.CourseName = course.Name;
            ViewBag.ModuleName = module.Name;
            return View("SubModal");
        }

        //TILMELDINGER
        
        [HttpPost]
        [Route("course/{courseId}/module/{moduleId}/tilmeld")]
        public ActionResult Subscribe(int courseId, int moduleId, Participant participant)
        {
            CourseModule courseModule = dBConnection.courseModules.Find(moduleId);

            //Course course = dBConnection.courses.Find(courseId);

            if (courseModule.Capacity <= courseModule.MaxCapacity)
            {
                if(DBConnection.IsEligibleToSubscribe(participant))
                {

                    Registration registration = new Registration(participant, courseModule);
                    courseModule.Capacity += 1;
                    dBConnection.UpdateSub(registration, courseModule);
                } else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Du har allerede tilmeldt dig maksimum antal hold.");
                }
            }
            else
            {
                //VENTELISTE LOGIK SKAL IND HER
                
                dBConnection.Update(courseModule);
                WaitRegistration yeet = new WaitRegistration(participant, courseModule, DateTime.Now);

                courseModule.Waitlist.AddLast(yeet);
                dBConnection.SaveChangesV2();
                return RedirectToAction(courseId +"/" + courseModule.Id + "/signup/waitlist", "course");
                }


                return TheView(null);
            }

            //TODO: Skal det her med?
            [HttpPost]
            [Route("Module/course/{courseId}/module/{moduleId}/tilmeld")]
            public ActionResult Subscribe(int courseId, List<int> moduleIds, Participant participant)
        {
            Course course = dBConnection.courses.Find(courseId);

            if (course == null)
            {
                //burde være en httpstatuscode
                return HttpNotFound("Course does not");
            }

            List<CourseModule> selectedModules = new List<CourseModule>();
            List<CourseModule> skippedModules = new List<CourseModule> ();

            foreach (var moduleId in moduleIds)
            {
                CourseModule module = dBConnection.courseModules.Find(moduleId);

                if (module != null && module.Capacity < module.MaxCapacity)
                {
                    selectedModules.Add(module);
                }
                else
                {
                   skippedModules.Add(module);
                }
            }



           //TODO: edit the 5 so it comes from some kind of setting. It's the max amount of subcribtions u can have at once
            if (selectedModules.Count + DBConnection.GetSubscribeCount(participant) > 5) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Deltager er tilmeldt for mange hold."); 
            }

            selectedModules.ForEach(sm =>
            {
                Registration registration = new Registration(participant, sm);
                sm.Registrations.Add(registration);

            });


            dBConnection.Update(selectedModules);
            dBConnection.SaveChanges();

            return TheView(skippedModules);
        }

        [HttpGet]
        [Route("course/{courseId}/{moduleId}/afmeld")]
        public ActionResult unsub(int courseId, int moduleId)
        {
            var course = dBConnection.courses.Find(courseId);
            var module = dBConnection.courseModules.Find(moduleId);

            ViewBag.CourseId = courseId;
            ViewBag.ModuleId = moduleId;
            ViewBag.CourseName = course.Name;
            ViewBag.ModuleName = module.Name;


            return View("UnSubModal");
        }

        [HttpPost]
        [Route("course/{courseId}/module/{moduleId}/afmeld/{tlfNr}")]
        public ActionResult Unsub(int courseId, int moduleId, string tlfNr)
        {
            //Returns null if succesful or HTTPstatus code result if it did not work.
            HttpStatusCodeResult res = DatabaseUtils.Unsub(courseId, moduleId, tlfNr);
            if(res != null)
            {
                return res;
            }



            return Redirect("~/course/" + courseId + "/ModuleOverview");
        }

        [HttpGet]
        public ActionResult TheView(IList skippedModules)
        {
            ViewBag.skippedModules = skippedModules;
            return View("ParticipantView");
        }

        // GET: api/course/{courseId}/{moduleId}/signup/waitlist (Sign up for the waitlist for a course module)
        [HttpGet]
        [Route("course/{courseId}/{moduleId}/signup/waitlist")]
        public ActionResult WaitlistSignup(int courseId, int moduleId)
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

            var participants = dBConnection.participants;
            foreach (var participant in participants) 
            { 
                //module.Waitlist.AddLast(participant);
            }

            return View(module);
        }
    }
}