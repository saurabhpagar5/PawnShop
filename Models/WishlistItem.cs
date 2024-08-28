using System;
using System.Collections.Generic;

namespace PawnShop.Models;

public partial class WishlistItem
{
    public int WishlistItemId { get; set; }

    public int? WishlistId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Wishlist? Wishlist { get; set; }
}
