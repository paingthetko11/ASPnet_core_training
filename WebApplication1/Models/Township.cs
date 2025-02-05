using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Township
{
    public string TownshipId { get; set; } = null!;

    public string? TownshipName { get; set; }

    public double? Lattitude { get; set; }

    public double? Longitude { get; set; }
}
