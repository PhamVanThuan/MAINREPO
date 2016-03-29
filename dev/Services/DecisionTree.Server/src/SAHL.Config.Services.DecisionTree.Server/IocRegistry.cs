using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DecisionTree.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("Capitec"));
                x.Convention<DecisionTreeHandlerDecoratorConvention>();
            });
        }
    }
}