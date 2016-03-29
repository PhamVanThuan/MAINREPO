using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
	/// </summary>
	public partial interface IAccountPersonalLoan : IEntityValidation, IBusinessModelObject, IAccount
	{
	}
}


