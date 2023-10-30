using BrewBuddy.Models;

namespace BrewBuddy.ViewModels
{
    public class ReadBeerViewModel
    {
        public Beer Beer { get; set; }
        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }

        public ReadBeerViewModel(Beer beer, Brewery brewery, BeerStyle style)
        {
            Beer = beer;
            Brewery = brewery;
            Style = style;

        }

    }
}
