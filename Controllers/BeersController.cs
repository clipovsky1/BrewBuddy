using BrewBuddy.Data;
using BrewBuddy.Models;
using BrewBuddy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BrewBuddy.Controllers
{
    public class BeersController : Controller
    {
        private readonly BrewBuddyDbContext _context;

        public BeersController(BrewBuddyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListAllBeers()
        {
            var allBeers = await _context.Beers
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .ToListAsync();

            var beerViewModels = new List<BeerViewModel>();

            foreach (var beer in allBeers)
            {
                var viewModel = new BeerViewModel
                {
                    Id = beer.Id,
                    Name = beer.Name,
                    BreweryId = beer.BreweryId,
                    StyleId = beer.StyleId,
                    BreweryName = beer.Brewery.Name,
                    StyleName = beer.Style.Name,
                    StyleDescription = beer.Style.Description,
                    Date = beer.Date,
                    Rating = beer.Rating
                };
                beerViewModels.Add(viewModel);
            }
            beerViewModels = beerViewModels.OrderByDescending(b => b.Date).ToList();
            return View(beerViewModels);
        }


        // GET: Beers
        public async Task<IActionResult> Index()
        {
            return await ListAllBeers();
        }

        // GET: Beers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var details = await _context.Beers
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (details == null) { return View("Error"); }

            var viewModel = new BeerViewModel
            {
                Id = details.Id,
                Name = details.Name,
                BreweryId = details.BreweryId,
                StyleId = details.StyleId,
                BreweryName = details.Brewery.Name,
                StyleName = details.Style.Name,
                StyleDescription = details.Style.Description,
                Date = details.Date,
                Rating = details.Rating
            };

            return View(viewModel);
        }

        // GET: Beers/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateBeerViewModel();
            var styles = new SelectList(await _context.BeerStyles.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");
            ViewBag.BeerStyles = styles;
            ViewBag.Rating = new SelectList(Enumerable.Range(1, 5));
            return View(viewModel);
        }

        // POST: Beers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBeerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return to the view with validation errors.
                var styles = new SelectList(await _context.BeerStyles.ToListAsync(), "Id", "Name");
                ViewBag.BeerStyles = styles;
                ViewBag.Rating = new SelectList(Enumerable.Range(1, 5));

                return View(viewModel);
            }

            var existingBeer = await _context.Beers.FirstOrDefaultAsync(b =>
                b.Name == viewModel.Name &&
                b.Brewery.Name == viewModel.BreweryName &&
                b.Style.Id == viewModel.SelectedStyleId);

            if (existingBeer != null)
            {
                ModelState.AddModelError("Name", "A beer with the same Name, Brewery, and Style already exists.");
                var styles = new SelectList(await _context.BeerStyles.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");
                ViewBag.BeerStyles = styles;
                ViewBag.Rating = new SelectList(Enumerable.Range(1, 5));

                return View(viewModel);
            }

            var brewery = await _context.Breweries.FirstOrDefaultAsync(b => b.Name == viewModel.BreweryName);
            var style = await _context.BeerStyles.FindAsync(viewModel.SelectedStyleId);

            // Create the beer
            var beer = new Beer
            {
                Name = viewModel.Name,
                Brewery = brewery ?? new Brewery { Name = viewModel.BreweryName },
                Style = style,
                Rating = viewModel.Rating,
                Date = DateTime.Now
            };

            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Beers/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var beer = await _context.Beers
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (beer == null)
            {
                return NotFound();
            }


            var viewModel = new EditBeerViewModel
            {
                Name = beer.Name,
                BreweryName = beer.Brewery.Name,
                SelectedStyleId = beer.StyleId,
                Rating = beer.Rating
            };
            ViewBag.StyleId = new SelectList(await _context.BeerStyles.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");
            ViewBag.Rating = new SelectList(Enumerable.Range(1, 5));
            return View(viewModel);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBeerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // If the ModelState is not valid, return to the Edit view with the provided viewModel.
                ViewBag.StyleId = new SelectList(await _context.BeerStyles.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");
                ViewBag.Rating = new SelectList(Enumerable.Range(1, 5));
                return View(viewModel);
            }

            // Fetch the beer from the database.
            var beer = await _context.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            // Update the beer properties with the values from the viewModel.
            var brewery = await _context.Breweries
                .FirstOrDefaultAsync(b => b.Name == viewModel.BreweryName);
            var originalBrewery = await _context.Breweries.FirstOrDefaultAsync(b => b.Id == beer.BreweryId);
            var style = await _context.BeerStyles.FindAsync(viewModel.SelectedStyleId);

            beer.Name = viewModel.Name;
            beer.Brewery = brewery ?? new Brewery { Name = viewModel.BreweryName };
            beer.Style = style;
            beer.Rating = viewModel.Rating;

            _context.Update(beer);
            await _context.SaveChangesAsync();


            if (originalBrewery != null && await _context.Beers.CountAsync(b => b.BreweryId == originalBrewery.Id) < 1)
            {
                _context.Remove(originalBrewery);
                await _context.SaveChangesAsync();
            }

            // Redirect to the index action.
            return RedirectToAction(nameof(Index));
        }

        // GET: Beers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Beers == null)
            {
                return NotFound();
            }

            var beer = await _context.Beers
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beer == null)
            {
                return NotFound();
            }

            var viewModel = new DeleteBeerViewModel
            {
                Id = id,
                Name = beer.Name,
                BreweryName = beer.Brewery.Name,
                StyleName = beer.Style.Name
            };

            return View(viewModel);
        }

        // POST: Beers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteBeerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var id = viewModel.Id;
            var beer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == id);

            if (beer == null)
            {
                return NotFound();
            }

            _context.Beers.Remove(beer);
            var countBeersWithBrewery = await _context.Beers.CountAsync(b => b.BreweryId == beer.BreweryId);

            await _context.SaveChangesAsync();

            // Check if the brewery has no associated beers
            if (countBeersWithBrewery == 1)
            {
                // If no associated beers, remove the brewery
                var brewery = _context.Breweries.FirstOrDefault(b => b.Id == beer.BreweryId);
                _context.Breweries.Remove(brewery);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Ajax(int id)
        {

            var beer = await _context.Beers.FirstOrDefaultAsync(b => b.Id == id);

            if (beer == null)
            {
                return NotFound();
            }

            _context.Beers.Remove(beer);
            var countBeersWithBrewery = await _context.Beers.CountAsync(b => b.BreweryId == beer.BreweryId);

            await _context.SaveChangesAsync();

            // Check if the brewery has no associated beers
            if (countBeersWithBrewery == 1)
            {
                // If no associated beers, remove the brewery
                var brewery = _context.Breweries.FirstOrDefault(b => b.Id == beer.BreweryId);
                _context.Breweries.Remove(brewery);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
