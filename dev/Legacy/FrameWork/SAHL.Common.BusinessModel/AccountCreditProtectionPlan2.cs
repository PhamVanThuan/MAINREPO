using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
	/// </summary>
	public partial class AccountCreditProtectionPlan : Account, IAccountCreditProtectionPlan
	{
		public override SAHL.Common.Globals.AccountTypes AccountType
		{
			get { return SAHL.Common.Globals.AccountTypes.CreditProtectionPlan; }
		}
	}
}


