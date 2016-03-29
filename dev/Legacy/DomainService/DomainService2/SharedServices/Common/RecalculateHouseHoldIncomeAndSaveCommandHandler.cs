using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class RecalculateHouseHoldIncomeAndSaveCommandHandler : IHandlesDomainServiceCommand<RecalculateHouseHoldIncomeAndSaveCommand>
    {
        private IApplicationRepository applicationRepository;

        public RecalculateHouseHoldIncomeAndSaveCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, RecalculateHouseHoldIncomeAndSaveCommand command)
        {
            IApplication application = this.applicationRepository.GetApplicationByKey(command.ApplicationKey);
            application.CalculateHouseHoldIncome();
            this.applicationRepository.SaveApplication(application);
        }
    }
}