using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Core
{
    public class PageSizePart : IPageSizePart
    {
        private readonly IQuery Query;
        private int PageSize { get; set; }

        public PageSizePart(IQuery query)
        {
            Query = query;
        }

        public IQuery WithPageSize(int pageSize)
        {
            PageSize = pageSize;
            return Query;
        }

        public string AsString()
        {
            return "pageSize: " + PageSize;
        }

    }
}