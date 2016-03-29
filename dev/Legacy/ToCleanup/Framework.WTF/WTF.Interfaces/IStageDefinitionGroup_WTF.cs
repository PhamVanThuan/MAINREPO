
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO
	/// </summary>
	public partial interface IStageDefinitionGroup_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GenericKeyTypeKey
		/// </summary>
		System.Int32 GenericKeyTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionGroups
		/// </summary>
		IEventList<IStageDefinitionGroup_WTF> StageDefinitionGroups
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
		IEventList<IStageDefinitionStageDefinitionGroup_WTF> StageDefinitionStageDefinitionGroups
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus_WTF GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionGroup_DAO.StageDefinitionGroup
		/// </summary>
        IStageDefinitionGroup_WTF ParentStageDefinitionGroup
		{
			get;
			set;
		}
	}
}



