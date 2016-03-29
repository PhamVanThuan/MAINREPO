namespace DomainService2.Workflow.Origination.FurtherLending
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class IsFurtherLoanApplicationCommandHandler : IHandlesDomainServiceCommand<IsFurtherLoanApplicationCommand>
    {
        private IApplicationReadOnlyRepository appRepo;
        public const int readvanceAdvanceApplicationTypeKey = 2;
        public const int furtherAdvanceApplicationTypeKey = 3;
        public const int furtherLoanApplicationTypeKey = 4;

        public IsFurtherLoanApplicationCommandHandler(IApplicationReadOnlyRepository applicationRepository)
        {
            if (applicationRepository == null)
            {
                throw new ArgumentNullException(Strings.ArgApplicationReadOnlyRepository);
            }
            this.appRepo = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, IsFurtherLoanApplicationCommand command)
        {
            int applicationType = this.appRepo.GetApplicationTypeFromApplicationKey(command.ApplicationKey);

            if (applicationType == furtherLoanApplicationTypeKey)
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