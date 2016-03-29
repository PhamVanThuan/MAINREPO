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
	/// SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO
	/// </summary>
	public partial class InterestRateAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO>, IInterestRateAdjustment
	{
				public InterestRateAdjustment(SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO InterestRateAdjustment) : base(InterestRateAdjustment)
		{
			this._DAO = InterestRateAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InterestRateAdjustment_DAO.Adjustment
		/// </summary>
		public Double Adjustment 
		{
			get { return _DAO.Adjustment; }
			set { _DAO.Adjustment = value;}
		}
	}
}


