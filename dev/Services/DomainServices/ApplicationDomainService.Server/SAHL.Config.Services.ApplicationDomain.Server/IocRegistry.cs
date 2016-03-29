using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using StructureMap.Configuration.DSL;

namespace SAHL.Config.Services.ApplicationDomain.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<IDomainRuleManager<AddLeadApplicantToApplicationCommand>>().Use<DomainRuleManager<AddLeadApplicantToApplicationCommand>>();
            For<IDomainRuleManager<ApplicantDomiciliumAddressModel>>().Use<DomainRuleManager<ApplicantDomiciliumAddressModel>>();
            For<IDomainRuleManager<ApplicantDeclarationsModel>>().Use<DomainRuleManager<ApplicantDeclarationsModel>>();
            For<IDomainRuleManager<VendorModel>>().Use<DomainRuleManager<VendorModel>>();
            For<IDomainRuleManager<ApplicantAffordabilityModel>>().Use<DomainRuleManager<ApplicantAffordabilityModel>>();
            For<IDomainRuleManager<ApplicationDebitOrderModel>>().Use<DomainRuleManager<ApplicationDebitOrderModel>>();
            For<IDomainRuleManager<ApplicationMailingAddressModel>>().Use<DomainRuleManager<ApplicationMailingAddressModel>>();
            For<IDomainRuleManager<NewPurchaseApplicationModel>>().Use<DomainRuleManager<NewPurchaseApplicationModel>>();
            For<IDomainRuleManager<SwitchApplicationModel>>().Use<DomainRuleManager<SwitchApplicationModel>>();
            For<IDomainRuleManager<RefinanceApplicationModel>>().Use<DomainRuleManager<RefinanceApplicationModel>>();
            For<IDomainRuleManager<ApplicantDomiciliumAddressModel>>().Use<DomainRuleManager<ApplicantDomiciliumAddressModel>>();
            For<IDomainRuleManager<ApplicationAttributeModel>>().Use<DomainRuleManager<ApplicationAttributeModel>>();
            For<IDomainRuleManager<OfferInformationDataModel>>().Use<DomainRuleManager<OfferInformationDataModel>>();
            For<IDomainRuleManager<ApplicationAttributeModel>>().Use<DomainRuleManager<ApplicationAttributeModel>>();
            For<IDomainRuleManager<ApplicantRoleModel>>().Use<DomainRuleManager<ApplicantRoleModel>>();
            For<IDomainRuleManager<AffordabilityAssessmentModel>>().Use<DomainRuleManager<AffordabilityAssessmentModel>>();
        }
    }
}