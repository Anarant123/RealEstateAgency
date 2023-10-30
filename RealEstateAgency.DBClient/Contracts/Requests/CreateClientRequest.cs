using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.DBClient.Contracts.Requests
{
    public class CreateClientRequest
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }
    }
}
