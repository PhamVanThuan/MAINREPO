using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.Common
{
    public class ClearCacheCommand : StandardDomainServiceCommand
    {
        public ClearCacheCommand(Object data)
        {
            this.Data = data;
        }

        public Object Data { get; set; }
    }
}
