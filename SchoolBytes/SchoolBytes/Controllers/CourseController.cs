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

                // Create dummy CourseModule objects with fully initialized properties
                var courseModule1 = new CourseModule
                {
                    Id = 1,
                    Name = "Module 1",
                    Teacher = teacher1,
                    FoodModule = null,
                    Date = DateTime.Now,
                    StartTime = new DateTime(2024, 11, 10, 9, 0, 0), // 9:00 AM
                    EndTime = new DateTime(2024, 11, 10, 12, 0, 0),  // 12:00 PM
                    Capacity = 20,
                    Course = course1,
                    Location = "Room 101"
                };

                var courseModule2 = new CourseModule
                {
                    Id = 2,
                    Name = "Module 2",
                    Teacher = teacher1,
                    FoodModule = null,
                    Date = DateTime.Now.AddDays(7),
                    StartTime = new DateTime(2024, 11, 11, 13, 0, 0), // 1:00 PM
                    EndTime = new DateTime(2024, 11, 11, 16, 0, 0),  // 4:00 PM
                    Capacity = 25,
                    Course = course1,
                    Location = "Room 102"
                };

                var courseModule3 = new CourseModule
                {
                    Id = 3,
                    Name = "Module 3",
                    Teacher = teacher1,
                    FoodModule = null,
                    Date = DateTime.Now.AddDays(14),
                    StartTime = new DateTime(2024, 11, 12, 10, 0, 0), // 10:00 AM
                    EndTime = new DateTime(2024, 11, 12, 13, 0, 0),  // 1:00 PM
                    Capacity = 15,
                    Course = course1,
                    Location = "Room 103"
                };


                course1.Participants.Add(participant1);
                course1.Participants.Add(participant2);
                course1.Courses.Add(courseModule1);
                course1.Courses.Add(courseModule2);
                course1.Courses.Add(courseModule3);

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
            
            var modules = new List<CourseModule>();
            var activeDays = new List<DayOfWeek>();
            
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
            
            var daysCount = activeDays.Count;
            if (daysCount == 0)
            {
                throw new InvalidOperationException("Ingen dage valgt på kursus.");
            }
            
            var modulesPerDay = courseDTO.numberOfModules / daysCount;
            var remainingModules = courseDTO.numberOfModules % daysCount;
            var currentDate = DateTime.Now;

            foreach (var activeDayDate in activeDays.Select(day => GetDayForWeekday(currentDate, day)))
            {
                for (var i = 0; i < modulesPerDay; i++)
                {
                    modules.Add(new CourseModule()
                    {
                        Name = $"Module {modules.Count + 1}",
                        Date = activeDayDate
                    });
                }
                
                if (remainingModules > 0)
                {
                    modules.Add(new CourseModule()
                    {
                        Name = $"Module {modules.Count + 1}",
                        Date = activeDayDate
                    });
                    remainingModules--;
                }
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

        // POST: api/course/update/{id} (Update course)
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
            return View(courses);
        }
        
        private static DateTime GetDayForWeekday(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysToAdd);
        }
    }
}
        
 