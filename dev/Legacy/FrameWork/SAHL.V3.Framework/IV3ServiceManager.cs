using SAHL.Common.Security;
using SAHL.Core.SystemMessages;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework
{
    public interface IV3ServiceManager
    {
        T Get<T>() where T : IV3Service;

        IContainer IOCContainer {get;}

        void HandleSystemMessages(ISystemMessageCollection systemMessageCollection);
    }
}
