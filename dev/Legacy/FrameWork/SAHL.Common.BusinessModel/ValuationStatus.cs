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
	/// ValuationStatus_DAO describes the status of a Valuation. A valuation can currently be pending or complete.
	/// </summary>
	public partial class ValuationStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationStatus_DAO>, IValuationStatus
	{
				public ValuationStatus(SAHL.Common.BusinessModel.DAO.ValuationStatus_DAO ValuationStatus) : base(ValuationStatus)
		{
			this._DAO = ValuationStatus;
		}
		/// <summary>
		/// Valuation status description.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


