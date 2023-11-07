using Microsoft.EntityFrameworkCore;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;

namespace RealEstateAgency.DBClient.Extensions
{
    public static class DealExtension
    {
        public static Deal GetDeal(this DBClient dbClient, int id)
        {
            return dbClient.context.Deals
                .Include(x => x.Need)
                    .ThenInclude(x => x.Type)
                .Include(x => x.Offer)
                    .ThenInclude(x => x.RealEstate)
                            .ThenInclude(x => x.PropertyAddress)
                .First(c => c.Id == id);
        }

        public static List<Deal> GetDeals(this DBClient dbClient)
        {
            return dbClient.context.Deals
                .Include(x => x.Offer)
                    .ThenInclude(x => x.RealEstate)
                        .ThenInclude(x => x.PropertyAddress)
                .ToList();
        }


        public static async Task<Deal> CreateDeal(this DBClient dbClient, CreateDealRequest createDeal)
        {
            var sellerCommission = createDeal.Offer.RealEstate.Type.CommissionAmount + (createDeal.Offer.RealEstate.Type.CommissionPercentage * 0.01 * createDeal.Offer.Price);
            var buyerCommission = createDeal.Offer.Price * 0.03;

            var deal = new Deal()
            {
                SellerCommission = sellerCommission,
                BuyerCommission = buyerCommission,
                Need = createDeal.Need,
                Offer = createDeal.Offer,
            };

            createDeal.Need.IsSatisfied = true;
            createDeal.Offer.IsSatisfied = true;

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

                await dbClient.context.SaveChangesAsync();
                return deal;
            }
            return null;
        }

        public static async Task DeleteDeal(this DBClient dbClient, int id)
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
