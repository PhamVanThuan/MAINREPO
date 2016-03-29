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
	public partial interface IUserGroupMapping : IEntityValidation
	{
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
		IEventList<IUserGroupAssignment> UserGroupAssignments
		{
			get;
		}
		/// <summary>
		/// 
		/// </summary>
		IFunctionalGroupDefinition FunctionalGroupDefinition
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		IOrganisationStructure OrganisationStructure
		{
			get;
			set;
		}
	}
}


