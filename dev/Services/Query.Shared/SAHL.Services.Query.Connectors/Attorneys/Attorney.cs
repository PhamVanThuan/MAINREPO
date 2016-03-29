using System.Dynamic;
using SAHL.Services.Interfaces.Query;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Query.Connector;

namespace SAHL.Services.Query.Connectors.Attorneys
{

    public interface IAttorney { }

    public class Attorney : IAttorney
    {
        private readonly IQueryServiceClient queryServiceClient;
        private readonly string getList = "/api/attorneys/";
        private readonly string getCount = "/api/attorneys/count";
        private readonly string getId = "/api/attorneys/{id}";

        public Attorney(IQueryServiceClient queryServiceClient)
        {
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
    
    }
    
    public interface IQueryServiceConnector
    {
        IQuery Find();
        IQueryWithIncludes FindById();
    }

}