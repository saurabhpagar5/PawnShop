using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PawnShop.Models;
using Razorpay.Api;

namespace PawnShop.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly PawnShopeeContext _context;

        public CartItemsController(PawnShopeeContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var pawnShopContext = _context.CartItems.Include(c => c.Cart).Include(c => c.Product);
            return View(await pawnShopContext.ToListAsync());
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Fetch the product by ID
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            // Logic to add the product to the cart
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                // If the item already exists in the cart, increment the quantity
                cartItem.Quantity += 1;
            }
            else
            {
                // Otherwise, add a new item to the cart
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    AddedDate = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to the Cart page
            return RedirectToAction("Index");
        }




        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        public IActionResult CartCheckOut()
        {
            // Retrieve all cart items along with their associated product details
            var cartItems = _context.CartItems
                                    .Include(ci => ci.Product) // Include related Product details
                                    .ToList();

            // Pass the cart items to the view
            return View(cartItems);
        }

        // GET: CartItems/Create
        //public IActionResult Create()
        //{
        //    ViewData["CartId"] = new SelectList(_context.ShoppingCarts, "CartId", "CartId");
        //    ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
        //    return View();
        //}

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartItemId,CartId,ProductId,Quantity,AddedDate")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.ShoppingCarts, "CartId", "CartId", cartItem.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.ShoppingCarts, "CartId", "CartId", cartItem.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cartItem.ProductId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartItemId,CartId,ProductId,Quantity,AddedDate")] CartItem cartItem)
        {
            if (id != cartItem.CartItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.ShoppingCarts, "CartId", "CartId", cartItem.CartId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.CartItemId == id);
        }


    }
}
