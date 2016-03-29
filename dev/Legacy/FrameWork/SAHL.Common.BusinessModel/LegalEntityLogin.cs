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
	/// Legal Entity Login DAO
	/// </summary>
	public partial class LegalEntityLogin : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO>, ILegalEntityLogin
	{
				public LegalEntityLogin(SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO LegalEntityLogin) : base(LegalEntityLogin)
		{
			this._DAO = LegalEntityLogin;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.LegalEntityLoginKey
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.Username
		/// </summary>
		public String Username 
		{
			get { return _DAO.Username; }
			set { _DAO.Username = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.Password
		/// </summary>
		public String Password 
		{
			get { return _DAO.Password; }
			set { _DAO.Password = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.LastLoginDate
		/// </summary>
		public DateTime? LastLoginDate
		{
			get { return _DAO.LastLoginDate; }
			set { _DAO.LastLoginDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.LegalEntityLogin_DAO.LegalEntity
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


