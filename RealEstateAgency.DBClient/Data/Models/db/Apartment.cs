﻿using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class Apartment : RealEstateObject
{
    public int Id { get; set; }

    public string Floor { get; set; } = null!;

    public string CountRooms { get; set; } = null!;

    public double? Area { get; set; }

    public virtual RealEstate RealEstate { get; set; } = null!;
}
