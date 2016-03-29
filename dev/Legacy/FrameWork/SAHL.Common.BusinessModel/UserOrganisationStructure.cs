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
	/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO
	/// </summary>
	public partial class UserOrganisationStructure : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO>, IUserOrganisationStructure
	{
				public UserOrganisationStructure(SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO UserOrganisationStructure) : base(UserOrganisationStructure)
		{
			this._DAO = UserOrganisationStructure;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.ADUser
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.OrganisationStructure
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
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GenericKeyType
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
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.UserOrganisationStructureRoundRobinStatus
		/// </summary>
		private DAOEventList<UserOrganisationStructureRoundRobinStatus_DAO, IUserOrganisationStructureRoundRobinStatus, UserOrganisationStructureRoundRobinStatus> _UserOrganisationStructureRoundRobinStatus;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.UserOrganisationStructureRoundRobinStatus
		/// </summary>
		public IEventList<IUserOrganisationStructureRoundRobinStatus> UserOrganisationStructureRoundRobinStatus
		{
			get
			{
				if (null == _UserOrganisationStructureRoundRobinStatus) 
				{
					if(null == _DAO.UserOrganisationStructureRoundRobinStatus)
						_DAO.UserOrganisationStructureRoundRobinStatus = new List<UserOrganisationStructureRoundRobinStatus_DAO>();
					_UserOrganisationStructureRoundRobinStatus = new DAOEventList<UserOrganisationStructureRoundRobinStatus_DAO, IUserOrganisationStructureRoundRobinStatus, UserOrganisationStructureRoundRobinStatus>(_DAO.UserOrganisationStructureRoundRobinStatus);
					_UserOrganisationStructureRoundRobinStatus.BeforeAdd += new EventListHandler(OnUserOrganisationStructureRoundRobinStatus_BeforeAdd);					
					_UserOrganisationStructureRoundRobinStatus.BeforeRemove += new EventListHandler(OnUserOrganisationStructureRoundRobinStatus_BeforeRemove);					
					_UserOrganisationStructureRoundRobinStatus.AfterAdd += new EventListHandler(OnUserOrganisationStructureRoundRobinStatus_AfterAdd);					
					_UserOrganisationStructureRoundRobinStatus.AfterRemove += new EventListHandler(OnUserOrganisationStructureRoundRobinStatus_AfterRemove);					
				}
				return _UserOrganisationStructureRoundRobinStatus;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_UserOrganisationStructureRoundRobinStatus = null;
			
		}
	}
}


