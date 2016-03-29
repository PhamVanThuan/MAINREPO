using SAHL.Services.Interfaces.Query;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Query.Connector;

namespace SAHL.Services.Query.Connectors.Finance
{

    public interface IThirdPartyInvoice
    {
    }

    public class ThirdPartyInvoice : IThirdPartyInvoice
    {

        private readonly IQueryServiceClient queryServiceClient;
        private readonly string getList = "/api/finance/thirdpartyinvoices";
        private readonly string getCount = "/api/finance/thirdpartyinvoices/count";
        private readonly string getId = "/api/finance/thirdpartyinvoices/{id}";

        public ThirdPartyInvoice(IQueryServiceClient queryServiceClient)
        {
            Items = new ThirdPartyInvoiceInvoiceItems();
            Items.Documents = new ThirdPartyInvoiceDocuments(queryServiceClient);
            this.queryServiceClient = queryServiceClient;
        }

        public IQuery Find()
        {
            return new Connector.Query(queryServiceClient, getList);
        }

        public IQueryWithIncludes FindById(string id)
        {
 
            string getById = getId.Replace("{id}", id);
            return new QueryWithIncludes(queryServiceClient, getById);
        }

        public IQueryWithWhere GetCount()
        {
            return new QueryWithWhere();
        }

        public ThirdPartyInvoiceInvoiceItems ForInvoice(string id)
        {
            Items.Documents.ParentId = id;
            return Items;
        }

        private ThirdPartyInvoiceInvoiceItems Items { get; set; }
        
    }

    public class ThirdPartyInvoiceInvoiceItems
    {
        public ThirdPartyInvoiceDocuments Documents { get; set; }
    }

    


}