using Microsoft.AspNetCore.Mvc;
using OneTop.Extensions;
using OneTop.Models;

namespace OneTop.Controllers
{
    public class CartController : Controller
    {
        private readonly ClothingStoreContext ctx;

        public CartController(ClothingStoreContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            List<CartItemModel> list =
                HttpContext.Session.GetObject<List<CartItemModel>>("cart");

            if (list == null)
            {
                list = new List<CartItemModel>();
            }

            CartModel cart = new CartModel();
            cart.items = list;

            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = ctx.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index", "Home");
            }

            CartItemModel item = new CartItemModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            };

            List<CartItemModel> list =
                HttpContext.Session.GetObject<List<CartItemModel>>("cart") ?? new();

            CartModel cart = new CartModel();
            cart.items = list;
            cart.Add(item);

            HttpContext.Session.SetObject("cart", cart.getAllItems());

            TempData["Message"] = "Đã thêm vào giỏ hàng thành công";

            return RedirectToAction("Index", "Home");
        }



        public IActionResult Increase(int id)
        {
            List<CartItemModel> list =
                HttpContext.Session.GetObject<List<CartItemModel>>("cart");

            if (list != null)
            {
                var item = list.FirstOrDefault(x => x.ProductId == id);
                if (item != null)
                {
                    item.Quantity++;
                }

                HttpContext.Session.SetObject("cart", list);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            List<CartItemModel> list =
                HttpContext.Session.GetObject<List<CartItemModel>>("cart");

            if (list != null)
            {
                var item = list.FirstOrDefault(x => x.ProductId == id);
                if (item != null)
                {
                    item.Quantity--;

                    if (item.Quantity <= 0)
                    {
                        list.Remove(item);
                    }
                }

                HttpContext.Session.SetObject("cart", list);
            }

            return RedirectToAction("Index");
        }


    }
}
