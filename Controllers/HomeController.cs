using Microsoft.AspNetCore.Mvc;
using BrewBuddy.Models; // Import your Beer model
using BrewBuddy.ViewModels; // Import the ReadBeerViewModel
using BrewBuddy.Data;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    // Assuming you have a service or data access layer to retrieve beers
    private readonly BrewBuddyDbContext _context; // Replace with your actual service or repository

    public HomeController(BrewBuddyDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

}
