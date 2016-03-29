using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Workflow.PersonalLoan
{
    public class CreateAndOpenPersonalLoanCommandHandler :IHandlesDomainServiceCommand<CreateAndOpenPersonalLoanCommand>
    {
        IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        IApplicationRepository applicationRepository;
        ICommonRepository commonRepository;

        public CreateAndOpenPersonalLoanCommandHandler(IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository, IApplicationRepository applicationRepository, ICommonRepository commonRepository)
        {
            this.applicationUnsecuredLendingRepository = applicationUnsecuredLendingRepository;
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, CreateAndOpenPersonalLoanCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            applicationUnsecuredLendingRepository.CreateAndOpenPersonalLoan(application.ReservedAccount.Key, command.UserID);
            commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
        }
    }
}
