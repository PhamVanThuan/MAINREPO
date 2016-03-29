using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// Account_DAO is the base class from which the product specific accounts are derived. The Account class has been discriminated
		/// according to the Product Type and has the following discriminations:
		/// Account Life PolicyAccount New Variable LoanAccount Super LoAccount Variable LoanAccount Varifix Loan
	/// </summary>
	public partial interface IAccount : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.FixedPayment
		/// </summary>
		System.Double FixedPayment
		{
			get;
		}
		/// <summary>
		/// The status of the account.  When selecting open accounts, you will need to look at accounts with a status of Open or Dormant.
		/// When selecting closed accounts, you will need to look at accounts with a status of Closed or Locked.
		/// </summary>
		IAccountStatus AccountStatus
		{
			get;
		}
		/// <summary>
		/// The date when the account record was inserted.
		/// </summary>
		System.DateTime InsertedDate
		{
			get;
		}
		/// <summary>
		/// Each of the Origination Sources are related to products which they are allowed to sell to clients. The primary key
		/// from the OriginationSourceProduct table where this relationship is held is stored here for each account.
		/// </summary>
		IOriginationSourceProduct OriginationSourceProduct
		{
			get;
		}
		/// <summary>
		/// The date when the account was opened.
		/// </summary>
		DateTime? OpenDate
		{
			get;
		}
		/// <summary>
		/// The date when the account is closed. This remains a NULL value until the account is closed.
		/// </summary>
		DateTime? CloseDate
		{
			get;
		}
		/// <summary>
		/// The Account base class is discriminated based on the Product Type. The different product types include VariFix,
		/// New Variable, Super Lo, Defending Discount Rate and Life Policies.
		/// </summary>
		IProduct Product
		{
			get;
		}
		/// <summary>
		/// This is the source of the account which is who was responsible its origination. This would include SA Home Loans, RCS
		/// Home Loans, the Agency Channel or the Mortgage Originators.
		/// </summary>
		IOriginationSource OriginationSource
		{
			get;
		}
		/// <summary>
		/// The UserID of the last person who updated information on the Account.
		/// </summary>
		System.String UserID
		{
			get;
		}
		/// <summary>
		/// The date when the Account record was last changed.
		/// </summary>
		DateTime? ChangeDate
		{
			get;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		System.Int32 Key
		{
			get;
		}
		/// <summary>
		/// An Account can have one or many Legal Entities which play a specific Role in the Account. i.e. Main Applicant/Suretor
		/// </summary>
		IEventList<IRole> Roles
		{
			get;
		}
		/// <summary>
		/// An Account can have many Financial Services. E.g. A VariFix account will have a Fixed Financial Service and a Variable Financial
		/// Service, where the payment due on each Financial Service will be recorded.
		/// </summary>
		IEventList<IFinancialService> FinancialServices
		{
			get;
		}
		/// <summary>
		/// The applications from the Offer table related to the Account.
		/// </summary>
		IEventList<IApplication> Applications
		{
			get;
		}
		/// <summary>
		/// This property retrieves the related child accounts through the use of the AccountRelationship table, where the AccountKey
		/// is equal to the AccountRelationship.AccountKey. The RelatedAccountKeys which are retrieved are the child accounts.
		/// </summary>
		IEventList<IAccount> RelatedChildAccounts
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.ParentAccount
		/// </summary>
		IAccount ParentAccount
		{
			get;
		}
		/// <summary>
		/// An Account can have many Subsidy Providers associated to it. This relationship is defined in the AccountSubsidy table where
		/// the AccountKey is equal to the AccountSubsidy.AccountKey. The SubsiderKeys which are retrieved are those Subsidy Providers related
		/// to the account.
		/// </summary>
		IEventList<ISubsidy> Subsidies
		{
			get;
		}
		/// <summary>
		/// An Account has a Mailing Address to which correspondence is sent. The relationship is defined in the MailingAddress table where
		/// the AccountKey is equal to the MailingAddress.AcccountKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Account.
		/// </summary>
		IEventList<IMailingAddress> MailingAddresses
		{
			get;
		}
		/// <summary>
		/// Certain pieces of information regarding the Account are stored in the AccountInformation table. Currently these would include
		/// Product Opt In/Out and Conversions, the Account Legal Name and the Interest Only Maturity Date.
		/// </summary>
		IEventList<IAccountInformation> AccountInformations
		{
			get;
		}
		/// <summary>
		/// An Account has many Details.  These will eventually be removed as Detail Types are being made obsolete.
		/// </summary>
		IEventList<IDetail> Details
		{
			get;
		}
		/// <summary>
		/// Basel Import Scores
		/// </summary>
		IEventList<IAccountBaselII> AccountBaselII
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Account_DAO.SPV
		/// </summary>
		ISPV SPV
		{
			get;
		}
	}
}


