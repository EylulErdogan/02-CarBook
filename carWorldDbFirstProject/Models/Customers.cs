using System;
using System.Collections.Generic;

namespace carWorldDbFirstProject.Models;

public partial class Customers
{
    public int Id { get; set; }

    public string? AdSoyad { get; set; }

    public string? Telefon { get; set; }

    public virtual ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();
}
