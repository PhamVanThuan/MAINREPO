using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
	public class Timer : ITimer
	{
		private System.Threading.Timer timer;
		public Timer(Action<object> action, int interval)
		{
			timer = new System.Threading.Timer(new System.Threading.TimerCallback(action), null, 1000, System.Threading.Timeout.Infinite);
		}
	
		public void Stop()
		{
			timer = null;
		}
	}
}
