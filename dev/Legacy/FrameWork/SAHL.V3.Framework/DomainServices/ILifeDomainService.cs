using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.DomainServices
{
    public interface ILifeDomainService : IV3Service
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery;

        ISystemMessageCollection PerformCommand<T>(T command) where T : IServiceCommand;

        bool CreateClaim(int AccountLifePolicyKey, int LegalEntityKey, out long instanceId);
    }
}
