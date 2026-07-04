using System;
using System.Collections.Generic;

namespace carWorldDbFirstProject.Models;

public partial class Cars
{
    public int Id { get; set; }

    public string? Marka { get; set; }

    public string? Model { get; set; }

    public decimal? Fiyat { get; set; }
    public string? ImageUrl { get; set; }
    public virtual ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();
}
