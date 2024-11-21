using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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


        //TILMELDINGER
        
        [HttpPost]
        [Route("Module/course/{courseId}/{moduleId}/tilmeld")]
        public ActionResult Subscribe(int courseId, int moduleId, Participant participant)
        {
            CourseModule courseModule = dBConnection.courseModules.Find(moduleId);

            //Course course = dBConnection.courses.Find(courseId);

            if (courseModule.Capacity <= courseModule.MaxCapacity)
            {
                if(DBConnection.IsEligibleToSubscribe(participant))
                {

                
                Participant newParticipant = new Participant(participant.Name, participant.PhoneNumber);
                Registration registration = new Registration(newParticipant, courseModule);
                courseModule.Capacity += 1;
                dBConnection.Update(courseModule);
                dBConnection.SaveChanges();
                } else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Du har allerede tilmeldt dig maksimum antal hold.");
                }
            }
            else
            {
                //VENTELISTE LOGIK SKAL IND HER - PLACEHOLDER INDTIL VIDERE
                return HttpNotFound("Hold fyldt");
            }


            return TheView(null);
        }

        [HttpPost]
        [Route("Module/course/{courseId}/{moduleId}/tilmeld")]
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
                   skippedModules.Add(module); // hvad er det?
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

        [HttpPost]
        [Route("Module/course/{courseId}/{moduleId}/afmeld")]
        public ActionResult Cancel(int courseId, int moduleId, string tlfNr)
        {
            CourseModule courseModule = dBConnection.courseModules.Find(moduleId);

            Course course = dBConnection.courses.Find(courseId);

            Participant participant = course.Participants.Find(p => p.PhoneNumber == tlfNr);
            if (participant != null)
            {
                course.Participants.Remove(participant);

                dBConnection.Update(courseModule);
                dBConnection.SaveChanges();
            }
            else
            {
                //placeholder
                return HttpNotFound("Ingen tilmeldte med opgivne informationer fundet");
            }



            return TheView(null);
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
                module.Waitlist.AddLast(participant);
            }

            return View(module);
        }
    }
}