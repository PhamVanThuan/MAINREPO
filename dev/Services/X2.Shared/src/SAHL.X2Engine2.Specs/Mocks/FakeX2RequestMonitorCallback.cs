using SAHL.X2Engine2.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class FakeX2RequestMonitorCallback : IX2RequestMonitorCallback
    {
        public FakeX2RequestMonitorCallback()
        {

        }

        public void RequestCompleted(Core.X2.Messages.X2Response response)
        {
            
        }

        public void RequestTimedout(Core.X2.Messages.IX2Request request, IResponseThreadWaiter threadWaiter)
        {
        }
    }
}
