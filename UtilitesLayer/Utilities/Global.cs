using DataLayer.Entities;
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
        public static string PersianToEnglish(this string persianStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            foreach (var item in persianStr)
            {
                persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            return persianStr;
        }
        public static string HomeWorkTypeToString(this HomeWorkType type)
        {
            return type switch
            {
                HomeWorkType.test => "امتحان",
                HomeWorkType.optional => "اختیاری",
                HomeWorkType.question => "پرسش",
                HomeWorkType.homework => "تکلیف",
                _ => "تکلیف",
            };
        }
        public static string DayOfWeekPersian(this DateTime data)
        {
            string result = "";
            switch (data.DayOfWeek)
            {
                case (DayOfWeek.Saturday):
                    result = "شنبه";
                    break;
                case (DayOfWeek.Sunday):
                    result = "یک شنبه";
                    break;
                case (DayOfWeek.Monday):
                    result = "دوشنبه";
                    break;
                case (DayOfWeek.Tuesday):
                    result = "سه شنبه";
                    break;
                case (DayOfWeek.Wednesday):
                    result = "چهار شنبه";
                    break;
                case (DayOfWeek.Thursday):
                    result = "پنج شنبه";
                    break;
                case (DayOfWeek.Friday):
                    result = "جمعه";
                    break;

            }

            return result;
        }
        public static string MonthOfYearPersian(this int data)
        {
            string result = "";
            switch (data)
            {
                case (1):
                    result = "فروردین";
                    break;
                case (2):
                    result = "اردیبهشت";
                    break;
                case (3):
                    result = "خرداد";
                    break;
                case (4):
                    result = "تیر";
                    break;
                case (5):
                    result = "مرداد";
                    break;
                case (6):
                    result = "شهریور";
                    break;
                case (7):
                    result = "مهر";
                    break;
                case (8):
                    result = "آبان";
                    break;
                case (9):
                    result = "آذر";
                    break;
                case (10):
                    result = "دی";
                    break;
                case (11):
                    result = "بهمن";
                    break;
                case (12):
                    result = "اسفند";
                    break;

            }

            return result;
        }
    }
}
