using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace carWorldDbFirstProject.Controllers
{
    public class CustomerRentalController : Controller
    {
        private readonly AppDbContext db;

        public CustomerRentalController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Index(int id)
        {
            var car = db.Cars.FirstOrDefault(x => x.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        public IActionResult CreateRental(
        int aracId,
        string adSoyad,
         string telefon,
        DateTime baslangicTarihi,
         DateTime bitisTarihi)
        {
            Customers customer = new Customers
            {
                AdSoyad = adSoyad,
                Telefon = telefon
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Rentals rental = new Rentals
            {
                AracId = aracId,
                MusteriId = customer.Id,
                BaslangicTarihi = baslangicTarihi,
                BitisTarihi = bitisTarihi
            };

            db.Rentals.Add(rental);
            db.SaveChanges();
            TempData["Success"] = "Aracınız başarıyla kiralanmıştır.";

            return RedirectToAction("Index", "Home");
        }
    }
}