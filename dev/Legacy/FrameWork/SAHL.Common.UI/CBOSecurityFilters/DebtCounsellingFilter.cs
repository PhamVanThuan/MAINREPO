using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SAHL.Common.UI.CBOSecurityFilters
{
	/// <summary>
	/// Debt Counselling Filter
	/// </summary>
	public class DebtCounsellingFilter : GenericRegExFilter
	{
		public DebtCounsellingFilter()
			: base()
		{
			if (_instanceNode != null && _instanceNode.Instance != null
				&& _instanceNode.Instance.State != null
				&& _instanceNode.Instance.State.Name != "45 Day Reminder" 
				&& _instanceNode.Instance.State.Name != "Manage Proposal")
			{
				//If the State is not 45 Day Reminder or Manage Proposal, then filter out Proposal
				_filters.Add(new Regex("Manage Counter Proposals", RegexOptions.IgnoreCase));

				//If the State is in Pend Proposal, Filter out Counter Proposal
				if (_instanceNode.Instance.State.Name != "Pend Proposal")
				{
					_filters.Add(new Regex("Manage Proposals", RegexOptions.IgnoreCase));
				}
			}
		}
	}
}
