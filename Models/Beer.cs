using System;
using System.Collections.Generic;

namespace BrewBuddy.Models;

public partial class Beer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int BreweryId { get; set; }

    public int StyleId { get; set; }

    public virtual Brewery Brewery { get; set; }

    public virtual BeerStyle Style { get; set; }
}
