using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class DateTimeParser
    {
        public static DateTime ParseExactAcceptableFormats(this string dateTimeString)
        {
            string[] formats = { "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd" };
             DateTime.TryParseExact(dateTimeString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
            return result;
        }
    }
}
