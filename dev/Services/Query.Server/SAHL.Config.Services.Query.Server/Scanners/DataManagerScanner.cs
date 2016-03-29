using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using SAHL.Core.Data;
using SAHL.Core.Extensions;
using SAHL.Services.Interfaces.Query.DataManagers;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.DataManagers;
using StructureMap.TypeRules;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server.Scanners
{
    public class DataManagerScanner
    {
        private readonly string assemblyToScan;

        public DataManagerScanner(string assemblyToScan)
        {
            this.assemblyToScan = assemblyToScan;
        }

        public Dictionary<Type, Type> GetMappings()
        {
            var result = new Dictionary<Type, Type>();

            var dataModelTypes = GetDataModelTypes();
            var sqlStatementTypes = GetSqlStatementTypes();

            foreach (var dataModelType in dataModelTypes)
            {
                var name = dataModelType.Name.Replace("DataModel", string.Empty);

                var sqlStatementType = sqlStatementTypes
                    .FirstOrDefault(x => x.Name.Equals("Get" + name + "Statement", StringComparison.OrdinalIgnoreCase));

                if (sqlStatementType == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot find matching SqlStatement for a {0}DataModel", name));
                }

                var dataManagerType = typeof (QueryServiceDataManager<,>).MakeGenericType(dataModelType, sqlStatementType);

                result.Add(dataModelType, dataManagerType);
            }

            return result;
        }

        private IEnumerable<Type> GetSqlStatementTypes()
        {
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(a => a.Name.StartsWith("Get") && a.Name.EndsWith("Statement"));
        }

        private IEnumerable<Type> GetDataModelTypes()
        {
            Type type = typeof (IQueryDataModel);
            return Assembly
                .Load(this.assemblyToScan)
                .GetTypes()
                .Where(type.IsAssignableFrom);
        }
    }
}
