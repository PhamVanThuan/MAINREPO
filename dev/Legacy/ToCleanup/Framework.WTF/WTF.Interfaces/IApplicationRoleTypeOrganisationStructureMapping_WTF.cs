
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO
	/// </summary>
	public partial interface IApplicationRoleTypeOrganisationStructureMapping_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.ApplicationRoleType
		/// </summary>
		IApplicationRoleType_WTF ApplicationRoleType
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.OrganisationStructure
		/// </summary>
		IOrganisationStructure_WTF OrganisationStructure
		{
			get;
			set;
		}
	}
}



