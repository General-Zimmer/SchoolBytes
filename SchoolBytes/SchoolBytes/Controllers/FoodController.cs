using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SchoolBytes.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        [HttpGet]
        public ActionResult Index()
        {
            FoodModule fm = new FoodModule();


            Course c1 = new Course();
            c1.Name = "TestCourse1";

            Course c2 = new Course();
            c2.Name = "TestCourse2";



            //ViewBag.course = new SelectListItem[] { new SelectListItem { Text = c1.Name, Value = c1.Name },
            //new SelectListItem { Text = c2.Name, Value = c2.Name }};

            fm.Course = ViewBag.theCourse;

            Teacher t1 = new Teacher();
            t1.Name = "TestTeacher1";
            Teacher t2 = new Teacher();
            t2.Name = "TestTeacher2";

            ViewBag.teacher = new SelectListItem[] { new SelectListItem { Text = t1.Name, Value = t1.Name },
                    new SelectListItem { Text = t2.Name, Value = t2.Name }}; ;

            return View(model: fm);
        }

        [HttpPost]
        public ActionResult CreateFoodModule(FoodModule fm, Course course, Teacher teacher)
        {
            //TODO: Nyt view?
            return View(model: fm);
        }

        [HttpGet]
        public ActionResult SletDetHer() {
            Course c3 = new Course();
            ViewBag.theCourse = c3;

            Teacher t1 = new Teacher();
            t1.Name = "TestTeacher1";
            Teacher t2 = new Teacher();
            t2.Name = "TestTeacher2";

            ViewBag.teacher = new SelectListItem[] { new SelectListItem { Text = t1.Name, Value = t1.Name },
                    new SelectListItem { Text = t2.Name, Value = t2.Name }}; ;

            return View();
        }

        [HttpGet]
        public HttpResponseMessage ModalWindow() {

            FoodModule fm = new FoodModule();
            fm.Course = ViewBag.theCourse;

            var content = new StringContent(RenderRazorViewToString("Index", fm), Encoding.UTF8, "application/json");

            return new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.OK
            };
           
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}