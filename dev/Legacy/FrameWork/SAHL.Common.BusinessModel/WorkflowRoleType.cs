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
	/// SAHL.Common.BusinessModel.DAO.WorkflowRoleType_DAO
	/// </summary>
	public partial class WorkflowRoleType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowRoleType_DAO>, IWorkflowRoleType
	{
				public WorkflowRoleType(SAHL.Common.BusinessModel.DAO.WorkflowRoleType_DAO WorkflowRoleType) : base(WorkflowRoleType)
		{
			this._DAO = WorkflowRoleType;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The description of the Workflow Role Type
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Determines the Workflow Role Type Group to which the Workflow Role Type belongs.
		/// </summary>
		public IWorkflowRoleTypeGroup WorkflowRoleTypeGroup 
		{
			get
			{
				if (null == _DAO.WorkflowRoleTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IWorkflowRoleTypeGroup, WorkflowRoleTypeGroup_DAO>(_DAO.WorkflowRoleTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.WorkflowRoleTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.WorkflowRoleTypeGroup = (WorkflowRoleTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


