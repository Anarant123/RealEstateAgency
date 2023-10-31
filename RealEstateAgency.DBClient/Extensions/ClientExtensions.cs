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
    public static class ClientExtensions
    {
        public static async Task<Client> GetClient(this DBClient dbClient, int id)
        {
            return await dbClient.context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public static List<Client> GetClients(this DBClient dbClient)
        {
            return dbClient.context.Clients.ToList();
        }


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

            dbClient.context.Clients.Add(client);
            await dbClient.context.SaveChangesAsync();

            return client;
        }

        public static async Task<Client> UpdateClient(this DBClient dbClient, UpdateClientRequest updateClient)
        {
            var client = dbClient.context.Clients.Find(updateClient.Id);

            if (client != null)
            {
                client.Name = updateClient.Name;
                client.LastName = updateClient.LastName;
                client.MiddleName = updateClient.MiddleName;
                client.PhoneNumber = updateClient.PhoneNumber;
                client.Email = updateClient.Email;

                await dbClient.context.SaveChangesAsync();
                return client;
            }
            return null;
        }

        public static async void DeleteClient(this DBClient dbClient, int id)
        {
            var client = dbClient.context.Clients.Find(id);

            if (client != null)
            {
                dbClient.context.Clients.Remove(client);
                await dbClient.context.SaveChangesAsync();
            }
        }
    }
}
