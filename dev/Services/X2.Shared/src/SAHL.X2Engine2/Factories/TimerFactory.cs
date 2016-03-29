using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Factories
{
    public class TimerFactory : ITimerFactory
    {
        private const int interval = 2000;

        public TimerFactory()
        { }

        public ITimer Get(Action<object> action)
        {
            return new X2Timer(action, interval);
        }
    }
}
