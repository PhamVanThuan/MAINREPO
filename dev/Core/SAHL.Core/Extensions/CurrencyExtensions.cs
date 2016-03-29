using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Extensions
{
    public static class CurrencyExtensions
    {
        public static string ToSAHLCurrencyFormat(this Double Number)
        {
            return Number.ToString("R ###,###,###,##0.00", CultureInfo.InvariantCulture);
        }
        public static string ToSAHLCurrencyNoCentsFormat(this Double Number)
        {
            return Number.ToString("R ###,###,###,##0", CultureInfo.InvariantCulture);
        }
    }
}
