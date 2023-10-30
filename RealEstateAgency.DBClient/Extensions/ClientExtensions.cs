using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Extensions
{
    public static class ClientExtensions
    {
        public static async Task<Client> CreateClient(this DBClient dbClient, CreateClientRequest createClient)
        {
            var client = new Client()
            {
                Name = createClient.Name,
                LastName = createClient.LastName,
                MiddleName = createClient.MiddleName,
                PhoneNumber = createClient.PhoneNumber,
                Email = createClient.Email,
            };

            //dbClient.context.Add(client);
            //await dbClient.context.SaveChangesAsync();

            return client;
        }
    }
}
