using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinancialDomain;
using System;

namespace SAHL.Services.FinancialDomain
{
    public class FinancialDomainService : IFinancialDomainService
    {
        private readonly IServiceCommandRouter _serviceCommandRouter;

        public FinancialDomainService(IServiceCommandRouter serviceCommandRouter)
        {
            if (serviceCommandRouter == null) { throw new ArgumentNullException("serviceCommandRouter"); }
            this._serviceCommandRouter = serviceCommandRouter;
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFinancialDomainCommand
        {
            return this._serviceCommandRouter.HandleCommand<T>(command,metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IFinancialDomainQuery
        {
            throw new NotImplementedException();
        }
    }
}