using System.Collections.Generic;
namespace PawnShop.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Product>? LatestProducts { get; set; }
        public IEnumerable<Product>? AllProducts { get; set; }

    }
}
