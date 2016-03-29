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
	/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO
	/// </summary>
	public partial class UserOrganisationStructureHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO>, IUserOrganisationStructureHistory
	{
				public UserOrganisationStructureHistory(SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO UserOrganisationStructureHistory) : base(UserOrganisationStructureHistory)
		{
			this._DAO = UserOrganisationStructureHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.UserOrganisationStructureKey
		/// </summary>
		public Int32 UserOrganisationStructureKey 
		{
			get { return _DAO.UserOrganisationStructureKey; }
			set { _DAO.UserOrganisationStructureKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.OrganisationStructureKey
		/// </summary>
		public IOrganisationStructure OrganisationStructureKey 
		{
			get
			{
				if (null == _DAO.OrganisationStructureKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.OrganisationStructureKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructureKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationStructureKey = (OrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GenericKeyType
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
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.Action
		/// </summary>
		public Char Action 
		{
			get { return _DAO.Action; }
			set { _DAO.Action = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureHistory_DAO.ADUser
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


