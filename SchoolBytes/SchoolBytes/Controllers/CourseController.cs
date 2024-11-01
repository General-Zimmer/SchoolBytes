using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {
        private List<Course> courses = new List<Course>();
        
        //Initial test data
        public void testData()
        {
            Teacher teacher1 = new Teacher { Name = "John Doe" };
            Participant participant1 = new Participant { Name = "Alice" };
            Participant participant2 = new Participant { Name = "Bob" };
            
            Course course1 = new Course(
                "Math 101",
                "An introductory course to basic math concepts",
                DateTime.Now,
                DateTime.Now.AddMonths(1),
                30
            );
            course1.Participants.Add(participant1);
            course1.Participants.Add(participant2);
            
            Course course2 = new Course(
                "Science 101",
                "A foundational course in basic scientific principles",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddMonths(1).AddDays(1),
                25
            );
            courses.Add(course1); 
            courses.Add(course2);
        }

        // POST: api/Course (Add new course)
        [HttpPost]
        public ActionResult AddCourse(Course course)
        {
            courses.Add(course);
            return View(course);
        }

		// GET: Course
        public ActionResult Index()
        {
            return View(courses);
        }

        // GET: api/Course/{id} (Get course by ID)
        [Route("course/{id}")]
        public ActionResult GetCourse(string id)
        {
            var course = courses.SingleOrDefault(c => c.id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }
            return View(course);
        }

        // POST: api/Course/{id} (Update course)
        [HttpPost]
        public ActionResult UpdateCourse(string id, Course updatedCourse)
        {
            var course = courses.SingleOrDefault(c => c.id == updatedCourse.id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }
            
            course.Name = updatedCourse.Name;
            course.Description = updatedCourse.Description;
            course.StartDate = updatedCourse.StartDate;
            course.EndDate = updatedCourse.EndDate;
            course.maxCapacity = updatedCourse.MaxCapacity;
                
            return View(course);
        }

		// DELETE: api/Course/{id} (Remove course)
        [HttpDelete]
        public ActionResult RemoveCourse(Course course)
        {
            courses.Remove(course);
            return View();
        }
    }
}