
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ADUser_DAO
	/// </summary>
	public partial interface IADUser_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.ADUserName
		/// </summary>
		System.String ADUserName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Password
		/// </summary>
		System.String Password
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordQuestion
		/// </summary>
		System.String PasswordQuestion
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.PasswordAnswer
		/// </summary>
		System.String PasswordAnswer
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.UserOrganisationStructures
		/// </summary>
		IEventList<IUserOrganisationStructure_WTF> UserOrganisationStructures
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitions
		/// </summary>
		IEventList<IStageTransition_WTF> StageTransitions
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.StageTransitionComposites
		/// </summary>
		IEventList<IStageTransitionComposite_WTF> StageTransitionComposites
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus_WTF GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ADUser_DAO.LegalEntity
		/// </summary>
		ILegalEntity_WTF LegalEntity
		{
			get;
			set;
		}
	}
}



