 using System.Linq;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Parsers;

namespace SAHL.Services.Query.Controllers
{
    public class DataManagerQueryResult
    {
        public string Id { get; private set; }
        public IQueryDataModel DataModel { get; private set; }
        public IQueryServiceDataManager DataManager { get; private set; }
        public IFindQuery FindQuery { get; private set; }

        public DataManagerQueryResult(IQueryServiceDataManager dataManager, IQueryDataModel dataModel, string id, IFindQuery findQuery)
        {
            this.Id = id;
            this.DataModel = dataModel;
            this.DataManager = dataManager;
            this.FindQuery = findQuery;

            //If we have at least 1 where clause, then ignore id
            if (findQuery != null && findQuery.Where != null && findQuery.Where.Any())
            {
                this.Id = null;
            }
        }
    }
}