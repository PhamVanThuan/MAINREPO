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
	/// LegalEntity_DAO is the base class from which Legal Entity Type specific Legal Entities are derived. Legal Entities are
		/// discriminated based on the LegalEntityType and has the following discriminations:
		/// Natural PersonClose CorporationTrustCompanyUnknown
	/// </summary>
	public partial interface ILegalEntity : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// The date on which the Legal Entity was first introduced to SA Home Loans
		/// </summary>
		System.DateTime IntroductionDate
		{
			get;
			set;
		}
		/// <summary>
		/// The Legal Entity's Tax Number
		/// </summary>
		System.String TaxNumber
		{
			get;
			set;
		}
		/// <summary>
		/// The Area Code for the Legal Entity's Home Phone Number
		/// </summary>
		System.String HomePhoneCode
		{
			get;
			set;
		}
		/// <summary>
		/// The Home Phone Number
		/// </summary>
		System.String HomePhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// The Area Code for the Legal Entity's Work Phone Number
		/// </summary>
		System.String WorkPhoneCode
		{
			get;
			set;
		}
		/// <summary>
		/// The Work Phone Number
		/// </summary>
		System.String WorkPhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// The Legal Entity's Cell Phone Number including the Code.
		/// </summary>
		System.String CellPhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// The Legal Entity's Email Address
		/// </summary>
		System.String EmailAddress
		{
			get;
			set;
		}
		/// <summary>
		/// The Area code for the Legal Entity's Fax Number
		/// </summary>
		System.String FaxCode
		{
			get;
			set;
		}
		/// <summary>
		/// The Fax Number
		/// </summary>
		System.String FaxNumber
		{
			get;
			set;
		}
		/// <summary>
		/// A Password chosen by the Legal Entity in order access Loan information online.
		/// </summary>
		System.String Password
		{
			get;
			set;
		}
		/// <summary>
		/// A Comments Property
		/// </summary>
		System.String Comments
		{
			get;
			set;
		}
		/// <summary>
		/// The UserID of the person who last updated the Legal Entity record.
		/// </summary>
		System.String UserID
		{
			get;
			set;
		}
		/// <summary>
		/// The date on which the Legal Entity record was last changed.
		/// </summary>
		DateTime? ChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// The preferred language in which the Legal Entity receives documentation.
		/// </summary>
		ILanguage DocumentLanguage
		{
			get;
			set;
		}
		/// <summary>
		/// The South African Residence status of the Legal Entity.
		/// </summary>
		IResidenceStatus ResidenceStatus
		{
			get;
			set;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// A Legal Entity can have many Addresses. This relationship is defined in the LegalEntityAddress table.
		/// </summary>
		IEventList<ILegalEntityAddress> LegalEntityAddresses
		{
			get;
		}
		/// <summary>
		/// Provides a list of dirty legal entity addresses.  This list should never be added to: it contains old
		/// records that could not be successfully migrated and required cleaning up.  Over time, these should be
		/// replaced with valid LegalEntityAddress objects and their IsCleaned properties set to true.
		/// </summary>
		IEventList<IFailedLegalEntityAddress> FailedLegalEntityAddresses
		{
			get;
		}
		/// <summary>
		/// A Legal Entity can have many Employment records. The Employment table stores a Foreign Key Reference to the LegalEntityKey.
		/// </summary>
		IEventList<IEmployment> Employment
		{
			get;
		}
		/// <summary>
		/// The details of the Affordability Assessment for the Legal Entity is required to be stored. A Legal Entity has a set of Affordability entries
		/// which is related to the Legal Entity by way of the LegalEntityAffordability table, which stores a foreign key reference
		/// to the Legal Entity.
		/// </summary>
		IEventList<ILegalEntityAffordability> LegalEntityAffordabilities
		{
			get;
		}
		/// <summary>
		/// In certain cases the Legal Entity is required to provide Asset and Liability information. A Legal Entity will have a set
		/// of Asset/Liability records which is related to the Legal Entity by way of the LegalEntityAssetLiability table, which stores
		/// a foreign key reference to the Legal Entity.
		/// </summary>
		IEventList<ILegalEntityAssetLiability> LegalEntityAssetLiabilities
		{
			get;
		}
		/// <summary>
		/// A Legal Entity can have many Bank Account records. This relationship is defined in the LegalEntityBankAccount table,
		/// which has a foreign key reference to the Legal Entity.
		/// </summary>
		IEventList<ILegalEntityBankAccount> LegalEntityBankAccounts
		{
			get;
		}
		/// <summary>
		/// A Legal Entity can choose many options by which SAHL can market new products to the client. This relationship is defined
		/// in the LegalEntityMarketingOption table, which has a foreign key reference to the Legal Entity.
		/// </summary>
		IEventList<ILegalEntityMarketingOption> LegalEntityMarketingOptions
		{
			get;
		}
		/// <summary>
		/// We are assuming the relationship exist in the relationship table
		/// </summary>
		IEventList<ILegalEntityRelationship> LegalEntityRelationships
		{
			get;
		}
		/// <summary>
		/// This is the result of the validation of the Legal Entity ID Number validation (Valid, Duplicate ID Numbers or Invalid)
		/// </summary>
		ILegalEntityExceptionStatus LegalEntityExceptionStatus
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the status of the Legal Entity from the LegalEntityStatus table. e.g. Alive, Deceased
		/// or Disabled.
		/// </summary>
		ILegalEntityStatus LegalEntityStatus
		{
			get;
			set;
		}
		/// <summary>
		/// A Legal Entity can play many Roles in many different Accounts at SA Home Loans. The Role table stores a foreign key
		/// reference to the Legal Entity and relates the Legal Entity to the RoleType and the Account.
		/// </summary>
		IEventList<IRole> Roles
		{
			get;
		}
		/// <summary>
		/// A Legal Entity can play many Roles in many different Applications at SA Home Loans. The OfferRole table stores a foreign key
		/// reference to the Legal Entity and links it to the Application.
		/// </summary>
		IEventList<IApplicationRole> ApplicationRoles
		{
			get;
		}
	}
}


