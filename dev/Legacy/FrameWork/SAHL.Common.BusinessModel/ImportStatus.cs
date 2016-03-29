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
	/// SAHL.Common.BusinessModel.DAO.ImportStatus_DAO
	/// </summary>
	public partial class ImportStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportStatus_DAO>, IImportStatus
	{
				public ImportStatus(SAHL.Common.BusinessModel.DAO.ImportStatus_DAO ImportStatus) : base(ImportStatus)
		{
			this._DAO = ImportStatus;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportStatus_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportStatus_DAO.ImportApplications
		/// </summary>
		private DAOEventList<ImportApplication_DAO, IImportApplication, ImportApplication> _ImportApplications;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ImportStatus_DAO.ImportApplications
		/// </summary>
		public IEventList<IImportApplication> ImportApplications
		{
			get
			{
				if (null == _ImportApplications) 
				{
					if(null == _DAO.ImportApplications)
						_DAO.ImportApplications = new List<ImportApplication_DAO>();
					_ImportApplications = new DAOEventList<ImportApplication_DAO, IImportApplication, ImportApplication>(_DAO.ImportApplications);
					_ImportApplications.BeforeAdd += new EventListHandler(OnImportApplications_BeforeAdd);					
					_ImportApplications.BeforeRemove += new EventListHandler(OnImportApplications_BeforeRemove);					
					_ImportApplications.AfterAdd += new EventListHandler(OnImportApplications_AfterAdd);					
					_ImportApplications.AfterRemove += new EventListHandler(OnImportApplications_AfterRemove);					
				}
				return _ImportApplications;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ImportApplications = null;
			
		}
	}
}


