using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}
