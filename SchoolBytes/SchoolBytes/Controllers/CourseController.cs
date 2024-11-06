using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace SchoolBytes.Controllers
{
    public class CourseController : Controller
    {
        private List<Course> courses = new List<Course>();

        //Initial test data
        public void TestData()
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
            courses.Add(course1);
            courses.Add(course2);
        }
        
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        // POST: api/Course (Add new course)
        [HttpPost]
        public ActionResult CreateCourse(Course course)
        {
            Debug.Print("TEEEEEEEEEEEEEEEST" +  ((FoodModule)Session["fm"]).Name);
            Debug.Print("Test tempdata" + ((FoodModule)TempData["fm2"]).Name);

         
            courses.Add(course);

            TestData();
            ViewBag.SelectedCourseId = 2;
            return View("CourseOverview", courses);
        }

        // GET: api/Course/{id} (Get course by ID)
        [Route("course/{id}")]
        public ActionResult GetCourse(int id)
        {
            var course = courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            return View(course);
        }

        // POST: api/Course/{id} (Update course)
        [HttpPost]
        public ActionResult UpdateCourse(int id, Course updatedCourse)
        {
            var course = courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }

            if (ModelState.IsValid)
            {
                    course.Name = updatedCourse.Name;
                    course.Description = updatedCourse.Description;
                    course.StartDate = updatedCourse.StartDate;
                    course.EndDate = updatedCourse.EndDate;
                    course.MaxCapacity = updatedCourse.MaxCapacity;
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // DELETE: api/Course/{id} (Remove course)
        [HttpDelete]
        public ActionResult DeleteCourse(int id)
        {
            var course = courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }
            courses.Remove(course);
            return RedirectToAction("Index");
        }

        public ActionResult CourseOverview(int? selectedCourseId = null)
        {
            TestData();
            ViewBag.SelectedCourseId = selectedCourseId;
            return View(courses);
        }

        [HttpPost]
        public ActionResult CreateFoodModule()
        {

            Debug.Print("TEEEEEEEEEST");
            Debug.Print(((FoodModule)Session["fm"]).Teacher.Name);

            Session["created"] = "true";

            //TODO: Gem modulet i session et sted
            return View("Index");
        }
    }
}
        
 