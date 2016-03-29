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
	/// Groups workflow role types.
	/// </summary>
	public partial class WorkflowRoleTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO>, IWorkflowRoleTypeGroup
	{
				public WorkflowRoleTypeGroup(SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO WorkflowRoleTypeGroup) : base(WorkflowRoleTypeGroup)
		{
			this._DAO = WorkflowRoleTypeGroup;
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
		/// The description of the Workflow Role Type Group
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO.GenericKeyType
		/// </summary>
		public IGenericKeyType GenericKeyType 
		{
			get
			{
				if (null == _DAO.GenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowRoleTypeGroup_DAO.WorkflowOrganisationStructure
		/// </summary>
		public IWorkflowOrganisationStructure WorkflowOrganisationStructure 
		{
			get
			{
				if (null == _DAO.WorkflowOrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IWorkflowOrganisationStructure, WorkflowOrganisationStructure_DAO>(_DAO.WorkflowOrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.WorkflowOrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.WorkflowOrganisationStructure = (WorkflowOrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


