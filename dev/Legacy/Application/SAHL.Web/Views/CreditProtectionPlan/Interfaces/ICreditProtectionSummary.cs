using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.CreditProtectionPlan.Interfaces
{
	public interface ICreditProtectionSummary : IViewBase
	{
		void BindAccountSummary(IAccountCreditProtectionPlan creditProtectionAccount);

        void BindLifePolicyClaimGrid(IList<SAHL.Common.BusinessModel.Interfaces.ILifePolicyClaim> lifePolicyClaims);
	}
}
