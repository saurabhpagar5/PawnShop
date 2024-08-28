using Microsoft.AspNetCore.Mvc;
using PawnShop.Models;
using System.Linq;

namespace PawnShop.Controllers
{
    public class LoginController : Controller
    {
        private readonly PawnShopeeContext _context;

        public LoginController(PawnShopeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


      

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Check the user's role and redirect accordingly
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "AdminPanel"); // Redirect to Admin Panel
                }
                else if (user.Role == "Customer")
                {
                    return RedirectToAction("Index", "CustomerPanel"); // Redirect to Customer Panel
                }
            }
            else
            {
                // Handle invalid login
                ViewBag.ErrorMessage = "Invalid Email or Password.";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // Set the default role to "Customer"
            user.Role = "Customer";

            // Add the new user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }
    }
}
