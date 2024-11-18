using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SchoolBytes.DTO;
using SchoolBytes.Models;

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
            Console.WriteLine("TEEEEEEST"  + req.ToString());

            var activeDays = new List<DayOfWeek>();

            

            if (req.AllKeys.Contains("Monday"))
                activeDays.Add(DayOfWeek.Monday);
            if (req.AllKeys.Contains("Tuesday"))
                activeDays.Add(DayOfWeek.Tuesday);
            if (req.AllKeys.Contains("Wednesday"))
                activeDays.Add(DayOfWeek.Wednesday);
            if (req.AllKeys.Contains("Thursday"))
                activeDays.Add(DayOfWeek.Thursday);
            if (req.AllKeys.Contains("Friday"))
                activeDays.Add(DayOfWeek.Friday);
            if (req.AllKeys.Contains("Saturday"))
                activeDays.Add(DayOfWeek.Saturday);
            if (req.AllKeys.Contains("Sunday"))
                activeDays.Add(DayOfWeek.Sunday);


            var daysCount = activeDays.Count;
            
            if (daysCount == 0)
            {
                //yeh this is not a good way of doing it... smider en hen på en fejlside
                throw new InvalidOperationException("Ingen dage valgt på kursus.");
            }

            
            var remainingModules = courseDTO.numberOfModules;
           


            for (DateTime start = course.StartDate; start <= course.EndDate; start=start.AddDays(1))
            {
                if (activeDays.Contains(start.DayOfWeek))
                {
                    CourseModule cm = new CourseModule()
                    {
                        Name = $"Module {course.CoursesModules.Count + 1}",
                        Date = start,
                        MaxCapacity = course.MaxCapacity,
                        Teacher = course.Teacher,
                    };
                    dbConnection.Add(cm);
                    course.CoursesModules.Add(cm);

                    remainingModules--;
                }
            }

            dbConnection.Add(course);
            dbConnection.SaveChanges();

            return RedirectToAction("CourseOverview");

        }
     
        // GET: api/course/{id} (Get course by ID)
        [Route("course/{id}")]
        public ActionResult GetCourse(int id)
        {
            Course course = dbConnection.courses.Find(id);

            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            return View(course);
        }

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

