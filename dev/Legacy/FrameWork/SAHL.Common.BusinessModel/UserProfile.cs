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
	/// SAHL.Common.BusinessModel.DAO.UserProfile_DAO
	/// </summary>
	public partial class UserProfile : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserProfile_DAO>, IUserProfile
	{
				public UserProfile(SAHL.Common.BusinessModel.DAO.UserProfile_DAO UserProfile) : base(UserProfile)
		{
			this._DAO = UserProfile;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.ADUserName
		/// </summary>
		public String ADUserName 
		{
			get { return _DAO.ADUserName; }
			set { _DAO.ADUserName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.Value
		/// </summary>
		public String Value 
		{
			get { return _DAO.Value; }
			set { _DAO.Value = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserProfile_DAO.ProfileType
		/// </summary>
		public IProfileType ProfileType 
		{
			get
			{
				if (null == _DAO.ProfileType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProfileType, ProfileType_DAO>(_DAO.ProfileType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ProfileType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ProfileType = (ProfileType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


