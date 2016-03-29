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
	/// This is derived from Valuation_DAO. When instantiated it represents a Lightstone Automated Valuation.
	/// </summary>
	public partial class ValuationDiscriminatedLightstoneAVM : Valuation, IValuationDiscriminatedLightstoneAVM
	{
		protected new SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedLightstoneAVM_DAO _DAO;
		public ValuationDiscriminatedLightstoneAVM(SAHL.Common.BusinessModel.DAO.ValuationDiscriminatedLightstoneAVM_DAO ValuationDiscriminatedLightstoneAVM) : base(ValuationDiscriminatedLightstoneAVM)
		{
			this._DAO = ValuationDiscriminatedLightstoneAVM;
		}
	}
}


