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
	/// This is derived from Valuation_DAO. When instantiated it represents a AdCheck Desktop Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedAdCheckDesktop : Valuation, IValuationDiscriminatedAdCheckDesktop
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedAdCheckDesktop_DAO _DAO;
		public ValuationDiscriminatedAdCheckDesktop(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedAdCheckDesktop_DAO ValuationDiscriminatedAdCheckDesktop) : base(ValuationDiscriminatedAdCheckDesktop)
		{
			this._DAO = ValuationDiscriminatedAdCheckDesktop;
		}
	}
}


