using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.Utilities
{
    public static class Global
    {
        public static bool ValidatePhoneNumber(this string phone)
        {
            if(phone.NormalizePhoneNumber()!= phone) { return false; }
            return true;
        }
    }
}
