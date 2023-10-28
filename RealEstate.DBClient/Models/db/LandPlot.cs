using System;
using System.Collections.Generic;

namespace RealEstate.DBClient.Models.db;

public partial class LandPlot
{
    public int Id { get; set; }

    public double? Area { get; set; }

    public int RealEstateId { get; set; }

    public virtual RealEstate RealEstate { get; set; } = null!;
}
