using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class PropertyAddress
{
    public int Id { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? HouseNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();

    public virtual ICollection<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
}
