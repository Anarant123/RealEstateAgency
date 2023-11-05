using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class CreateRealEstateRequest
    {
        public virtual Apartment? Apartment { get; set; }

        public virtual House? House { get; set; }

        public virtual LandPlot? LandPlot { get; set; }

        public virtual Сoordinate? Coordinates { get; set; }

        public virtual PropertyAddress? PropertyAddress { get; set; }

        public virtual TypeOfRealEstate Type { get; set; } = null!;

    }
}
