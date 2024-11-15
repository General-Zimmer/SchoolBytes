using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolBytes.Models;

namespace SchoolBytes.Controllers
{
    
    public class ParticipantController : Controller
    {
        DBConnection dbConnection = DBConnection.getDBContext();

        [HttpGet]
        [Route("participant")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Create()
        {
            return View();
        }

        
    }
}