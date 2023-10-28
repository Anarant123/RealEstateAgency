using System;
using System.Collections.Generic;

namespace RealEstate.DBClient.Models.db;

public partial class TypeOfRealEstate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CommissionAmount { get; set; }

    public int CommissionPercentage { get; set; }

    public virtual ICollection<Need> Needs { get; set; } = new List<Need>();

    public virtual ICollection<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
}
