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
    public class HaloWizardTilePageConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var configuration = type.GetInterfaces().SingleOrDefault(x => x.Name.StartsWith("IHaloWizardTilePageConfiguration`1"));
            if (configuration == null) { return; }

            var wizardTileConfiguration = configuration.GenericTypeArguments[0];
            var wizardTilePage          = typeof(IHaloWizardTilePageConfiguration<>);
            var genericType             = wizardTilePage.MakeGenericType(wizardTileConfiguration);

            registry.For(genericType).Use(type);
        }
    }
}
