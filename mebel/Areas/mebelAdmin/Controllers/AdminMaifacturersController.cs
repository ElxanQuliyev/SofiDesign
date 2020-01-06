using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mebel.Models;

namespace mebel.Areas.mebelAdmin.Controllers
{

    [AuthenticationFilter]
    public class AdminMaifacturersController : Controller
    {
        private MebelDB db = new MebelDB();

        // GET: mebelAdmin/AdminMaifacturers
        public ActionResult Index()
        {
            return View(db.Maifacturers.ToList());
        }

        // GET: mebelAdmin/AdminMaifacturers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maifacturer maifacturer = db.Maifacturers.Find(id);
            if (maifacturer == null)
            {
                return HttpNotFound();
            }
            return View(maifacturer);
        }

        // GET: mebelAdmin/AdminMaifacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mebelAdmin/AdminMaifacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Maifacturer maifacturer)
        {
            if (ModelState.IsValid)
            {
                db.Maifacturers.Add(maifacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maifacturer);
        }

        // GET: mebelAdmin/AdminMaifacturers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maifacturer maifacturer = db.Maifacturers.Find(id);
            if (maifacturer == null)
            {
                return HttpNotFound();
            }
            return View(maifacturer);
        }

        // POST: mebelAdmin/AdminMaifacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Maifacturer maifacturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maifacturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maifacturer);
        }

        // GET: mebelAdmin/AdminMaifacturers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maifacturer maifacturer = db.Maifacturers.Find(id);
            if (maifacturer == null)
            {
                return HttpNotFound();
            }
            return View(maifacturer);
        }

        // POST: mebelAdmin/AdminMaifacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maifacturer maifacturer = db.Maifacturers.Find(id);
            db.Maifacturers.Remove(maifacturer);
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
