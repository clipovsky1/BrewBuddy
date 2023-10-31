using System.ComponentModel.DataAnnotations;

namespace BrewBuddy.ViewModels
{
    public class CreateBeerViewModel
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Brewery field is required.")]
        public string BreweryName { get; set; }

        [Required(ErrorMessage = "The Beer Style field is required.")]
        public int SelectedStyleId { get; set; }

    }
}
