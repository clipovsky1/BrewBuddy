using BrewBuddy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BrewBuddy.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }
        // 
        // GET: /HelloWorld/Welcome/ 
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }

        // GET: /HelloWorld/BeerLog
        public String BeerLog()
        {
            BrewBuddyDbContext _context = new BrewBuddyDbContext();

            var result = _context.Beers
                .Where(item => item.Id == 2)
                .Select(item => item.Name)
                .SingleOrDefault();

            return result;

        }
    }
}
