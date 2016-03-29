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
	/// This is derived from Valuation_DAO. When instantiated it represents a Client Estimate Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedSAHLClientEstimate : Valuation, IValuationDiscriminatedSAHLClientEstimate
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedSAHLClientEstimate_DAO _DAO;
		public ValuationDiscriminatedSAHLClientEstimate(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedSAHLClientEstimate_DAO ValuationDiscriminatedSAHLClientEstimate) : base(ValuationDiscriminatedSAHLClientEstimate)
		{
			this._DAO = ValuationDiscriminatedSAHLClientEstimate;
		}
	}
}


