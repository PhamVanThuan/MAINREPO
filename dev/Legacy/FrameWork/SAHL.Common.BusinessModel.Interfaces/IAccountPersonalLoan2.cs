using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
	/// </summary>
	public partial interface IAccountPersonalLoan : IEntityValidation, IBusinessModelObject, IAccount
	{
		double AccruedInterestMTD { get; }
		void CalculateInterest(out double interestMonthToDate, out double interestTotalforMonth);
        //double TotalInstalment { get; }
		int MaxNewRemainingInstalmentsAllowed { get; }
		int MaxTermExtension { get; }
        IExternalLifePolicy ExternalLifePolicy { get; }
	}
}
