using Microsoft.EntityFrameworkCore;
using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Extensions
{
    public static class TypeOfRealEstateExtensions
    {
        public static async Task<TypeOfRealEstate> GetTypeOfRealEstate(this DBClient dbClient, int id)
        {
            return await dbClient.context.TypeOfRealEstates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<TypeOfRealEstate> GetTypeOfRealEstates(this DBClient dbClient)
        {
            return dbClient.context.TypeOfRealEstates.ToList();
        }
    }
}
