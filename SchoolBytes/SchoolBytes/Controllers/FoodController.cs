using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class FoodController : Controller
    {
        // GET: Food
        [HttpGet]
        public ActionResult Index()
        {
            FoodModule fm = new FoodModule();

            return View(model: fm);
        }

        [HttpPost]
        public ActionResult CreateFoodModule()
        {
            
            Debug.Print("TEEEEEEEEEST");
            Debug.Print(((FoodModule)Session["fm"]).Teacher.Name);

            Session["created"] = "true";

            //TODO: Gem modulet i session et sted
            return View("SletDetHer");
        }

        [HttpGet]
        public ActionResult SletDetHer() {
            ViewBag.created = Session["created"];

            return View();
        }

        [HttpGet]
        public ActionResult ModalWindow() {

            FoodModule fm = new FoodModule();

            fm.Teacher = new Teacher();
            fm.Teacher.Name = "HENNY TEACHER";

            //fm.Course = new Course();
            fm.Course.Name = "HENNY COURSE";

            Session["fm"] = fm;

            var htmlContent = RenderRazorViewToString("Index", fm);

            return Content(htmlContent, "text/html", Encoding.UTF8);
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