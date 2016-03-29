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
	/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO
	/// </summary>
	public partial class Disbursement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Disbursement_DAO>, IDisbursement
	{
				public Disbursement(SAHL.Common.BusinessModel.DAO.Disbursement_DAO Disbursement) : base(Disbursement)
		{
			this._DAO = Disbursement;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.PreparedDate
		/// </summary>
		public DateTime? PreparedDate
		{
			get { return _DAO.PreparedDate; }
			set { _DAO.PreparedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ActionDate
		/// </summary>
		public DateTime? ActionDate
		{
			get { return _DAO.ActionDate; }
			set { _DAO.ActionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountName
		/// </summary>
		public String AccountName 
		{
			get { return _DAO.AccountName; }
			set { _DAO.AccountName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountNumber
		/// </summary>
		public String AccountNumber 
		{
			get { return _DAO.AccountNumber; }
			set { _DAO.AccountNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Amount
		/// </summary>
		public Double? Amount
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.CapitalAmount
		/// </summary>
		public Double? CapitalAmount
		{
			get { return _DAO.CapitalAmount; }
			set { _DAO.CapitalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.GuaranteeAmount
		/// </summary>
		public Double? GuaranteeAmount
		{
			get { return _DAO.GuaranteeAmount; }
			set { _DAO.GuaranteeAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestRate
		/// </summary>
		public Double? InterestRate
		{
			get { return _DAO.InterestRate; }
			set { _DAO.InterestRate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestStartDate
		/// </summary>
		public DateTime? InterestStartDate
		{
			get { return _DAO.InterestStartDate; }
			set { _DAO.InterestStartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.InterestApplied
		/// </summary>
		public Char InterestApplied 
		{
			get { return _DAO.InterestApplied; }
			set { _DAO.InterestApplied = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.PaymentAmount
		/// </summary>
		public Double? PaymentAmount
		{
			get { return _DAO.PaymentAmount; }
			set { _DAO.PaymentAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountDebtSettlements
		/// </summary>
		private DAOEventList<AccountDebtSettlement_DAO, IAccountDebtSettlement, AccountDebtSettlement> _AccountDebtSettlements;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.AccountDebtSettlements
		/// </summary>
		public IEventList<IAccountDebtSettlement> AccountDebtSettlements
		{
			get
			{
				if (null == _AccountDebtSettlements) 
				{
					if(null == _DAO.AccountDebtSettlements)
						_DAO.AccountDebtSettlements = new List<AccountDebtSettlement_DAO>();
					_AccountDebtSettlements = new DAOEventList<AccountDebtSettlement_DAO, IAccountDebtSettlement, AccountDebtSettlement>(_DAO.AccountDebtSettlements);
					_AccountDebtSettlements.BeforeAdd += new EventListHandler(OnAccountDebtSettlements_BeforeAdd);					
					_AccountDebtSettlements.BeforeRemove += new EventListHandler(OnAccountDebtSettlements_BeforeRemove);					
					_AccountDebtSettlements.AfterAdd += new EventListHandler(OnAccountDebtSettlements_AfterAdd);					
					_AccountDebtSettlements.AfterRemove += new EventListHandler(OnAccountDebtSettlements_AfterRemove);					
				}
				return _AccountDebtSettlements;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.LoanTransactions
		/// </summary>
		private DAOEventList<FinancialTransaction_DAO, IFinancialTransaction, FinancialTransaction> _LoanTransactions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.LoanTransactions
		/// </summary>
		public IEventList<IFinancialTransaction> LoanTransactions
		{
			get
			{
				if (null == _LoanTransactions) 
				{
					if(null == _DAO.LoanTransactions)
						_DAO.LoanTransactions = new List<FinancialTransaction_DAO>();
					_LoanTransactions = new DAOEventList<FinancialTransaction_DAO, IFinancialTransaction, FinancialTransaction>(_DAO.LoanTransactions);
					_LoanTransactions.BeforeAdd += new EventListHandler(OnLoanTransactions_BeforeAdd);					
					_LoanTransactions.BeforeRemove += new EventListHandler(OnLoanTransactions_BeforeRemove);					
					_LoanTransactions.AfterAdd += new EventListHandler(OnLoanTransactions_AfterAdd);					
					_LoanTransactions.AfterRemove += new EventListHandler(OnLoanTransactions_AfterRemove);					
				}
				return _LoanTransactions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBBank
		/// </summary>
		public IACBBank ACBBank 
		{
			get
			{
				if (null == _DAO.ACBBank) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IACBBank, ACBBank_DAO>(_DAO.ACBBank);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ACBBank = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ACBBank = (ACBBank_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBBranch
		/// </summary>
		public IACBBranch ACBBranch 
		{
			get
			{
				if (null == _DAO.ACBBranch) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IACBBranch, ACBBranch_DAO>(_DAO.ACBBranch);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ACBBranch = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ACBBranch = (ACBBranch_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.ACBType
		/// </summary>
		public IACBType ACBType 
		{
			get
			{
				if (null == _DAO.ACBType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IACBType, ACBType_DAO>(_DAO.ACBType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ACBType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ACBType = (ACBType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.Account
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
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.DisbursementStatus
		/// </summary>
		public IDisbursementStatus DisbursementStatus 
		{
			get
			{
				if (null == _DAO.DisbursementStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDisbursementStatus, DisbursementStatus_DAO>(_DAO.DisbursementStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DisbursementStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DisbursementStatus = (DisbursementStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Disbursement_DAO.DisbursementTransactionType
		/// </summary>
		public IDisbursementTransactionType DisbursementTransactionType 
		{
			get
			{
				if (null == _DAO.DisbursementTransactionType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDisbursementTransactionType, DisbursementTransactionType_DAO>(_DAO.DisbursementTransactionType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DisbursementTransactionType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DisbursementTransactionType = (DisbursementTransactionType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_AccountDebtSettlements = null;
			_LoanTransactions = null;
			
		}
	}
}


