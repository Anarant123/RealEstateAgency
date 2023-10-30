using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class Offer
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int RealtorId { get; set; }

    public int RealEstateId { get; set; }

    public int Price { get; set; }

    public bool IsSatisfied { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual RealEstate RealEstate { get; set; } = null!;

    public virtual Realtor Realtor { get; set; } = null!;
}
