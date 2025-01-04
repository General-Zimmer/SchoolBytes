using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SchoolBytes.DTO;
using SchoolBytes.Models;
using SchoolBytes.util;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {
        DBConnection dbConnection = DBConnection.getDBContext();


        // GET: Course
        [HttpGet]
        [Route("course")]

        public ActionResult CourseOverview(int? selectedCourseId = null)
        {
            if(selectedCourseId != null)
            {
                ViewBag.SelectedCourseId = selectedCourseId;
            }
           
            return View(dbConnection.courses.ToList());
        }
      

        // POST: api/course (Add new course)
        [HttpPost]
        [Route("course/create")]
        public ActionResult Create(CourseDTO courseDTO)
        {
            var course = new Course
            {
                Name = courseDTO.Name,
                Description = courseDTO.Description,
                Teacher = courseDTO.Teacher,
                Participants = courseDTO.Participants,
                CoursesModules = courseDTO.CoursesModules,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                MaxCapacity = courseDTO.MaxCapacity,
                Id = courseDTO.Id
            };

            var req = Request.Form;

            var activeDays = new List<DayOfWeek>();

            foreach (DayOfWeek day in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
            {
                bool isDayContained = req.AllKeys.Contains(day.ToString());

                if (isDayContained) activeDays.Add(day);

                PropertyInfo prop = course.GetType().GetProperty(day.ToString(), BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(course, isDayContained, null);
                }
            }

            var daysCount = activeDays.Count;
            
            if (daysCount == 0)
            {
                //TODO: yeh this is not a good way of doing it... smider en hen på en fejlside
                throw new InvalidOperationException("Ingen dage valgt for kursus.");
            }         


            for (DateTime start = course.StartDate; start <= course.EndDate; start=start.AddDays(1))
            {
                if (activeDays.Contains(start.DayOfWeek))
                {
                    //gets the start of the current module's day then adds the hours corresponding to the ending time for the module. This
                    //time comes from the initial 'end' field and is the time chosen from this field. This value is the end value for every module.
                    var endTime = start.Date.AddHours(courseDTO.EndDate.Hour);           
                    CourseModule cm = new CourseModule()
                    {
                        Name = $"Lektion {course.CoursesModules.Count + 1}",
                        Date = start,
                        MaxCapacity = course.MaxCapacity,
                        Teacher = course.Teacher,
                        StartTime = start,
                        EndTime = endTime

                    };
                    dbConnection.Add(cm);
                    course.CoursesModules.Add(cm);

                }
            }

            dbConnection.Add(course);
            dbConnection.SaveChanges();

            return RedirectToAction("CourseOverview");

        }
     
        //// GET: api/course/{id} (Get course by ID)
        //[Route("course/{id}")]
        //public ActionResult GetCourse(int id)
        //{
        //    Course course = dbConnection.courses.Find(id);

        //    if (course == null)
        //    {
        //        return HttpNotFound("Course not found");
        //    }

        //    return View(course);
        //}

        // POST: api/course/update/{id} (Update course)
        [HttpPost]
        [Route("course/update/{id}")]
        public ActionResult Update(int id, Course updatedCourse)
        {
            
            Course course = dbConnection.courses.Find(id);
           
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            if (ModelState.IsValid)
            {
                course.Name = updatedCourse.Name;
                course.Description = updatedCourse.Description;
                Console.WriteLine(course.Description);
                course.StartDate = updatedCourse.StartDate;
                course.EndDate = updatedCourse.EndDate;
                course.MaxCapacity = updatedCourse.MaxCapacity;

                    course.Name = updatedCourse.Name;
                    course.Description = updatedCourse.Description;
                    course.StartDate = updatedCourse.StartDate;
                    course.EndDate = updatedCourse.EndDate;
                    course.MaxCapacity = updatedCourse.MaxCapacity;
                    
                dbConnection.SaveChanges();

                return RedirectToAction("CourseOverview");
            }

            return View(course);
        }

        // DELETE: api/course/{id} (Remove course)
        [HttpPost]
        [Route("course/delete/{id}")]
        public ActionResult Delete(int id)
        {

            Course course = dbConnection.courses.Find(id);
   
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }        
            else
            {
                //neccesarry evil because for some reason foodmodule has a ref to course instead of cm. Ideally would change this instead
                //but it's more time consuming
                foreach(var cm in course.CoursesModules)
                {
                    if(cm.FoodModule != null)
                    {
                        dbConnection.Entry(cm.FoodModule).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    }
                }
                 dbConnection.Remove(course);
                 dbConnection.SaveChanges();

                 return RedirectToAction("CourseOverview");
            }
        }

        private static DateTime GetDayForWeekday(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysToAdd);
        }


        //TILMELDINGER
        [HttpPost]
        [Route("course/{id}/tilmeld")]
        public ActionResult Subscribe(int id, Participant participant)
        {
            Course course = dbConnection.courses.Find(id);

            Participant newParticipant = new Participant(participant.Name, participant.PhoneNumber);

            if (course.Participants.Count < course.MaxCapacity)
            {
                course.Participants.Add(newParticipant);

                dbConnection.Update(course);
                dbConnection.SaveChanges();
            } else
            {
                //VENTELISTE LOGIK SKAL IND HER - PLACEHOLDER INDTIL VIDERE
                return HttpNotFound("Hold fyldt");
            }
            

            return RedirectToAction("CourseOverview");
        }

        [HttpPost]
        [Route("course/{id}/afmeld")]
        public ActionResult Cancel(int id, string tlfNr)
        {
            Course course = dbConnection.courses.Find(id);

            Participant participant = course.Participants.Find(p => p.PhoneNumber == tlfNr);
            if (participant != null)
            {
                course.Participants.Remove(participant);

                dbConnection.Update(course);
                dbConnection.SaveChanges();
            } else
            {
                //placeholder
                return HttpNotFound("Ingen tilmeldte med opgivne informationer fundet");
            }
            


            return View();
        }

        


    }
}

