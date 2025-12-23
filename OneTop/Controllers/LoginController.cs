using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneTop.Models;

namespace OneTop.Controllers
{
    public class LoginController : Controller
    {
        private readonly ClothingStoreContext context;

        public LoginController(ClothingStoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = context.Users
                    .FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already in use.");
                    return View(model);
                }
                var user = new User
                {
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    FullName = model.FullName,
                    Role = "Customer"
                };
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (existingUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", existingUser.UserId);
                    HttpContext.Session.SetString("FullName", existingUser.FullName ?? "");
                    HttpContext.Session.SetString("Role", existingUser.Role ?? "User");
                    if (existingUser.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Home", "Home", new { area = "Account" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
                return View("~/Views/Login/Login.cshtml", model);
            }
            return View();
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}


