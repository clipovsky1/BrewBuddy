using System;
using System.Collections.Generic;

namespace BrewBuddy.Models;

public partial class BeerStyle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Beer> Beers { get; set; } = new List<Beer>();
}
