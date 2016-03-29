using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO
	/// </summary>
	public partial class PreWorkflowDataFilter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO>, IPreWorkflowDataFilter
	{
				public PreWorkflowDataFilter(SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO PreWorkflowDataFilter) : base(PreWorkflowDataFilter)
		{
			this._DAO = PreWorkflowDataFilter;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.WorkflowDataKey
		/// </summary>
		public Int32 WorkflowDataKey 
		{
			get { return _DAO.WorkflowDataKey; }
			set { _DAO.WorkflowDataKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.PreWorkflowDataFilter_DAO.OfferTypeKey
		/// </summary>
		public Int32 OfferTypeKey 
		{
			get { return _DAO.OfferTypeKey; }
			set { _DAO.OfferTypeKey = value;}
		}
	}
}


