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
	/// Application_DAO is the base class from which the Application Type specific Applications are derived.
	/// </summary>
	public partial class Application : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Application_DAO>, IApplication
	{
				public Application(SAHL.Common.BusinessModel.DAO.Application_DAO Application) : base(Application)
		{
			this._DAO = Application;
		}
		/// <summary>
		/// The Account Key reserved for the Application.
		/// </summary>
		public IAccountSequence ReservedAccount 
		{
			get
			{
				if (null == _DAO.ReservedAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccountSequence, AccountSequence_DAO>(_DAO.ReservedAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReservedAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReservedAccount = (AccountSequence_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Callbacks
		/// </summary>
		private DAOEventList<Callback_DAO, ICallback, Callback> _Callbacks;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Callbacks
		/// </summary>
		public IEventList<ICallback> Callbacks
		{
			get
			{
				if (null == _Callbacks) 
				{
					if(null == _DAO.Callbacks)
						_DAO.Callbacks = new List<Callback_DAO>();
					_Callbacks = new DAOEventList<Callback_DAO, ICallback, Callback>(_DAO.Callbacks);
					_Callbacks.BeforeAdd += new EventListHandler(OnCallbacks_BeforeAdd);					
					_Callbacks.BeforeRemove += new EventListHandler(OnCallbacks_BeforeRemove);					
					_Callbacks.AfterAdd += new EventListHandler(OnCallbacks_AfterAdd);					
					_Callbacks.AfterRemove += new EventListHandler(OnCallbacks_AfterRemove);					
				}
				return _Callbacks;
			}
		}
		/// <summary>
		/// The date on which the Application commenced.
		/// </summary>
		public DateTime? ApplicationStartDate
		{
			get { return _DAO.ApplicationStartDate; }
			set { _DAO.ApplicationStartDate = value;}
		}
		/// <summary>
		/// The date on which the Application ends.
		/// </summary>
		public DateTime? ApplicationEndDate
		{
			get { return _DAO.ApplicationEndDate; }
			set { _DAO.ApplicationEndDate = value;}
		}
		/// <summary>
		/// Indicates the estimated number of applicants when the application is first captured.
		/// </summary>
		public Int32? EstimateNumberApplicants
		{
			get { return _DAO.EstimateNumberApplicants; }
			set { _DAO.EstimateNumberApplicants = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Account
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _DAO.Account) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// A Reference number for the Application.
		/// </summary>
		public String Reference 
		{
			get { return _DAO.Reference; }
			set { _DAO.Reference = value;}
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
		/// An Application can have a many Conditions associated to it. This relationship is defined in the OfferCondition table where the
		/// Offer.OfferKey = OfferCondition.OfferKey. The OfferCondition.ConditionKey's which are retrieved are the Conditions attached
		/// to the Application.
		/// </summary>
		private DAOEventList<ApplicationCondition_DAO, IApplicationCondition, ApplicationCondition> _ApplicationConditions;
		/// <summary>
		/// An Application can have a many Conditions associated to it. This relationship is defined in the OfferCondition table where the
		/// Offer.OfferKey = OfferCondition.OfferKey. The OfferCondition.ConditionKey's which are retrieved are the Conditions attached
		/// to the Application.
		/// </summary>
		public IEventList<IApplicationCondition> ApplicationConditions
		{
			get
			{
				if (null == _ApplicationConditions) 
				{
					if(null == _DAO.ApplicationConditions)
						_DAO.ApplicationConditions = new List<ApplicationCondition_DAO>();
					_ApplicationConditions = new DAOEventList<ApplicationCondition_DAO, IApplicationCondition, ApplicationCondition>(_DAO.ApplicationConditions);
					_ApplicationConditions.BeforeAdd += new EventListHandler(OnApplicationConditions_BeforeAdd);					
					_ApplicationConditions.BeforeRemove += new EventListHandler(OnApplicationConditions_BeforeRemove);					
					_ApplicationConditions.AfterAdd += new EventListHandler(OnApplicationConditions_AfterAdd);					
					_ApplicationConditions.AfterRemove += new EventListHandler(OnApplicationConditions_AfterRemove);					
				}
				return _ApplicationConditions;
			}
		}
		/// <summary>
		/// An Application can have a many Attributes associated to it. This relationship is defined in the OfferAttribute table where the
		/// Offer.OfferKey = OfferAttribute.OfferKey.
		/// </summary>
		private DAOEventList<ApplicationAttribute_DAO, IApplicationAttribute, ApplicationAttribute> _ApplicationAttributes;
		/// <summary>
		/// An Application can have a many Attributes associated to it. This relationship is defined in the OfferAttribute table where the
		/// Offer.OfferKey = OfferAttribute.OfferKey.
		/// </summary>
		public IEventList<IApplicationAttribute> ApplicationAttributes
		{
			get
			{
				if (null == _ApplicationAttributes) 
				{
					if(null == _DAO.ApplicationAttributes)
						_DAO.ApplicationAttributes = new List<ApplicationAttribute_DAO>();
					_ApplicationAttributes = new DAOEventList<ApplicationAttribute_DAO, IApplicationAttribute, ApplicationAttribute>(_DAO.ApplicationAttributes);
					_ApplicationAttributes.BeforeAdd += new EventListHandler(OnApplicationAttributes_BeforeAdd);					
					_ApplicationAttributes.BeforeRemove += new EventListHandler(OnApplicationAttributes_BeforeRemove);					
					_ApplicationAttributes.AfterAdd += new EventListHandler(OnApplicationAttributes_AfterAdd);					
					_ApplicationAttributes.AfterRemove += new EventListHandler(OnApplicationAttributes_AfterRemove);					
				}
				return _ApplicationAttributes;
			}
		}
		/// <summary>
		/// An Application can have many Application Information records associated to it. The Application Information records are
		/// stored in the OfferInformation table
		/// </summary>
		private DAOEventList<ApplicationInformation_DAO, IApplicationInformation, ApplicationInformation> _ApplicationInformations;
		/// <summary>
		/// An Application can have many Application Information records associated to it. The Application Information records are
		/// stored in the OfferInformation table
		/// </summary>
		public IEventList<IApplicationInformation> ApplicationInformations
		{
			get
			{
				if (null == _ApplicationInformations) 
				{
					if(null == _DAO.ApplicationInformations)
						_DAO.ApplicationInformations = new List<ApplicationInformation_DAO>();
					_ApplicationInformations = new DAOEventList<ApplicationInformation_DAO, IApplicationInformation, ApplicationInformation>(_DAO.ApplicationInformations);
					_ApplicationInformations.BeforeAdd += new EventListHandler(OnApplicationInformations_BeforeAdd);					
					_ApplicationInformations.BeforeRemove += new EventListHandler(OnApplicationInformations_BeforeRemove);					
					_ApplicationInformations.AfterAdd += new EventListHandler(OnApplicationInformations_AfterAdd);					
					_ApplicationInformations.AfterRemove += new EventListHandler(OnApplicationInformations_AfterRemove);					
				}
				return _ApplicationInformations;
			}
		}
		/// <summary>
		/// Each Application is linked to a specific Campaign reference which business areas like Telecenter and SAHL Direct use
		/// as references.
		/// </summary>
		public IApplicationCampaign ApplicationCampaign 
		{
			get
			{
				if (null == _DAO.ApplicationCampaign) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationCampaign, ApplicationCampaign_DAO>(_DAO.ApplicationCampaign);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationCampaign = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationCampaign = (ApplicationCampaign_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The Status of the Application (Open/Closed/Accepted/Declined/NTU'd)
		/// </summary>
		public IApplicationStatus ApplicationStatus 
		{
			get
			{
				if (null == _DAO.ApplicationStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationStatus, ApplicationStatus_DAO>(_DAO.ApplicationStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationStatus = (ApplicationStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The Origination Source of this application.
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

			set
			{
				if(value == null)
				{
					_DAO.OriginationSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSource = (OriginationSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Specifies where the application came from.
		/// </summary>
		public IApplicationSource ApplicationSource 
		{
			get
			{
				if (null == _DAO.ApplicationSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationSource, ApplicationSource_DAO>(_DAO.ApplicationSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationSource = (ApplicationSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Specifies the type of Application  e.g. Readvance, Further Loan etc.
		/// </summary>
		public IApplicationType ApplicationType 
		{
			get
			{
				if (null == _DAO.ApplicationType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationType, ApplicationType_DAO>(_DAO.ApplicationType);
					}
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationMarketingSurveyTypes
		/// </summary>
		private DAOEventList<ApplicationMarketingSurveyType_DAO, IApplicationMarketingSurveyType, ApplicationMarketingSurveyType> _ApplicationMarketingSurveyTypes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationMarketingSurveyTypes
		/// </summary>
		public IEventList<IApplicationMarketingSurveyType> ApplicationMarketingSurveyTypes
		{
			get
			{
				if (null == _ApplicationMarketingSurveyTypes) 
				{
					if(null == _DAO.ApplicationMarketingSurveyTypes)
						_DAO.ApplicationMarketingSurveyTypes = new List<ApplicationMarketingSurveyType_DAO>();
					_ApplicationMarketingSurveyTypes = new DAOEventList<ApplicationMarketingSurveyType_DAO, IApplicationMarketingSurveyType, ApplicationMarketingSurveyType>(_DAO.ApplicationMarketingSurveyTypes);
					_ApplicationMarketingSurveyTypes.BeforeAdd += new EventListHandler(OnApplicationMarketingSurveyTypes_BeforeAdd);					
					_ApplicationMarketingSurveyTypes.BeforeRemove += new EventListHandler(OnApplicationMarketingSurveyTypes_BeforeRemove);					
					_ApplicationMarketingSurveyTypes.AfterAdd += new EventListHandler(OnApplicationMarketingSurveyTypes_AfterAdd);					
					_ApplicationMarketingSurveyTypes.AfterRemove += new EventListHandler(OnApplicationMarketingSurveyTypes_AfterRemove);					
				}
				return _ApplicationMarketingSurveyTypes;
			}
		}
		/// <summary>
		/// An Application has a Mailing Address to which correspondence can be sent. This relationship is defined in the OfferMailingAddress
		/// table where Offer.OfferKey = OfferMailingAddress.OfferKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Application.
		/// </summary>
		private DAOEventList<ApplicationMailingAddress_DAO, IApplicationMailingAddress, ApplicationMailingAddress> _ApplicationMailingAddresses;
		/// <summary>
		/// An Application has a Mailing Address to which correspondence can be sent. This relationship is defined in the OfferMailingAddress
		/// table where Offer.OfferKey = OfferMailingAddress.OfferKey. The AddressKey which is retrieved is the Mailing Address for the
		/// Application.
		/// </summary>
		public IEventList<IApplicationMailingAddress> ApplicationMailingAddresses
		{
			get
			{
				if (null == _ApplicationMailingAddresses) 
				{
					if(null == _DAO.ApplicationMailingAddresses)
						_DAO.ApplicationMailingAddresses = new List<ApplicationMailingAddress_DAO>();
					_ApplicationMailingAddresses = new DAOEventList<ApplicationMailingAddress_DAO, IApplicationMailingAddress, ApplicationMailingAddress>(_DAO.ApplicationMailingAddresses);
					_ApplicationMailingAddresses.BeforeAdd += new EventListHandler(OnApplicationMailingAddresses_BeforeAdd);					
					_ApplicationMailingAddresses.BeforeRemove += new EventListHandler(OnApplicationMailingAddresses_BeforeRemove);					
					_ApplicationMailingAddresses.AfterAdd += new EventListHandler(OnApplicationMailingAddresses_AfterAdd);					
					_ApplicationMailingAddresses.AfterRemove += new EventListHandler(OnApplicationMailingAddresses_AfterRemove);					
				}
				return _ApplicationMailingAddresses;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationDebitOrders
		/// </summary>
		private DAOEventList<ApplicationDebitOrder_DAO, IApplicationDebitOrder, ApplicationDebitOrder> _ApplicationDebitOrders;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationDebitOrders
		/// </summary>
		public IEventList<IApplicationDebitOrder> ApplicationDebitOrders
		{
			get
			{
				if (null == _ApplicationDebitOrders) 
				{
					if(null == _DAO.ApplicationDebitOrders)
						_DAO.ApplicationDebitOrders = new List<ApplicationDebitOrder_DAO>();
					_ApplicationDebitOrders = new DAOEventList<ApplicationDebitOrder_DAO, IApplicationDebitOrder, ApplicationDebitOrder>(_DAO.ApplicationDebitOrders);
					_ApplicationDebitOrders.BeforeAdd += new EventListHandler(OnApplicationDebitOrders_BeforeAdd);					
					_ApplicationDebitOrders.BeforeRemove += new EventListHandler(OnApplicationDebitOrders_BeforeRemove);					
					_ApplicationDebitOrders.AfterAdd += new EventListHandler(OnApplicationDebitOrders_AfterAdd);					
					_ApplicationDebitOrders.AfterRemove += new EventListHandler(OnApplicationDebitOrders_AfterRemove);					
				}
				return _ApplicationDebitOrders;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.RelatedAccounts
		/// </summary>
		private DAOEventList<Account_DAO, IAccount, Account> _RelatedAccounts;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.RelatedAccounts
		/// </summary>
		public IEventList<IAccount> RelatedAccounts
		{
			get
			{
				if (null == _RelatedAccounts) 
				{
					if(null == _DAO.RelatedAccounts)
						_DAO.RelatedAccounts = new List<Account_DAO>();
					_RelatedAccounts = new DAOEventList<Account_DAO, IAccount, Account>(_DAO.RelatedAccounts);
					_RelatedAccounts.BeforeAdd += new EventListHandler(OnRelatedAccounts_BeforeAdd);					
					_RelatedAccounts.BeforeRemove += new EventListHandler(OnRelatedAccounts_BeforeRemove);					
					_RelatedAccounts.AfterAdd += new EventListHandler(OnRelatedAccounts_AfterAdd);					
					_RelatedAccounts.AfterRemove += new EventListHandler(OnRelatedAccounts_AfterRemove);					
				}
				return _RelatedAccounts;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationExpenses
		/// </summary>
		private DAOEventList<ApplicationExpense_DAO, IApplicationExpense, ApplicationExpense> _ApplicationExpenses;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationExpenses
		/// </summary>
		public IEventList<IApplicationExpense> ApplicationExpenses
		{
			get
			{
				if (null == _ApplicationExpenses) 
				{
					if(null == _DAO.ApplicationExpenses)
						_DAO.ApplicationExpenses = new List<ApplicationExpense_DAO>();
					_ApplicationExpenses = new DAOEventList<ApplicationExpense_DAO, IApplicationExpense, ApplicationExpense>(_DAO.ApplicationExpenses);
					_ApplicationExpenses.BeforeAdd += new EventListHandler(OnApplicationExpenses_BeforeAdd);					
					_ApplicationExpenses.BeforeRemove += new EventListHandler(OnApplicationExpenses_BeforeRemove);					
					_ApplicationExpenses.AfterAdd += new EventListHandler(OnApplicationExpenses_AfterAdd);					
					_ApplicationExpenses.AfterRemove += new EventListHandler(OnApplicationExpenses_AfterRemove);					
				}
				return _ApplicationExpenses;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Subsidies
		/// </summary>
		private DAOEventList<Subsidy_DAO, ISubsidy, Subsidy> _Subsidies;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Subsidies
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
		public override void Refresh()
		{
			base.Refresh();
			_Callbacks = null;
			_ApplicationConditions = null;
			_ApplicationAttributes = null;
			_ApplicationInformations = null;
			_ApplicationMarketingSurveyTypes = null;
			_ApplicationMailingAddresses = null;
			_ApplicationDebitOrders = null;
			_RelatedAccounts = null;
			_ApplicationExpenses = null;
			_Subsidies = null;
			
		}
	}
}


