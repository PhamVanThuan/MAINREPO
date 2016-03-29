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
	public partial class AccountPersonalLoan : Account, IAccountPersonalLoan
	{
		protected new SAHL.Common.BusinessModel.DAO.AccountPersonalLoan_DAO _DAO;
		public AccountPersonalLoan(SAHL.Common.BusinessModel.DAO.AccountPersonalLoan_DAO AccountPersonalLoan) : base(AccountPersonalLoan)
		{
			this._DAO = AccountPersonalLoan;
		}
	}
}


