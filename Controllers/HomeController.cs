using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawnShop.Models;
using System.Diagnostics;

namespace PawnShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly PawnShopeeContext _context;

        public HomeController(PawnShopeeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var latestProducts = _context.Products
                                         .OrderByDescending(p => p.DateAdded)
                                         .Take(12) // 4 rows, assuming 3 products per row
                                         .ToList();
            var allProducts = _context.Products.ToList();

            var model = new HomeViewModel
            {
                Categories = categories,
                LatestProducts = latestProducts,
                AllProducts = allProducts
            };

            return View(model);
        }
        public IActionResult ProductsByCategory(int categoryId)
        {
            var products = _context.Products
                                   .Where(p => p.CategoryId == categoryId)
                                   .ToList();

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);

            var model = new HomeViewModel
            {
                Categories = _context.Categories.ToList(),
                AllProducts = products
            };

            ViewBag.CategoryName = category?.CategoryName;

            return View(model);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
