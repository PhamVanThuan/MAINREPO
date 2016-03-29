using SAHL.Services.Calendar;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;

namespace SAHL.Config.Services.Calendar.Server
{
    public class JobConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var interfaces = type.GetInterfaces().Where(x => x.Name.StartsWith("IJobConfiguration")).ToList();
            foreach (var configuration in interfaces)
            {
                var jobConfiguration = typeof(IJobConfiguration);
                registry.For(jobConfiguration).Use(type);
            }
        }
    }
}
