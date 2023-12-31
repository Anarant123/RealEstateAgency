﻿using Microsoft.EntityFrameworkCore;
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
    public static class NeedExtensions
    {
        public static Need GetNeed(this DBClient dbClient, int id)
        {
            return dbClient.context.Needs
                .Include(x => x.PropertyAddress)
                .Include(x => x.Client)
                .Include(x => x.Realtor)
                .Include(x => x.Deals)
                .Include(x => x.Type)
                .Include(x => x.NeedForApartments)
                .Include(x => x.NeedForHomes)
                .Include(x => x.NeedForLandPlots)
                .First(x => x.Id == id);
        }

        public static List<Need> GetNeeds(this DBClient dbClient)
        {
            return dbClient.context.Needs
                .Include(x => x.PropertyAddress)
                .Include(x => x.Client)
                .Include(x => x.Realtor)
                .Include(x => x.Deals)
                .Include(x => x.Type)
                .Include(x => x.NeedForApartments)
                .Include(x => x.NeedForHomes)
                .Include(x => x.NeedForLandPlots)
                .ToList();
        }


        public static async Task<Need> CreateNeed(this DBClient dbClient, CreateNeedRequest createNeed)
        {
            var need = new Need()
            {
                Client = createNeed.Client,
                Realtor = createNeed.Realtor,
                Type = createNeed.Type,
                PropertyAddress = createNeed.PropertyAddress,
                MinPrice = createNeed.MinPrice,
                MaxPrice = createNeed.MaxPrice,
            };

            switch (need.Type.Id)
            {
                case 1:
                    need.NeedForApartments = createNeed.NeedForApartments;
                    break;
                case 2:
                    need.NeedForHomes = createNeed.NeedForHomes;
                    break;
                case 3:
                    need.NeedForLandPlots = createNeed.NeedForLandPlots;
                    break;
            }

            dbClient.context.Needs.Add(need);
            await dbClient.context.SaveChangesAsync();

            return need;
        }

        public static async Task<Need> UpdateNeed(this DBClient dbClient, UpdateNeedRequest updateNeed)
        {
            var need = dbClient.context.Needs
                .Include(x => x.PropertyAddress)
                .Include(x => x.Client)
                .Include(x => x.Realtor)
                .Include(x => x.Deals)
                .Include(x => x.Type)
                .Include(x => x.NeedForApartments)
                .Include(x => x.NeedForHomes)
                .Include(x => x.NeedForLandPlots)
                .First(x => x.Id == updateNeed.Id);

            if (need != null)
            {
                need.Type = updateNeed.Type;
                need.PropertyAddress = updateNeed.PropertyAddress;
                need.MinPrice = updateNeed.MinPrice;
                need.MaxPrice = updateNeed.MaxPrice;

                switch (need.Type.Id)
                {
                    case 1:
                        need.NeedForApartments = updateNeed.NeedForApartments;
                        need.NeedForHomes = null;
                        need.NeedForLandPlots = null;
                        break;
                    case 2:
                        need.NeedForApartments = null;
                        need.NeedForHomes = updateNeed.NeedForHomes;
                        need.NeedForLandPlots = null;
                        break;
                    case 3:
                        need.NeedForApartments = null;
                        need.NeedForHomes = null;
                        need.NeedForLandPlots = updateNeed.NeedForLandPlots;
                        break;
                }

                await dbClient.context.SaveChangesAsync();
                return need;
            }
            return null;
        }

        public static async Task DeleteNeed(this DBClient dbClient, int id)
        {
            var need = dbClient.context.Needs
                .Include(x => x.PropertyAddress)
                .Include(x => x.Client)
                .Include(x => x.Realtor)
                .Include(x => x.Deals)
                .Include(x => x.Type)
                .Include(x => x.NeedForApartments)
                .Include(x => x.NeedForHomes)
                .Include(x => x.NeedForLandPlots)
                .First(x => x.Id == id);

            if (need != null)
            {
                var na = need.NeedForApartments.FirstOrDefault();
                if (na != null)
                    dbClient.context.NeedForApartments.Remove(na);

                var nh = need.NeedForHomes.FirstOrDefault();
                if (nh != null)
                    dbClient.context.NeedForHomes.Remove(nh);

                var nl = need.NeedForLandPlots.FirstOrDefault();
                if (nl != null)
                    dbClient.context.NeedForLandPlots.Remove(nl);

                dbClient.context.Needs.Remove(need);
                await dbClient.context.SaveChangesAsync();
            }
        }

        public static async Task<Need> CreateNeed<T>(this DBClient dbClient, T needObject, CreateNeedRequest createNeed) where T : NeedObject
        {
            var need = await dbClient.CreateNeed(createNeed);

            needObject.NeedId = need.Id;
            dbClient.context.Set<T>().Add(needObject);
            await dbClient.context.SaveChangesAsync();
            return need;
        }
    }
}
