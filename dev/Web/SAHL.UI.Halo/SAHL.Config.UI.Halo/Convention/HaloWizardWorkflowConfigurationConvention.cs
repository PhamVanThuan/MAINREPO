using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.UI.Halo.Convention
{
    public class HaloWizardWorkflowConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloWizardWorkflowConfiguration"));
            if (configuration == null) { return; }

            registry.For(configuration).Use(type);
        }
    }
}
