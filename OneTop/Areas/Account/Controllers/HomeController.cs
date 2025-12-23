using Microsoft.AspNetCore.Mvc;
using OneTop.Models;
using System.Linq;

namespace OneTop.Areas.Account.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ClothingStoreContext context;

        public HomeController(ClothingStoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Home()
        {
            var products = context.Products.ToList();
            return View(products);
        }
    }
}
