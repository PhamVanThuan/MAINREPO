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
	/// the DAO class that reflect the workflowmenu data structure.
	/// </summary>
	public partial class WorkflowMenu : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO>, IWorkflowMenu
	{
				public WorkflowMenu(SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO WorkflowMenu) : base(WorkflowMenu)
		{
			this._DAO = WorkflowMenu;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.WorkflowName
		/// </summary>
		public String WorkflowName 
		{
			get { return _DAO.WorkflowName; }
			set { _DAO.WorkflowName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.StateName
		/// </summary>
		public String StateName 
		{
			get { return _DAO.StateName; }
			set { _DAO.StateName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.ProcessName
		/// </summary>
		public String ProcessName 
		{
			get { return _DAO.ProcessName; }
			set { _DAO.ProcessName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.WorkflowMenu_DAO.CoreBusinessObjectMenu
		/// </summary>
		public ICBOMenu CoreBusinessObjectMenu 
		{
			get
			{
				if (null == _DAO.CoreBusinessObjectMenu) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICBOMenu, CBOMenu_DAO>(_DAO.CoreBusinessObjectMenu);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CoreBusinessObjectMenu = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CoreBusinessObjectMenu = (CBOMenu_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


