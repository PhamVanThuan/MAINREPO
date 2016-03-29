using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Commands
{
    public class RefreshCacheCommand : ServiceCommand
    {
        public RefreshCacheCommand(Object data)
        {
            this.Data = data;
        }

        public Object Data { get; set; }
    }
}
