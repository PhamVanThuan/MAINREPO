using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CATS
{
    public interface ICATSServiceClient : IServiceClient
    {
        ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata serviceRequestMetadata) where T : ICATSServiceCommand;

        ISystemMessageCollection PerformQuery<T>(T query) where T : ICATSServiceQuery;
    }
}
