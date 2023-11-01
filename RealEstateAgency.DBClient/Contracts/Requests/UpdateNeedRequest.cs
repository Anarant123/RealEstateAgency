using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class UpdateNeedRequest
    {
        public int Id { get; set; }

        public virtual Client Client { get; set; } = null!;

        public virtual Realtor Realtor { get; set; } = null!;

        public virtual TypeOfRealEstate Type { get; set; } = null!;

        public virtual PropertyAddress? PropertyAddress { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public bool IsSatisfied { get; set; }

        public virtual ICollection<NeedForApartment> NeedForApartments { get; set; } = new List<NeedForApartment>();

        public virtual ICollection<NeedForHome> NeedForHomes { get; set; } = new List<NeedForHome>();

        public virtual ICollection<NeedForLandPlot> NeedForLandPlots { get; set; } = new List<NeedForLandPlot>();
    }
}
