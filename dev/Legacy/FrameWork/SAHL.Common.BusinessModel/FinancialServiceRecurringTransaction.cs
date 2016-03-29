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

	/*public partial class FinancialServiceRecurringTransaction : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO>, IFinancialServiceRecurringTransaction
	{
				public FinancialServiceRecurringTransaction(SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO FinancialServiceRecurringTransaction) : base(FinancialServiceRecurringTransaction)
		{
			this._DAO = FinancialServiceRecurringTransaction;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.InsertDate
		/// </summary>
		public DateTime? InsertDate
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Frequency
		/// </summary>
		public Int32? Frequency
		{
			get { return _DAO.Frequency; }
			set { _DAO.Frequency = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.TransactionType
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
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.FrequencyType
		/// </summary>
		public Char FrequencyType 
		{
			get { return _DAO.FrequencyType; }
			set { _DAO.FrequencyType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.NumUntilNextRun
		/// </summary>
		public Int32? NumUntilNextRun
		{
			get { return _DAO.NumUntilNextRun; }
			set { _DAO.NumUntilNextRun = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Reference
		/// </summary>
		public String Reference 
		{
			get { return _DAO.Reference; }
			set { _DAO.Reference = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Active
		/// </summary>
		public Boolean Active 
		{
			get { return _DAO.Active; }
			set { _DAO.Active = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.StartDate
		/// </summary>
		public DateTime? StartDate
		{
			get { return _DAO.StartDate; }
			set { _DAO.StartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Term
		/// </summary>
		public Int32? Term
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.RemainingTerm
		/// </summary>
		public Int32? RemainingTerm
		{
			get { return _DAO.RemainingTerm; }
			set { _DAO.RemainingTerm = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.TransactionDay
		/// </summary>
		public Int32? TransactionDay
		{
			get { return _DAO.TransactionDay; }
			set { _DAO.TransactionDay = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.HourOfRun
		/// </summary>
		public Int32? HourOfRun
		{
			get { return _DAO.HourOfRun; }
			set { _DAO.HourOfRun = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Amount
		/// </summary>
		public Double? Amount
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.PreviousRunDate
		/// </summary>
		public DateTime? PreviousRunDate
		{
			get { return _DAO.PreviousRunDate; }
			set { _DAO.PreviousRunDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.UserName
		/// </summary>
		public String UserName 
		{
			get { return _DAO.UserName; }
			set { _DAO.UserName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Notes
		/// </summary>
		public String Notes 
		{
			get { return _DAO.Notes; }
			set { _DAO.Notes = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.BankAccount
		/// </summary>
		public IBankAccount BankAccount 
		{
			get
			{
				if (null == _DAO.BankAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBankAccount, BankAccount_DAO>(_DAO.BankAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.BankAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.BankAccount = (BankAccount_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.FinancialService
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
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceRecurringTransaction_DAO.RecurringTransactionType
		/// </summary>
		public IRecurringTransactionType RecurringTransactionType 
		{
			get
			{
				if (null == _DAO.RecurringTransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IRecurringTransactionType, RecurringTransactionType_DAO>(_DAO.RecurringTransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RecurringTransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RecurringTransactionType = (RecurringTransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}*/
}


