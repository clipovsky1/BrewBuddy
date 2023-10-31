using System;
using System.Collections.Generic;

namespace BrewBuddy.Models;

public partial class Brewery
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Beer> Beers { get; set; } = new List<Beer>();
}
