using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace mebel.Controllers
{
    public class MainLanguageController : Controller
    {
        // GET: MainLanguage
        public ActionResult Language(string langAttr)
        {
            if (langAttr != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langAttr);
                Thread.CurrentThread.CurrentUICulture= new CultureInfo(langAttr);
            }
            else
            {   
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("az");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("az");
            }
            Response.Cookies.Add(new HttpCookie("samir")
            {
                Value = langAttr
            });
            return RedirectToAction("Index","Home");
        }
    }
}