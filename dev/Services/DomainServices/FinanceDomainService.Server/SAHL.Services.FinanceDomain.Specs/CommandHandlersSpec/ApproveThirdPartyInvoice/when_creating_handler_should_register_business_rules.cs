using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.FinanceDomain.Rules;
using SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ApproveThirdPartyInvoice
{
    public class when_creating_handler_should_register_business_rules : WithCoreFakes
    {
        private static ApproveThirdPartyInvoiceCommandHandler handler;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceApprovalMandateProvider approvalMandateProvider;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static IThirdPartyInvoiceManager manager;
        private static IDomainRuleManager<ServiceRequestMetadata> metadataDomainRuleManager;

        private Establish context = () =>
        {
            metadataDomainRuleManager = An<IDomainRuleManager<ServiceRequestMetadata>>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            approvalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            manager = An<IThirdPartyInvoiceManager>();
        };

        private Because of = () =>
        {
            handler = new ApproveThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager
                , eventRaiser
                , serviceCommandRouter
                , approvalMandateProvider
                , domainRuleManager
                , metadataDomainRuleManager
                , manager);
        };

        private It should_ensure_the_invoice_is_valid = () =>
        {
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<InvoiceMustHaveAtLeastOneLineItemRule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<ThirdPartyMustBeCapturedAgainstTheInvoiceRule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<InvoiceNumberMustBeCapturedRule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<InvoiceDateMustBeCapturedRule>()));
        };

        private It should_register_the_approval_mandate_rules = () =>
        {
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<UserWithApproverUnderR15000CapabilityCannotApproveInvoicesGreaterThanR15000Rule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule>()));
            domainRuleManager.WasToldTo(y => y.RegisterRule(Param.IsAny<UserWithApproverUpToR60000CapabilityCannotApproveInvoicesGreaterThanR60000Rule>()));
        };

        private It should_register_the_metadata_rules = () =>
        {
            metadataDomainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<AllCapabilitiesMustBeLinkedToUserRule>()));
        };
    }
}