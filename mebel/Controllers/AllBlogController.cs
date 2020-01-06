using mebel.Models;
using mebel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mebel.Controllers
{
    public class AllBlogController : Controller
    {
        MebelDB db = new MebelDB();
        // GET: AllBlog
        public ActionResult Index()
        {
            var vm = new homeModels
            {
                blogList = db.Blogs.OrderByDescending(pr => pr.Id).ToList(),
                category = db.Categories.ToList(),

            };
            return View(vm);
        }
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();
            Blog selectedBlog = db.Blogs.FirstOrDefault(bl => bl.Id == id);
            if (selectedBlog == null) return HttpNotFound();
            var vm = new homeModels
            {
                blogList = db.Blogs.OrderByDescending(bl => bl.Id).Take(5).ToList(),
                category = db.Categories.ToList(),
                blogDetail = selectedBlog
            };
            return View(vm);
        }

    }
}