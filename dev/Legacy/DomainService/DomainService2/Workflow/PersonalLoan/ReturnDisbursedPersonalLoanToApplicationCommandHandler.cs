using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Workflow.PersonalLoan
{
    public class ReturnDisbursedPersonalLoanToApplicationCommandHandler :IHandlesDomainServiceCommand<ReturnDisbursedPersonalLoanToApplicationCommand>
    {
        IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        IApplicationRepository applicationRepository;
        ICommonRepository commonRepository;

        public ReturnDisbursedPersonalLoanToApplicationCommandHandler(IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository, IApplicationRepository applicationRepository, ICommonRepository commonRepository)
        {
            this.applicationUnsecuredLendingRepository = applicationUnsecuredLendingRepository;
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, ReturnDisbursedPersonalLoanToApplicationCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            applicationUnsecuredLendingRepository.ReturnDisbursedPersonalLoanToApplication(application.Account.Key);
            commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
        }
    }
}
