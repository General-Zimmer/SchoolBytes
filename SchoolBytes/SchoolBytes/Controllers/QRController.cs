using System.Web.Mvc;
using System;
using SchoolBytes.Models;
using QRService;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;


namespace SchoolBytes.Controllers
{
    public class QRController : Controller
    {
        readonly QRGeneratorService generatorService = new QRGeneratorService();
        [HttpGet]
        public ActionResult Index()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            string fullUrl = baseUrl + "/QR/Index";
            Bitmap bitmap = generatorService.GenerateBitmap(fullUrl);
            String base64String;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            ViewBag.Message = "The QR brought you here but i will bring you salvation.";
            ViewBag.Title = "Tjek ind";
            ViewBag.ImageData = base64String;
            return View();
        }
    }
}