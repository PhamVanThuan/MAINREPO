using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class CheckCreditSubmissionPrimaryITCRulesCommandHandler : RuleSetDomainServiceCommandHandler<CheckCreditSubmissionPrimaryITCRulesCommand>
    {
        private ICreditScoringRepository creditScoringRepository;
        private IApplicationRepository applicationRepository;

        public CheckCreditSubmissionPrimaryITCRulesCommandHandler(ICommandHandler commandHandler, ICreditScoringRepository creditScoringRepository, IApplicationRepository applicationRepository)
            : base(commandHandler)
        {
            this.creditScoringRepository = creditScoringRepository;
            this.applicationRepository = applicationRepository;
        }

        public override void SetupRule()
        {
            var applicationMortgageLoan = applicationRepository.GetApplicationByKey(command.ApplicationKey) as IApplicationMortgageLoan;

            //Attempt to get the Credit Score Information for the Rules
            IITC primaryITC = null;
            int riskMatrixRevisionKey = -1;
            IScoreCard scoreCard = null;

            var creditScoreInformation = creditScoringRepository.GetCreditScoreInfoForRules(applicationMortgageLoan);
            if (creditScoreInformation.Count == 0)
            {
                command.RuleParameters = new object[] { };
                return;
            }
            primaryITC = creditScoreInformation.ContainsKey("primaryITC") ? creditScoreInformation["primaryITC"] as IITC : null;
            riskMatrixRevisionKey = creditScoreInformation.ContainsKey("riskMatrixRevisionKey") ? (int)creditScoreInformation["riskMatrixRevisionKey"] : -1;
            scoreCard = creditScoreInformation.ContainsKey("scoreCard") ? creditScoreInformation["scoreCard"] as IScoreCard : null;

            if (primaryITC != null)
            {
                command.RuleParameters = new object[]
				{
					command.ApplicationKey,
					primaryITC,
					riskMatrixRevisionKey,
					scoreCard
				};
            }
        }
    }
}