﻿using System;
using System.Collections.Generic;

namespace RealEstate.DBClient.Models.db;

public partial class House
{
    public int Id { get; set; }

    public string CountFloors { get; set; } = null!;

    public string CountRooms { get; set; } = null!;

    public double? Area { get; set; }

    public int RealEstateId { get; set; }

    public virtual RealEstate RealEstate { get; set; } = null!;
}
