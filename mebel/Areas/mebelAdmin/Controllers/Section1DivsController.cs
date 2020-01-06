using mebel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace mebel.Areas.mebelAdmin.Controllers
{

    [AuthenticationFilter]
    public class Section1DivsController : Controller
    {
        MebelDB db = new MebelDB();
        // GET: mebelAdmin/Section1Divs
        public ActionResult Index()
        {
            return View(db.section1Divs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(section1Divs section1, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                if (Image.ContentType.ToLower() == "image/jpeg"
                    || Image.ContentType.ToLower()=="image/png"
                    || Image.ContentType.ToLower()=="Image/gif"
                    || Image.ContentType.ToLower()=="Image/jpg"

                    )
                {

                WebImage img = new WebImage(Image.InputStream);
                FileInfo newFile = new FileInfo(Image.FileName);
                string fileName = Guid.NewGuid().ToString() + newFile.Extension;
                img.Save("~/Uploads/Section1Photo/" + fileName);
                section1.Photo = "/Uploads/Section1Photo/" + fileName;
                }
            }
            db.section1Divs.Add(section1);
            db.SaveChanges();
             
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) { return HttpNotFound(); }
            section1Divs selectedSection1 = db.section1Divs.FirstOrDefault(sc => sc.Id == id);
            if (selectedSection1 == null) { return HttpNotFound(); }
            
            return View(selectedSection1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id,section1Divs section1,HttpPostedFileBase Image)
        {
            section1Divs selectedSection1 = db.section1Divs.SingleOrDefault(sc => sc.Id == id);

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

            selectedSection1.Header = section1.Header;
            selectedSection1.Descripiton = section1.Descripiton;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null) { return HttpNotFound(); }
            section1Divs selectedSection1 = db.section1Divs.FirstOrDefault(sc => sc.Id == id);
            if (selectedSection1 == null) { return HttpNotFound(); }
            return View(selectedSection1);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            section1Divs brend = db.section1Divs.Find(id);
            if (brend == null)
            {
                return HttpNotFound();
            }
            return View(brend);
        }

        // POST: Admin/Brends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            section1Divs brend = db.section1Divs.Find(id);
            db.section1Divs.Remove(brend);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}