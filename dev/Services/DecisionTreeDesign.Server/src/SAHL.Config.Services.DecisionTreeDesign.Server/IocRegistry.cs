using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DecisionTreeDesign.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("Capitec"));
            });
        }
    }
}