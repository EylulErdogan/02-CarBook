using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace carWorldDbFirstProject.Controllers
{
    public class CustomersController : Controller
    {

        private readonly AppDbContext dbcontext;
        public CustomersController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var result = dbcontext.Customers.ToList();
            return View(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Customers customers)
        {

            dbcontext.Customers.Add(customers);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            var result = dbcontext.Customers.Find(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(Customers customers)
        {
            dbcontext.Update(customers);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var result = dbcontext.Customers.Find(id);

            if (result != null)
            {
                dbcontext.Customers.Remove(result);
                dbcontext.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
