﻿using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class UpdateRealEstateRequest
    {
        public int Id { get; set; }

        public virtual Apartment? Apartment { get; set; }

        public virtual House? House { get; set; }

        public virtual LandPlot? LandPlot { get; set; }

        public virtual Сoordinate? Coordinates { get; set; }

        public virtual PropertyAddress? PropertyAddress { get; set; }

    }
}
