using SAHL.Services.Interfaces.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.SolrIndex.Tests.Extensions
{
    public static class EnumerableExtensions
    {
        private static Random random = new Random();

        public static T SelectRandom<T>(this IEnumerable<T> sequence) where T : SearchForThirdPartyQueryResult
        {
            if (sequence == null)
                throw new ArgumentNullException();
            if (!sequence.Any())
                return sequence.FirstOrDefault();
            int count = sequence.Count();
            int index = random.Next(0, count);
            return sequence.ElementAtOrDefault(index);
        }
    }
}
