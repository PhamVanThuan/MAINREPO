using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree;

namespace SAHL.Services.Capitec.Specs.ApplicationServiceSpecs
{
    public class with_application_service : WithFakes
    {
        protected static ILookupManager lookupService;
        protected static IApplicantManager applicantService;
        protected static IApplicationDataManager applicationDataService;
        protected static IITCManager applicantITCService;
        protected static IDecisionTreeServiceClient decisionTreeService;
        protected static IDecisionTreeResultManager decisionTreeResultService;
        protected static IServiceCommandRouter serviceCommandRouter;
        protected static ApplicationManager applicationService;

        private Establish context = () =>
            {
                lookupService = An<ILookupManager>();
                applicantService = An<IApplicantManager>();
                applicationDataService = An<IApplicationDataManager>();
                applicantITCService = An<IITCManager>();
                decisionTreeService = An<IDecisionTreeServiceClient>();
                decisionTreeResultService = An<IDecisionTreeResultManager>();
                serviceCommandRouter = An<IServiceCommandRouter>();
                applicationService = new ApplicationManager(lookupService, applicantService, applicationDataService, decisionTreeService, decisionTreeResultService, applicantITCService, serviceCommandRouter);
            };
    }
}