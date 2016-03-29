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
	/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO
	/// </summary>
	public partial class WorkflowContext : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO>, IWorkflowContext
	{
				public WorkflowContext(SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO WorkflowContext) : base(WorkflowContext)
		{
			this._DAO = WorkflowContext;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.TableName
		/// </summary>
		public String TableName 
		{
			get { return _DAO.TableName; }
			set { _DAO.TableName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.Alias
		/// </summary>
		public String Alias 
		{
			get { return _DAO.Alias; }
			set { _DAO.Alias = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO.PrimaryKeyColumn
		/// </summary>
		public String PrimaryKeyColumn 
		{
			get { return _DAO.PrimaryKeyColumn; }
			set { _DAO.PrimaryKeyColumn = value;}
		}
	}
}


