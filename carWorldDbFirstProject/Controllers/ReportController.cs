using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ReportController : Controller
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult MostRentedCars()
    {
        var values = _context.Rentals
            .Include(x => x.Cars)
            .GroupBy(x => new
            {
                x.Cars.Id,
                x.Cars.Marka,
                x.Cars.Model
            })
            .Select(x => new
            {
                Marka = x.Key.Marka,
                Model = x.Key.Model,
                RentalCount = x.Count()
            })
            .OrderByDescending(x => x.RentalCount)
            .ToList();

        return View(values);
    }

    public IActionResult MostRentingCustomers()
    {
        var values = _context.Rentals
            .Include(x => x.Customers)
            .GroupBy(x => new
            {
                x.Customers.Id,
                x.Customers.AdSoyad,
                x.Customers.Telefon
            })
            .Select(x => new
            {
                AdSoyad = x.Key.AdSoyad,
                Telefon = x.Key.Telefon,
                RentalCount = x.Count()
            })
            .OrderByDescending(x => x.RentalCount)
            .ToList();

        return View(values);
    }

    public IActionResult ActiveRentals()
    {
        var today = DateTime.Today;

        var values = _context.Rentals
            .Include(x => x.Cars)
            .Include(x => x.Customers)
            .Where(x => x.BaslangicTarihi <= today && x.BitisTarihi >= today)
            .ToList();

        return View(values);
    }

    public IActionResult MonthlyRentals()
    {
        var today = DateTime.Today;

        var values = _context.Rentals
            .Include(x => x.Cars)
            .Include(x => x.Customers)
            .Where(x => x.BaslangicTarihi.Value.Month == today.Month &&
                        x.BaslangicTarihi.Value.Year == today.Year)
            .ToList();

        return View(values);
    }
    public IActionResult MonthlyIncome()
    {
        var today = DateTime.Today;

        decimal total = _context.Rentals
            .Include(x => x.Cars)
            .Where(x => x.BaslangicTarihi.Value.Month == today.Month &&
                        x.BaslangicTarihi.Value.Year == today.Year)
            .Sum(x => x.Cars.Fiyat ?? 0);

        ViewBag.Total = total;

        return View();
    }
    public IActionResult YearlyIncome()
    {
        var today = DateTime.Today;

        decimal total = _context.Rentals
            .Include(x => x.Cars)
            .Where(x => x.BaslangicTarihi.Value.Year == today.Year)
            .Sum(x => x.Cars.Fiyat ?? 0);

        ViewBag.Total = total;

        return View();
    }
}