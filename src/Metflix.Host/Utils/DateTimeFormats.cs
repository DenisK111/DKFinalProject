using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class DateTimeFormats
    {
        public static string[] AcceptableInputFormats => new[] { "yyyy-MM-dd hh:mm:ss tt", "yyyy-MM-dd" };
        public static DateTime TryParseExactAcceptableFormats(this string dateTimeString)
        {
            dateTimeString = dateTimeString.Trim();
            DateTime.TryParseExact(dateTimeString, AcceptableInputFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
            return result;
        }
    }
}
