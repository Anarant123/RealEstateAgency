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
    public static class DealExtension
    {
        public static async Task<Deal> GetDeal(this DBClient dbClient, int id)
        {
            return await dbClient.context.Deals.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<Deal> GetDeals(this DBClient dbClient)
        {
            return dbClient.context.Deals.ToList();
        }


        public static async Task<Deal> CreateDeal(this DBClient dbClient, CreateDealRequest createDeal)
        {
            var deal = new Deal()
            {
                SellerCommission = createDeal.SellerCommission,
                BuyerCommission = createDeal.BuyerCommission,
                Need = createDeal.Need,
                Offer = createDeal.Offer,
            };

            dbClient.context.Deals.Add(deal);
            await dbClient.context.SaveChangesAsync();

            return deal;
        }

        public static async Task<Deal> UpdateDeal(this DBClient dbClient, UpdateDealRequest updateDeal)
        {
            var deal = dbClient.context.Deals.Find(updateDeal.Id);

            if (deal != null)
            {
                deal.SellerCommission = updateDeal.SellerCommission;
                deal.BuyerCommission = updateDeal.BuyerCommission;
                deal.Need = updateDeal.Need;
                deal.Offer = updateDeal.Offer;

                await dbClient.context.SaveChangesAsync();
                return deal;
            }
            return null;
        }

        public static async void DeleteDeal(this DBClient dbClient, int id)
        {
            var deal = dbClient.context.Deals.Find(id);

            if (deal != null)
            {
                dbClient.context.Deals.Remove(deal);
                await dbClient.context.SaveChangesAsync();
            }
        }
    }
}
