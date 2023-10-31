using Microsoft.EntityFrameworkCore;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Extensions
{
    public static class RealtorExtensions
    {
        public static async Task<Realtor> GetRealtor(this DBClient dbClient, int id)
        {
            return await dbClient.context.Realtors.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<Realtor> GetRealtors(this DBClient dbClient)
        {
            return dbClient.context.Realtors.ToList();
        }


        public static async Task<Realtor> CreateRealtor(this DBClient dbClient, CreateRealtorRequest createRealtor)
        {
            var realtor = new Realtor()
            {
                Name = createRealtor.Name,
                LastName = createRealtor.LastName,
                MiddleName = createRealtor.MiddleName,
                ShareCommission = createRealtor.ShareCommission,
            };
            if (realtor.ShareCommission == null)
                realtor.ShareCommission = 45;

            dbClient.context.Realtors.Add(realtor);
            await dbClient.context.SaveChangesAsync();

            return realtor;
        }

        public static async Task<Realtor> UpdateRealtor(this DBClient dbClient, UpdateRealtorRequest updateRealtor)
        {
            var realtor = dbClient.context.Realtors.Find(updateRealtor.Id);

            if (realtor != null)
            {
                realtor.Name = updateRealtor.Name;
                realtor.LastName = updateRealtor.LastName;
                realtor.MiddleName = updateRealtor.MiddleName;
                realtor.ShareCommission = updateRealtor.ShareCommission;

                await dbClient.context.SaveChangesAsync();
                return realtor;
            }
            return null;
        }

        public static async void DeleteRealtor(this DBClient dbClient, int id)
        {
            var realtor = dbClient.context.Realtors.Find(id);

            if (realtor != null)
            {
                dbClient.context.Realtors.Remove(realtor);
                await dbClient.context.SaveChangesAsync();
            }
        }
    }
}
