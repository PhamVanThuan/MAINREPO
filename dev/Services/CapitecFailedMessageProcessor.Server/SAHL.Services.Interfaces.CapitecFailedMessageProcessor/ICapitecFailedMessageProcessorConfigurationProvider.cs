﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
	public interface ICapitecFailedMessageProcessorConfigurationProvider
	{
		int MessagePollInterval { get; }
	}
}
