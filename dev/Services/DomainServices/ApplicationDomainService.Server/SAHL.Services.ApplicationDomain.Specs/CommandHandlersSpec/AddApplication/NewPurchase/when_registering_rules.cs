using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplication.NewPurchase
{
    public class when_registering_rules : WithCoreFakes
    {
        private static AddNewPurchaseApplicationCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IDomainRuleManager<NewPurchaseApplicationModel> domainRuleContext;
        private static IDomainQueryServiceClient domainQueryService;        
        private static IApplicationManager applicationManager;        

        private Establish context = () =>
        {
            //create mock objects
            domainRuleContext = An<IDomainRuleManager<NewPurchaseApplicationModel>>();
            applicationManager = An<IApplicationManager>();
            applicationDataManager = An<IApplicationDataManager>();                    
            domainQueryService = An<IDomainQueryServiceClient>();
        };

        private Because of = () =>
        {
            handler = new AddNewPurchaseApplicationCommandHandler(serviceCommandRouter, applicationDataManager, linkedKeyManager, eventRaiser, unitOfWorkFactory, applicationManager, 
                domainRuleContext);
        };

        private It should_register_NewBusinessRequestedLoanAmountMustBeAboveMinimumRequiredRule = () =>
        {
            domainRuleContext.WasToldTo(x => x.RegisterRule(Param.IsAny<NewBusinessRequestedLoanAmountMustBeAboveMinimumRequiredRule>()));
        };       
    }
}
