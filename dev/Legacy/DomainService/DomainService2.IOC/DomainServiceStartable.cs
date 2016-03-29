using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.IOC
{
    public class DomainServiceStartable : MarshalByRefObject, SAHL.Core.X2.Shared.IDomainServiceStartable
    {
        public void Start(string processName)
        {
            DomainServiceLoader.ProcessName = processName;
            var loader = DomainServiceLoader.Instance;
        }
    }
}
