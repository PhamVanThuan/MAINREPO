using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.UI.Halo.Shared.Configuration;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.UI.Halo.Convention
{
    public class HaloWorkflowTileActionProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloWorkflowTileActionProvider`1"));
            if (configuration == null) { return; }

            var applicationModule      = configuration.GenericTypeArguments[0];
            var workflowActionProvider = typeof(IHaloWorkflowTileActionProvider<>);
            var genericType            = workflowActionProvider.MakeGenericType(applicationModule);

            registry.For(genericType).Use(type);
        }
    }
}
