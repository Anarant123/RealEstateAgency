﻿using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class NeedForHome : NeedObject
{
    public int Id { get; set; }

    public double? MinArea { get; set; }

    public double? MaxArea { get; set; }

    public string MinCountRooms { get; set; } = null!;

    public string MaxCountRooms { get; set; } = null!;

    public string MinCountFloors { get; set; } = null!;

    public string MaxCountFloors { get; set; } = null!;

    public virtual Need Need { get; set; } = null!;
}
