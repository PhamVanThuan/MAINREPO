using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
	/// </summary>
	public partial class AccountCreditProtectionPlan : Account, IAccountCreditProtectionPlan
	{
		protected new SAHL.Common.BusinessModel.DAO.AccountCreditProtectionPlan_DAO _DAO;
		public AccountCreditProtectionPlan(SAHL.Common.BusinessModel.DAO.AccountCreditProtectionPlan_DAO AccountCreditProtectionPlan) : base(AccountCreditProtectionPlan)
		{
			this._DAO = AccountCreditProtectionPlan;
		}
	}
}


