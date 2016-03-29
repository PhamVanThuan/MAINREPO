
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO
	/// </summary>
	public partial interface IUiStatement_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ApplicationName
		/// </summary>
		System.String ApplicationName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.StatementName
		/// </summary>
		System.String StatementName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ModifyDate
		/// </summary>
		System.DateTime? ModifyDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Version
		/// </summary>
		System.Int32 Version
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.ModifyUser
		/// </summary>
		System.String ModifyUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Statement
		/// </summary>
		System.String Statement
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Type
		/// </summary>
		System.Int32 Type
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.LastAccessedDate
		/// </summary>
		System.DateTime? LastAccessedDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UiStatement_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
	}
}



