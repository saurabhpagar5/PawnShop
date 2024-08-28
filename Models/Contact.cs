using System;
using System.Collections.Generic;

namespace PawnShop.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int? UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? ContactDate { get; set; }

    public virtual User? User { get; set; }
}
