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
	/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO
	/// </summary>
	public partial class FinancialService : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialService_DAO>, IFinancialService
	{
				public FinancialService(SAHL.Common.BusinessModel.DAO.FinancialService_DAO FinancialService) : base(FinancialService)
		{
			this._DAO = FinancialService;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Payment
		/// </summary>
		public Double Payment 
		{
			get { return _DAO.Payment; }
			set { _DAO.Payment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Trade
		/// </summary>
		public ITrade Trade 
		{
			get
			{
				if (null == _DAO.Trade) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITrade, Trade_DAO>(_DAO.Trade);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Trade = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Trade = (Trade_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.NextResetDate
		/// </summary>
		public DateTime? NextResetDate
		{
			get { return _DAO.NextResetDate; }
			set { _DAO.NextResetDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceBankAccounts
		/// </summary>
		private DAOEventList<FinancialServiceBankAccount_DAO, IFinancialServiceBankAccount, FinancialServiceBankAccount> _FinancialServiceBankAccounts;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceBankAccounts
		/// </summary>
		public IEventList<IFinancialServiceBankAccount> FinancialServiceBankAccounts
		{
			get
			{
				if (null == _FinancialServiceBankAccounts) 
				{
					if(null == _DAO.FinancialServiceBankAccounts)
						_DAO.FinancialServiceBankAccounts = new List<FinancialServiceBankAccount_DAO>();
					_FinancialServiceBankAccounts = new DAOEventList<FinancialServiceBankAccount_DAO, IFinancialServiceBankAccount, FinancialServiceBankAccount>(_DAO.FinancialServiceBankAccounts);
					_FinancialServiceBankAccounts.BeforeAdd += new EventListHandler(OnFinancialServiceBankAccounts_BeforeAdd);					
					_FinancialServiceBankAccounts.BeforeRemove += new EventListHandler(OnFinancialServiceBankAccounts_BeforeRemove);					
					_FinancialServiceBankAccounts.AfterAdd += new EventListHandler(OnFinancialServiceBankAccounts_AfterAdd);					
					_FinancialServiceBankAccounts.AfterRemove += new EventListHandler(OnFinancialServiceBankAccounts_AfterRemove);					
				}
				return _FinancialServiceBankAccounts;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceConditions
		/// </summary>
		private DAOEventList<FinancialServiceCondition_DAO, IFinancialServiceCondition, FinancialServiceCondition> _FinancialServiceConditions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceConditions
		/// </summary>
		public IEventList<IFinancialServiceCondition> FinancialServiceConditions
		{
			get
			{
				if (null == _FinancialServiceConditions) 
				{
					if(null == _DAO.FinancialServiceConditions)
						_DAO.FinancialServiceConditions = new List<FinancialServiceCondition_DAO>();
					_FinancialServiceConditions = new DAOEventList<FinancialServiceCondition_DAO, IFinancialServiceCondition, FinancialServiceCondition>(_DAO.FinancialServiceConditions);
					_FinancialServiceConditions.BeforeAdd += new EventListHandler(OnFinancialServiceConditions_BeforeAdd);					
					_FinancialServiceConditions.BeforeRemove += new EventListHandler(OnFinancialServiceConditions_BeforeRemove);					
					_FinancialServiceConditions.AfterAdd += new EventListHandler(OnFinancialServiceConditions_AfterAdd);					
					_FinancialServiceConditions.AfterRemove += new EventListHandler(OnFinancialServiceConditions_AfterRemove);					
				}
				return _FinancialServiceConditions;
			}
		}
        ///// <summary>
        ///// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceRecurringTransactions
        ///// </summary>
        //private DAOEventList<FinancialServiceRecurringTransaction_DAO, IFinancialServiceRecurringTransaction, FinancialServiceRecurringTransaction> _FinancialServiceRecurringTransactions;
        ///// <summary>
        ///// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceRecurringTransactions
        ///// </summary>
        //public IEventList<IFinancialServiceRecurringTransaction> FinancialServiceRecurringTransactions
        //{
        //    get
        //    {
        //        if (null == _FinancialServiceRecurringTransactions) 
        //        {
        //            if(null == _DAO.FinancialServiceRecurringTransactions)
        //                _DAO.FinancialServiceRecurringTransactions = new List<FinancialServiceRecurringTransaction_DAO>();
        //            _FinancialServiceRecurringTransactions = new DAOEventList<FinancialServiceRecurringTransaction_DAO, IFinancialServiceRecurringTransaction, FinancialServiceRecurringTransaction>(_DAO.FinancialServiceRecurringTransactions);
        //            _FinancialServiceRecurringTransactions.BeforeAdd += new EventListHandler(OnFinancialServiceRecurringTransactions_BeforeAdd);					
        //            _FinancialServiceRecurringTransactions.BeforeRemove += new EventListHandler(OnFinancialServiceRecurringTransactions_BeforeRemove);					
        //            _FinancialServiceRecurringTransactions.AfterAdd += new EventListHandler(OnFinancialServiceRecurringTransactions_AfterAdd);					
        //            _FinancialServiceRecurringTransactions.AfterRemove += new EventListHandler(OnFinancialServiceRecurringTransactions_AfterRemove);					
        //        }
        //        return null; //_FinancialServiceRecurringTransactions;
        //    }
        //}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ManualDebitOrders
        /// </summary>
        private DAOEventList<ManualDebitOrder_DAO, IManualDebitOrder, ManualDebitOrder> _ManualDebitOrders;
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ManualDebitOrders
        /// </summary>
        public IEventList<IManualDebitOrder> ManualDebitOrders
        {
            get 
            {
                if (null == _ManualDebitOrders)
                {
                    if (null == _DAO.ManualDebitOrders)
                        _DAO.ManualDebitOrders = new List<ManualDebitOrder_DAO>();
                    _ManualDebitOrders = new DAOEventList<ManualDebitOrder_DAO, IManualDebitOrder, ManualDebitOrder>(_DAO.ManualDebitOrders);
                }
                return _ManualDebitOrders;
            }
        }


		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialAdjustments
		/// </summary>
		private DAOEventList<FinancialAdjustment_DAO, IFinancialAdjustment, FinancialAdjustment> _FinancialAdjustments;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialAdjustments
		/// </summary>
		public IEventList<IFinancialAdjustment> FinancialAdjustments
		{
			get
			{
				if (null == _FinancialAdjustments) 
				{
					if(null == _DAO.FinancialAdjustments)
						_DAO.FinancialAdjustments = new List<FinancialAdjustment_DAO>();
					_FinancialAdjustments = new DAOEventList<FinancialAdjustment_DAO, IFinancialAdjustment, FinancialAdjustment>(_DAO.FinancialAdjustments);
				}
				return _FinancialAdjustments;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceAttributes
		/// </summary>
		private DAOEventList<FinancialServiceAttribute_DAO, IFinancialServiceAttribute, FinancialServiceAttribute> _FinancialServiceAttributes;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceAttributes
		/// </summary>
		public IEventList<IFinancialServiceAttribute> FinancialServiceAttributes
		{
			get
			{
				if (null == _FinancialServiceAttributes) 
				{
					if(null == _DAO.FinancialServiceAttributes)
						_DAO.FinancialServiceAttributes = new List<FinancialServiceAttribute_DAO>();
					_FinancialServiceAttributes = new DAOEventList<FinancialServiceAttribute_DAO, IFinancialServiceAttribute, FinancialServiceAttribute>(_DAO.FinancialServiceAttributes);
				}
				return _FinancialServiceAttributes;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Account
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
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.AccountStatus
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
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Category
		/// </summary>
		public ICategory Category 
		{
			get
			{
				if (null == _DAO.Category) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICategory, Category_DAO>(_DAO.Category);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Category = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Category = (Category_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialTransactions
		/// </summary>
		private DAOEventList<FinancialTransaction_DAO, IFinancialTransaction, FinancialTransaction> _FinancialTransactions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialTransactions
		/// </summary>
		public IEventList<IFinancialTransaction> FinancialTransactions
		{
			get
			{
				if (null == _FinancialTransactions) 
				{
					if(null == _DAO.FinancialTransactions)
						_DAO.FinancialTransactions = new List<FinancialTransaction_DAO>();
					_FinancialTransactions = new DAOEventList<FinancialTransaction_DAO, IFinancialTransaction, FinancialTransaction>(_DAO.FinancialTransactions);
					_FinancialTransactions.BeforeAdd += new EventListHandler(OnFinancialTransactions_BeforeAdd);					
					_FinancialTransactions.BeforeRemove += new EventListHandler(OnFinancialTransactions_BeforeRemove);					
					_FinancialTransactions.AfterAdd += new EventListHandler(OnFinancialTransactions_AfterAdd);					
					_FinancialTransactions.AfterRemove += new EventListHandler(OnFinancialTransactions_AfterRemove);					
				}
				return _FinancialTransactions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServiceParent
		/// </summary>
		public IFinancialService FinancialServiceParent 
		{
			get
			{
				if (null == _DAO.FinancialServiceParent) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialServiceParent);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceParent = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceParent = (FinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.OpenDate
		/// </summary>
		public DateTime? OpenDate
		{
			get { return _DAO.OpenDate; }
			set { _DAO.OpenDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.CloseDate
		/// </summary>
		public DateTime? CloseDate
		{
			get { return _DAO.CloseDate; }
			set { _DAO.CloseDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServices
		/// </summary>
		private DAOEventList<FinancialService_DAO, IFinancialService, FinancialService> _FinancialServices;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.FinancialServices
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
				}
				return _FinancialServices;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Balance
		/// </summary>
		public IBalance Balance 
		{
			get
			{
				if (null == _DAO.Balance) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBalance, Balance_DAO>(_DAO.Balance);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Balance = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Balance = (Balance_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ArrearTransactions
		/// </summary>
		private DAOEventList<ArrearTransaction_DAO, IArrearTransaction, ArrearTransaction> _ArrearTransactions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.ArrearTransactions
		/// </summary>
		public IEventList<IArrearTransaction> ArrearTransactions
		{
			get
			{
				if (null == _ArrearTransactions) 
				{
					if(null == _DAO.ArrearTransactions)
						_DAO.ArrearTransactions = new List<ArrearTransaction_DAO>();
					_ArrearTransactions = new DAOEventList<ArrearTransaction_DAO, IArrearTransaction, ArrearTransaction>(_DAO.ArrearTransactions);
				}
				return _ArrearTransactions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Fees
		/// </summary>
		private DAOEventList<Fee_DAO, IFee, Fee> _Fees;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.Fees
		/// </summary>
		public IEventList<IFee> Fees
		{
			get
			{
				if (null == _Fees) 
				{
					if(null == _DAO.Fees)
						_DAO.Fees = new List<Fee_DAO>();
					_Fees = new DAOEventList<Fee_DAO, IFee, Fee>(_DAO.Fees);
				}
				return _Fees;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialService_DAO.LifePolicy
		/// </summary>
		public ILifePolicy LifePolicy 
		{
			get
			{
				if (null == _DAO.LifePolicy) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILifePolicy, LifePolicy_DAO>(_DAO.LifePolicy);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifePolicy = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifePolicy = (LifePolicy_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FinancialServiceBankAccounts = null;
			_FinancialServiceConditions = null;
			_ManualDebitOrders = null;
			_FinancialAdjustments = null;
			_FinancialServiceAttributes = null;
			_FinancialTransactions = null;
			_FinancialServices = null;
			_ArrearTransactions = null;
			_Fees = null;
			
		}
    }
}


