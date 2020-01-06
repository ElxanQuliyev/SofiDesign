using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mebel.Areas.mebelAdmin.Controllers
{
    public class HomeController : Controller
    {
        // GET: mebelAdmin/Home
        [AuthenticationFilter]
        public ActionResult Index()
        {
         
                return View();
        }
    }
}