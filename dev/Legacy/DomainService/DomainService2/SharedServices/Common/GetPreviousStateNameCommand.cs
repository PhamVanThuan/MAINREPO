using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.SharedServices.Common
{
	public class GetPreviousStateNameCommand : StandardDomainServiceCommand
	{
		public long InstanceID { get; set; }
		public string Result { get; set; }
		public GetPreviousStateNameCommand(long instanceID)
		{
			this.InstanceID = instanceID;
		}
	}
}
