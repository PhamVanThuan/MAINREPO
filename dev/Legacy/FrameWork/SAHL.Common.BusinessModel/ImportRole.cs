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
	public partial class ImportRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ImportRole_DAO>, IImportRole
	{
				public ImportRole(SAHL.Common.BusinessModel.DAO.ImportRole_DAO ImportRole) : base(ImportRole)
		{
			this._DAO = ImportRole;
		}
		/// <summary>
		/// 
		/// </summary>
		public String RoleTypeKey 
		{
			get { return _DAO.RoleTypeKey; }
			set { _DAO.RoleTypeKey = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public IImportLegalEntity ImportLegalEntity 
		{
			get
			{
				if (null == _DAO.ImportLegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IImportLegalEntity, ImportLegalEntity_DAO>(_DAO.ImportLegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ImportLegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ImportLegalEntity = (ImportLegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


