﻿using System;
using System.Collections.Generic;

namespace PawnShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    public string Condition { get; set; } = null!;

    public int StockQuantity { get; set; }

    public DateTime? DateAdded { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }
}
