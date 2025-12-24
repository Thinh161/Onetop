using Microsoft.AspNetCore.Mvc;
using OneTop.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace OneTop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly ClothingStoreContext context;

        public UserController(ClothingStoreContext context)
        {
            this.context = context;
        }

        public IActionResult UserManagement()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("UserManagement");
            }

            using var transaction = context.Database.BeginTransaction();
            try
            {
                
                context.Database.ExecuteSqlRaw(
                    "DELETE CI FROM CartItems CI INNER JOIN ShoppingCart SC ON CI.CartID = SC.CartID WHERE SC.UserID = {0}",
                    id);
               
                context.Database.ExecuteSqlRaw("DELETE FROM ShoppingCart WHERE UserID = {0}", id);

                context.Users.Remove(user);
                context.SaveChanges();

                transaction.Commit();
                TempData["Message"] = "User deleted.";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["Error"] = "Unable to delete user: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction("UserManagement");
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User model)
        {
            if (ModelState.IsValid)
            {
                // ensure username unique
                var existing = context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existing != null)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                model.Role = model.Role ?? "Customer";
                context.Users.Add(model);
                context.SaveChanges();
                return RedirectToAction("UserManagement");
            }

            return View(model);
        }
    }
}
