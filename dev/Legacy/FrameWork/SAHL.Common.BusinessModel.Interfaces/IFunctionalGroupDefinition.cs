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
	public partial interface IFunctionalGroupDefinition : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String FunctionalGroupName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Boolean AllowMany
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
		IEventList<IUserGroupMapping> UserGroupMappings
		{
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		IGenericKeyType GenericKeyType
		{
			get;
			set;
		}
	}
}


