using System.Runtime.InteropServices.ComTypes;
using SAHL.Core;
using SAHL.Core.Validation;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Core
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IIocContainer>().Use<StructureMapIocContainer>();
            For<ITypeMetaDataLookup>().Singleton().Use<TypeMetaDataLookup>();
        }
    }
}