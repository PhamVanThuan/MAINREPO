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
	/// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO
	/// </summary>
	public partial class RegentStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RegentStatus_DAO>, IRegentStatus
	{
				public RegentStatus(SAHL.Common.BusinessModel.DAO.RegentStatus_DAO RegentStatus) : base(RegentStatus)
		{
			this._DAO = RegentStatus;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO.RegentStatusDescription
		/// </summary>
		public String RegentStatusDescription 
		{
			get { return _DAO.RegentStatusDescription; }
			set { _DAO.RegentStatusDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


