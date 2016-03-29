using SAHL.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
	public class CapitecFailedMessageProcessorConfigurationProvider : ICapitecFailedMessageProcessorConfigurationProvider
	{
		private AppSettingsSection appSettingsSection;
		public CapitecFailedMessageProcessorConfigurationProvider(IConfigurationProvider configurationProvider)
		{
			this.appSettingsSection = configurationProvider.Config.AppSettings;
		}
		public int MessagePollInterval
		{
			get
			{
				return Convert.ToInt32(appSettingsSection.Settings["MessagePollInterval"].Value);
			}
		}
	}
}
