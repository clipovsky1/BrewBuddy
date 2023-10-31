using System.ComponentModel.DataAnnotations;

namespace BrewBuddy.ViewModels
{
    public class DeleteBeerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Brewery")]
        public string BreweryName { get; set; }
        [Display(Name = "Style")]
        public string StyleName { get; set; }
    }
}
