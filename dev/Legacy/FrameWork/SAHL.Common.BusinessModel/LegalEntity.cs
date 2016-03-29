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
	/// LegalEntity_DAO is the base class from which Legal Entity Type specific Legal Entities are derived. Legal Entities are
		/// discriminated based on the LegalEntityType and has the following discriminations:
		/// Natural PersonClose CorporationTrustCompanyUnknown
	/// </summary>
	public abstract partial class LegalEntity : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntity_DAO>, ILegalEntity
	{
				public LegalEntity(SAHL.Common.BusinessModel.DAO.LegalEntity_DAO LegalEntity) : base(LegalEntity)
		{
			this._DAO = LegalEntity;
		}
		/// <summary>
		/// The date on which the Legal Entity was first introduced to SA Home Loans
		/// </summary>
		public DateTime IntroductionDate 
		{
			get { return _DAO.IntroductionDate; }
			set { _DAO.IntroductionDate = value;}
		}
		/// <summary>
		/// The Legal Entity's Tax Number
		/// </summary>
		public String TaxNumber 
		{
			get { return _DAO.TaxNumber; }
			set { _DAO.TaxNumber = value;}
		}
		/// <summary>
		/// The Area Code for the Legal Entity's Home Phone Number
		/// </summary>
		public String HomePhoneCode 
		{
			get { return _DAO.HomePhoneCode; }
			set { _DAO.HomePhoneCode = value;}
		}
		/// <summary>
		/// The Home Phone Number
		/// </summary>
		public String HomePhoneNumber 
		{
			get { return _DAO.HomePhoneNumber; }
			set { _DAO.HomePhoneNumber = value;}
		}
		/// <summary>
		/// The Area Code for the Legal Entity's Work Phone Number
		/// </summary>
		public String WorkPhoneCode 
		{
			get { return _DAO.WorkPhoneCode; }
			set { _DAO.WorkPhoneCode = value;}
		}
		/// <summary>
		/// The Work Phone Number
		/// </summary>
		public String WorkPhoneNumber 
		{
			get { return _DAO.WorkPhoneNumber; }
			set { _DAO.WorkPhoneNumber = value;}
		}
		/// <summary>
		/// The Legal Entity's Cell Phone Number including the Code.
		/// </summary>
		public String CellPhoneNumber 
		{
			get { return _DAO.CellPhoneNumber; }
			set { _DAO.CellPhoneNumber = value;}
		}
		/// <summary>
		/// The Legal Entity's Email Address
		/// </summary>
		public String EmailAddress 
		{
			get { return _DAO.EmailAddress; }
			set { _DAO.EmailAddress = value;}
		}
		/// <summary>
		/// The Area code for the Legal Entity's Fax Number
		/// </summary>
		public String FaxCode 
		{
			get { return _DAO.FaxCode; }
			set { _DAO.FaxCode = value;}
		}
		/// <summary>
		/// The Fax Number
		/// </summary>
		public String FaxNumber 
		{
			get { return _DAO.FaxNumber; }
			set { _DAO.FaxNumber = value;}
		}
		/// <summary>
		/// A Password chosen by the Legal Entity in order access Loan information online.
		/// </summary>
		public String Password 
		{
			get { return _DAO.Password; }
			set { _DAO.Password = value;}
		}
		/// <summary>
		/// A Comments Property
		/// </summary>
		public String Comments 
		{
			get { return _DAO.Comments; }
			set { _DAO.Comments = value;}
		}
		/// <summary>
		/// The UserID of the person who last updated the Legal Entity record.
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// The date on which the Legal Entity record was last changed.
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// The preferred language in which the Legal Entity receives documentation.
		/// </summary>
		public ILanguage DocumentLanguage 
		{
			get
			{
				if (null == _DAO.DocumentLanguage) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.DocumentLanguage);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DocumentLanguage = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DocumentLanguage = (Language_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The South African Residence status of the Legal Entity.
		/// </summary>
		public IResidenceStatus ResidenceStatus 
		{
			get
			{
				if (null == _DAO.ResidenceStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IResidenceStatus, ResidenceStatus_DAO>(_DAO.ResidenceStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ResidenceStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ResidenceStatus = (ResidenceStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
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
		/// A Legal Entity can have many Addresses. This relationship is defined in the LegalEntityAddress table.
		/// </summary>
		private DAOEventList<LegalEntityAddress_DAO, ILegalEntityAddress, LegalEntityAddress> _LegalEntityAddresses;
		/// <summary>
		/// A Legal Entity can have many Addresses. This relationship is defined in the LegalEntityAddress table.
		/// </summary>
		public IEventList<ILegalEntityAddress> LegalEntityAddresses
		{
			get
			{
				if (null == _LegalEntityAddresses) 
				{
					if(null == _DAO.LegalEntityAddresses)
						_DAO.LegalEntityAddresses = new List<LegalEntityAddress_DAO>();
					_LegalEntityAddresses = new DAOEventList<LegalEntityAddress_DAO, ILegalEntityAddress, LegalEntityAddress>(_DAO.LegalEntityAddresses);
					_LegalEntityAddresses.BeforeAdd += new EventListHandler(OnLegalEntityAddresses_BeforeAdd);					
					_LegalEntityAddresses.BeforeRemove += new EventListHandler(OnLegalEntityAddresses_BeforeRemove);					
					_LegalEntityAddresses.AfterAdd += new EventListHandler(OnLegalEntityAddresses_AfterAdd);					
					_LegalEntityAddresses.AfterRemove += new EventListHandler(OnLegalEntityAddresses_AfterRemove);					
				}
				return _LegalEntityAddresses;
			}
		}
		/// <summary>
		/// Provides a list of dirty legal entity addresses.  This list should never be added to: it contains old
		/// records that could not be successfully migrated and required cleaning up.  Over time, these should be
		/// replaced with valid LegalEntityAddress objects and their IsCleaned properties set to true.
		/// </summary>
		private DAOEventList<FailedLegalEntityAddress_DAO, IFailedLegalEntityAddress, FailedLegalEntityAddress> _FailedLegalEntityAddresses;
		/// <summary>
		/// Provides a list of dirty legal entity addresses.  This list should never be added to: it contains old
		/// records that could not be successfully migrated and required cleaning up.  Over time, these should be
		/// replaced with valid LegalEntityAddress objects and their IsCleaned properties set to true.
		/// </summary>
		public IEventList<IFailedLegalEntityAddress> FailedLegalEntityAddresses
		{
			get
			{
				if (null == _FailedLegalEntityAddresses) 
				{
					if(null == _DAO.FailedLegalEntityAddresses)
						_DAO.FailedLegalEntityAddresses = new List<FailedLegalEntityAddress_DAO>();
					_FailedLegalEntityAddresses = new DAOEventList<FailedLegalEntityAddress_DAO, IFailedLegalEntityAddress, FailedLegalEntityAddress>(_DAO.FailedLegalEntityAddresses);
					_FailedLegalEntityAddresses.BeforeAdd += new EventListHandler(OnFailedLegalEntityAddresses_BeforeAdd);					
					_FailedLegalEntityAddresses.BeforeRemove += new EventListHandler(OnFailedLegalEntityAddresses_BeforeRemove);					
					_FailedLegalEntityAddresses.AfterAdd += new EventListHandler(OnFailedLegalEntityAddresses_AfterAdd);					
					_FailedLegalEntityAddresses.AfterRemove += new EventListHandler(OnFailedLegalEntityAddresses_AfterRemove);					
				}
				return _FailedLegalEntityAddresses;
			}
		}
		/// <summary>
		/// A Legal Entity can have many Employment records. The Employment table stores a Foreign Key Reference to the LegalEntityKey.
		/// </summary>
		private DAOEventList<Employment_DAO, IEmployment, Employment> _Employment;
		/// <summary>
		/// A Legal Entity can have many Employment records. The Employment table stores a Foreign Key Reference to the LegalEntityKey.
		/// </summary>
		public IEventList<IEmployment> Employment
		{
			get
			{
				if (null == _Employment) 
				{
					if(null == _DAO.Employment)
						_DAO.Employment = new List<Employment_DAO>();
					_Employment = new DAOEventList<Employment_DAO, IEmployment, Employment>(_DAO.Employment);
					_Employment.BeforeAdd += new EventListHandler(OnEmployment_BeforeAdd);					
					_Employment.BeforeRemove += new EventListHandler(OnEmployment_BeforeRemove);					
					_Employment.AfterAdd += new EventListHandler(OnEmployment_AfterAdd);					
					_Employment.AfterRemove += new EventListHandler(OnEmployment_AfterRemove);					
				}
				return _Employment;
			}
		}
		/// <summary>
		/// The details of the Affordability Assessment for the Legal Entity is required to be stored. A Legal Entity has a set of Affordability entries
		/// which is related to the Legal Entity by way of the LegalEntityAffordability table, which stores a foreign key reference
		/// to the Legal Entity.
		/// </summary>
		private DAOEventList<LegalEntityAffordability_DAO, ILegalEntityAffordability, LegalEntityAffordability> _LegalEntityAffordabilities;
		/// <summary>
		/// The details of the Affordability Assessment for the Legal Entity is required to be stored. A Legal Entity has a set of Affordability entries
		/// which is related to the Legal Entity by way of the LegalEntityAffordability table, which stores a foreign key reference
		/// to the Legal Entity.
		/// </summary>
		public IEventList<ILegalEntityAffordability> LegalEntityAffordabilities
		{
			get
			{
				if (null == _LegalEntityAffordabilities) 
				{
					if(null == _DAO.LegalEntityAffordabilities)
						_DAO.LegalEntityAffordabilities = new List<LegalEntityAffordability_DAO>();
					_LegalEntityAffordabilities = new DAOEventList<LegalEntityAffordability_DAO, ILegalEntityAffordability, LegalEntityAffordability>(_DAO.LegalEntityAffordabilities);
					_LegalEntityAffordabilities.BeforeAdd += new EventListHandler(OnLegalEntityAffordabilities_BeforeAdd);					
					_LegalEntityAffordabilities.BeforeRemove += new EventListHandler(OnLegalEntityAffordabilities_BeforeRemove);					
					_LegalEntityAffordabilities.AfterAdd += new EventListHandler(OnLegalEntityAffordabilities_AfterAdd);					
					_LegalEntityAffordabilities.AfterRemove += new EventListHandler(OnLegalEntityAffordabilities_AfterRemove);					
				}
				return _LegalEntityAffordabilities;
			}
		}
		/// <summary>
		/// In certain cases the Legal Entity is required to provide Asset and Liability information. A Legal Entity will have a set
		/// of Asset/Liability records which is related to the Legal Entity by way of the LegalEntityAssetLiability table, which stores
		/// a foreign key reference to the Legal Entity.
		/// </summary>
		private DAOEventList<LegalEntityAssetLiability_DAO, ILegalEntityAssetLiability, LegalEntityAssetLiability> _LegalEntityAssetLiabilities;
		/// <summary>
		/// In certain cases the Legal Entity is required to provide Asset and Liability information. A Legal Entity will have a set
		/// of Asset/Liability records which is related to the Legal Entity by way of the LegalEntityAssetLiability table, which stores
		/// a foreign key reference to the Legal Entity.
		/// </summary>
		public IEventList<ILegalEntityAssetLiability> LegalEntityAssetLiabilities
		{
			get
			{
				if (null == _LegalEntityAssetLiabilities) 
				{
					if(null == _DAO.LegalEntityAssetLiabilities)
						_DAO.LegalEntityAssetLiabilities = new List<LegalEntityAssetLiability_DAO>();
					_LegalEntityAssetLiabilities = new DAOEventList<LegalEntityAssetLiability_DAO, ILegalEntityAssetLiability, LegalEntityAssetLiability>(_DAO.LegalEntityAssetLiabilities);
					_LegalEntityAssetLiabilities.BeforeAdd += new EventListHandler(OnLegalEntityAssetLiabilities_BeforeAdd);					
					_LegalEntityAssetLiabilities.BeforeRemove += new EventListHandler(OnLegalEntityAssetLiabilities_BeforeRemove);					
					_LegalEntityAssetLiabilities.AfterAdd += new EventListHandler(OnLegalEntityAssetLiabilities_AfterAdd);					
					_LegalEntityAssetLiabilities.AfterRemove += new EventListHandler(OnLegalEntityAssetLiabilities_AfterRemove);					
				}
				return _LegalEntityAssetLiabilities;
			}
		}
		/// <summary>
		/// A Legal Entity can have many Bank Account records. This relationship is defined in the LegalEntityBankAccount table,
		/// which has a foreign key reference to the Legal Entity.
		/// </summary>
		private DAOEventList<LegalEntityBankAccount_DAO, ILegalEntityBankAccount, LegalEntityBankAccount> _LegalEntityBankAccounts;
		/// <summary>
		/// A Legal Entity can have many Bank Account records. This relationship is defined in the LegalEntityBankAccount table,
		/// which has a foreign key reference to the Legal Entity.
		/// </summary>
		public IEventList<ILegalEntityBankAccount> LegalEntityBankAccounts
		{
			get
			{
				if (null == _LegalEntityBankAccounts) 
				{
					if(null == _DAO.LegalEntityBankAccounts)
						_DAO.LegalEntityBankAccounts = new List<LegalEntityBankAccount_DAO>();
					_LegalEntityBankAccounts = new DAOEventList<LegalEntityBankAccount_DAO, ILegalEntityBankAccount, LegalEntityBankAccount>(_DAO.LegalEntityBankAccounts);
					_LegalEntityBankAccounts.BeforeAdd += new EventListHandler(OnLegalEntityBankAccounts_BeforeAdd);					
					_LegalEntityBankAccounts.BeforeRemove += new EventListHandler(OnLegalEntityBankAccounts_BeforeRemove);					
					_LegalEntityBankAccounts.AfterAdd += new EventListHandler(OnLegalEntityBankAccounts_AfterAdd);					
					_LegalEntityBankAccounts.AfterRemove += new EventListHandler(OnLegalEntityBankAccounts_AfterRemove);					
				}
				return _LegalEntityBankAccounts;
			}
		}
		/// <summary>
		/// A Legal Entity can choose many options by which SAHL can market new products to the client. This relationship is defined
		/// in the LegalEntityMarketingOption table, which has a foreign key reference to the Legal Entity.
		/// </summary>
		private DAOEventList<LegalEntityMarketingOption_DAO, ILegalEntityMarketingOption, LegalEntityMarketingOption> _LegalEntityMarketingOptions;
		/// <summary>
		/// A Legal Entity can choose many options by which SAHL can market new products to the client. This relationship is defined
		/// in the LegalEntityMarketingOption table, which has a foreign key reference to the Legal Entity.
		/// </summary>
		public IEventList<ILegalEntityMarketingOption> LegalEntityMarketingOptions
		{
			get
			{
				if (null == _LegalEntityMarketingOptions) 
				{
					if(null == _DAO.LegalEntityMarketingOptions)
						_DAO.LegalEntityMarketingOptions = new List<LegalEntityMarketingOption_DAO>();
					_LegalEntityMarketingOptions = new DAOEventList<LegalEntityMarketingOption_DAO, ILegalEntityMarketingOption, LegalEntityMarketingOption>(_DAO.LegalEntityMarketingOptions);
					_LegalEntityMarketingOptions.BeforeAdd += new EventListHandler(OnLegalEntityMarketingOptions_BeforeAdd);					
					_LegalEntityMarketingOptions.BeforeRemove += new EventListHandler(OnLegalEntityMarketingOptions_BeforeRemove);					
					_LegalEntityMarketingOptions.AfterAdd += new EventListHandler(OnLegalEntityMarketingOptions_AfterAdd);					
					_LegalEntityMarketingOptions.AfterRemove += new EventListHandler(OnLegalEntityMarketingOptions_AfterRemove);					
				}
				return _LegalEntityMarketingOptions;
			}
		}
		/// <summary>
		/// We are assuming the relationship exist in the relationship table
		/// </summary>
		private DAOEventList<LegalEntityRelationship_DAO, ILegalEntityRelationship, LegalEntityRelationship> _LegalEntityRelationships;
		/// <summary>
		/// We are assuming the relationship exist in the relationship table
		/// </summary>
		public IEventList<ILegalEntityRelationship> LegalEntityRelationships
		{
			get
			{
				if (null == _LegalEntityRelationships) 
				{
					if(null == _DAO.LegalEntityRelationships)
						_DAO.LegalEntityRelationships = new List<LegalEntityRelationship_DAO>();
					_LegalEntityRelationships = new DAOEventList<LegalEntityRelationship_DAO, ILegalEntityRelationship, LegalEntityRelationship>(_DAO.LegalEntityRelationships);
					_LegalEntityRelationships.BeforeAdd += new EventListHandler(OnLegalEntityRelationships_BeforeAdd);					
					_LegalEntityRelationships.BeforeRemove += new EventListHandler(OnLegalEntityRelationships_BeforeRemove);					
					_LegalEntityRelationships.AfterAdd += new EventListHandler(OnLegalEntityRelationships_AfterAdd);					
					_LegalEntityRelationships.AfterRemove += new EventListHandler(OnLegalEntityRelationships_AfterRemove);					
				}
				return _LegalEntityRelationships;
			}
		}
		/// <summary>
		/// This is the result of the validation of the Legal Entity ID Number validation (Valid, Duplicate ID Numbers or Invalid)
		/// </summary>
		public ILegalEntityExceptionStatus LegalEntityExceptionStatus 
		{
			get
			{
				if (null == _DAO.LegalEntityExceptionStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityExceptionStatus, LegalEntityExceptionStatus_DAO>(_DAO.LegalEntityExceptionStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityExceptionStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityExceptionStatus = (LegalEntityExceptionStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// This is the foreign key reference to the status of the Legal Entity from the LegalEntityStatus table. e.g. Alive, Deceased
		/// or Disabled.
		/// </summary>
		public ILegalEntityStatus LegalEntityStatus 
		{
			get
			{
				if (null == _DAO.LegalEntityStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityStatus, LegalEntityStatus_DAO>(_DAO.LegalEntityStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityStatus = (LegalEntityStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// A Legal Entity can play many Roles in many different Accounts at SA Home Loans. The Role table stores a foreign key
		/// reference to the Legal Entity and relates the Legal Entity to the RoleType and the Account.
		/// </summary>
		private DAOEventList<Role_DAO, IRole, Role> _Roles;
		/// <summary>
		/// A Legal Entity can play many Roles in many different Accounts at SA Home Loans. The Role table stores a foreign key
		/// reference to the Legal Entity and relates the Legal Entity to the RoleType and the Account.
		/// </summary>
		public IEventList<IRole> Roles
		{
			get
			{
				if (null == _Roles) 
				{
					if(null == _DAO.Roles)
						_DAO.Roles = new List<Role_DAO>();
					_Roles = new DAOEventList<Role_DAO, IRole, Role>(_DAO.Roles);
					_Roles.BeforeAdd += new EventListHandler(OnRoles_BeforeAdd);					
					_Roles.BeforeRemove += new EventListHandler(OnRoles_BeforeRemove);					
					_Roles.AfterAdd += new EventListHandler(OnRoles_AfterAdd);					
					_Roles.AfterRemove += new EventListHandler(OnRoles_AfterRemove);					
				}
				return _Roles;
			}
		}
		/// <summary>
		/// A Legal Entity can play many Roles in many different Applications at SA Home Loans. The OfferRole table stores a foreign key
		/// reference to the Legal Entity and links it to the Application.
		/// </summary>
		private DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole> _ApplicationRoles;
		/// <summary>
		/// A Legal Entity can play many Roles in many different Applications at SA Home Loans. The OfferRole table stores a foreign key
		/// reference to the Legal Entity and links it to the Application.
		/// </summary>
		public IEventList<IApplicationRole> ApplicationRoles
		{
			get
			{
				if (null == _ApplicationRoles) 
				{
					if(null == _DAO.ApplicationRoles)
						_DAO.ApplicationRoles = new List<ApplicationRole_DAO>();
					_ApplicationRoles = new DAOEventList<ApplicationRole_DAO, IApplicationRole, ApplicationRole>(_DAO.ApplicationRoles);
					_ApplicationRoles.BeforeAdd += new EventListHandler(OnApplicationRoles_BeforeAdd);					
					_ApplicationRoles.BeforeRemove += new EventListHandler(OnApplicationRoles_BeforeRemove);					
					_ApplicationRoles.AfterAdd += new EventListHandler(OnApplicationRoles_AfterAdd);					
					_ApplicationRoles.AfterRemove += new EventListHandler(OnApplicationRoles_AfterRemove);					
				}
				return _ApplicationRoles;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_LegalEntityAddresses = null;
			_FailedLegalEntityAddresses = null;
			_Employment = null;
			_LegalEntityAffordabilities = null;
			_LegalEntityAssetLiabilities = null;
			_LegalEntityBankAccounts = null;
			_LegalEntityMarketingOptions = null;
			_LegalEntityRelationships = null;
			_Roles = null;
			_ApplicationRoles = null;
		}
	}
}


