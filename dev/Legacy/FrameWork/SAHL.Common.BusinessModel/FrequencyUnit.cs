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
	/// SAHL.Common.BusinessModel.DAO.FrequencyUnit_DAO
	/// </summary>
	public partial class FrequencyUnit : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FrequencyUnit_DAO>, IFrequencyUnit
	{
				public FrequencyUnit(SAHL.Common.BusinessModel.DAO.FrequencyUnit_DAO FrequencyUnit) : base(FrequencyUnit)
		{
			this._DAO = FrequencyUnit;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FrequencyUnit_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FrequencyUnit_DAO.Unit
		/// </summary>
		public String Unit 
		{
			get { return _DAO.Unit; }
			set { _DAO.Unit = value;}
		}
	}
}


