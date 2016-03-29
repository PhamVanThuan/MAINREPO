using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToSAHLDateString(this DateTime Date)
        {
            return Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static string ToSAHLDateTimeString(this DateTime Date)
        {
            return Date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public static string ToSAHLDateTimeWithSecondsString(this DateTime Date)
        {
            return Date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }


        public static string GetDateYYYYMMDD(DateTime dateTime)
        {
            StringBuilder dateYYYYMMDD = new StringBuilder();
            dateYYYYMMDD.Append(dateTime.Year.ToString());
            dateYYYYMMDD.Append(dateTime.Month.ToString().PadLeft(2, '0'));
            dateYYYYMMDD.Append(dateTime.Day.ToString().PadLeft(2, '0'));
            return dateYYYYMMDD.ToString();
        }

        public static string GetTimeHHMMSSFromDateTime(DateTime dateTime)
        {
            StringBuilder timeHHMMSS = new StringBuilder();
            timeHHMMSS.Append(dateTime.Hour.ToString().PadLeft(2, '0'));
            timeHHMMSS.Append(dateTime.Minute.ToString().PadLeft(2, '0'));
            timeHHMMSS.Append(dateTime.Second.ToString().PadLeft(2, '0'));
            return timeHHMMSS.ToString();
        }
    }
}
