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
    public class HaloTileEditorDataProviderConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsAbstract) { return; }

            var editorDataProviderConfigurations = type.GetInterfaces().Where(x => x.Name.StartsWith("IHaloTileEditorDataProvider`1"));
            if (!editorDataProviderConfigurations.Any()) { return; }

            foreach (var currentTileDataConfiguration in editorDataProviderConfigurations)
            {
                var haloTileConfiguration   = currentTileDataConfiguration.GenericTypeArguments[0];
                var halotileContentProvider = typeof(IHaloTileEditorDataProvider<>);
                var genericType             = halotileContentProvider.MakeGenericType(haloTileConfiguration);

                registry.For(genericType).Use(type);
            }
        }
    }
}
