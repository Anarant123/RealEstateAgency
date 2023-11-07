using Microsoft.EntityFrameworkCore;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models;
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
        public static RealEstate GetRealEstate(this DBClient dbClient, int id)
        {
            return dbClient.context.RealEstates
                           .Include(x => x.Apartment)
                           .Include(x => x.House)
                           .Include(x => x.LandPlot)
                           .Include(x => x.Coordinates)
                           .Include(x => x.PropertyAddress)
                           .First(x => x.Id == id);
        }

        public static List<RealEstate> GetRealEstates(this DBClient dbClient)
        {
            return dbClient.context.RealEstates
                .Include(x => x.Type)
                .Include(x => x.PropertyAddress)
                .ToList();
        }


        public static async Task<RealEstate> CreateRealEstate(this DBClient dbClient, CreateRealEstateRequest createRealEstate)
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
            var realEstate = dbClient.context.RealEstates
                .Include(x => x.Apartment)
                .Include(x => x.House)
                .Include(x => x.LandPlot)
                .Include(x => x.Coordinates)
                .Include(x => x.PropertyAddress)
                .First(x => x.Id == updateRealEstate.Id);

            if (realEstate != null)
            {

                realEstate.Coordinates = updateRealEstate.Coordinates;
                realEstate.PropertyAddress = updateRealEstate.PropertyAddress;
                realEstate.Apartment = updateRealEstate.Apartment;
                realEstate.House = updateRealEstate.House;
                realEstate.LandPlot = updateRealEstate.LandPlot;

                await dbClient.context.SaveChangesAsync();
                return realEstate;
            }
            return null;
        }

        public static async Task DeleteRealEstate(this DBClient dbClient, int id)
        {
            var realEstate = dbClient.context.RealEstates
                           .Include(x => x.Apartment)
                           .Include(x => x.House)
                           .Include(x => x.LandPlot)
                           .Include(x => x.Coordinates)
                           .Include(x => x.PropertyAddress)
                           .First(x => x.Id == id);

            if (realEstate != null)
            {
                var a = realEstate.Apartment;
                if (a != null)
                    dbClient.context.Apartments.Remove(a);

                var h = realEstate.House;
                if (h != null)
                    dbClient.context.Houses.Remove(h);

                var l = realEstate.LandPlot;
                if (l != null)
                    dbClient.context.LandPlots.Remove(l);

                dbClient.context.RealEstates.Remove(realEstate);
                await dbClient.context.SaveChangesAsync();
            }
        }

        public static async Task<RealEstate> CreateRealEstate<T>(this DBClient dbClient, T realEstateObject, CreateRealEstateRequest createRealEstate) where T : RealEstateObject
        {
            var realEstate = await dbClient.CreateRealEstate(createRealEstate);

            realEstateObject.RealEstateId = realEstate.Id;
            dbClient.context.Set<T>().Add(realEstateObject);
            await dbClient.context.SaveChangesAsync();
            return realEstate;
        }


    }
}
