namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class OlderThan45DaysCommandHandler : IHandlesDomainServiceCommand<OlderThan45DaysCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ILifeRepository LifeRepository;
        private ILookupRepository LookupRepository;

        public OlderThan45DaysCommandHandler(IApplicationRepository applicationRepository, ILifeRepository lifeRepository, ILookupRepository lookupRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.LifeRepository = lifeRepository;
            this.LookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, OlderThan45DaysCommand command)
        {
            // Get the application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            // if the offer status is NTU or Declined, call the NTU Archive method
            if (applicationLife.ApplicationStatus.Key == Convert.ToInt32(SAHL.Common.Globals.OfferStatuses.NTU)
             || applicationLife.ApplicationStatus.Key == Convert.ToInt32(SAHL.Common.Globals.OfferStatuses.Declined))
            {
                // Close the Account & Save the Stage Transition
                LifeRepository.CloseLifeApplication(applicationLife.Account.Key, applicationLife.Key, "NTU'd Application Archived");
            }

            // if the offer status is Open then perform the AUTO Archive
            if (applicationLife.ApplicationStatus.Key == Convert.ToInt32(SAHL.Common.Globals.OfferStatuses.Open))
            {
                // Close the Account & Save the Stage Transition
                LifeRepository.CloseLifeApplication(applicationLife.Account.Key, applicationLife.Key, "Policy Archived after 45 days");

                // Close the Application
                applicationLife.ApplicationStatus = LookupRepository.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferStatuses.Closed)];
                ApplicationRepository.SaveApplication(applicationLife);

                // Complete any outstanding callbacks and send the email
                if (ApplicationRepository.CompleteCallback(command.ApplicationKey, DateTime.Now))
                {
                    // Sends an internal email with the details of the pending call backs
                    LifeRepository.LifeApplicationArchivedWithCallBacks_SendInternalEmail(applicationLife.Account.Key, applicationLife.Key, command.InstanceID, applicationLife.Consultant.LegalEntity.EmailAddress);
                }

                // send an NTU Letter to the client
                LifeRepository.LifeApplicationSendNTU_Letter(applicationLife.Account.Key, applicationLife.Account.OriginationSourceProduct.Key);
            }
        }
    }
}