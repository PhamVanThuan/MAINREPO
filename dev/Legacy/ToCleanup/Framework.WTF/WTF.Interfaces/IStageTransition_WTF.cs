
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO
	/// </summary>
	public partial interface IStageTransition_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.GenericKey
		/// </summary>
		System.Int32 GenericKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.TransitionDate
		/// </summary>
		System.DateTime TransitionDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Comments
		/// </summary>
		System.String Comments
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.EndTransitionDate
		/// </summary>
		System.DateTime? EndTransitionDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageTransitionComposites
		/// </summary>
		IEventList<IStageTransitionComposite_WTF> StageTransitionComposites
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.ADUser
		/// </summary>
		IADUser_WTF ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageTransition_DAO.StageDefinitionStageDefinitionGroup
		/// </summary>
		IStageDefinitionStageDefinitionGroup_WTF StageDefinitionStageDefinitionGroup
		{
			get;
			set;
		}
	}
}



