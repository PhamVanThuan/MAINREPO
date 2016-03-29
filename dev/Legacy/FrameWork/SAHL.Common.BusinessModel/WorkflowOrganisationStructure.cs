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
	/// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO
	/// </summary>
	public partial class WorkflowOrganisationStructure : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO>, IWorkflowOrganisationStructure
	{
				public WorkflowOrganisationStructure(SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO WorkflowOrganisationStructure) : base(WorkflowOrganisationStructure)
		{
			this._DAO = WorkflowOrganisationStructure;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.OrganisationStructure
		/// </summary>
		public IOrganisationStructure OrganisationStructure 
		{
			get
			{
				if (null == _DAO.OrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.OrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationStructure = (OrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.WorkflowName
		/// </summary>
		public String WorkflowName 
		{
			get { return _DAO.WorkflowName; }
			set { _DAO.WorkflowName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowOrganisationStructure_DAO.ProcessName
		/// </summary>
		public String ProcessName 
		{
			get { return _DAO.ProcessName; }
			set { _DAO.ProcessName = value;}
		}
	}
}


