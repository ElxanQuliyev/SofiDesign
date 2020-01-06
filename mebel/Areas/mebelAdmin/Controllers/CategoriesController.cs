using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using mebel.Models;

namespace mebel.Areas.mebelAdmin.Controllers
{
    [AuthenticationFilter]
    public class CategoriesController : Controller
    {
        private MebelDB db = new MebelDB();

        // GET: mebelAdmin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: mebelAdmin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: mebelAdmin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mebelAdmin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Icons")] Category category,HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    if (Image.ContentType.ToLower() == "image/jpeg"
                        || Image.ContentType.ToLower() == "image/png"
                        || Image.ContentType.ToLower() == "Image/gif"
                        || Image.ContentType.ToLower() == "Image/jpg"

                        )
                    {

                        WebImage img = new WebImage(Image.InputStream);
                        FileInfo newFile = new FileInfo(Image.FileName);
                        string fileName = Guid.NewGuid().ToString() + newFile.Extension;
                        img.Save("~/Uploads/CategoryPhoto/" + fileName);
                        category.Icons= "/Uploads/CategoryPhoto/" + fileName;
                    }
                }

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: mebelAdmin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: mebelAdmin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Icons")] Category category,HttpPostedFileBase Image,int id)
        {
            if (ModelState.IsValid)
            {
                Category selectedSection1 = db.Categories.SingleOrDefault(sc => sc.Id == id);

                if (Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(selectedSection1.Icons)))
                    {
                        System.IO.File.Delete(Server.MapPath(selectedSection1.Icons));
                    }
                    if (Image.ContentType.ToLower() == "image/jpeg"
                        || Image.ContentType.ToLower() == "image/png"
                        || Image.ContentType.ToLower() == "Image/gif"
                        || Image.ContentType.ToLower() == "Image/jpg"
                        )
                    {
                        WebImage img = new WebImage(Image.InputStream);
                        FileInfo newFile = new FileInfo(Image.FileName);
                        string fileName = Guid.NewGuid().ToString() + newFile.Extension;
                        img.Save("~/Uploads/CategoryPhoto/" + fileName);
                        selectedSection1.Icons = "/Uploads/CategoryPhoto/" + fileName;
                    }
                }

                selectedSection1.Name = category.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: mebelAdmin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: mebelAdmin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
