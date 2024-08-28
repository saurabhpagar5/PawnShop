using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawnShop.Models;
using System.Linq;

namespace PawnShop.Controllers
{
    public class CustomerPanelController : Controller
    {
        private readonly PawnShopeeContext _context;

        public CustomerPanelController(PawnShopeeContext context)
        {
            _context = context;
        }

        // Dashboard view
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            string? userEmail = User?.Identity?.Name; // Replace with actual user email retrieval logic
            var latestProducts = _context.Products
                                      .OrderByDescending(p => p.DateAdded)
                                      .Take(12) // 4 rows, assuming 3 products per row
                                      .ToList();
            var allProducts = _context.Products.ToList();

            var model = new CustomerPanelViewModel
            {
                Products = products,
                User = _context.Users.FirstOrDefault(u => u.Email == userEmail),
                LatestProducts = latestProducts,
                AllProducts = allProducts
            };

            return View(model);
        }

        // Customer Profile
        //public IActionResult Profile()
        //{
        //    string? userEmail = User?.Identity?.Name; // Replace with actual user email retrieval logic
        //    var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        //    return View(user);
        //}
        public IActionResult Profile()
        {
            var userEmail = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Login"); // Redirect to login if email is not found
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // Add to Cart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            string? userEmail = User?.Identity?.Name; // Replace with actual user email retrieval logic
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (user == null || product == null || quantity <= 0)
                return BadRequest("Invalid data");

            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.UserId == user.UserId)
                       ?? new ShoppingCart { UserId = user.UserId, CreatedDate = DateTime.Now };

            if (cart.CartId == 0)
            {
                _context.ShoppingCarts.Add(cart);
                _context.SaveChanges();
            }

            var cartItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                CartId = cart.CartId
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ProductsByCategory(int categoryId)
        {
            var products = _context.Products
                                   .Where(p => p.CategoryId == categoryId)
                                   .ToList();

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);

            var model = new CustomerPanelViewModel
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

        // Add to Wishlist
        //[HttpPost]
        //public IActionResult AddToWishlist(int productId)
        //{
        //    string? userEmail = User?.Identity?.Name; // Replace with actual user email retrieval logic
        //    var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

        //    if (user == null || product == null)
        //        return BadRequest("Invalid data");

        //    var wishlist = _context.Wishlists.FirstOrDefault(w => w.UserId == user.UserId)
        //                   ?? new Wishlist { UserId = user.UserId };

        //    if (wishlist.WishlistId == 0)
        //    {
        //        _context.Wishlists.Add(wishlist);
        //        _context.SaveChanges();
        //    }

        //    var wishlistItem = new WishlistItem
        //    {
        //        ProductId = productId,
        //        WishlistId = wishlist.WishlistId,
        //        AddedDate = DateTime.Now
        //    };

        //    _context.WishlistItems.Add(wishlistItem);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}
    }
}
//using Microsoft.AspNetCore.Mvc;
//using PawnShop.Models;
//using System.Linq;

//namespace PawnShop.Controllers
//{
//    public class CustomerPanelController : Controller
//    {
//        private readonly PawnShopeeContext _context;

//        public CustomerPanelController(PawnShopeeContext context)
//        {
//            _context = context;
//        }

//        // Index Page
//        public IActionResult Index()
//        {
//            var userEmail = User?.Identity?.Name;
//            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

//            if (user == null)
//            {
//                return RedirectToAction("Login", "Account");
//            }

//            var products = _context.Products.ToList();
//            var model = new CustomerPanelViewModel
//            {
//                Products = products
//            };

//            return View(model);
//        }

//        [HttpPost]
//        public IActionResult AddToCart(int productId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var userEmail = User.Identity.Name;
//                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

//                if (user == null)
//                {
//                    return RedirectToAction("Index");
//                }

//                var product = _context.Products.Find(productId);

//                if (product == null)
//                {
//                    return RedirectToAction("Index");
//                }

//                var cart = _context.ShoppingCarts.FirstOrDefault(c => c.UserId == user.UserId);

//                if (cart == null)
//                {
//                    cart = new ShoppingCart
//                    {
//                        UserId = user.UserId,
//                        CreatedDate = DateTime.Now
//                    };
//                    _context.ShoppingCarts.Add(cart);
//                    _context.SaveChanges();
//                }

//                var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == productId);

//                if (cartItem == null)
//                {
//                    cartItem = new CartItem
//                    {
//                        CartId = cart.CartId,
//                        ProductId = productId,
//                        Quantity = 1,
//                        AddedDate = DateTime.Now
//                    };
//                    _context.CartItems.Add(cartItem);
//                }
//                else
//                {
//                    cartItem.Quantity++;
//                }

//                _context.SaveChanges();

//                return RedirectToAction("Index");
//            }
//            else
//            {
//                return RedirectToAction("Login", "Account");
//            }
//        }

//        [HttpPost]
//        public IActionResult AddToWishlist(int productId)
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                var userEmail = User.Identity.Name;
//                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

//                if (user == null)
//                {
//                    return RedirectToAction("Index");
//                }

//                var product = _context.Products.Find(productId);

//                if (product == null)
//                {
//                    return RedirectToAction("Index");
//                }

//                var wishlist = _context.Wishlists.FirstOrDefault(w => w.UserId == user.UserId);

//                if (wishlist == null)
//                {
//                    wishlist = new Wishlist
//                    {
//                        UserId = user.UserId
//                    };
//                    _context.Wishlists.Add(wishlist);
//                    _context.SaveChanges();
//                }

//                var wishlistItem = _context.WishlistItems.FirstOrDefault(wi => wi.WishlistId == wishlist.WishlistId && wi.ProductId == productId);

//                if (wishlistItem == null)
//                {
//                    wishlistItem = new WishlistItem
//                    {
//                        WishlistId = wishlist.WishlistId,
//                        ProductId = productId,
//                        AddedDate = DateTime.Now
//                    };
//                    _context.WishlistItems.Add(wishlistItem);
//                    _context.SaveChanges();
//                }

//                return RedirectToAction("Index");
//            }
//            else
//            {
//                return RedirectToAction("Login", "Account");
//            }
//        }
//    }
//}

