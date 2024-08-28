using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PawnShop.Models;

namespace PawnShop.Views
{
    public class IndexModel : PageModel
    {
        private readonly PawnShop.Models.PawnShopeeContext _context;

        public IndexModel(PawnShop.Models.PawnShopeeContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.Category).ToListAsync();
        }
    }
}
