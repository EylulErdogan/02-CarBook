using System;
using System.Collections.Generic;

namespace carWorldDbFirstProject.Models;

public partial class Rentals
{
    public int Id { get; set; }

    public int? AracId { get; set; }

    public int? MusteriId { get; set; }

    public DateTime? BaslangicTarihi { get; set; }
    public DateTime? BitisTarihi { get; set; }
    public virtual Cars? Cars { get; set; }

    public virtual Customers? Customers { get; set; }
}
