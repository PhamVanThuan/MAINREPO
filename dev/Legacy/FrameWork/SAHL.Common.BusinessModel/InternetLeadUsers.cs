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
	/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO
	/// </summary>
	public partial class InternetLeadUsers : BusinessModelBase<SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO>, IInternetLeadUsers
	{
				public InternetLeadUsers(SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO InternetLeadUsers) : base(InternetLeadUsers)
		{
			this._DAO = InternetLeadUsers;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.Flag
		/// </summary>
		public Boolean Flag 
		{
			get { return _DAO.Flag; }
			set { _DAO.Flag = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.CaseCount
		/// </summary>
		public Int32 CaseCount 
		{
			get { return _DAO.CaseCount; }
			set { _DAO.CaseCount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.LastCaseKey
		/// </summary>
		public Int32 LastCaseKey 
		{
			get { return _DAO.LastCaseKey; }
			set { _DAO.LastCaseKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.InternetLeadUsers_DAO.ADUser
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


