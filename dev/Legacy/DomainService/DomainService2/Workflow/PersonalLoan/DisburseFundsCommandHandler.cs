using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Workflow.PersonalLoan
{
    public class DisburseFundsCommandHandler : IHandlesDomainServiceCommand<DisburseFundsCommand>
    {
        IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        IApplicationRepository applicationRepository;
        ICommonRepository commonRepository;

        public DisburseFundsCommandHandler(IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository, IApplicationRepository applicationRepository, ICommonRepository commonRepository)
        {
            this.applicationUnsecuredLendingRepository = applicationUnsecuredLendingRepository;
            this.applicationRepository = applicationRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, DisburseFundsCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            applicationUnsecuredLendingRepository.DisbursePersonalLoan(application.ReservedAccount.Key, command.UserID);
            commonRepository.RefreshDAOObject<IApplication>(command.ApplicationKey);
            commonRepository.RefreshDAOObject<IAccount>(application.ReservedAccount.Key);
        }
    }
}
