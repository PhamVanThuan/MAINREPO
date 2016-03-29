namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class ActivateLifePolicyCommandHandler : IHandlesDomainServiceCommand<ActivateLifePolicyCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ILifeRepository LifeRepository;
        private ILookupRepository LookupRepository;
        private ICommonRepository commonRepository;

        public ActivateLifePolicyCommandHandler(IApplicationRepository applicationRepository, ILifeRepository lifeRepository, ILookupRepository lookupRepository, ICommonRepository commonRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.LifeRepository = lifeRepository;
            this.LookupRepository = lookupRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, ActivateLifePolicyCommand command)
        {
            // Get the Life application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            DateTime dateNow = System.DateTime.Now;

            // Accept the application & Update the DateOfAcceptance on the OfferLife record
            applicationLife.ApplicationStatus = LookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferStatuses.Accepted)];
            applicationLife.DateLastUpdated = dateNow;
            applicationLife.DateOfAcceptance = dateNow;
            ApplicationRepository.SaveApplication(applicationLife);

            // Create the LifePolicy and FinancialService
            LifeRepository.CreateLifePolicy(applicationLife.Account.Key);
            commonRepository.RefreshDAOObject<IApplicationLife>(command.ApplicationKey);
        }
    }
}