using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Builders.Core;

namespace SAHL.Services.Query.Parsers
{
    public class PagingParser : IPagingParser
    {
        public IPagedPart FindPaging(string paging)
        {
            if (paging.Length == 0)
            {
                return null;
            }

            var pager = JsonConvert.DeserializeObject<PagingPart>(paging);

            if (pager.Paging == null)
            {
                return null;
            }
            CheckPageLimit(pager);
            return pager.Paging;
        }

        private static void CheckPageLimit(PagingPart pager)
        {
            if (pager.Paging.PageSize > 100)
            {
                pager.Paging.PageSize = 100;
            }
        }
    }
}
