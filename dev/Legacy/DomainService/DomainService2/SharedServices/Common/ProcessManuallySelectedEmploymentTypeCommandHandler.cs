using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.SharedServices.Common
{
    public class ProcessApplicationForManuallySelectedEmploymentTypeCommandHandler : IHandlesDomainServiceCommand<ProcessApplicationForManuallySelectedEmploymentTypeCommand>
    {
        IApplicationRepository ApplicationRepo;
        public ProcessApplicationForManuallySelectedEmploymentTypeCommandHandler(IApplicationRepository applicationRepository)
        {
            this.ApplicationRepo = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, ProcessApplicationForManuallySelectedEmploymentTypeCommand command)
        {
            var application = ApplicationRepo.GetApplicationByKey(command.ApplicationKey);
            application.CreateRevision();
            application.SetManuallySelectedEmploymentType(command.SelectedEmploymentTypeKey);
            ApplicationRepo.DetermineGEPFAttribute(application);
            application.CalculateApplicationDetail(command.IsBondExceptionAction, false);
            ApplicationRepo.SaveApplication(application);
            application.PricingForRisk();
            ApplicationRepo.SaveApplication(application);
        }
    }
}
