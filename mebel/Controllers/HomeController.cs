using mebel.Models;
using mebel.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mebel.Controllers
{
    public class HomeController : Controller
    {
        MebelDB db = new MebelDB();
        public ActionResult Index(int Page=1)
        {
           
            var vm = new homeModels
            {
                section1 = db.section1Divs.ToList(),
                products = db.Products.Where(pr=>pr.LanguageTB.CultureCode==MainLanguage.lb).OrderByDescending(pr => pr.Id).ToPagedList(Page, 15),
                proPhoto = db.ProductImages.ToList(),
                category = db.Categories.ToList(),
                ourstroy=db.OurStories.FirstOrDefault(),
                blogList=db.Blogs.OrderByDescending(bl=>bl.Id).Take(3).ToList()
            };
            
            
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var vm = new homeModels
            {
                ourstroy = db.OurStories.FirstOrDefault(),

                category = db.Categories.ToList(),
            };
            return View(vm);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}