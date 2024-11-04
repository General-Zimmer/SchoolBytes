using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {
        private List<Course> courses {
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
            set
            {

            } }
        
        // GET: Course
        public ActionResult Index()
        {
            return RedirectToAction("CourseOverview");
        }

        // POST: api/course (Add new course)
        [HttpPost]
        [Route("course/create")]
        public ActionResult Create(Course course)
        {
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
    }
}
        
 