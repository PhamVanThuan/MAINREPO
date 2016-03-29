using SAHL.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
	public class TimerFactory : ITimerFactory
	{
		private ICapitecFailedMessageProcessorConfigurationProvider configurationProvider;
		public TimerFactory(ICapitecFailedMessageProcessorConfigurationProvider configurationProvider)
		{
			this.configurationProvider = configurationProvider;
		}
		public ITimer Get(Action<object> action)
		{
			return new Timer(action, configurationProvider.MessagePollInterval);
		}
	}
}
