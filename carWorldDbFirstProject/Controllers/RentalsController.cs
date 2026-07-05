using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace carWorldDbFirstProject.Controllers
{
    public class RentalsController : Controller
    {
        private readonly AppDbContext dbcontext;

        public RentalsController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var rentals = dbcontext.Rentals
                .Include(x => x.Cars)
                .Include(x => x.Customers)
                .ToList();

            return View(rentals);
        }

        public IActionResult Create()
        {
            ViewBag.AracId = new SelectList(dbcontext.Cars.ToList(), "Id", "Marka");
            ViewBag.MusteriId = new SelectList(dbcontext.Customers.ToList(), "Id", "AdSoyad");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Rentals rentals)
        {
            dbcontext.Rentals.Add(rentals);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var rental = dbcontext.Rentals.Find(id);

            ViewBag.AracId = new SelectList(dbcontext.Cars.ToList(), "Id", "Marka", rental?.AracId);
            ViewBag.MusteriId = new SelectList(dbcontext.Customers.ToList(), "Id", "AdSoyad", rental?.MusteriId);

            return View(rental);
        }

        [HttpPost]
        public IActionResult Edit(Rentals rentals)
        {
            dbcontext.Rentals.Update(rentals);
            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var rental = dbcontext.Rentals.Find(id);

            if (rental != null)
            {
                dbcontext.Rentals.Remove(rental);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}