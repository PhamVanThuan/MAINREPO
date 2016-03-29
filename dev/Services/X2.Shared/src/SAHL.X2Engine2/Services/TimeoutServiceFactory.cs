using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Services
{
    public class TimeoutServiceFactory : ITimeoutServiceFactory
    {
        private IX2EngineConfigurationProvider engineConfigurationProvider;

        public TimeoutServiceFactory(IX2EngineConfigurationProvider engineConfigurationProvider)
        {
            this.engineConfigurationProvider = engineConfigurationProvider;
        }

        public ITimeoutService Create(IX2Request request,IX2RequestMonitorCallback requestCallback, IResponseThreadWaiter threadWaiter)
        {
            return new TimeoutService(request, requestCallback, engineConfigurationProvider.GetRequestTimeoutInMilliseconds(), threadWaiter);
        }
    }
}