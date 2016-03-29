using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Factories
{
    public interface ITimerFactory
    {
        ITimer Get(Action<object> action);
    }
}
