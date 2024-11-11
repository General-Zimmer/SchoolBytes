using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using SchoolBytes.DTO;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {
        private List<Course> courses
        {
            get
            {
                var teacher1 = new Teacher { Name = "John Doe" };
                var participant1 = new Participant { Name = "Alice" };
                var participant2 = new Participant { Name = "Bob" };

                var course1 = new Course(
                    "Science 101",
                    "A foundational course in basic scientific principles",
                    teacher1,
                    DateTime.Now,
                    DateTime.Now.AddMonths(1).AddDays(1),
                    25,
                    1
                );
                course1.Participants.Add(participant1);
                course1.Participants.Add(participant2);

                var course2 = new Course(
                    "Math 101",
                    "A foundational course in basic math principles",
                    teacher1,
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddMonths(1).AddDays(1),
                    25,
                    2

                );
                var courseTest = new List<Course>();
                courseTest.Add(course1);
                courseTest.Add(course2);
                return courseTest;
            }
            set { }
        }

        // GET: Course
        public ActionResult Index()
        {
            return RedirectToAction("CourseOverview");
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
            
            var modules = new List<CourseModule>(); // Liste over alle moduller
            var activeDays = new List<DayOfWeek>(); // Liste over dage som er valgt
            
            //Checkbox data
            if (courseDTO.Monday)
                activeDays.Add(DayOfWeek.Monday);
            if (courseDTO.Tuesday)
                activeDays.Add(DayOfWeek.Tuesday);
            if (courseDTO.Wednesday)
                activeDays.Add(DayOfWeek.Wednesday);
            if (courseDTO.Thursday)
                activeDays.Add(DayOfWeek.Thursday);
            if (courseDTO.Friday)
                activeDays.Add(DayOfWeek.Friday);
            if (courseDTO.Saturday)
                activeDays.Add(DayOfWeek.Saturday);
            if (courseDTO.Sunday)
                activeDays.Add(DayOfWeek.Sunday);
            
            // Dage som er valgt ud fra checkbox
            var daysSelected = activeDays.Count;
            
            //Udregning for antal moduler i alt
            var moduleCount = (int)(courseDTO.EndDate - courseDTO.StartDate).TotalDays;
            
            //Uddeling af moduler pr dage
            var modulesPerDay = moduleCount / daysSelected;
            var remainingModules = moduleCount % daysSelected;
            
            var currentDate = DateTime.Now;

            //Skiftevis vælge dag 
            foreach (var activeDayDate in activeDays.Select(day => GetDayForWeekday(currentDate, day)))
            {
                for (var i = 0; i < modulesPerDay; i++)
                {
                    modules.Add(new CourseModule
                    {
                        Name = $"Modul {modules.Count + 1}",
                        Date = activeDayDate
                    });
                }

                if (remainingModules <= 0) continue;
                modules.Add(new CourseModule
                {
                    Name = $"Modul {modules.Count + 1}",
                    Date = activeDayDate
                });
                remainingModules--;
            }
            
            course.CoursesModules = modules;
            courses.Add(course);
            return RedirectToAction("CourseOverview");
        }

        // GET: api/course/{id} (Get course by ID)
        [Route("course/{id}")]
        public ActionResult GetCourse(int id)
        {
            // var course = courses.SingleOrDefault(c => c.Id == id);
            var course = courses.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }
            
            return View(course);
        } 

        // POST: api/course/{id} (Update course)
        [HttpPost]
        [Route("course/update/{id}")]
        public ActionResult Update(int id, Course updatedCourse)
        {
            var course = courses.FirstOrDefault(x => x.Id == id);
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

                return RedirectToAction("CourseOverview");
            }

            ViewBag.Teacker
            return View(course);
        }

        // DELETE: api/course/{id} (Remove course)
        [HttpPost]
        [Route("course/delete/{id}")]
        public ActionResult Delete(int id)
        {
            var course = courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            courses.Remove(course);
            return RedirectToAction("CourseOverview");
        }

        public ActionResult CourseOverview(int? selectedCourseId = null)
        {
            ViewBag.SelectedCourseId = selectedCourseId;
            //ViewBag.CourseDTO = new CourseDTO(); //ved ikke
            return View(courses);
        }
        
        private static DateTime GetDayForWeekday(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysToAdd);
        }
    }
}
        
 