using SAHL.Config.Core.Conventions;
using SAHL.Config.Services.Core.Conventions;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.X2Engine.Console
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
