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
	/// OfferRole_DAO is instantiated to represent the different Roles that Legal Entities are playing on the Application.
	/// </summary>
	public partial class ApplicationRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO>, IApplicationRole
	{
				public ApplicationRole(SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO ApplicationRole) : base(ApplicationRole)
		{
			this._DAO = ApplicationRole;
		}
		/// <summary>
		/// The date on which the status of the Role was last changed.
		/// </summary>
		public DateTime StatusChangeDate 
		{
			get { return _DAO.StatusChangeDate; }
			set { _DAO.StatusChangeDate = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// Each Role belongs to a specific Application Role Type. The Role Types are defined in the OfferRoleType table and include
		/// Insurer, Valuator, Branch Consultant etc.
		/// </summary>
		public IApplicationRoleType ApplicationRoleType 
		{
			get
			{
				if (null == _DAO.ApplicationRoleType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationRoleType, ApplicationRoleType_DAO>(_DAO.ApplicationRoleType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationRoleType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationRoleType = (ApplicationRoleType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The status of the ApplicationRole either Active or Inactive.
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
		/// The details regarding the Legal Entity playing the Role in the Application is stored in the LegalEntity table. This is
		/// the LegalEntityKey for that Legal Entity.
		/// </summary>
		public Int32 LegalEntityKey 
		{
			get { return _DAO.LegalEntityKey; }
			set { _DAO.LegalEntityKey = value;}
		}
		/// <summary>
		/// The key of the application to which the ApplicationRole belongs.
		/// </summary>
		public Int32 ApplicationKey 
		{
			get { return _DAO.ApplicationKey; }
			set { _DAO.ApplicationKey = value;}
		}
		/// <summary>
		/// A collection of role attributes that are defined for this Role.
		/// </summary>
		private DAOEventList<ApplicationRoleAttribute_DAO, IApplicationRoleAttribute, ApplicationRoleAttribute> _ApplicationRoleAttributes;
		/// <summary>
		/// A collection of role attributes that are defined for this Role.
		/// </summary>
		public IEventList<IApplicationRoleAttribute> ApplicationRoleAttributes
		{
			get
			{
				if (null == _ApplicationRoleAttributes) 
				{
					if(null == _DAO.ApplicationRoleAttributes)
						_DAO.ApplicationRoleAttributes = new List<ApplicationRoleAttribute_DAO>();
					_ApplicationRoleAttributes = new DAOEventList<ApplicationRoleAttribute_DAO, IApplicationRoleAttribute, ApplicationRoleAttribute>(_DAO.ApplicationRoleAttributes);
					_ApplicationRoleAttributes.BeforeAdd += new EventListHandler(OnApplicationRoleAttributes_BeforeAdd);					
					_ApplicationRoleAttributes.BeforeRemove += new EventListHandler(OnApplicationRoleAttributes_BeforeRemove);					
					_ApplicationRoleAttributes.AfterAdd += new EventListHandler(OnApplicationRoleAttributes_AfterAdd);					
					_ApplicationRoleAttributes.AfterRemove += new EventListHandler(OnApplicationRoleAttributes_AfterRemove);					
				}
				return _ApplicationRoleAttributes;
			}
		}
		/// <summary>
		/// A collection of application declarations that are defined for this Role.
		/// </summary>
		private DAOEventList<ApplicationDeclaration_DAO, IApplicationDeclaration, ApplicationDeclaration> _ApplicationDeclarations;
		/// <summary>
		/// A collection of application declarations that are defined for this Role.
		/// </summary>
		public IEventList<IApplicationDeclaration> ApplicationDeclarations
		{
			get
			{
				if (null == _ApplicationDeclarations) 
				{
					if(null == _DAO.ApplicationDeclarations)
						_DAO.ApplicationDeclarations = new List<ApplicationDeclaration_DAO>();
					_ApplicationDeclarations = new DAOEventList<ApplicationDeclaration_DAO, IApplicationDeclaration, ApplicationDeclaration>(_DAO.ApplicationDeclarations);
					_ApplicationDeclarations.BeforeAdd += new EventListHandler(OnApplicationDeclarations_BeforeAdd);					
					_ApplicationDeclarations.BeforeRemove += new EventListHandler(OnApplicationDeclarations_BeforeRemove);					
					_ApplicationDeclarations.AfterAdd += new EventListHandler(OnApplicationDeclarations_AfterAdd);					
					_ApplicationDeclarations.AfterRemove += new EventListHandler(OnApplicationDeclarations_AfterRemove);					
				}
				return _ApplicationDeclarations;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.OfferRoleDomiciliums
		/// </summary>
		private DAOEventList<ApplicationRoleDomicilium_DAO, IApplicationRoleDomicilium, ApplicationRoleDomicilium> _ApplicationRoleDomiciliums;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.OfferRoleDomiciliums
		/// </summary>
		public IEventList<IApplicationRoleDomicilium> ApplicationRoleDomiciliums
		{
			get
			{
				if (null == _ApplicationRoleDomiciliums) 
				{
					if(null == _DAO.ApplicationRoleDomiciliums)
						_DAO.ApplicationRoleDomiciliums = new List<ApplicationRoleDomicilium_DAO>();
					_ApplicationRoleDomiciliums = new DAOEventList<ApplicationRoleDomicilium_DAO, IApplicationRoleDomicilium, ApplicationRoleDomicilium>(_DAO.ApplicationRoleDomiciliums);
				}
				return _ApplicationRoleDomiciliums;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.LegalEntityDomiciliums
		/// </summary>
		private DAOEventList<LegalEntityDomicilium_DAO, ILegalEntityDomicilium, LegalEntityDomicilium> _LegalEntityDomiciliums;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.LegalEntityDomiciliums
		/// </summary>
		public override void Refresh()
		{
			base.Refresh();
			_ApplicationRoleAttributes = null;
			_ApplicationDeclarations = null;
			_ApplicationRoleDomiciliums = null;
			_LegalEntityDomiciliums = null;
			
		}
	}
}


