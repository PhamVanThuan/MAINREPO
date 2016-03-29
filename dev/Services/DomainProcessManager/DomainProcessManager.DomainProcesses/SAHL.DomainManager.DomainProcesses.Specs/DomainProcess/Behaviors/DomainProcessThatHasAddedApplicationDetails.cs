using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.FinancialDomain.Commands;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasAddedApplicationDetails
    {
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static IFinancialDomainServiceClient financialDomainService;
        protected static int applicationNumber;

        private It should_add_application_household_income = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<DetermineApplicationHouseholdIncomeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_application_employment_type = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<SetApplicationEmploymentTypeCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_price_the_application = () =>
        {
            financialDomainService.WasToldTo(x => x.PerformCommand(
                Param<PriceNewBusinessApplicationCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_fund_the_application = () =>
        {
            financialDomainService.WasToldTo(x => x.PerformCommand(
                Param<FundNewBusinessApplicationCommand>.Matches(m => m.ApplicationNumber == applicationNumber),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}