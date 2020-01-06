using mebel.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mebel.ViewModels
{
    public class homeModels
    {
        public IEnumerable<section1Divs> section1 { get; set; }
        public IPagedList<Product>products { get; set; }
        public IEnumerable<ProductImage> proPhoto{ get; set; }
        public IEnumerable<Category> category{ get; set; }
        public  OurStory ourstroy { get; set; }
        public Product productDetail { get; set; }
        public IEnumerable<ProductImage> productImage{ get; set; }
        public Blog blogDetail{ get; set; }
        public IEnumerable<Blog> blogList{ get; set; }


    }
}