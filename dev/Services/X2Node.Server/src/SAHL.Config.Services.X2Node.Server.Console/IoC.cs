using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using SAHL.Config.Core.Conventions;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Data;
using SAHL.Config.Services.Core.Conventions;

namespace SAHL.Config.Services.X2.NodeServer.Console
{
    public class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.Convention<CommandHandlerDecoratorConvention>();
                    scan.LookForRegistries();
                });

            });

            return ObjectFactory.Container;
        }
    }
}
