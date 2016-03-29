using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public partial interface IUserGroupAssignment : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.Int32 GenericKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime ChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime InsertedDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		IADUser ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		IUserGroupMapping UserGroupMapping
		{
			get;
			set;
		}
	}
}


