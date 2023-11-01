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
    public static class RealEstateExtensions
    {
        public static async Task<RealEstate> GetRealEstate(this DBClient dbClient, int id)
        {
            return await dbClient.context.RealEstates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<RealEstate> GetRealEstates(this DBClient dbClient)
        {
            return dbClient.context.RealEstates.ToList();
        }


        public static async Task<RealEstate> CreateRealEstates(this DBClient dbClient, CreateRealEstateRequest createRealEstate)
        {
            var realEstate = new RealEstate()
            {
                Coordinates = createRealEstate.Coordinates,
                PropertyAddress = createRealEstate.PropertyAddress,
                Type= createRealEstate.Type,
            };

            switch (realEstate.Type.Id)
            {
                case 1:
                    realEstate.Apartment = createRealEstate.Apartment;
                    break;
                case 2:
                    realEstate.House = createRealEstate.House;
                    break;
                case 3:
                    realEstate.LandPlot = createRealEstate.LandPlot;
                    break;
            }

            dbClient.context.RealEstates.Add(realEstate);
            await dbClient.context.SaveChangesAsync();

            return realEstate;
        }

        public static async Task<RealEstate> UpdateRealEstate(this DBClient dbClient, UpdateRealEstateRequest updateRealEstate)
        {
            var realEstate = dbClient.context.RealEstates.Find(updateRealEstate.Id);

            if (realEstate != null)
            {
                realEstate.Coordinates = updateRealEstate.Coordinates;
                realEstate.PropertyAddress = updateRealEstate.PropertyAddress;
                realEstate.Type = updateRealEstate.Type;

                switch (realEstate.Type.Id)
                {
                    case 1:
                        realEstate.Apartment = updateRealEstate.Apartment;
                        realEstate.House = null;
                        realEstate.LandPlot = null;
                        break;
                    case 2:
                        realEstate.Apartment = null;
                        realEstate.House = updateRealEstate.House;
                        realEstate.LandPlot = null;
                        break;
                    case 3:
                        realEstate.Apartment = null;
                        realEstate.House = null;
                        realEstate.LandPlot = updateRealEstate.LandPlot;
                        break;
                }

                await dbClient.context.SaveChangesAsync();
                return realEstate;
            }
            return null;
        }

        public static async void DeleteRealEstate(this DBClient dbClient, int id)
        {
            var realEstate = dbClient.context.RealEstates.Find(id);

            if (realEstate != null)
            {
                dbClient.context.RealEstates.Remove(realEstate);
                await dbClient.context.SaveChangesAsync();
            }
        }
    }
}
