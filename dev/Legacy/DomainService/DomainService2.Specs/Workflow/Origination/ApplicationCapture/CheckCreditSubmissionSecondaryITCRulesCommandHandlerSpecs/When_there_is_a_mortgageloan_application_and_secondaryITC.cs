using System.Collections.Generic;
using System.Linq;
using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CheckCreditSubmissionSecondaryITCRulesCommandHandlerSpecs
{
    [Subject(typeof(CheckCreditSubmissionSecondaryITCRulesCommandHandler))]
    public class When_there_is_a_mortgageloan_application_and_secondaryITC : RuleSetDomainServiceSpec<CheckCreditSubmissionSecondaryITCRulesCommand, CheckCreditSubmissionSecondaryITCRulesCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static ICreditScoringRepository creditScoringRepository;

        private static IDictionary<string, object> creditScoringInformation;
        private static IITC secondaryITC;
        private static IScoreCard scoreCard;
        private static int riskMatrixRevisionKey;

        Establish context = () =>
        {
            var ignoreWarnings = false;

            messages = An<IDomainMessageCollection>();

            secondaryITC = An<IITC>();
            scoreCard = An<IScoreCard>();
            riskMatrixRevisionKey = -1;

            creditScoringInformation = new Dictionary<string, object>();
            creditScoringInformation.Add("secondaryITC", secondaryITC);
            creditScoringInformation.Add("scoreCard", scoreCard);
            creditScoringInformation.Add("riskMatrixRevisionKey", riskMatrixRevisionKey);

            applicationRepository = An<IApplicationRepository>();
            creditScoringRepository = An<ICreditScoringRepository>();

            command = new CheckCreditSubmissionSecondaryITCRulesCommand(Param.IsAny<int>(), ignoreWarnings);
            handler = new CheckCreditSubmissionSecondaryITCRulesCommandHandler(commandHandler, creditScoringRepository, applicationRepository);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(command.ApplicationKey)).Return((IApplicationMortgageLoan)null);
            creditScoringRepository.WhenToldTo(x => x.GetCreditScoreInfoForRules((IApplicationMortgageLoan)null)).Return(creditScoringInformation);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_set_rule_parameters_applicationkey = () =>
        {
            command.RuleParameters.Contains(command.ApplicationKey);
        };

        It should_set_rule_parameters_secondaryITC = () =>
        {
            command.RuleParameters.Contains(secondaryITC);
        };

        It should_set_rule_parameters_riskMatrixRevisionKey = () =>
        {
            command.RuleParameters.Contains(riskMatrixRevisionKey);
        };

        It should_set_rule_parameters_scoreCard = () =>
        {
            command.RuleParameters.Contains(scoreCard);
        };

        It should_set_the_rule_set_to_credit_scoring = () =>
        {
            command.WorkflowRuleSetName.ShouldBeEqualIgnoringCase(SAHL.Common.RuleSets.CreditScoring);
        };
    }
}