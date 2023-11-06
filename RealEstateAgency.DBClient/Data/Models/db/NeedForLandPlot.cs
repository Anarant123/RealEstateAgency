using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class NeedForLandPlot : NeedObject
{
    public int Id { get; set; }

    public double? MinArea { get; set; }

    public double? MaxArea { get; set; }

    public virtual Need Need { get; set; } = null!;
}
