using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Core
{
    public class PagingPart : IPagingPart
    {
        private readonly IQuery Query;
        private int currentPage;
        private IPageSizePart pageSizePart;

        public PagingPart(IQuery query)
        {
            Query = query;
        }

        public IPageSizePart SetCurrentPageTo(int page)
        {
            pageSizePart = new PageSizePart(Query);
            currentPage = page;
            return pageSizePart;
        }

        public string AsString()
        {
            return "Paging: {currentPage: " + currentPage + ", " + pageSizePart.AsString() + "}";
        }


    }
}