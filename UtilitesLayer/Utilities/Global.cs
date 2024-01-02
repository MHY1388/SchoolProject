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
            if (phone.Length != 11)
            {
                return false;
            }
            try
            {
                var a = Convert.ToInt64(phone);
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }
    }
}
