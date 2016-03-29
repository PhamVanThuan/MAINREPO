
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO
	/// </summary>
	public partial interface IStageDefinitionStageDefinitionGroup_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionComposites
		/// </summary>
        IEventList<IStageDefinitionComposite_WTF> StageDefinitionComposites
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitions
		/// </summary>
		IEventList<IStageTransition_WTF> StageTransitions
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageTransitionComposites
		/// </summary>
		IEventList<IStageTransitionComposite_WTF> StageTransitionComposites
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinition
		/// </summary>
		IStageDefinition_WTF StageDefinition
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionStageDefinitionGroup_DAO.StageDefinitionGroup
		/// </summary>
		IStageDefinitionGroup_WTF StageDefinitionGroup
		{
			get;
			set;
		}
	}
}



