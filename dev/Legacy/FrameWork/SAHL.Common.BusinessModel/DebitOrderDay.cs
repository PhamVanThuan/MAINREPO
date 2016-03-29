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
	/// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO
	/// </summary>
	public partial class DebitOrderDay : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO>, IDebitOrderDay
	{
				public DebitOrderDay(SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO DebitOrderDay) : base(DebitOrderDay)
		{
			this._DAO = DebitOrderDay;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO.Day
		/// </summary>
		public Int32 Day 
		{
			get { return _DAO.Day; }
			set { _DAO.Day = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


