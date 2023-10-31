using BrewBuddy.Models;
using System.ComponentModel.DataAnnotations;

namespace BrewBuddy.ViewModels
{
    public class BeerViewModel
    {
        public int Id { get; set; }
        public int BreweryId { get; set; }
        public int StyleId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Brewery")]
        public string BreweryName { get; set; }
        [Display(Name = "Style")]
        public string StyleName { get; set; }
        public string StyleDescription { get; set; }
    }
}
