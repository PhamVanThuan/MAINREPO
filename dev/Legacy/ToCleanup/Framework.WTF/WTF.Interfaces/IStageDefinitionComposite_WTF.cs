
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO
	/// </summary>
    public partial interface IStageDefinitionComposite_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.UseThisDate
		/// </summary>
		System.Boolean UseThisDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.Sequence
		/// </summary>
		System.Int32 Sequence
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.UseThisReason
		/// </summary>
		System.Boolean UseThisReason
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO.StageDefinitionStageDefinitionGroup
		/// </summary>
		IStageDefinitionStageDefinitionGroup_WTF StageDefinitionStageDefinitionGroup
		{
			get;
			set;
		}
	}
}



