using SAHL.VSExtensions.Interfaces.Configuration;
using StructureMap.Graph;
using System.Linq;

namespace SAHomeloans.SAHL_VSExtensions.Mappings
{
    public class ISAHLDialogConvention : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(ISAHLConfiguration)))
            {
                registry.For(typeof(ISAHLConfiguration)).Use(type);
            }
        }
    }
}