using SAHL.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CapitecFailedMessageProcessor
{
	public interface ICapitecFailedMessageProcessor : IStartableService, IStoppableService
    {
		void Initialize();
		void ProcessFailedMessages(object state);
		void Teardown();
    }
}
