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
using SAHL.Common.Security;
using SAHL.Common.Globals;
using System.Security.Principal;
using SAHL.Common.DomainMessages;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Valuation : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Valuation_DAO>, IValuation
	{

		public override void OnPopulateRules(List<string> Rules)
		{
			base.OnPopulateRules(Rules);

			Rules.Add("ValuationValuer");
			Rules.Add("ValuationValuationDateThreshold");
			Rules.Add("ValuationValuationAmountMinimum");
			Rules.Add("ValuationHOCRoof");
			Rules.Add("ValuationHOCAmount");
			Rules.Add("ValuationActiveStatus");
			Rules.Add("ValuationTypeValidation");
		}

	   

	}
}


