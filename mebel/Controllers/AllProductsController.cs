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
    public class AllProductsController : Controller
    {
        MebelDB db = new MebelDB();
        // GET: AllProducts
        public ActionResult Index(int? catId, int Page = 1)
        {
            IPagedList<Product> selectedPro;
            if (catId == null)
            {
                selectedPro = db.Products.OrderByDescending(pr => pr.Id).ToPagedList(Page,9 ) ;
            
            }
            else
            {
            selectedPro= db.Products.Where(pr => pr.CategoryId==catId).OrderByDescending(pr => pr.Id).ToPagedList(Page,9);

            }
            var vm = new homeModels
            {
                category = db.Categories.ToList(),
                products = selectedPro,
                proPhoto = db.ProductImages.ToList(),

            };
            return View(vm);
        }
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();
            Product selectedPro = db.Products.FirstOrDefault(pr => pr.Id == id);
            if (selectedPro == null) return HttpNotFound();
            var vm = new homeModels
            {
                category = db.Categories.ToList(),
                productDetail = selectedPro,
                productImage = db.ProductImages.Where(pr => pr.ProductId == selectedPro.Id).ToList()
            };
            return View(vm);
        }
    }
}