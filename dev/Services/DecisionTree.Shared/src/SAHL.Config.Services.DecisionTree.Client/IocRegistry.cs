using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.DecisionTree;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.DecisionTree.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IServiceUrlConfigurationProvider>().Use<ServiceUrlConfigurationProvider>().Named("DecisionTreeUrlConfiguration")
                                                   .Ctor<string>("serviceName").Is("DecisionTreeService");

            For<IDecisionTreeServiceClient>().Use<DecisionTreeServiceClient>()
                                       .Ctor<IServiceUrlConfigurationProvider>()
                                            .Is(x => x.TheInstanceNamed("DecisionTreeUrlConfiguration"))
                                       .Ctor<IJsonActivator>("jsonActivator")
                                            .Is<JsonActivator>();
        }
    }
}