using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Config.Data.Dapper;
using SAHL.Core.Data;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DependencyResolution
{
    public static class BootStrapper
    {
        public static IContainer Initialize()
        {
            try
            {
                ObjectFactory.Initialize(x =>
                {
                    x.Scan(scan =>
                    {
                        scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL.V3") || y.FullName.StartsWith("SAHL.Config") || y.FullName.StartsWith("SAHL.Core"));
                        scan.LookForRegistries();
                        scan.TheCallingAssembly();
                        scan.WithDefaultConventions();
                    });

                    x.For<IApplicationRepository>().Singleton().Use(RepositoryFactory.GetRepository<IApplicationRepository>());
                    x.For<ILegalEntityRepository>().Singleton().Use(RepositoryFactory.GetRepository<ILegalEntityRepository>());
                    x.For<ICreditScoringRepository>().Singleton().Use(RepositoryFactory.GetRepository<ICreditScoringRepository>());
                    x.For<IEmploymentRepository>().Singleton().Use(RepositoryFactory.GetRepository<IEmploymentRepository>());
                    
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("BootStrapper IContainer Initialize : {0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex)));
                Trace.Indent();
                throw ex;
            }
            return ObjectFactory.Container;
        }
    }
}

