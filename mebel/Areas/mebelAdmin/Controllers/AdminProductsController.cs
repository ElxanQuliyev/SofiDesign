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
    public class AdminProductsController : Controller
    {
        // GET: mebelAdmin/AdminProducts
        MebelDB db = new MebelDB();
        public ActionResult Index()
        {

            return View(db.Products.OrderByDescending(pr=>pr.Id).ToList());
        }
        
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.LanguageId = new SelectList(db.LanguageTBs, "Id", "CultureName");

            ViewBag.MaifacturerId = new SelectList(db.Maifacturers, "Id", "Name");
            ViewBag.MaterialId= new SelectList(db.Materials, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase Image)
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
                    img.Save("~/Uploads/ProductPhoto/" + fileName);
                    product.Photo = "/Uploads/ProductPhoto/" + fileName;
                }
            }

            db.Products.Add(product);
            db.SaveChanges();
    

           
         
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name",product.CategoryId);
            ViewBag.LanguageId = new SelectList(db.LanguageTBs, "Id", "CultureName", product.LanguageId);

            ViewBag.MaifacturerId = new SelectList(db.Maifacturers, "Id", "Name",product.MaifacturerId);
            ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name",product.MaterialId);

            return RedirectToAction("index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)   return HttpNotFound();
            
            Product selectedPro = db.Products.FirstOrDefault(pr => pr.Id == id);
           
            if (selectedPro == null) return HttpNotFound();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.LanguageId = new SelectList(db.LanguageTBs, "Id", "CultureName");

            ViewBag.MaifacturerId = new SelectList(db.Maifacturers, "Id", "Name");
            ViewBag.MaterialId = new SelectList(db.Materials, "Id", "Name");
            return View(selectedPro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product,int id,HttpPostedFileBase Image)
        {
            Product selectedPro = db.Products.SingleOrDefault(sc => sc.Id == id);
    
            if (Image != null)
            {
                if (System.IO.File.Exists(Server.MapPath(selectedPro.Photo)))
                {
                    System.IO.File.Delete(Server.MapPath(selectedPro.Photo));
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
                    img.Save("~/Uploads/ProductPhoto/" + fileName);
                    selectedPro.Photo = "/Uploads/ProductPhoto/" + fileName;
                }
            }
            selectedPro.Name = product.Name;
            selectedPro.Price = product.Price;
            selectedPro.CategoryId = product.CategoryId;
            selectedPro.MaifacturerId = product.MaifacturerId;
            selectedPro.LanguageId = product.LanguageId;
            selectedPro.color = product.color;
            selectedPro.Size = product.Size;
            selectedPro.Description = product.Description;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AddImages(int? id)
        {
            if (id == null) return HttpNotFound();
            Product selectPro = db.Products.FirstOrDefault(pr => pr.Id == id);
            if (selectPro == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            return View(selectPro);
        }
        [HttpPost]
        public ActionResult UploadsProductImages(int? id, HttpPostedFileBase[] productpictures)
        {


            foreach (var pp in productpictures)
            {

                WebImage image = new WebImage(pp.InputStream);
                FileInfo photoInfo = new FileInfo(pp.FileName);
                string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                ProductImage proPicture = new ProductImage();

                image.Save("~/Uploads/ProductPhoto/" + newPhoto);
                proPicture.Photo= "/Uploads/ProductPhoto/" + newPhoto;

                proPicture.ProductId = (int)id;
                db.ProductImages.Add(proPicture);
                db.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult GetProductImages(int? id)
        {
            if (id == null) return HttpNotFound();

            List<ProductImage> proImage = db.ProductImages.Where(pr => pr.ProductId == id).ToList();
            return PartialView("_ProductPict", proImage);


        }
        public ActionResult RemoveProductPicture(int? id)
        {
            if (id == null) return HttpNotFound();
            db.ProductImages.Remove(db.ProductImages.Find(id));
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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