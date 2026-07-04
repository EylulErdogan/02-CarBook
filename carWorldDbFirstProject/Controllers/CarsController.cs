using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace carWorldDbFirstProject.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext dbcontext;
        public CarsController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index(string search)
        {
            var cars = dbcontext.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                cars = cars.Where(x =>
                    x.Marka.Contains(search) ||
                    x.Model.Contains(search));
            }

            ViewBag.Search = search;

            return View(cars.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cars cars)
        {

            dbcontext.Cars.Add(cars);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            var result = dbcontext.Cars.Find(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(Cars cars)
        {
            dbcontext.Update(cars);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var result = dbcontext.Cars.Find(id);

            if (result != null)
            {
                dbcontext.Cars.Remove(result);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}

