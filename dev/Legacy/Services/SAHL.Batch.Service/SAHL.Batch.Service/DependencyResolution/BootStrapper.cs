using SAHL.Batch.Common;
using SAHL.Core.Logging;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Diagnostics;
using StructureMap.Configuration.DSL;

namespace SAHL.Batch.Service.DependencyResolution
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
                            scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                            scan.LookForRegistries();
                        });
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