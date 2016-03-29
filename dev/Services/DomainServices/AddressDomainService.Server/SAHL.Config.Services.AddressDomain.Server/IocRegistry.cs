using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using StructureMap.Configuration.DSL;
using SAHL.Core.Rules;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.AddressDomain.Rules;
using SAHL.Services.Interfaces.AddressDomain.Model;

namespace SAHL.Config.Services.AddressDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<AddressDataModel>>().Use<DomainRuleManager<AddressDataModel>>();
            For<IDomainRuleManager<ClientAddressModel>>().Use<DomainRuleManager<ClientAddressModel>>();
        }
    }
}