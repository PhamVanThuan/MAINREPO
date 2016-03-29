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
	/// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO
	/// </summary>
	public partial class LifeCommissionSecurity : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO>, ILifeCommissionSecurity
	{
				public LifeCommissionSecurity(SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO LifeCommissionSecurity) : base(LifeCommissionSecurity)
		{
			this._DAO = LifeCommissionSecurity;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.Administrator
		/// </summary>
		public Boolean Administrator 
		{
			get { return _DAO.Administrator; }
			set { _DAO.Administrator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeCommissionSecurity_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


