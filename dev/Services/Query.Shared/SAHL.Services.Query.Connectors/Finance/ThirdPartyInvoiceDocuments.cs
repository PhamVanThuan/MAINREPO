using SAHL.Services.Interfaces.Query;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Query.Connector;

namespace SAHL.Services.Query.Connectors.Finance
{

    public interface IThirdPartyInvoiceDocuments
    {
    }

    public class ThirdPartyInvoiceDocuments : IThirdPartyInvoiceDocuments
    {
        private readonly IQueryServiceClient queryServiceClient;
        private string getList = "/api/finance/thirdpartyinvoices/{invoiceId}/documents";
        private string getCount = "/api/finance/thirdpartyinvoices/count";
        private string getId = "/api/finance/thirdpartyinvoices/{invoiceId}/documents/{documentId}";

        public string ParentId { get; set; }

        public ThirdPartyInvoiceDocuments(IQueryServiceClient queryServiceClient)
        {
            this.queryServiceClient = queryServiceClient;
        }

        public IQuery Find()
        {
            string getListItems = getList.Replace("{invoiceId}", ParentId);
            return new Connector.Query(queryServiceClient, getListItems);
        }

        public IQueryWithIncludes FindById(string id)
        {
            string getById = getId.Replace("{documentId}", id);
            getById = getById.Replace("{invoiceId}", ParentId);
            return new QueryWithIncludes(queryServiceClient, getById);
        }

        public IQueryWithWhere GetCount()
        {
            return new QueryWithWhere();
        }

    }

}