using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace carWorldDbFirstProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext dbcontext)
        {
            _context = dbcontext;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;

            ViewBag.CarCount = _context.Cars.Count();
            ViewBag.CustomerCount = _context.Customers.Count();
            ViewBag.RentalCount = _context.Rentals.Count();

            ViewBag.ThisMonthIncome = _context.Rentals
                .Include(x => x.Cars)
                .Where(x => x.BaslangicTarihi.HasValue &&
                            x.BaslangicTarihi.Value.Month == today.Month &&
                            x.BaslangicTarihi.Value.Year == today.Year)
                .Sum(x => x.Cars.Fiyat ?? 0);

            ViewBag.YearlyIncome = _context.Rentals
                .Include(x => x.Cars)
                .Where(x => x.BaslangicTarihi.HasValue &&
                            x.BaslangicTarihi.Value.Year == today.Year)
                .Sum(x => x.Cars.Fiyat ?? 0);

            var monthlyChartData = new decimal[12];

            var rentals = _context.Rentals
                .Include(x => x.Cars)
                .Where(x => x.BaslangicTarihi.HasValue &&
                            x.BaslangicTarihi.Value.Year == today.Year)
                .ToList();

            foreach (var item in rentals)
            {
                int month = item.BaslangicTarihi.Value.Month;
                monthlyChartData[month - 1] += item.Cars?.Fiyat ?? 0;
            }

            ViewBag.MonthlyChartData = JsonSerializer.Serialize(monthlyChartData);
            ViewBag.TopCars = _context.Rentals
                .Include(x => x.Cars)
                .Where(x => x.Cars != null)
                .GroupBy(x => new
                {
                    x.Cars.Marka,
                    x.Cars.Model
                })
                .Select(x => new
                {
                    CarName = x.Key.Marka + " " + x.Key.Model,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.TopCustomers = _context.Rentals
                .Include(x => x.Customers)
                .Where(x => x.Customers != null)
                .GroupBy(x => x.Customers.AdSoyad)
                .Select(x => new
                {
                    Customer = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            return View();
        }
    }
}