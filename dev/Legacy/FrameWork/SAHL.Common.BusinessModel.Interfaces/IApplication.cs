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
	/// Application_DAO is the base class from which the Application Type specific Applications are derived.
	/// </summary>
	public partial interface IApplication : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// The Account Key reserved for the Application.
		/// </summary>
		IAccountSequence ReservedAccount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Callbacks
		/// </summary>
		IEventList<ICallback> Callbacks
		{
			get;
		}
		/// <summary>
		/// The date on which the Application commenced.
		/// </summary>
		DateTime? ApplicationStartDate
		{
			get;
			set;
		}
		/// <summary>
		/// The date on which the Application ends.
		/// </summary>
		DateTime? ApplicationEndDate
		{
			get;
			set;
		}
		/// <summary>
		/// Indicates the estimated number of applicants when the application is first captured.
		/// </summary>
		Int32? EstimateNumberApplicants
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Account
		/// </summary>
		IAccount Account
		{
			get;
			set;
		}
		/// <summary>
		/// A Reference number for the Application.
		/// </summary>
		System.String Reference
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
		/// An Application can have a many Conditions associated to it. This relationship is defined in the OfferCondition table where the
		/// Offer.OfferKey = OfferCondition.OfferKey. The OfferCondition.ConditionKey's which are retrieved are the Conditions attached
		/// to the Application.
		/// </summary>
		IEventList<IApplicationCondition> ApplicationConditions
		{
			get;
		}
		/// <summary>
		/// An Application can have a many Attributes associated to it. This relationship is defined in the OfferAttribute table where the
		/// Offer.OfferKey = OfferAttribute.OfferKey.
		/// </summary>
		IEventList<IApplicationAttribute> ApplicationAttributes
		{
			get;
		}
		/// <summary>
		/// An Application can have many Application Information records associated to it. The Application Information records are
		/// stored in the OfferInformation table
		/// </summary>
		IEventList<IApplicationInformation> ApplicationInformations
		{
			get;
		}
		/// <summary>
		/// Each Application is linked to a specific Campaign reference which business areas like Telecenter and SAHL Direct use
		/// as references.
		/// </summary>
		IApplicationCampaign ApplicationCampaign
		{
			get;
			set;
		}
		/// <summary>
		/// The Status of the Application (Open/Closed/Accepted/Declined/NTU'd)
		/// </summary>
		IApplicationStatus ApplicationStatus
		{
			get;
			set;
		}
		/// <summary>
		/// The Origination Source of this application.
		/// </summary>
		IOriginationSource OriginationSource
		{
			get;
			set;
		}
		/// <summary>
		/// Specifies where the application came from.
		/// </summary>
		IApplicationSource ApplicationSource
		{
			get;
			set;
		}
		/// <summary>
		/// Specifies the type of Application  e.g. Readvance, Further Loan etc.
		/// </summary>
		IApplicationType ApplicationType
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationMarketingSurveyTypes
		/// </summary>
		IEventList<IApplicationMarketingSurveyType> ApplicationMarketingSurveyTypes
		{
			get;
		}
		/// <summary>
		/// An Application has a Mailing Address to which correspondence can be sent. This relationship is defined in the OfferMailingAddress
		/// table where Offer.OfferKey = OfferMailingAddress.OfferKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Application.
		/// </summary>
		IEventList<IApplicationMailingAddress> ApplicationMailingAddresses
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationDebitOrders
		/// </summary>
		IEventList<IApplicationDebitOrder> ApplicationDebitOrders
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.RelatedAccounts
		/// </summary>
		IEventList<IAccount> RelatedAccounts
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationExpenses
		/// </summary>
		IEventList<IApplicationExpense> ApplicationExpenses
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Subsidies
		/// </summary>
		IEventList<ISubsidy> Subsidies
		{
			get;
		}
	}
}


