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
	/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO
	/// </summary>
	public partial class UserProfileSetting : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO>, IUserProfileSetting
	{
				public UserProfileSetting(SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO UserProfileSetting) : base(UserProfileSetting)
		{
			this._DAO = UserProfileSetting;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingName
		/// </summary>
		public String SettingName 
		{
			get { return _DAO.SettingName; }
			set { _DAO.SettingName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingValue
		/// </summary>
		public String SettingValue 
		{
			get { return _DAO.SettingValue; }
			set { _DAO.SettingValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.SettingType
		/// </summary>
		public String SettingType 
		{
			get { return _DAO.SettingType; }
			set { _DAO.SettingType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfileSetting_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


