using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mebel.Models;
using System.Web.Helpers;
using System.IO;

namespace mebel.Areas.mebelAdmin.Controllers
{
    [AuthenticationFilter]
    public class AdminBlogsController : Controller
    {
        private MebelDB db = new MebelDB();

        // GET: mebelAdmin/AdminBlogs
        public async Task<ActionResult> Index()
        {
            return View(await db.Blogs.ToListAsync());
        }

        // GET: mebelAdmin/AdminBlogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: mebelAdmin/AdminBlogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mebelAdmin/AdminBlogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Header,Description,Photo,CreateDate")] Blog blog,HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    if (Image.ContentType.ToLower() == "image/jpeg"
                        || Image.ContentType.ToLower() == "image/png"
                        || Image.ContentType.ToLower() == "image/gif"
                        || Image.ContentType.ToLower() == "image/jpg"

                        )
                    {

                        WebImage img = new WebImage(Image.InputStream);
                        FileInfo newFile = new FileInfo(Image.FileName);
                        string fileName = Guid.NewGuid().ToString() + newFile.Extension;
                        img.Save("~/Uploads/BlogImage/" + fileName);
                        blog.Photo = "/Uploads/BlogImage/" + fileName;
                    }
                }
                blog.CreateDate = DateTime.Now;
                db.Blogs.Add(blog);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: mebelAdmin/AdminBlogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: mebelAdmin/AdminBlogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Header,Description,Photo,CreateDate")] Blog blog,HttpPostedFileBase Image,int id)
        {
            if (ModelState.IsValid)
            {
                Blog selectedBlog= db.Blogs.SingleOrDefault(sc => sc.Id == id);

                if (Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(selectedBlog.Photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(selectedBlog.Photo));
                    }
                    if (Image.ContentType.ToLower() == "image/jpeg"
                        || Image.ContentType.ToLower() == "image/png"
                        || Image.ContentType.ToLower() == "image/gif"
                        || Image.ContentType.ToLower() == "image/jpg"

                        )
                    {

                        WebImage img = new WebImage(Image.InputStream);
                        FileInfo newFile = new FileInfo(Image.FileName);
                        string fileName = Guid.NewGuid().ToString() + newFile.Extension;
                        img.Save("~/Uploads/BlogImage/" + fileName);
                        selectedBlog.Photo = "/Uploads/BlogImage/" + fileName;
                    }
                }

                selectedBlog.Header = blog.Header;
                selectedBlog.Description = blog.Description;
                selectedBlog.CreateDate = DateTime.Now;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: mebelAdmin/AdminBlogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: mebelAdmin/AdminBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Blog blog = await db.Blogs.FindAsync(id);
            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
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
