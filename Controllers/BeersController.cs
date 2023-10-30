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

        public IActionResult ListAllBeers()
        {
            // Retrieve all beers from the data source
            var allBeers = _context.Beers.ToList();

            // Create a list of ReadBeerViewModel instances to store the data for each beer
            var beerViewModels = new List<ReadBeerViewModel>();

            foreach (var beer in allBeers)
            {
                var brewery = _context.Breweries.Find(beer.BreweryId);
                var style = _context.BeerStyles.Find(beer.StyleId);

                if (brewery != null && style != null)
                {
                    // Create a ReadBeerViewModel for each beer and add it to the list
                    var viewModel = new ReadBeerViewModel(beer, brewery, style);
                    beerViewModels.Add(viewModel);
                }
            }

            return View(beerViewModels);
        }


        // GET: Beers
        public async Task<IActionResult> Index()
        {
            return ListAllBeers();
        }

        // GET: Beers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var beerDetails = await _context.Beers
                .Include(b => b.Brewery)
                .Include(b => b.Style)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beerDetails == null) { return  View("Error"); }

            var viewModel = new ReadBeerViewModel(beerDetails, beerDetails.Brewery, beerDetails.Style);

            return View(viewModel);
        }

        // GET: Beers/Create
        public IActionResult Create()
        {
            var viewModel = new CreateBeerViewModel();
            var styles = new SelectList(_context.BeerStyles, "Id", "Name");
            ViewBag.BeerStyles = styles;
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
                var styles = new SelectList(_context.BeerStyles, "Id", "Name");
                ViewBag.BeerStyles = styles;

                return View(viewModel);
            }

            var existingBeer = await _context.Beers.FirstOrDefaultAsync(b =>
                b.Name == viewModel.Name &&
                b.Brewery.Name == viewModel.BreweryName &&
                b.Style.Id == viewModel.SelectedStyleId);

            if (existingBeer != null)
            {
                ModelState.AddModelError("Name", "A beer with the same Name, Brewery, and Style already exists.");
                var styles = new SelectList(_context.BeerStyles, "Id", "Name");
                ViewBag.BeerStyles = styles;

                return View(viewModel);
            }

            var brewery = await _context.Breweries.FirstOrDefaultAsync(b => b.Name == viewModel.BreweryName);
            var style = await _context.BeerStyles.FindAsync(viewModel.SelectedStyleId);

            // Create the beer
            var beer = new Beer
            {
                Name = viewModel.Name,
                Brewery = brewery ?? new Brewery { Name = viewModel.BreweryName },
                Style = style
            };

            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Beers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Beers == null)
            {
                return NotFound();
            }

            var beer = await _context.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound();
            }

            ViewData["BreweryName"] = _context.Breweries.FirstOrDefault(b => b.Id == beer.BreweryId)?.Name ?? string.Empty;
            ViewData["StyleId"] = new SelectList(_context.BeerStyles, "Id", "Name", beer.StyleId);
            return View(beer);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BreweryId,StyleId")] Beer beer, String breweryName)
        {
            if (id != beer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // check if brewery to update to is in database. This default returns 0 if nothing is found. Ids shouldn't ever be 0?
                    var currentBreweryId = beer.BreweryId;
                    int updateToBreweryId = await _context.Breweries
                                           .Where(b => b.Name == breweryName)
                                           .Select(b => b.Id)
                                           .FirstOrDefaultAsync();
                    if (currentBreweryId != null && currentBreweryId == updateToBreweryId)
                    {
                        _context.Beers.Update(beer);
                        await _context.SaveChangesAsync();
                    }
                    else if (updateToBreweryId != 0 && currentBreweryId != null)
                    {
                        beer.BreweryId = updateToBreweryId;
                        _context.Beers.Update(beer);
                        await _context.SaveChangesAsync();
                        // check if previousBreweryId was only instantiated with Beer at hand.
                        int count = await _context.Beers
                                              .Where(b => b.Id == currentBreweryId)
                                              .CountAsync();
                        if (count < 1)
                        {
                            var breweryToDelete = await _context.Breweries.FindAsync(currentBreweryId);
                            if (breweryToDelete != null)
                            {
                                _context.Breweries.Remove(breweryToDelete);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }
                    else
                    {
                        // Brewery is not in database and needs to to be created.
                        Brewery newBrewery = new() { Name = breweryName };
                        _context.Breweries.Add(newBrewery);
                        _context.SaveChanges();

                        int newBreweryId = await _context.Breweries
                                                 .Where(b => b.Name == breweryName)
                                                 .Select(b => b.Id)
                                                 .FirstOrDefaultAsync();
                        beer.BreweryId = newBreweryId;
                        _context.Beers.Update(beer);
                        await _context.SaveChangesAsync();

                        int count = await _context.Beers
                                    .Where(b => b.BreweryId == currentBreweryId)
                                    .CountAsync();
                        if (count < 1)
                        {
                            var breweryToDelete = await _context.Breweries.FindAsync(currentBreweryId);
                            if (breweryToDelete != null)
                            {
                                _context.Breweries.Remove(breweryToDelete);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }
                    // End of try
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeerExists(beer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreweryId"] = new SelectList(_context.Breweries, "Id", "Id", beer.BreweryId);
            ViewData["StyleId"] = new SelectList(_context.BeerStyles, "Id", "Id", beer.StyleId);
            return View(beer);
        }

        // GET: Beers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Beers == null)
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

            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Beers == null)
            {
                return Problem("Entity set 'BrewBuddyDbContext.Beers'  is null.");
            }

            var beer = await _context.Beers.FindAsync(id);
            if (beer != null && beer.BreweryId != null)
            {
                var countBreweryIdInstances = await _context.Beers.CountAsync(b => b.BreweryId == beer.BreweryId);
                if (countBreweryIdInstances <= 1)
                {
                    var breweryId = (int)beer.BreweryId;
                    var breweryToRemove = new Brewery { Id = breweryId };
                    // Remove the brewery and the beer in a single transaction
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.Breweries.Remove(breweryToRemove);
                        _context.Beers.Remove(beer);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                else
                {
                    // Only remove the beer
                    _context.Beers.Remove(beer);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BeerExists(int id)
        {
            return (_context.Beers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
