using OneTop.Models;
using Microsoft.AspNetCore.Mvc;

namespace OneTop.Controllers
{
    public class ProductsController : Controller
    {
        private ClothingStoreContext context;
        public ProductsController(ClothingStoreContext context)
        {
            this.context = context;
        }
        public IActionResult AllProduct()
        {
            List<Product> products = context.Products.ToList();
            return View(products);
        }
        public IActionResult Shirts()
        {
            List<Product> shirts = context.Products.Where(p => p.CategoryName == "Shirt").ToList();
            return View(shirts);
        }

        public IActionResult Pants()
        {
            List<Product> Pants = context.Products.Where(p => p.CategoryName == "Pants").ToList();
            return View(Pants);
        }

        public IActionResult Accessories()
        {
            List<Product> Accessories = context.Products.Where(p => p.CategoryName == "Accessories").ToList();
            return View(Accessories);
        }

        [HttpPost]
        public IActionResult SearchByName()
        {
            string name = Request.Form["txtKeywords"].ToString();
            List<Product> categories = context.Products.Where(x => x.ProductName.Contains(name)).ToList();
            return View(categories);
        }

        public IActionResult Detail(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
