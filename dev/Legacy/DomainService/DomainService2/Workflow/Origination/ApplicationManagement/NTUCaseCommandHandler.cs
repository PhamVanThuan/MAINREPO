using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class NTUCaseCommandHandler : IHandlesDomainServiceCommand<NTUCaseCommand>
    {
        IApplicationRepository ApplicationRepository;
        ILookupRepository LookUpRepo;

        public NTUCaseCommandHandler(IApplicationRepository applicationRepository, ILookupRepository lookUpRepo)
        {
            this.ApplicationRepository = applicationRepository;
            this.LookUpRepo = lookUpRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, NTUCaseCommand command)
        {
            IApplication application = ApplicationRepository.GetApplicationByKey(command.ApplicationKey);

            application.ApplicationStatus = LookUpRepo.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)OfferStatuses.NTU)];

            ApplicationRepository.SaveApplication(application);
            command.Result = true;
        }
    }
}