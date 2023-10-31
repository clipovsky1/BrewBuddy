using BrewBuddy.Models;

namespace BrewBuddy.ViewModels
{
    public class EditViewModel
    {
        public Beer Beer { get; set; }
        public Brewery Brewery { get; set; }
        public BeerStyle Style { get; set; }

        public EditViewModel(Beer entity) 
        {
            Beer = entity;
            Brewery = entity.Brewery;
            Style = entity.Style;
        }
    }
}
