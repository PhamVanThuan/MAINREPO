using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO
	/// </summary>
	public partial class StaticRateAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO>, IStaticRateAdjustment
	{
		public StaticRateAdjustment(SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO StaticRateAdjustment)
			: base(StaticRateAdjustment)
		{
			this._DAO = StaticRateAdjustment;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO.Key
		/// </summary>
		public Int32 Key
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value; }
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StaticRateAdjustment_DAO.Rate
		/// </summary>
		public Double Rate
		{
			get { return _DAO.Rate; }
			set { _DAO.Rate = value; }
		}
	}
}
