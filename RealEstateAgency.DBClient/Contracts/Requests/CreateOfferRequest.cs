using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class CreateOfferRequest
    {
        public virtual Client Client { get; set; } = null!;

        public virtual Realtor Realtor { get; set; } = null!;

        public virtual RealEstate RealEstate { get; set; } = null!;

        public int Price { get; set; }

    }
}
