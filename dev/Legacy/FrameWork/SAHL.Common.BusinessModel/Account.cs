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
	/// Account_DAO is the base class from which the product specific accounts are derived. The Account class has been discriminated
		/// according to the Product Type and has the following discriminations:
		/// Account Life PolicyAccount New Variable LoanAccount Super LoAccount Variable LoanAccount Varifix Loan
	/// </summary>
	public abstract partial class Account : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Account_DAO>, IAccount
	{
				public Account(SAHL.Common.BusinessModel.DAO.Account_DAO Account) : base(Account)
		{
			this._DAO = Account;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.FixedPayment
		/// </summary>
		public Double FixedPayment 
		{
			get { return _DAO.FixedPayment; }
			set { _DAO.FixedPayment = value;}
		}
		/// <summary>
		/// The status of the account.  When selecting open accounts, you will need to look at accounts with a status of Open or Dormant.
		/// When selecting closed accounts, you will need to look at accounts with a status of Closed or Locked.
		/// </summary>
		public IAccountStatus AccountStatus 
		{
			get
			{
				if (null == _DAO.AccountStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccountStatus, AccountStatus_DAO>(_DAO.AccountStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AccountStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AccountStatus = (AccountStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The date when the account record was inserted.
		/// </summary>
		public DateTime InsertedDate 
		{
			get { return _DAO.InsertedDate; }
			set { _DAO.InsertedDate = value;}
		}
		/// <summary>
		/// Each of the Origination Sources are related to products which they are allowed to sell to clients. The primary key
		/// from the OriginationSourceProduct table where this relationship is held is stored here for each account.
		/// </summary>
		public IOriginationSourceProduct OriginationSourceProduct 
		{
			get
			{
				if (null == _DAO.OriginationSourceProduct) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSourceProduct, OriginationSourceProduct_DAO>(_DAO.OriginationSourceProduct);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSourceProduct = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The date when the account was opened.
		/// </summary>
		public DateTime? OpenDate
		{
			get { return _DAO.OpenDate; }
			set { _DAO.OpenDate = value;}
		}
		/// <summary>
		/// The date when the account is closed. This remains a NULL value until the account is closed.
		/// </summary>
		public DateTime? CloseDate
		{
			get { return _DAO.CloseDate; }
			set { _DAO.CloseDate = value;}
		}
		/// <summary>
		/// The Account base class is discriminated based on the Product Type. The different product types include VariFix,
		/// New Variable, Super Lo, Defending Discount Rate and Life Policies.
		/// </summary>
		public IProduct Product 
		{
			get
			{
				if (null == _DAO.Product) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProduct, Product_DAO>(_DAO.Product);
					}
			}
		}
		/// <summary>
		/// This is the source of the account which is who was responsible its origination. This would include SA Home Loans, RCS
		/// Home Loans, the Agency Channel or the Mortgage Originators.
		/// </summary>
		public IOriginationSource OriginationSource 
		{
			get
			{
				if (null == _DAO.OriginationSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSource, OriginationSource_DAO>(_DAO.OriginationSource);
					}
			}
		}
		/// <summary>
		/// The UserID of the last person who updated information on the Account.
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// The date when the Account record was last changed.
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
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
		/// An Account can have one or many Legal Entities which play a specific Role in the Account. i.e. Main Applicant/Suretor
		/// </summary>
		private DAOEventList<Role_DAO, IRole, Role> _Roles;
		/// <summary>
		/// An Account can have one or many Legal Entities which play a specific Role in the Account. i.e. Main Applicant/Suretor
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
		/// An Account can have many Financial Services. E.g. A VariFix account will have a Fixed Financial Service and a Variable Financial
		/// Service, where the payment due on each Financial Service will be recorded.
		/// </summary>
		private DAOEventList<FinancialService_DAO, IFinancialService, FinancialService> _FinancialServices;
		/// <summary>
		/// An Account can have many Financial Services. E.g. A VariFix account will have a Fixed Financial Service and a Variable Financial
		/// Service, where the payment due on each Financial Service will be recorded.
		/// </summary>
		public IEventList<IFinancialService> FinancialServices
		{
			get
			{
				if (null == _FinancialServices) 
				{
					if(null == _DAO.FinancialServices)
						_DAO.FinancialServices = new List<FinancialService_DAO>();
					_FinancialServices = new DAOEventList<FinancialService_DAO, IFinancialService, FinancialService>(_DAO.FinancialServices);
					_FinancialServices.BeforeAdd += new EventListHandler(OnFinancialServices_BeforeAdd);					
					_FinancialServices.BeforeRemove += new EventListHandler(OnFinancialServices_BeforeRemove);					
					_FinancialServices.AfterAdd += new EventListHandler(OnFinancialServices_AfterAdd);					
					_FinancialServices.AfterRemove += new EventListHandler(OnFinancialServices_AfterRemove);					
				}
				return _FinancialServices;
			}
		}
		/// <summary>
		/// The applications from the Offer table related to the Account.
		/// </summary>
		private DAOEventList<Application_DAO, IApplication, Application> _Applications;
		/// <summary>
		/// The applications from the Offer table related to the Account.
		/// </summary>
		public IEventList<IApplication> Applications
		{
			get
			{
				if (null == _Applications) 
				{
					if(null == _DAO.Applications)
						_DAO.Applications = new List<Application_DAO>();
					_Applications = new DAOEventList<Application_DAO, IApplication, Application>(_DAO.Applications);
					_Applications.BeforeAdd += new EventListHandler(OnApplications_BeforeAdd);					
					_Applications.BeforeRemove += new EventListHandler(OnApplications_BeforeRemove);					
					_Applications.AfterAdd += new EventListHandler(OnApplications_AfterAdd);					
					_Applications.AfterRemove += new EventListHandler(OnApplications_AfterRemove);					
				}
				return _Applications;
			}
		}
		/// <summary>
		/// This property retrieves the related child accounts through the use of the AccountRelationship table, where the AccountKey
		/// is equal to the AccountRelationship.AccountKey. The RelatedAccountKeys which are retrieved are the child accounts.
		/// </summary>
		private DAOEventList<Account_DAO, IAccount, Account> _RelatedChildAccounts;
		/// <summary>
		/// This property retrieves the related child accounts through the use of the AccountRelationship table, where the AccountKey
		/// is equal to the AccountRelationship.AccountKey. The RelatedAccountKeys which are retrieved are the child accounts.
		/// </summary>
		public IEventList<IAccount> RelatedChildAccounts
		{
			get
			{
				if (null == _RelatedChildAccounts) 
				{
					if(null == _DAO.RelatedChildAccounts)
						_DAO.RelatedChildAccounts = new List<Account_DAO>();
					_RelatedChildAccounts = new DAOEventList<Account_DAO, IAccount, Account>(_DAO.RelatedChildAccounts);
					_RelatedChildAccounts.BeforeAdd += new EventListHandler(OnRelatedChildAccounts_BeforeAdd);					
					_RelatedChildAccounts.BeforeRemove += new EventListHandler(OnRelatedChildAccounts_BeforeRemove);					
					_RelatedChildAccounts.AfterAdd += new EventListHandler(OnRelatedChildAccounts_AfterAdd);					
					_RelatedChildAccounts.AfterRemove += new EventListHandler(OnRelatedChildAccounts_AfterRemove);					
				}
				return _RelatedChildAccounts;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.ParentAccount
		/// </summary>
		public IAccount ParentAccount 
		{
			get
			{
				if (null == _DAO.ParentAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.ParentAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ParentAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ParentAccount = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// An Account can have many Subsidy Providers associated to it. This relationship is defined in the AccountSubsidy table where
		/// the AccountKey is equal to the AccountSubsidy.AccountKey. The SubsiderKeys which are retrieved are those Subsidy Providers related
		/// to the account.
		/// </summary>
		private DAOEventList<Subsidy_DAO, ISubsidy, Subsidy> _Subsidies;
		/// <summary>
		/// An Account can have many Subsidy Providers associated to it. This relationship is defined in the AccountSubsidy table where
		/// the AccountKey is equal to the AccountSubsidy.AccountKey. The SubsiderKeys which are retrieved are those Subsidy Providers related
		/// to the account.
		/// </summary>
		public IEventList<ISubsidy> Subsidies
		{
			get
			{
				if (null == _Subsidies) 
				{
					if(null == _DAO.Subsidies)
						_DAO.Subsidies = new List<Subsidy_DAO>();
					_Subsidies = new DAOEventList<Subsidy_DAO, ISubsidy, Subsidy>(_DAO.Subsidies);
					_Subsidies.BeforeAdd += new EventListHandler(OnSubsidies_BeforeAdd);					
					_Subsidies.BeforeRemove += new EventListHandler(OnSubsidies_BeforeRemove);					
					_Subsidies.AfterAdd += new EventListHandler(OnSubsidies_AfterAdd);					
					_Subsidies.AfterRemove += new EventListHandler(OnSubsidies_AfterRemove);					
				}
				return _Subsidies;
			}
		}
		/// <summary>
		/// An Account has a Mailing Address to which correspondence is sent. The relationship is defined in the MailingAddress table where
		/// the AccountKey is equal to the MailingAddress.AcccountKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Account.
		/// </summary>
		private DAOEventList<MailingAddress_DAO, IMailingAddress, MailingAddress> _MailingAddresses;
		/// <summary>
		/// An Account has a Mailing Address to which correspondence is sent. The relationship is defined in the MailingAddress table where
		/// the AccountKey is equal to the MailingAddress.AcccountKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Account.
		/// </summary>
		public IEventList<IMailingAddress> MailingAddresses
		{
			get
			{
				if (null == _MailingAddresses) 
				{
					if(null == _DAO.MailingAddresses)
						_DAO.MailingAddresses = new List<MailingAddress_DAO>();
					_MailingAddresses = new DAOEventList<MailingAddress_DAO, IMailingAddress, MailingAddress>(_DAO.MailingAddresses);
					_MailingAddresses.BeforeAdd += new EventListHandler(OnMailingAddresses_BeforeAdd);					
					_MailingAddresses.BeforeRemove += new EventListHandler(OnMailingAddresses_BeforeRemove);					
					_MailingAddresses.AfterAdd += new EventListHandler(OnMailingAddresses_AfterAdd);					
					_MailingAddresses.AfterRemove += new EventListHandler(OnMailingAddresses_AfterRemove);					
				}
				return _MailingAddresses;
			}
		}
		/// <summary>
		/// Certain pieces of information regarding the Account are stored in the AccountInformation table. Currently these would include
		/// Product Opt In/Out and Conversions, the Account Legal Name and the Interest Only Maturity Date.
		/// </summary>
		private DAOEventList<AccountInformation_DAO, IAccountInformation, AccountInformation> _AccountInformations;
		/// <summary>
		/// Certain pieces of information regarding the Account are stored in the AccountInformation table. Currently these would include
		/// Product Opt In/Out and Conversions, the Account Legal Name and the Interest Only Maturity Date.
		/// </summary>
		public IEventList<IAccountInformation> AccountInformations
		{
			get
			{
				if (null == _AccountInformations) 
				{
					if(null == _DAO.AccountInformations)
						_DAO.AccountInformations = new List<AccountInformation_DAO>();
					_AccountInformations = new DAOEventList<AccountInformation_DAO, IAccountInformation, AccountInformation>(_DAO.AccountInformations);
					_AccountInformations.BeforeAdd += new EventListHandler(OnAccountInformations_BeforeAdd);					
					_AccountInformations.BeforeRemove += new EventListHandler(OnAccountInformations_BeforeRemove);					
					_AccountInformations.AfterAdd += new EventListHandler(OnAccountInformations_AfterAdd);					
					_AccountInformations.AfterRemove += new EventListHandler(OnAccountInformations_AfterRemove);					
				}
				return _AccountInformations;
			}
		}
		/// <summary>
		/// An Account has many Details.  These will eventually be removed as Detail Types are being made obsolete.
		/// </summary>
		private DAOEventList<Detail_DAO, IDetail, Detail> _Details;
		/// <summary>
		/// An Account has many Details.  These will eventually be removed as Detail Types are being made obsolete.
		/// </summary>
		public IEventList<IDetail> Details
		{
			get
			{
				if (null == _Details) 
				{
					if(null == _DAO.Details)
						_DAO.Details = new List<Detail_DAO>();
					_Details = new DAOEventList<Detail_DAO, IDetail, Detail>(_DAO.Details);
					_Details.BeforeAdd += new EventListHandler(OnDetails_BeforeAdd);					
					_Details.BeforeRemove += new EventListHandler(OnDetails_BeforeRemove);					
					_Details.AfterAdd += new EventListHandler(OnDetails_AfterAdd);					
					_Details.AfterRemove += new EventListHandler(OnDetails_AfterRemove);					
				}
				return _Details;
			}
		}
		/// <summary>
		/// Basel Import Scores
		/// </summary>
		private DAOEventList<AccountBaselII_DAO, IAccountBaselII, AccountBaselII> _AccountBaselII;
		/// <summary>
		/// Basel Import Scores
		/// </summary>
		public IEventList<IAccountBaselII> AccountBaselII
		{
			get
			{
				if (null == _AccountBaselII) 
				{
					if(null == _DAO.AccountBaselII)
						_DAO.AccountBaselII = new List<AccountBaselII_DAO>();
					_AccountBaselII = new DAOEventList<AccountBaselII_DAO, IAccountBaselII, AccountBaselII>(_DAO.AccountBaselII);
					_AccountBaselII.BeforeAdd += new EventListHandler(OnAccountBaselII_BeforeAdd);					
					_AccountBaselII.BeforeRemove += new EventListHandler(OnAccountBaselII_BeforeRemove);					
					_AccountBaselII.AfterAdd += new EventListHandler(OnAccountBaselII_AfterAdd);					
					_AccountBaselII.AfterRemove += new EventListHandler(OnAccountBaselII_AfterRemove);					
				}
				return _AccountBaselII;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.SPV
		/// </summary>
		public ISPV SPV 
		{
			get
			{
				if (null == _DAO.SPV) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ISPV, SPV_DAO>(_DAO.SPV);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.SPV = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.SPV = (SPV_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Roles = null;
			_FinancialServices = null;
			_Applications = null;
			_RelatedChildAccounts = null;
			_Subsidies = null;
			_MailingAddresses = null;
			_AccountInformations = null;
			_Details = null;
			_AccountBaselII = null;
			
		}
	}
}


