using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RealEstateAgency.DBClient.Data.Models;

namespace RealEstateAgency.DBClient
{
    public class DBClient
    {
        public RealEstateAgencyContext context = new RealEstateAgencyContext();
        public DBClient(RealEstateAgencyContext context) 
        {
            context = context ?? new RealEstateAgencyContext();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }
    }
}
