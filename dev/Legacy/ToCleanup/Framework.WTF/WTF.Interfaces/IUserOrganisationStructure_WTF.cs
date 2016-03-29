
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO
	/// </summary>
	public partial interface IUserOrganisationStructure_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.ADUser
		/// </summary>
		IADUser_WTF ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.OrganisationStructure
		/// </summary>
		IOrganisationStructure_WTF OrganisationStructure
		{
			get;
			set;
		}
	}
}



