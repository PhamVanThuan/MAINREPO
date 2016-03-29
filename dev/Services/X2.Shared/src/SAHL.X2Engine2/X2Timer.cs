using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2
{
    public class X2Timer : ITimer
    {
		private System.Threading.Timer timer;

        public X2Timer(Action<object> action, int interval)
		{
            timer = new System.Threading.Timer((x) => { action(x); }, null, 0, interval);
		}
	
		public void Stop()
		{
			timer = null;
		}
    }
}
