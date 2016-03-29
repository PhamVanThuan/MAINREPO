using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using Castle.ActiveRecord;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
	/// <summary>
	/// 
	/// </summary>
	public class LoanSummaryBase : SAHLCommonBasePresenter<ILoanSummary>
	{
		/// <summary>
		/// 
		/// </summary>
		protected IAccountRepository _accRepository;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public LoanSummaryBase(ILoanSummary view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		/// <summary>
		/// Hook the events fired by the view and call relevant methods to bind control data
		/// </summary>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;

			_accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mortgageLoanAccount"></param>
		/// <param name="financialService"></param>
		/// <param name="currenttoDate"></param>
		/// <param name="totalforMonth"></param>
		/// <param name="previousMonth"></param>
		private void GetAccruedInterestValues(IMortgageLoanAccount mortgageLoanAccount, IFinancialService financialService, out double currenttoDate, out double totalforMonth, out double previousMonth)
		{
			currenttoDate = 0;
			totalforMonth = 0;
			previousMonth = 0;

			try
			{
				mortgageLoanAccount.CalculateInterest(financialService.Key, out currenttoDate, out totalforMonth);
			}
			catch
			{

			}

			List<int> tranTypes = new List<int>();
			tranTypes.Add((int)SAHL.Common.Globals.TransactionTypes.MonthlyInterestDebit); // 210
			tranTypes.Add((int)SAHL.Common.Globals.TransactionTypes.MonthlyInterestDebitCorrection); // 1210
			DataTable dtLoanTransactions = financialService.GetTransactions(_view.Messages, tranTypes);
			double previousMonthInterest = 0;
			for (int i = 0; i < dtLoanTransactions.Rows.Count; i++)
			{
				DataRow transactionRow = dtLoanTransactions.Rows[i];
				if (Convert.ToDateTime(transactionRow["LoanTransactionEffectiveDate"]) == (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)))
					previousMonthInterest += Convert.ToDouble(transactionRow["LoanTransactionAmount"]);
			}
			previousMonth = previousMonthInterest;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="account"></param>
		private void BindData(IAccount account)
		{
			double householdIncome = 0;
			double currentBalance = 0;
			double payment = 0;
			double latestValuation = 0;

			double gridTotalInstallment = 0;
			double gridTotalArrearBalance = 0;
			double gridTotalCurrentBalance = 0;

			double gridShortTermTotalInstallment = 0;
			double gridShortTermTotalArrearBalance = 0;
			double gridShortTermTotalCurrentBalance = 0;

			IList<IMortgageLoan> lstMortgageLoans = new List<IMortgageLoan>();
			IList<IMortgageLoan> lstShortTermLoans = new List<IMortgageLoan>();

			//do variable stuff first
			IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
			if (mortgageLoanAccount != null)
			{
				IMortgageLoan mortgageLoan = mortgageLoanAccount.SecuredMortgageLoan;
				if (mortgageLoan != null)
				{
					//add to the list we will use to bind the loan grid
					lstMortgageLoans.Add(mortgageLoanAccount.SecuredMortgageLoan);

					//calculate the accrued interest portions
					double currenttoDate;
					double totalforMonth;
					double previousMonth;
					IFinancialService financialService = mortgageLoan as IFinancialService;
					if (financialService != null)
					{
						GetAccruedInterestValues(mortgageLoanAccount, financialService, out currenttoDate, out totalforMonth, out previousMonth);
						_view.InterestCurrenttoDateVariable = currenttoDate;
						_view.InterestTotalforMonthVariable = totalforMonth;
						_view.InterestPreviousMonthVariable = previousMonth;

						payment += financialService.Payment;

						if (financialService.FinancialServiceBankAccounts != null &&
							financialService.FinancialServiceBankAccounts.Count > 0)
						{
							_view.DebitOrderDay = financialService.FinancialServiceBankAccounts[0].DebitOrderDay;

                            _view.NaedoCompliant = financialService.CurrentBankAccount.IsNaedoCompliant;
						}
					}

					latestValuation = mortgageLoan.GetActiveValuationAmount();
					DateTime? valuationDate = mortgageLoan.GetActiveValuationDate();
					if (valuationDate.HasValue)
						_view.ValuationDate = valuationDate.Value;

					_view.LoanAgreementAmount = mortgageLoan.SumBondLoanAgreementAmounts();
					_view.TotalBondAmount = mortgageLoan.SumBondRegistrationAmounts();
					_view.CommittedLoanValue = mortgageLoan.Account.CommittedLoanValue;


					if (mortgageLoan.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Open) ||
						mortgageLoan.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Dormant))
					{
						gridTotalInstallment += mortgageLoan.Payment;
						gridTotalArrearBalance += mortgageLoan.ArrearBalance;
						gridTotalCurrentBalance += mortgageLoan.CurrentBalance;

						currentBalance += mortgageLoan.CurrentBalance;
					}

					_view.MaturityDateTitleText = "Maturity Date";

					var edgeAdjustment = mortgageLoan.FinancialAdjustments.FirstOrDefault(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.Edge &&
															 x.FinancialAdjustmentType.Key == (int)FinancialAdjustmentTypes.InterestOnly &&
															 x.FinancialAdjustmentStatus.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentStatuses.Active));

					var interestOnlyAdjustment = mortgageLoan.FinancialAdjustments.FirstOrDefault(x => x.FinancialAdjustmentSource.Key == (int)FinancialAdjustmentSources.InterestOnly &&
															 x.FinancialAdjustmentType.Key == (int)FinancialAdjustmentTypes.InterestOnly &&
															 x.FinancialAdjustmentStatus.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentStatuses.Active));
					if (edgeAdjustment != null)
					{
						_view.MaturityDateTitleText = "Expiry Date";
						if (edgeAdjustment.EndDate != null)
							_view.MaturityDate = Convert.ToDateTime(edgeAdjustment.EndDate);
					}
					if (interestOnlyAdjustment != null)
					{
						_view.MaturityDate = Convert.ToDateTime(mortgageLoan.InterestOnly.MaturityDate);
					}

					// check for non performing loan
					bool nonPerforming = false;
					foreach (IFinancialAdjustment financialAdjustment in mortgageLoan.FinancialAdjustments)
					{
						//todo change to use enumertaor value once its in the db
						if (financialAdjustment.FinancialAdjustmentTypeSource != null)
							if (financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.NonPerforming
								&& financialAdjustment.FinancialAdjustmentStatus.Key == (int)SAHL.Common.Globals.FinancialAdjustmentStatuses.Active)
							{
								nonPerforming = true;
								break;
							}
					}

					// Do an additional check to find if there is a pending FinancialAdjustment against the ML
					if (!nonPerforming)
					{
						nonPerforming = mortgageLoan.GetPendingFinancialAdjustmentByTypeSource(FinancialAdjustmentTypeSources.NonPerforming) == null ? false : true;
					}
					_view.NonPerformingLoan = nonPerforming;

					_view.TitleDeedOnFile = _accRepository.TitleDeedsOnFile(account.Key);

                    _view.ManualLifePolicyPaymentVisible = mortgageLoanAccount.Details.Any(x => x.DetailType.Key == (int)DetailTypes.ManualLifePolicyPayment);

                    _view.IsGEPFFunded = account.HasAccountInformationType(AccountInformationTypes.GovernmentEmployeePensionFund);
				}

				_view.SpvDescription = mortgageLoanAccount.SPV.Description;

				if (mortgageLoanAccount.UnsecuredMortgageLoans != null)
				{
					for (int shortTermIndex = 0; shortTermIndex < mortgageLoanAccount.UnsecuredMortgageLoans.Count; shortTermIndex++)
					{
						lstShortTermLoans.Add(mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex]);
						if (mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex].AccountStatus.Key == Convert.ToInt32(AccountStatuses.Open) ||
							mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex].AccountStatus.Key == Convert.ToInt32(AccountStatuses.Dormant))
						{
							gridShortTermTotalInstallment += mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex].Payment;
							gridShortTermTotalArrearBalance += mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex].ArrearBalance;
							gridShortTermTotalCurrentBalance += mortgageLoanAccount.UnsecuredMortgageLoans[shortTermIndex].CurrentBalance;
						}
					}
				}
			}

			_view.FixedLoanControlsVisible = false;
			IAccountVariFixLoan varifixLoanAccount = account as IAccountVariFixLoan;
			if (varifixLoanAccount != null)
			{
				IMortgageLoan fixedmortgageLoan = varifixLoanAccount.FixedSecuredMortgageLoan;
				if (fixedmortgageLoan != null)
				{
					lstMortgageLoans.Add(fixedmortgageLoan);

					double currenttoDate;
					double totalforMonth;
					double previousMonth;

					//calculate the accrued interest portions
					IFinancialService financialService = fixedmortgageLoan as IFinancialService;
					if (financialService != null)
					{
						GetAccruedInterestValues(mortgageLoanAccount, financialService, out currenttoDate, out totalforMonth, out previousMonth);
						_view.InterestCurrenttoDateFixed = currenttoDate;
						_view.InterestTotalforMonthFixed = totalforMonth;
						_view.InterestPreviousMonthFixed = previousMonth;

						payment += financialService.Payment;
					}
					_view.FixedLoanControlsVisible = true;

					if (fixedmortgageLoan.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Open) ||
						fixedmortgageLoan.AccountStatus.Key == Convert.ToInt32(AccountStatuses.Dormant))
					{
						gridTotalInstallment += fixedmortgageLoan.Payment;
						gridTotalArrearBalance += fixedmortgageLoan.ArrearBalance;
						gridTotalCurrentBalance += fixedmortgageLoan.CurrentBalance;

						currentBalance += fixedmortgageLoan.CurrentBalance;
					}
				}
			}
			householdIncome = account.GetHouseholdIncome();
			_view.HouseholdIncome = householdIncome;
			_view.LatestProperyValuationAmount = latestValuation;
			if (householdIncome != 0)
				_view.PTI = account.CalcAccountPTI;
			if (latestValuation != 0)
				_view.LTV = currentBalance / latestValuation;

			_view.BindTotalData();
			_view.BindMorgageLoanData();
			_view.BindLoansGrid(lstMortgageLoans, gridTotalInstallment, gridTotalArrearBalance, gridTotalCurrentBalance);
			_view.BindShortTermLoansGrid(lstShortTermLoans, gridShortTermTotalInstallment, gridShortTermTotalArrearBalance, gridShortTermTotalCurrentBalance);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="account"></param>
		protected void BindAccountToView(IAccount account)
		{
			_view.BindAccountLoanSummaryData(account);

			//set all the mortgage loan data
			BindData(account);
		}


	}
}
