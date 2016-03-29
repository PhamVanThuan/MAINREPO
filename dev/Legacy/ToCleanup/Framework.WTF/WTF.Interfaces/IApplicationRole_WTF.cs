
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO
	/// </summary>
	public partial interface IApplicationRole_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.ApplicationRoleTypeKey
		/// </summary>
		System.Int32 ApplicationRoleTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.StatusChangeDate
		/// </summary>
		System.DateTime? StatusChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus_WTF GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.LegalEntity
		/// </summary>
        System.Int32 LegalEntityKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.Application
		/// </summary>
        System.Int32 ApplicationKey
		{
			get;
			set;
		}
	}
}



