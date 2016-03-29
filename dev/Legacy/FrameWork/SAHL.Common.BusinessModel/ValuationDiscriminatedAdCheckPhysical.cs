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
	/// This is derived from Valuation_DAO. When instantiated it represents a AdCheck Physical Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedAdCheckPhysical : Valuation, IValuationDiscriminatedAdCheckPhysical
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedAdCheckPhysical_DAO _DAO;
		public ValuationDiscriminatedAdCheckPhysical(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedAdCheckPhysical_DAO ValuationDiscriminatedAdCheckPhysical) : base(ValuationDiscriminatedAdCheckPhysical)
		{
			this._DAO = ValuationDiscriminatedAdCheckPhysical;
		}
	}
}


