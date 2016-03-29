using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;
using StructureMap.Configuration.DSL;
using System;
using StructureMap;

namespace SAHL.Config.Services.Halo.Client
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IHaloService>().Use<HaloServiceClient>().Ctor<IServiceUrlConfiguration>().Is(x => x.GetInstance<IServiceUrlConfiguration>("HaloService"));
        }
    }
}