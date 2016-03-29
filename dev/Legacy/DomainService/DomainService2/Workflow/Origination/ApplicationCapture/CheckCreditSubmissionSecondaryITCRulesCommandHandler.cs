using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckCreditSubmissionSecondaryITCRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckCreditSubmissionSecondaryITCRulesCommand>
    {
        private ICreditScoringRepository creditScoringRepository;
        private IApplicationRepository applicationRepository;

        public CheckCreditSubmissionSecondaryITCRulesCommandHandler(ICommandHandler commandHandler, ICreditScoringRepository creditScoringRepository, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.creditScoringRepository = creditScoringRepository;
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            var applicationMortgageLoan = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;

            //Attempt to get the Credit Score Information for the Rules
            IITC secondaryITC = null;
            int riskMatrixRevisionKey = -1;
            IScoreCard scoreCard = null;

            var creditScoreInformation = creditScoringRepository.GetCreditScoreInfoForRules(applicationMortgageLoan);
            if (creditScoreInformation.Count == 0)
            {
                command.RuleParameters = new object[] { };
                return;
            }
            secondaryITC = creditScoreInformation.ContainsKey("secondaryITC") ? creditScoreInformation["secondaryITC"] as IITC : null;
            riskMatrixRevisionKey = creditScoreInformation.ContainsKey("riskMatrixRevisionKey") ? (int)creditScoreInformation["riskMatrixRevisionKey"] : -1;
            scoreCard = creditScoreInformation.ContainsKey("scoreCard") ? creditScoreInformation["scoreCard"] as IScoreCard : null;

            if (secondaryITC != null)
            {
                command.RuleParameters = new object[]
				{
					command.ApplicationKey,
					secondaryITC,
					riskMatrixRevisionKey,
					scoreCard
				};
            }
        }
    }
}