using Microsoft.AspNetCore.Mvc;
using PawnShop.Models;
using System.Linq;

namespace PawnShop.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly PawnShopeeContext _context;

        public AdminPanelController(PawnShopeeContext context)
        {
            _context = context;
        }

        // Dashboard view
        public IActionResult Index()
        {
            return View();
        }

        // Product Management
        public IActionResult ProductManagement()
        {
            var products = _context.Products.ToList();
            var categories = _context.Categories.ToList();
            var model = new ProductManagementViewModel
            {
                Products = products,
                Categories = categories
            };
            return View(model);
        }

        // User Management
        public IActionResult UserManagement()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Order Management
        public IActionResult OrderManagement()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        // Payment Integration
        public IActionResult PaymentManagement()
        {
            // Implement payment management view
            return View();
        }

        // Promotions and Discounts
        //public IActionResult PromotionsManagement()
        //{
        //    var coupons = _context.Coupons.ToList();
        //    return View(coupons);
        //}

        // Reporting and Analytics
        public IActionResult Reporting()
        {
            // Implement reporting view
            return View();
        }

        // Content Management
        public IActionResult ContentManagement()
        {
            // Implement content management view
            return View();
        }

        // Customer Experience
        public IActionResult CustomerExperience()
        {
            // Implement customer experience view
            return View();
        }
    }
}
