namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class ReactivatePolicyCommandHandler : IHandlesDomainServiceCommand<ReactivatePolicyCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private IAccountRepository AccountRepository;
        private ILifeRepository LifeRepository;
        private ILookupRepository LookupRepository;
        private IReportRepository ReportRepository;
        private ICorrespondenceRepository CorrespondenceRepository;
        private ICommonRepository commonRepository;

        public ReactivatePolicyCommandHandler(IApplicationRepository applicationRepository,
            IAccountRepository accountRepository,
            ILifeRepository lifeRepository,
            ILookupRepository lookupRepository,
            IReportRepository reportRepository,
            ICorrespondenceRepository correspondenceRepository,
                        ICommonRepository commonRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.AccountRepository = accountRepository;
            this.LifeRepository = lifeRepository;
            this.LookupRepository = lookupRepository;
            this.ReportRepository = reportRepository;
            this.CorrespondenceRepository = correspondenceRepository;
            this.commonRepository = commonRepository;        
        }

        public void Handle(IDomainMessageCollection messages, ReactivatePolicyCommand command)
        {
            // Get the application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            // Open the Application
            applicationLife.DateLastUpdated = System.DateTime.Now;
            applicationLife.ApplicationStatus = LookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)OfferStatuses.Open)];
            ApplicationRepository.SaveApplication(applicationLife);

            // add rule exclusion set
            command.ExclusionSets.Add(RuleExclusionSets.LifeApplicationNTUReinstate);

            // Close the Account
            AccountRepository.UpdateAccount(applicationLife.Account.Key, null, 0, "X2");
            commonRepository.RefreshDAOObject<IAccount>(applicationLife.Account.Key);

            IReportStatement reportStatement = ReportRepository.GetReportStatementByNameAndOSP("NTU Letter", applicationLife.Account.OriginationSourceProduct.Key);
            int genericKeyTypeKey = (int)GenericKeyTypes.Account;

            // Recalculate Premiums - No transactions generated, recalc discount factor = true
            LifeRepository.RecalculateSALifePremium((IAccountLifePolicy)applicationLife.Account, true);

            // Remove any pending NTU Letters from the Correspondence  / CorrespondenceParameters tables
            CorrespondenceRepository.RemoveCorrespondenceByReportStatementAndGenericKey(reportStatement.Key, applicationLife.Account.Key, genericKeyTypeKey, true);
        }
    }
}