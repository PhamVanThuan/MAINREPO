using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Cuttlefish.Worker.Shared.Workers
{
    public class LatencyMessage_v2Worker : ICuttlefishMessageWorker
    {
        private IDbFactory dbFactory;

        public LatencyMessage_v2Worker(IDbFactory dbFactory)
        {

        }

        public void ProcessMessage(string queueMessage)
        {

        }
    }
}
