using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.DecisionTreeResult;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.ITC;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class with_applicant_itc_service : WithFakes
    {
        protected static ITCManager applicantITCService;
        protected static IApplicantDataManager applicantDataService;
        protected static ILookupManager lookupService;
        protected static IDecisionTreeServiceClient decisionTreeService;
        protected static IDecisionTreeResultManager decisionTreeResultService;
        protected static IItcServiceClient itcServiceClient;
        protected static IITCDataManager applicantItcDataService;
        protected static ILogger logger;
        protected static ILoggerSource loggerSource;

        private Establish context = () =>
        {
            applicantDataService = An<IApplicantDataManager>();
            itcServiceClient = An<IItcServiceClient>();
            lookupService = An<ILookupManager>();
            decisionTreeService = An<IDecisionTreeServiceClient>();
            decisionTreeResultService = An<IDecisionTreeResultManager>();
            applicantItcDataService = An<IITCDataManager>();

            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();
            applicantITCService = new ITCManager(lookupService, applicantDataService, applicantItcDataService, decisionTreeService,
                decisionTreeResultService, logger, loggerSource, itcServiceClient);
        };
    }
}