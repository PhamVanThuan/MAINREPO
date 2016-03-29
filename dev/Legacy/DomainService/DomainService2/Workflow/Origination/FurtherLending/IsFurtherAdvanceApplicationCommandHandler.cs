namespace DomainService2.Workflow.Origination.FurtherLending
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class IsFurtherAdvanceApplicationCommandHandler : IHandlesDomainServiceCommand<IsFurtherAdvanceApplicationCommand>
    {
        private IApplicationReadOnlyRepository appRepo;
        public const int furtherAdvanceApplicationTypeKey = 3;

        public IsFurtherAdvanceApplicationCommandHandler(IApplicationReadOnlyRepository applicationRepository)
        {
            if (applicationRepository == null)
            {
                throw new ArgumentNullException(Strings.ArgApplicationReadOnlyRepository);
            }
            this.appRepo = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsFurtherAdvanceApplicationCommand command)
        {
            int applicationType = this.appRepo.GetApplicationTypeFromApplicationKey(command.ApplicationKey);

            if (applicationType == furtherAdvanceApplicationTypeKey)
            {
                command.Result = true;
            }
            else
            {
                command.Result = false;
            }
        }
    }
}