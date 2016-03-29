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
	/// 
	/// </summary>
	public partial class WorkflowRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO>, IWorkflowRole
	{
				public WorkflowRole(SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO WorkflowRole) : base(WorkflowRole)
		{
			this._DAO = WorkflowRole;
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
		/// The details regarding the Legal Entity playing the Workflow Role is stored in the LegalEntity table. This is 
		/// the LegalEntityKey for that Legal Entity.
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.WorkflowRoleType
		/// </summary>
		public IWorkflowRoleType WorkflowRoleType 
		{
			get
			{
				if (null == _DAO.WorkflowRoleType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IWorkflowRoleType, WorkflowRoleType_DAO>(_DAO.WorkflowRoleType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.WorkflowRoleType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.WorkflowRoleType = (WorkflowRoleType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The date on which the status of the Role was last changed.
		/// </summary>
		public DateTime StatusChangeDate 
		{
			get { return _DAO.StatusChangeDate; }
			set { _DAO.StatusChangeDate = value;}
		}
	}
}


