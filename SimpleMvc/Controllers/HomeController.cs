using SimpleMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleMvc.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ICalculator calculator, IEmailService emailService)
        {
            // Example use of injected dependencies.
            var result = calculator.Add(1, 2);
            emailService.Send("test@email.com", "testing");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}