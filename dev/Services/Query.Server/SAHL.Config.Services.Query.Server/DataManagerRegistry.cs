using System.Collections;
using SAHL.Config.Services.Query.Server.Scanners;
using SAHL.Core.Data;
using SAHL.Core.Data.Dapper;
using SAHL.Services.Query.Metadata;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;

namespace SAHL.Config.Services.Query.Server
{
    public class DataManagerRegistry : Registry
    {
        public DataManagerRegistry()
        {
            var scanner = new DataManagerScanner("SAHL.Services.Query");
            var mappedItems = scanner.GetMappings();

            this.For<IDataManagerCollection>()
                .Singleton()
                .Use<DataManagerCollection>()
                .Ctor<IDictionary<Type, Type>>()
                .Is(mappedItems);
        }
    }
}
