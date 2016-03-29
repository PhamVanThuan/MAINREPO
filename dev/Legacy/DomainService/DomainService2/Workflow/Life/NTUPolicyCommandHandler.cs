namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class NTUPolicyCommandHandler : IHandlesDomainServiceCommand<NTUPolicyCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private IAccountRepository AccountRepository;
        private ILookupRepository LookupRepository;
        private IReportRepository ReportRepository;
        private ICorrespondenceRepository CorrespondenceRepository;

        public NTUPolicyCommandHandler(IApplicationRepository applicationRepository,
            IAccountRepository accountRepository,
            ILookupRepository lookupRepository,
            IReportRepository reportRepository,
            ICorrespondenceRepository correspondenceRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.AccountRepository = accountRepository;
            this.LookupRepository = lookupRepository;
            this.ReportRepository = reportRepository;
            this.CorrespondenceRepository = correspondenceRepository;
        }

        public void Handle(IDomainMessageCollection messages, NTUPolicyCommand command)
        {
            // Get the application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            // Update the Application status
            applicationLife.DateLastUpdated = System.DateTime.Now;
            applicationLife.ApplicationStatus = LookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)OfferStatuses.NTU)];
            ApplicationRepository.SaveApplication(applicationLife);

            // add rule exclusion set
            command.ExclusionSets.Add(RuleExclusionSets.LifeApplicationNTUReinstate);

            // Update the Account
            //AccountRepository.UpdateAccount(applicationLife.Account.Key, null, 0, "X2");

            IReportStatement reportStatement = ReportRepository.GetReportStatementByNameAndOSP("NTU Letter", applicationLife.Account.OriginationSourceProduct.Key);
            int genericKeyTypeKey = (int)GenericKeyTypes.Account;

            // Remove any pending Letters (except NTU Letter) from the Correspondence  / CorrespondenceParameters tables
            CorrespondenceRepository.RemoveCorrespondenceByGenericKey(applicationLife.Account.Key, genericKeyTypeKey, true, reportStatement.Key);
        }
    }
}