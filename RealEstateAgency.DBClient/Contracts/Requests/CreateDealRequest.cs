using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class CreateDealRequest
    {
        public double SellerCommission { get; set; }

        public double BuyerCommission { get; set; }

        public virtual Need Need { get; set; } = null!;

        public virtual Offer Offer { get; set; } = null!;
    }
}
