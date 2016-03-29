using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap.Graph;
using StructureMap.Configuration.DSL;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Convention
{
    public class HaloWizardTileConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (!type.GetInterfaces().Contains(typeof (IHaloWizardTileConfiguration)) || type.IsAbstract) { return; }

            var wizardConfiguration = Activator.CreateInstance(type) as IHaloWizardTileConfiguration;
            if (wizardConfiguration == null) { return; }

            registry.For(typeof(IHaloWizardTileConfiguration)).Singleton().Use(type).Named(wizardConfiguration.Name);
        }
    }
}
