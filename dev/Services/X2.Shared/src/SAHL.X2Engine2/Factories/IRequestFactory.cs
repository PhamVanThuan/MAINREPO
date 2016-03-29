using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Factories
{
    public interface IRequestFactory
    {
        IX2Request CreateRequest(IX2BundledNotificationCommand command);
    }
}
