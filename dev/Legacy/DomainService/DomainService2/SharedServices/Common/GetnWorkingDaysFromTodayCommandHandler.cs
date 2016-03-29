using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetnWorkingDaysFromTodayCommandHandler : IHandlesDomainServiceCommand<GetnWorkingDaysFromTodayCommand>
    {
        private ICommonRepository commonRepository;

        public GetnWorkingDaysFromTodayCommandHandler(ICommonRepository commonRepository)
        {
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetnWorkingDaysFromTodayCommand command)
        {
            command.Result = this.commonRepository.GetnWorkingDaysFromToday(command.NDays);
        }
    }
}