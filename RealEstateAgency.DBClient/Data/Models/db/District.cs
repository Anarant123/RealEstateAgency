using System;
using System.Collections.Generic;

namespace RealEstateAgency.DBClient.Data.Models.db;

public partial class District
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Сoordinate> Сoordinates { get; set; } = new List<Сoordinate>();
}
