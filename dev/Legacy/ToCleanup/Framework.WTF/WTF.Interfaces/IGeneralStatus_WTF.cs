
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO
	/// </summary>
	public partial interface IGeneralStatus_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
	}
}



