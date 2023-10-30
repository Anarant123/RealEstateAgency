using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class RealEstate
{
    public int Id { get; set; }

    public int? PropertyAddressId { get; set; }

    public int TypeId { get; set; }

    public int? CoordinatesId { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Сoordinate? Coordinates { get; set; }

    public virtual House? House { get; set; }

    public virtual LandPlot? LandPlot { get; set; }

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public virtual PropertyAddress? PropertyAddress { get; set; }

    public virtual TypeOfRealEstate Type { get; set; } = null!;
}
