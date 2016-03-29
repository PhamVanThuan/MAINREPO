namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class DeclineQuoteCommandHandler : IHandlesDomainServiceCommand<DeclineQuoteCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private IAccountRepository AccountRepository;
        private ILookupRepository LookupRepository;
        private IReportRepository ReportRepository;
        private ICorrespondenceRepository CorrespondenceRepository;
        private ICommonRepository commonRepository;

        public DeclineQuoteCommandHandler(IApplicationRepository applicationRepository,
            IAccountRepository accountRepository,
            ILookupRepository lookupRepository,
            IReportRepository reportRepository,
            ICorrespondenceRepository correspondenceRepository,
            ICommonRepository commonRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.AccountRepository = accountRepository;
            this.LookupRepository = lookupRepository;
            this.ReportRepository = reportRepository;
            this.CorrespondenceRepository = correspondenceRepository;
            this.commonRepository = commonRepository;        }

        public void Handle(IDomainMessageCollection messages, DeclineQuoteCommand command)
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
            AccountRepository.UpdateAccount(applicationLife.Account.Key, null, 0, "X2");
            commonRepository.RefreshDAOObject<IAccount>(applicationLife.Account.Key);

            IReportStatement reportStatement = ReportRepository.GetReportStatementByNameAndOSP("NTU Letter", applicationLife.Account.OriginationSourceProduct.Key);
            int genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Account;

            // Remove any pending Letters (except NTU Letter) from the Correspondence  / CorrespondenceParameters tables
            CorrespondenceRepository.RemoveCorrespondenceByGenericKey(applicationLife.Account.Key, genericKeyTypeKey, true, reportStatement.Key);
        }
    }
}