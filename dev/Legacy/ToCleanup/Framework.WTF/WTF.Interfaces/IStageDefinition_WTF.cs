
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO
	/// </summary>
	public partial interface IStageDefinition_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.IsComposite
		/// </summary>
		System.Boolean IsComposite
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Name
		/// </summary>
		System.String Name
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.CompositeTypeName
		/// </summary>
		System.String CompositeTypeName
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.HasCompositeLogic
		/// </summary>
		System.Boolean HasCompositeLogic
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.StageDefinitionStageDefinitionGroups
		/// </summary>
		IEventList<IStageDefinitionStageDefinitionGroup_WTF> StageDefinitionStageDefinitionGroups
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinition_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus_WTF GeneralStatus
		{
			get;
			set;
		}
	}
}



