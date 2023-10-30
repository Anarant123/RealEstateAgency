using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class Deal
{
    public int Id { get; set; }

    public int NeedId { get; set; }

    public int OfferId { get; set; }

    public double SellerCommission { get; set; }

    public double BuyerCommission { get; set; }

    public virtual Need Need { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;
}
