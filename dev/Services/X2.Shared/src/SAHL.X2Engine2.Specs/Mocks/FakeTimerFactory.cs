using SAHL.X2Engine2.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.Mocks
{
    public class FakeTimerFactory : ITimerFactory
    {
        public FakeTimerFactory()
        {

        }
        public ITimer Get(Action<object> action)
        {
            var timer =  new InstantFakeTimer();
            timer.Start(0, action, null);
            return timer;

        }
    }
}
