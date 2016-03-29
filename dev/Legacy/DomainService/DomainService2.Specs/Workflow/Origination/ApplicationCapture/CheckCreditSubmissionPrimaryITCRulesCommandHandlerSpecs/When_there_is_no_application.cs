using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CheckCreditSubmissionPrimaryITCRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditSubmissionPrimaryITCRulesCommandHandler))]
    public class When_there_is_no_application : RuleSetDomainServiceSpec<CheckCreditSubmissionPrimaryITCRulesCommand, CheckCreditSubmissionPrimaryITCRulesCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static ICreditScoringRepository creditScoringRepository;
        private static IDictionary<string, object> creditScoringInformation;
        private static IApplicationMortgageLoan application;

        Establish context = () =>
        {
            var ignoreWarnings = false;

            messages = An<IDomainMessageCollection>();
            creditScoringInformation = new Dictionary<string, object>();
            application = null;

            applicationRepository = An<IApplicationRepository>();
            creditScoringRepository = An<ICreditScoringRepository>();

            command = new CheckCreditSubmissionPrimaryITCRulesCommand(Param.IsAny<int>(), ignoreWarnings);
            handler = new CheckCreditSubmissionPrimaryITCRulesCommandHandler(commandHandler, creditScoringRepository, applicationRepository);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            creditScoringRepository.WhenToldTo(x => x.GetCreditScoreInfoForRules((IApplicationMortgageLoan)null)).Return(creditScoringInformation);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It the_rule_should_fail = () =>
        {
            command.Result.ShouldBeFalse();
        };

        It should_contain_no_credit_score_information = () =>
        {
            command.RuleParameters.ShouldBeEmpty();
        };
    }
}