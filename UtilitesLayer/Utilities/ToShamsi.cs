using System.Globalization;

namespace UtilitesLayer.Utilities;

public static class ToShamsi
{
    public static string PersianDate(this DateTime DateTime1)
    {
        PersianCalendar PersianCalendar1 = new PersianCalendar();
        return string.Format(@"{0}/{1}/{2} - {3}:{4}",
            PersianCalendar1.GetYear(DateTime1),
            PersianCalendar1.GetMonth(DateTime1),
            PersianCalendar1.GetDayOfMonth(DateTime1),
            PersianCalendar1.GetHour(DateTime1),
            PersianCalendar1.GetMinute(DateTime1));
    }
}