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
    public class AdminOurStoriesController : Controller
    {
        private MebelDB db = new MebelDB();

        // GET: mebelAdmin/AdminOurStories
        public ActionResult Index()
        {
            return View(db.OurStories.ToList());
        }

        // GET: mebelAdmin/AdminOurStories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurStory ourStory = db.OurStories.Find(id);
            if (ourStory == null)
            {
                return HttpNotFound();
            }
            return View(ourStory);
        }


        // GET: mebelAdmin/AdminOurStories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurStory ourStory = db.OurStories.Find(id);
            if (ourStory == null)
            {
                return HttpNotFound();
            }
            return View(ourStory);
        }

        // POST: mebelAdmin/AdminOurStories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Headers,Description,Photo")] OurStory ourStory,int id, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                OurStory selectedSection1 = db.OurStories.SingleOrDefault(sc => sc.Id == id);

                if (Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(selectedSection1.Photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(selectedSection1.Photo));
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
                        img.Save("~/Uploads/Section1Photo/" + fileName);
                        selectedSection1.Photo = "/Uploads/Section1Photo/" + fileName;
                    }
                }

                selectedSection1.Headers = ourStory.Headers;
                selectedSection1.Description = ourStory.Description;
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(ourStory);
        }

        // GET: mebelAdmin/AdminOurStories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OurStory ourStory = db.OurStories.Find(id);
            if (ourStory == null)
            {
                return HttpNotFound();
            }
            return View(ourStory);
        }

        //// POST: mebelAdmin/AdminOurStories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    OurStory ourStory = db.OurStories.Find(id);
        //    db.OurStories.Remove(ourStory);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
