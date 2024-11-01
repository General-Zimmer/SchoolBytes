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

        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        public void testData()
        {
            //Test kodning
            Teacher teacher1 = new Teacher { Name = "John Doe" };
            Participant participant1 = new Participant { Name = "Alice" };
            Participant participant2 = new Participant { Name = "Bob" };

            // Creating the first course
            Course course1 = new Course(
                "Math 101",
                "An introductory course to basic math concepts",
                DateTime.Now,
                DateTime.Now.AddMonths(1),
                30
            );
            course1.Participants.Add(participant1);
            course1.Participants.Add(participant2);

            // Creating the second course
            Course course2 = new Course(
                "Science 101",
                "A foundational course in basic scientific principles",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddMonths(1).AddDays(1),
                25
            );

            // Create a list of courses
            courses.Add(course1); 
            courses.Add(course2);
        }


        //CRUD METODER



        [HttpPost]
        public ActionResult addCourse(Course course)
        {
            courses.Add(course);

            return View(course);
        }

        [HttpDelete]
        public ActionResult removeCourse(Course course)
        {
            courses.Remove(course);

            return View();
        }

        [Route("hold/{id}")]
        public ActionResult getCourse(string id)
        {
            var course = courses.SingleOrDefault(c => c.id == id);

            return View(course);
        }

        [HttpPost]
        public ActionResult updateCourse(string id, Course updatedCourse)
        {
            //FIND EKSISTERENDE HOLD
            var course = courses.SingleOrDefault(c => c.id == updatedCourse.id);

            if (course != null)
            {
                course.Name = updatedCourse.Name;
                course.Description = updatedCourse.Description;
                course.StartDate = updatedCourse.StartDate;
                course.EndDate = updatedCourse.EndDate;
                course.maxCapacity = updatedCourse.MaxCapacity;
            } 

            return View(course);
        }

    }
}