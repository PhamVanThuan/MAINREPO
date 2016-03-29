using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Common
{
    public class Timer : ITimer
    {
        private System.Timers.Timer systemTimer;

        public void Start(int timeout, Action callback)
        {
            systemTimer = new System.Timers.Timer(timeout) { AutoReset = false };
            systemTimer.Elapsed += (sender, eventArgs) => { callback(); };
            systemTimer.Start();

        }

        public void Reset()
        {
            systemTimer.Stop();
            systemTimer.Start();
        }
    }
}
