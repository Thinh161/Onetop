using Microsoft.AspNetCore.Mvc;
using OneTop.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OneTop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly ClothingStoreContext context;

        public OrderController(ClothingStoreContext context)
        {
            this.context = context;
        }

        public IActionResult OrderManagement()
        {
            var orders = context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId != null)
                .ToList();

            return View(orders);
        }
    }
}
