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
	/// This is derived from Valuation_DAO. When instantiated it represents a Lightstone Physical Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedLightStonePhysical : Valuation, IValuationDiscriminatedLightStonePhysical
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedLightStonePhysical_DAO _DAO;
		public ValuationDiscriminatedLightStonePhysical(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedLightStonePhysical_DAO ValuationDiscriminatedLightStonePhysical) : base(ValuationDiscriminatedLightStonePhysical)
		{
			this._DAO = ValuationDiscriminatedLightStonePhysical;
		}
	}
}


