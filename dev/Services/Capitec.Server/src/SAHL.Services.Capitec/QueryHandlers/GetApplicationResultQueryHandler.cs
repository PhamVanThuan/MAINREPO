using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using System.Linq;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class GetApplicationResultQueryHandler : IServiceQueryHandler<GetApplicationResultQuery>
    {
        private IApplicationManager applicationService;
        private IDecisionTreeResultManager decisionTreeResultService;
        
        public GetApplicationResultQueryHandler(IApplicationManager applicationService, IDecisionTreeResultManager decisionTreeResultService)
        {
            this.applicationService = applicationService;
            this.decisionTreeResultService = decisionTreeResultService;
        }

        public ISystemMessageCollection HandleQuery(GetApplicationResultQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            query.Result = new GetApplicationResultQueryResult();

            var applicants = applicationService.GetApplicantsForApplication(query.ApplicationID).Where(x=>x.IncomeContributor).OrderBy(x=>x.Name).ToList();

            bool itcsPassed = true;
            var firstItcResult = decisionTreeResultService.GetITCResultForApplicant(applicants[0].ID);
            if (firstItcResult != null && firstItcResult.ITCMessages != null)
            {
                query.Result.FirstApplicantITCMessages = new SystemMessageCollection(firstItcResult.ITCMessages.AllMessages.Where(x => x.Severity == SystemMessageSeverityEnum.Warning).ToList());
                query.Result.FirstApplicantITCPassed = firstItcResult.ITCPassed;
                query.Result.FirstApplicantName = applicants[0].Name;

                itcsPassed = firstItcResult.ITCPassed;
            }
            if (applicants.Count == 2)
            {
                var secondItcResult = decisionTreeResultService.GetITCResultForApplicant(applicants[1].ID);
                if(secondItcResult!=null)
                {
                    query.Result.SecondApplicantITCMessages = new SystemMessageCollection(secondItcResult.ITCMessages.AllMessages.Where(x=>x.Severity==SystemMessageSeverityEnum.Warning).ToList());
                    query.Result.SecondApplicantITCPassed = secondItcResult.ITCPassed;
                    query.Result.SecondApplicantName = applicants[1].Name;

                    if (itcsPassed) { itcsPassed = secondItcResult.ITCPassed; }
                }
            }

            query.Result.ApplicationCalculationMessages = SystemMessageCollection.Empty();
            if (itcsPassed)
            {
                var calculationResult = decisionTreeResultService.GetCalculationResultForApplication(query.ApplicationID);
                if (calculationResult != null)
                {
                    query.Result.ApplicationCalculationMessages = new SystemMessageCollection(calculationResult.Messages.AllMessages.Where(x=>x.Severity==SystemMessageSeverityEnum.Warning).ToList());
                    query.Result.Submitted = calculationResult.EligibleApplication;
                }
            }
            query.Result.ApplicationNumber = applicationService.GetApplicationNumberForApplication(query.ApplicationID);

            return messages;
        }
    }
}
