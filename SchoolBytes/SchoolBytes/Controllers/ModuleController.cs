using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    public class ModuleController : Controller
    {
        // dummy data for testing purposes
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
            set
            {

            }
        }

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
    }
}