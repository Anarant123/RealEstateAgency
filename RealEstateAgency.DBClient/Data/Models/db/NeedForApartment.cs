using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class NeedForApartment : NeedObject
{
    public int Id { get; set; }

    public double? MinArea { get; set; }

    public double? MaxArea { get; set; }

    public string MinFloor { get; set; } = null!;

    public string MaxFloor { get; set; } = null!;

    public string MinCountRooms { get; set; } = null!;

    public string MaxCountRooms { get; set; } = null!;

    public virtual Need Need { get; set; } = null!;
}
