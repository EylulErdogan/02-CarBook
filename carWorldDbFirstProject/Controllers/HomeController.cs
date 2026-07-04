using carWorldDbFirstProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace carWorldDbFirstProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext dbcontext;
        public HomeController(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var result = dbcontext.Cars.ToList();
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
