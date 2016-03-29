using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.Origination.ApplicationCapture
{
	public class SendNewClientConsultantDetailsSMSCommand : StandardDomainServiceCommand
	{
		public int ApplicationKey { get; protected set; }
		public SendNewClientConsultantDetailsSMSCommand(int applicationKey)
		{
			this.ApplicationKey = applicationKey;
		}
	}
}
