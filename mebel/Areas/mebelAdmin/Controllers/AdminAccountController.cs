using mebel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mebel.Areas.mebelAdmin.Controllers
{
    public class AdminAccountController : Controller
    {
        MebelDB db = new MebelDB();
            // GET: mebelAdmin/AdminAccount
        public ActionResult Login(string Email,string Password)
        {
            if(Email!="" && Password != "")
            {
                Setting selectedAdmin = db.Settings.FirstOrDefault(adm => adm.AdminEmail == Email);
                if (selectedAdmin != null)
                {
                    if (selectedAdmin.AdminPassword == Password)
                    {
                        Session["AdminLogged"] = selectedAdmin;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session["activeAdmin"] = null;
            return RedirectToAction("Login", "AdminAccount");
        }
    }
}