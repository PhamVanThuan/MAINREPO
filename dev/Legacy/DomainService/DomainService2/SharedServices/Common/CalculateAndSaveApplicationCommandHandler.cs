using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class CalculateAndSaveApplicationCommandHandler : IHandlesDomainServiceCommand<CalculateAndSaveApplicationCommand>
    {
        private IApplicationRepository applicationRepository;

		public CalculateAndSaveApplicationCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

		public void Handle(IDomainMessageCollection messages, CalculateAndSaveApplicationCommand command)
		{
			var application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
			application.CalculateApplicationDetail(command.IsBondExceptionAction, false);
			applicationRepository.SaveApplication(application);
		}
	}
}