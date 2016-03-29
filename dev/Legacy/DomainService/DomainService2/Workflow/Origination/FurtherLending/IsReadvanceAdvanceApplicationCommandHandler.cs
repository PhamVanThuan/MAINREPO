namespace DomainService2.Workflow.Origination.FurtherLending
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class IsReadvanceAdvanceApplicationCommandHandler : IHandlesDomainServiceCommand<IsReadvanceAdvanceApplicationCommand>
    {
        private IApplicationReadOnlyRepository appRepo;
        public const int readvanceAdvanceApplicationTypeKey = 2;

        public IsReadvanceAdvanceApplicationCommandHandler(IApplicationReadOnlyRepository applicationRepository)
        {
            if (applicationRepository == null)
            {
                throw new ArgumentNullException(Strings.ArgApplicationReadOnlyRepository);
            }
            this.appRepo = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsReadvanceAdvanceApplicationCommand command)
        {
            int applicationType = this.appRepo.GetApplicationTypeFromApplicationKey(command.ApplicationKey);

            if (applicationType == readvanceAdvanceApplicationTypeKey)
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