using Microsoft.EntityFrameworkCore;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Extensions
{
    public static class OfferExtentions
    {
        public static async Task<Offer> GetOffer(this DBClient dbClient, int id)
        {
            return await dbClient.context.Offers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<Offer> GetOffers(this DBClient dbClient)
        {
            return dbClient.context.Offers.ToList();
        }


        public static async Task<Offer> CreateOffer(this DBClient dbClient, CreateOfferRequest createOffer)
        {
            var offer = new Offer()
            {
                Client = createOffer.Client,
                Realtor = createOffer.Realtor,
                RealEstate = createOffer.RealEstate,
                Price = createOffer.Price,
            };

            dbClient.context.Offers.Add(offer);
            await dbClient.context.SaveChangesAsync();

            return offer;
        }

        public static async Task<Offer> UpdateOffer(this DBClient dbClient, UpdateOfferRequest updateOffer)
        {
            var offer = dbClient.context.Offers.Find(updateOffer.Id);

            if (offer != null)
            {
                offer.Client = updateOffer.Client;
                offer.Realtor = updateOffer.Realtor;
                offer.RealEstate = updateOffer.RealEstate;
                offer.Price = updateOffer.Price;

                await dbClient.context.SaveChangesAsync();
                return offer;
            }
            return null;
        }

        public static async void DeleteOffer(this DBClient dbClient, int id)
        {
            var offer = dbClient.context.Offers.Find(id);

            if (offer != null)
            {
                dbClient.context.Offers.Remove(offer);
                await dbClient.context.SaveChangesAsync();
            }
        }
    }
}
