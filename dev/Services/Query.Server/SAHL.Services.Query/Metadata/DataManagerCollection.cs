using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Interfaces.Query.DataManagers;

namespace SAHL.Services.Query.Metadata
{
    public class DataManagerCollection : IDataManagerCollection
    {
        private readonly IDictionary<Type, Type> dataManagers;
        private readonly IDbFactory dbFactory;
        private readonly IDataModelCoordinator dataModelCoordinator;

        public DataManagerCollection(IDictionary<Type, Type> dataManagers, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
        {
            this.dataManagers = dataManagers;
            this.dbFactory = dbFactory;
            this.dataModelCoordinator = dataModelCoordinator;
        }

        public IQueryServiceDataManager Get(Type dataModelType)
        {
            Type dataManagerType;
            if (!this.dataManagers.TryGetValue(dataModelType, out dataManagerType))
            {
                return null;
            }
            //we should be caching this data manager, and not new-ing it up every time
            return (IQueryServiceDataManager) Activator.CreateInstance(dataManagerType, dbFactory, dataModelCoordinator);
        }
    }
}
