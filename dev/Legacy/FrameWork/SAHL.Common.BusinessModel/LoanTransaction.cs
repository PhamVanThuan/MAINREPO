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
	/// This class interacts with the LoanTransaction view in 2AM that points to the table of the same name in SAHLDB.
		/// 
		/// NB: This object should NOT be queried with HQL, it will fall over spectacularly
		/// The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
	/// </summary>
	public partial class LoanTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO>, ILoanTransaction
	{
				public LoanTransaction(SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO LoanTransaction) : base(LoanTransaction)
		{
			this._DAO = LoanTransaction;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.TransactionType
		/// </summary>
		public ITransactionType TransactionType 
		{
			get
			{
				if (null == _DAO.TransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITransactionType, TransactionType_DAO>(_DAO.TransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TransactionType = (TransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.FinancialService
		/// </summary>
		public IFinancialService FinancialService 
		{
			get
			{
				if (null == _DAO.FinancialService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionInsertDate
		/// </summary>
		public DateTime LoanTransactionInsertDate 
		{
			get { return _DAO.LoanTransactionInsertDate; }
			set { _DAO.LoanTransactionInsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionEffectiveDate
		/// </summary>
		public DateTime LoanTransactionEffectiveDate 
		{
			get { return _DAO.LoanTransactionEffectiveDate; }
			set { _DAO.LoanTransactionEffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionRate
		/// </summary>
		public Single LoanTransactionRate 
		{
			get { return _DAO.LoanTransactionRate; }
			set { _DAO.LoanTransactionRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionAmount
		/// </summary>
		public Double LoanTransactionAmount 
		{
			get { return _DAO.LoanTransactionAmount; }
			set { _DAO.LoanTransactionAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionNewBalance
		/// </summary>
		public Double LoanTransactionNewBalance 
		{
			get { return _DAO.LoanTransactionNewBalance; }
			set { _DAO.LoanTransactionNewBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionReference
		/// </summary>
		public String LoanTransactionReference 
		{
			get { return _DAO.LoanTransactionReference; }
			set { _DAO.LoanTransactionReference = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionUserID
		/// </summary>
		public String LoanTransactionUserID 
		{
			get { return _DAO.LoanTransactionUserID; }
			set { _DAO.LoanTransactionUserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.SPVNumber
		/// </summary>
		public Decimal SPVNumber 
		{
			get { return _DAO.SPVNumber; }
			set { _DAO.SPVNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionActualEffectiveDate
		/// </summary>
		public DateTime? LoanTransactionActualEffectiveDate
		{
			get { return _DAO.LoanTransactionActualEffectiveDate; }
			set { _DAO.LoanTransactionActualEffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.RolledBackInd
		/// </summary>
		public String RolledBackInd 
		{
			get { return _DAO.RolledBackInd; }
			set { _DAO.RolledBackInd = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanAccountCurrentBalance
		/// </summary>
		public Double LoanAccountCurrentBalance 
		{
			get { return _DAO.LoanAccountCurrentBalance; }
			set { _DAO.LoanAccountCurrentBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.Adjustments
		/// </summary>
		public Double Adjustments 
		{
			get { return _DAO.Adjustments; }
			set { _DAO.Adjustments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.StandardRate
		/// </summary>
		public Single StandardRate 
		{
			get { return _DAO.StandardRate; }
			set { _DAO.StandardRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.CoPayment
		/// </summary>
		public Double CoPayment 
		{
			get { return _DAO.CoPayment; }
			set { _DAO.CoPayment = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionActiveMarketRate
		/// </summary>
		public Double LoanTransactionActiveMarketRate 
		{
			get { return _DAO.LoanTransactionActiveMarketRate; }
			set { _DAO.LoanTransactionActiveMarketRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionNumber
		/// </summary>
		public Decimal LoanTransactionNumber 
		{
			get { return _DAO.LoanTransactionNumber; }
			set { _DAO.LoanTransactionNumber = value;}
		}
	}
}


