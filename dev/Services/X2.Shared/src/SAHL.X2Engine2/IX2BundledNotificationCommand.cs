using SAHL.Core.Services;
using SAHL.X2Engine2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2
{
    public interface IX2BundledNotificationCommand
    {
        X2BundledNotificationCommandType CommandType {get;}
    }
}
