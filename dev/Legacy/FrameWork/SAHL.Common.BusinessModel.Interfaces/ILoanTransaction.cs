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
	/// This class interacts with the LoanTransaction view in 2AM that points to the table of the same name in SAHLDB.
		/// 
		/// NB: This object should NOT be queried with HQL, it will fall over spectacularly
		/// The FinancialService is built up from the LoanNumber (NotNull = true), this is only valid for Transactions after Nov 2006.
	/// </summary>
	public partial interface ILoanTransaction : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.TransactionType
		/// </summary>
		ITransactionType TransactionType
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.FinancialService
		/// </summary>
		IFinancialService FinancialService
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionInsertDate
		/// </summary>
		System.DateTime LoanTransactionInsertDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionEffectiveDate
		/// </summary>
		System.DateTime LoanTransactionEffectiveDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionRate
		/// </summary>
		System.Single LoanTransactionRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionAmount
		/// </summary>
		System.Double LoanTransactionAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionNewBalance
		/// </summary>
		System.Double LoanTransactionNewBalance
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionReference
		/// </summary>
		System.String LoanTransactionReference
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionUserID
		/// </summary>
		System.String LoanTransactionUserID
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.SPVNumber
		/// </summary>
		System.Decimal SPVNumber
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionActualEffectiveDate
		/// </summary>
		DateTime? LoanTransactionActualEffectiveDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.RolledBackInd
		/// </summary>
		System.String RolledBackInd
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanAccountCurrentBalance
		/// </summary>
		System.Double LoanAccountCurrentBalance
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.Adjustments
		/// </summary>
		System.Double Adjustments
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.StandardRate
		/// </summary>
		System.Single StandardRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.CoPayment
		/// </summary>
		System.Double CoPayment
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionActiveMarketRate
		/// </summary>
		System.Double LoanTransactionActiveMarketRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LoanTransaction_DAO.LoanTransactionNumber
		/// </summary>
		System.Decimal LoanTransactionNumber
		{
			get;
			set;
		}
	}
}


