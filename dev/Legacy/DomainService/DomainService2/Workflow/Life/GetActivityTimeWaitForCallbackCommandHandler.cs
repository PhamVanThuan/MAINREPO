namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    public class GetActivityTimeWaitForCallbackCommandHandler : IHandlesDomainServiceCommand<GetActivityTimeWaitForCallbackCommand>
    {
        private IApplicationRepository ApplicationRepository;

        public GetActivityTimeWaitForCallbackCommandHandler(IApplicationRepository applicationRepository)
        {
            this.ApplicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, GetActivityTimeWaitForCallbackCommand command)
        {
            // get the latest callback that hasnt been completed yet...and check the callback timer
            ICallback callback = ApplicationRepository.GetLatestCallBackByApplicationKey(command.ApplicationKey, true);

            if (callback != null)
                command.ActivityTimeResult = callback.CallbackDate;
            else
                command.ActivityTimeResult = DateTime.Now;
        }
    }
}