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
    public class HaloWorkflowActionConditionalProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }
            if (!type.GetInterfaces().Contains(typeof(IHaloWorkflowActionConditionalProvider))) { return; }

            registry.For(typeof(IHaloWorkflowActionConditionalProvider)).Use(type);
        }
    }
}
