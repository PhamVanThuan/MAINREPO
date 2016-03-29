using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Workflow.DebtCounselling
{
	public class NTUOpenPersonalLoanCommand : StandardDomainServiceCommand
	{
		public int DebtCounsellingKey { get; protected set; }
		public NTUOpenPersonalLoanCommand(int debtCounsellingKey)
		{
			this.DebtCounsellingKey = debtCounsellingKey;
		}
	}
}
