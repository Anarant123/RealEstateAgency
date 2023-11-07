using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class UpdateDealRequest
    {
        public int Id { get; set; }

        public double SellerCommission { get; set; }

        public double BuyerCommission { get; set; }
    }
}
