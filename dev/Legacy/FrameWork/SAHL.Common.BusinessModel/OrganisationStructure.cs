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
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO
	/// </summary>
	public partial class OrganisationStructure : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO>, IOrganisationStructure
	{
				public OrganisationStructure(SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO OrganisationStructure) : base(OrganisationStructure)
		{
			this._DAO = OrganisationStructure;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.OrganisationType
		/// </summary>
		public IOrganisationType OrganisationType 
		{
			get
			{
				if (null == _DAO.OrganisationType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationType, OrganisationType_DAO>(_DAO.OrganisationType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationType = (OrganisationType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
		/// </summary>
		private DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure> _ChildOrganisationStructures;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
		/// </summary>
		public IEventList<IOrganisationStructure> ChildOrganisationStructures
		{
			get
			{
				if (null == _ChildOrganisationStructures) 
				{
					if(null == _DAO.ChildOrganisationStructures)
						_DAO.ChildOrganisationStructures = new List<OrganisationStructure_DAO>();
					_ChildOrganisationStructures = new DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure>(_DAO.ChildOrganisationStructures);
					_ChildOrganisationStructures.BeforeAdd += new EventListHandler(OnChildOrganisationStructures_BeforeAdd);					
					_ChildOrganisationStructures.BeforeRemove += new EventListHandler(OnChildOrganisationStructures_BeforeRemove);					
					_ChildOrganisationStructures.AfterAdd += new EventListHandler(OnChildOrganisationStructures_AfterAdd);					
					_ChildOrganisationStructures.AfterRemove += new EventListHandler(OnChildOrganisationStructures_AfterRemove);					
				}
				return _ChildOrganisationStructures;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Parent
		/// </summary>
		public IOrganisationStructure Parent 
		{
			get
			{
				if (null == _DAO.Parent) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.Parent);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Parent = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Parent = (OrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ADUsers
		/// </summary>
		private DAOEventList<ADUser_DAO, IADUser, ADUser> _ADUsers;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ADUsers
		/// </summary>
		public IEventList<IADUser> ADUsers
		{
			get
			{
				if (null == _ADUsers) 
				{
					if(null == _DAO.ADUsers)
						_DAO.ADUsers = new List<ADUser_DAO>();
					_ADUsers = new DAOEventList<ADUser_DAO, IADUser, ADUser>(_DAO.ADUsers);
					_ADUsers.BeforeAdd += new EventListHandler(OnADUsers_BeforeAdd);					
					_ADUsers.BeforeRemove += new EventListHandler(OnADUsers_BeforeRemove);					
					_ADUsers.AfterAdd += new EventListHandler(OnADUsers_AfterAdd);					
					_ADUsers.AfterRemove += new EventListHandler(OnADUsers_AfterRemove);					
				}
				return _ADUsers;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypes
		/// </summary>
		private DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType> _ApplicationRoleTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypes
		/// </summary>
		public IEventList<IApplicationRoleType> ApplicationRoleTypes
		{
			get
			{
				if (null == _ApplicationRoleTypes) 
				{
					if(null == _DAO.ApplicationRoleTypes)
						_DAO.ApplicationRoleTypes = new List<ApplicationRoleType_DAO>();
					_ApplicationRoleTypes = new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(_DAO.ApplicationRoleTypes);
					_ApplicationRoleTypes.BeforeAdd += new EventListHandler(OnApplicationRoleTypes_BeforeAdd);					
					_ApplicationRoleTypes.BeforeRemove += new EventListHandler(OnApplicationRoleTypes_BeforeRemove);					
					_ApplicationRoleTypes.AfterAdd += new EventListHandler(OnApplicationRoleTypes_AfterAdd);					
					_ApplicationRoleTypes.AfterRemove += new EventListHandler(OnApplicationRoleTypes_AfterRemove);					
				}
				return _ApplicationRoleTypes;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ChildOrganisationStructures = null;
			_ADUsers = null;
			_ApplicationRoleTypes = null;
			
		}
	}
}


