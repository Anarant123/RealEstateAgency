using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealEstateAgency.Desktop.Utilities
{
    public class Validation
    {
        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Match match = Regex.Match(email, pattern);
            return match.Success;
        }

        public bool IsNotNull(string str)
        {
            if(string.IsNullOrEmpty(str)) return false;
            else return true;
        }

        public bool IsValidPhone(string phone)
        {
            string pattern = @"^(\+)[1-9][0-9\-().]{9,15}$";
            Match match = Regex.Match(phone, pattern);
            return match.Success;
        }
    }
}
