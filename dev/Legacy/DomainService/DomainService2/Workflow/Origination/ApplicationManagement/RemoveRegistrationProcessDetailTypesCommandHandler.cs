using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class RemoveRegistrationProcessDetailTypesCommandHandler : IHandlesDomainServiceCommand<RemoveRegistrationProcessDetailTypesCommand>
    {
        IApplicationRepository applicationRepository;

        public RemoveRegistrationProcessDetailTypesCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, RemoveRegistrationProcessDetailTypesCommand command)
        {
            List<int> registrationProcessDetails = new List<int>();
            List<IDetail> detailsToRemove = new List<IDetail>();

            // List of detail types that need to be removed if found against the account
            registrationProcessDetails.Add((int)DetailTypes.Scheduled);
            registrationProcessDetails.Add((int)DetailTypes.InstructionNotSent);
            registrationProcessDetails.Add((int)DetailTypes.InstructionSent);
            registrationProcessDetails.Add((int)DetailTypes.InstructionReceived);
            registrationProcessDetails.Add((int)DetailTypes.ReplyReceived);
            registrationProcessDetails.Add((int)DetailTypes.RegistrationReceived);
            registrationProcessDetails.Add((int)DetailTypes.Lodged);
            registrationProcessDetails.Add((int)DetailTypes.ReAppliedforLodgement);
            registrationProcessDetails.Add((int)DetailTypes.ProceedWithLodgement);
            registrationProcessDetails.Add((int)DetailTypes.UnableToLodge);
            registrationProcessDetails.Add((int)DetailTypes.UpforFees);
            registrationProcessDetails.Add((int)DetailTypes.ClientWantsToNTU);
            registrationProcessDetails.Add((int)DetailTypes.ClientWonOver);

            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application.Account != null)
            {
                foreach (var detail in application.Account.Details)
                {
                    if (registrationProcessDetails.Contains(detail.DetailType.Key))
                    {
                        detailsToRemove.Add(detail);
                    }
                }

                foreach (var detail in detailsToRemove)
                {
                    application.Account.Details.Remove(messages, detail);
                }

                if (detailsToRemove.Count > 0)
                {
                    applicationRepository.SaveApplication(application);
                }
            }
        }
    }
}