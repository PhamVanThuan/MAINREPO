using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Common
{
    public static class EnumerableExtension
    {
        public static void Update<TSource>(this IEnumerable<TSource> outer, Action<TSource> updator)
        {
            foreach (var item in outer)
            {
                updator(item);
            }
        }
    }
}