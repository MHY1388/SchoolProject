using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtilitesLayer.Utilities
{
    public static class NormailizePhone
    {
        public static string NormalizePhoneNumber(this string phoneNumber)
        {
            // حذف هر چیزی که غیر از اعداد در شماره تلفن وجود دارد
            phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]+", "");



            // اگر شماره تلفن با +98 یا 0098 آغاز شود، آن را به 0 تبدیل کنید
            if (phoneNumber.StartsWith("+98") || phoneNumber.StartsWith("98"))
            {
                phoneNumber = "0" + phoneNumber.Substring(3);
            }

            // اگر شماره تلفن بیشتر از 11 رقم باشد، آن را به 11 رقم تقلیل دهید
            if (phoneNumber.Length > 11)
            {
                phoneNumber = phoneNumber.Substring(0, 11);
            }

            return phoneNumber;
        }

    }
}
