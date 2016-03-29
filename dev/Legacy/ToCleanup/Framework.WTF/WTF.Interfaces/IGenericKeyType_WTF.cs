
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO
	/// </summary>
	public partial interface IGenericKeyType_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.TableName
		/// </summary>
		System.String TableName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.PrimaryKeyColumn
		/// </summary>
		System.String PrimaryKeyColumn
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericKeyType_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
	}
}



