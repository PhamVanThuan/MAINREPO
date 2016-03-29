using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Workflow.ScenarioMaps
{
    public class ThrowDAO_ExceptionCommandHandler : IHandlesDomainServiceCommand<ThrowDAO_ExceptionCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ICommonRepository CommonRepository;

        public ThrowDAO_ExceptionCommandHandler(ICommonRepository commonRepository, IApplicationRepository applicationRepository)
        {
            this.CommonRepository = commonRepository;
            this.ApplicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, ThrowDAO_ExceptionCommand command)
        {
            ICallback callback = CommonRepository.CreateEmpty<ICallback, Callback_DAO>();
            callback.CallbackUser = null;
            ApplicationRepository.SaveCallback(callback);
        }
    }
}