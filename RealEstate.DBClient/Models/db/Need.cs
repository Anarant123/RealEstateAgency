using System;
using System.Collections.Generic;

namespace RealEstate.DBClient.Models.db;

public partial class Need
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int RealtorId { get; set; }

    public int TypeId { get; set; }

    public int? PropertyAddressId { get; set; }

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public bool IsSatisfied { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual ICollection<NeedForApartment> NeedForApartments { get; set; } = new List<NeedForApartment>();

    public virtual ICollection<NeedForHome> NeedForHomes { get; set; } = new List<NeedForHome>();

    public virtual ICollection<NeedForLandPlot> NeedForLandPlots { get; set; } = new List<NeedForLandPlot>();

    public virtual PropertyAddress? PropertyAddress { get; set; }

    public virtual Realtor Realtor { get; set; } = null!;

    public virtual TypeOfRealEstate Type { get; set; } = null!;
}
