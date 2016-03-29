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
	/// This class interacts with the FinancialTransaction view in 2AM that points to the table of the same name in SAHLDB.
		/// 
		/// NB: This object should NOT be queried with HQL, it will fall over spectacularly
		/// The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
	/// </summary>
	public partial class FinancialTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO>, IFinancialTransaction
	{
				public FinancialTransaction(SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO FinancialTransaction) : base(FinancialTransaction)
		{
			this._DAO = FinancialTransaction;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.TransactionType
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
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.FinancialService
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
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.EffectiveDate
		/// </summary>
		public DateTime EffectiveDate 
		{
			get { return _DAO.EffectiveDate; }
			set { _DAO.EffectiveDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.CorrectionDate
		/// </summary>
		public DateTime CorrectionDate 
		{
			get { return _DAO.CorrectionDate; }
			set { _DAO.CorrectionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.InterestRate
		/// </summary>
		public Single? InterestRate
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.ActiveMarketRate
		/// </summary>
		public Double? ActiveMarketRate
		{
			get { return _DAO.ActiveMarketRate; }
			set { _DAO.ActiveMarketRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Balance
		/// </summary>
		public Double Balance 
		{
			get { return _DAO.Balance; }
			set { _DAO.Balance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.Reference
		/// </summary>
		public String Reference 
		{
			get { return _DAO.Reference; }
			set { _DAO.Reference = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.SPV
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialTransaction_DAO.IsRolledBack
		/// </summary>
		public Boolean IsRolledBack 
		{
			get { return _DAO.IsRolledBack; }
			set { _DAO.IsRolledBack = value;}
		}
	}
}


