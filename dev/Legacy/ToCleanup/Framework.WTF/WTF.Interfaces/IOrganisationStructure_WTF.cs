
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO
	/// </summary>
	public partial interface IOrganisationStructure_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ParentKey
		/// </summary>
		System.Int32 ParentKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.OrganisationTypeKey
		/// </summary>
		System.Int32 OrganisationTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.GeneralStatusKey
		/// </summary>
		System.Int32 GeneralStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.UserOrganisationStructures
		/// </summary>
		IEventList<IUserOrganisationStructure_WTF> UserOrganisationStructures
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypeOrganisationStructureMappings
		/// </summary>
		IEventList<IApplicationRoleTypeOrganisationStructureMapping_WTF> ApplicationRoleTypeOrganisationStructureMappings
		{
			get;
		}
	}
}



