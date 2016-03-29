using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DataSets;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using NHibernate;

namespace SAHL.Common.BusinessModel.Repositories
{
	[FactoryType(typeof(IDebtCounsellingRepository))]
	public class DebtCounsellingRepository : AbstractRepositoryBase, IDebtCounsellingRepository
	{
		private ICastleTransactionsService castleTransactionService;
        private IAccountRepository accountRepository;

		public DebtCounsellingRepository()
		{
			if (castleTransactionService == null)
			{
				castleTransactionService = new CastleTransactionsService();
			}

            if (accountRepository == null)
            {
                accountRepository = new AccountRepository();
            }
		}

        public DebtCounsellingRepository(ICastleTransactionsService castleTransactionsService, IAccountRepository accountRepository)
		{
			this.castleTransactionService = castleTransactionsService;
            this.accountRepository = accountRepository;
		}

		public IDebtCounselling GetDebtCounsellingByKey(int key)
		{
			return base.GetByKey<IDebtCounselling, DebtCounselling_DAO>(key);
		}

		public List<IDebtCounselling> GetDebtCounsellingByAccountKey(int accountKey)
		{
			List<IDebtCounselling> debtCounsellingList = new List<IDebtCounselling>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetDebtCounsellingByAccountKey");

			SimpleQuery<DebtCounselling_DAO> p = new SimpleQuery<DebtCounselling_DAO>(QueryLanguage.Sql, query, accountKey);
			p.AddSqlReturnDefinition(typeof(DebtCounselling_DAO), "dc");

			DebtCounselling_DAO[] res = p.Execute();

			if (res == null || res.Length == 0)
				return debtCounsellingList;

			foreach (DebtCounselling_DAO pr in res)
			{
				debtCounsellingList.Add(BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(pr));
			}

			return debtCounsellingList;
		}

        public int GetExternalRoleTypeKeyForDebtCounsellingKeyAndLegalEntityKey(int debtCounsellingKey, int legalEntityKey)
        {
            string HQL = "select d from ExternalRole_DAO d where d.GenericKey=? and d.LegalEntity.Key=? and d.GenericKeyType.Key=27 and d.GeneralStatus.Key=1";

            SimpleQuery<ExternalRole_DAO> q = new SimpleQuery<ExternalRole_DAO>(HQL, debtCounsellingKey, legalEntityKey);
            q.SetQueryRange(1); //although there should never be more than 1 anyway...
            ExternalRole_DAO[] res = q.Execute();
            if (res.Length == 0)
                return 0;
            return res[0].ExternalRoleType.Key;
        }

		public List<IDebtCounselling> GetDebtCounsellingByAccountKey(int accountKey, DebtCounsellingStatuses debtCounsellingStatus)
		{
			List<IDebtCounselling> debtCounsellingList = new List<IDebtCounselling>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetDebtCounsellingByAccountKey");

			SimpleQuery<DebtCounselling_DAO> p = new SimpleQuery<DebtCounselling_DAO>(QueryLanguage.Sql, query, accountKey);
			p.AddSqlReturnDefinition(typeof(DebtCounselling_DAO), "dc");

			DebtCounselling_DAO[] res = p.Execute();

			if (res == null || res.Length == 0)
				return debtCounsellingList;

			foreach (DebtCounselling_DAO pr in res)
			{
				debtCounsellingList.Add(BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(pr));
			}

			List<IDebtCounselling> debtCounsellingListActive = new List<IDebtCounselling>();
			foreach (var dc in debtCounsellingList)
			{
				if (dc.DebtCounsellingStatus.Key == (int)debtCounsellingStatus)
				{
					debtCounsellingListActive.Add(dc);
				}
			}

			return debtCounsellingListActive;
		}

		public IDebtCounselling CreateEmptyDebtCounselling()
		{
			return base.CreateEmpty<IDebtCounselling, DebtCounselling_DAO>();
		}

		public IDebtCounsellingGroup CreateEmptyDebtCounsellingGroup()
		{
			return base.CreateEmpty<IDebtCounsellingGroup, DebtCounsellingGroup_DAO>();
		}

		public void SaveDebtCounsellingGroup(IDebtCounsellingGroup debtCounsellingGroup)
		{
			base.Save<IDebtCounsellingGroup, DebtCounsellingGroup_DAO>(debtCounsellingGroup);
		}

		public IDebtCounsellingGroup GetDebtCounsellingGroupByKey(int debtCounsellingGroupKey)
		{
			return base.GetByKey<IDebtCounsellingGroup, DebtCounsellingGroup_DAO>(debtCounsellingGroupKey);
		}

		public void SaveDebtCounselling(IDebtCounselling debtCounselling)
		{
			base.Save<IDebtCounselling, DebtCounselling_DAO>(debtCounselling);
		}

		public void CreateAccountSnapShot(int debtCounsellingKey)
		{
			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "CreateAccountSnapShot");

			var parameters = new ParameterCollection
								 {
									 new SqlParameter("@DebtCounsellingKey", debtCounsellingKey),
								 };

			// Execute Query
			castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
		}

        public void GetPostDebtCounsellingMortgageLoanInstallment(int debtCounsellingKey, out double preDCInstalment, out double linkRate, out double marketRate, out double interestRate, out int term)
		{
			ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@DebtCounsellingKey", debtCounsellingKey));

			var preDCInstalmentParam = new SqlParameter("@PreDCInstalment", SqlDbType.Decimal);
			preDCInstalmentParam.Direction = ParameterDirection.Output;
            preDCInstalmentParam.Precision = 22;
            preDCInstalmentParam.Scale = 10;
			prms.Add(preDCInstalmentParam);

            var linkRateParam = new SqlParameter("@LinkRate", SqlDbType.Decimal);
            linkRateParam.Direction = ParameterDirection.Output;
            linkRateParam.Precision = 22;
            linkRateParam.Scale = 10;
            prms.Add(linkRateParam);

            var marketRateParam = new SqlParameter("@MarketRate", SqlDbType.Decimal);
            marketRateParam.Direction = ParameterDirection.Output;
            marketRateParam.Precision = 22;
            marketRateParam.Scale = 10;
            prms.Add(marketRateParam);

            var interestRateParam = new SqlParameter("@InterestRate", SqlDbType.Decimal);
            interestRateParam.Direction = ParameterDirection.Output;
            interestRateParam.Precision = 22;
            interestRateParam.Scale = 10;
            prms.Add(interestRateParam);

            var termParam = new SqlParameter("@Term", SqlDbType.Int);
            termParam.Direction = ParameterDirection.Output;
            prms.Add(termParam);

			this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForRead("Repositories.DebtCounsellingRepository", "GetPostDebtCounsellingMortgageLoanInstallment", prms);

            preDCInstalment = Convert.ToDouble(preDCInstalmentParam.Value == System.DBNull.Value ? 0D : preDCInstalmentParam.Value);
            linkRate = Convert.ToDouble(linkRateParam.Value == System.DBNull.Value ? 0D : linkRateParam.Value);
            marketRate = Convert.ToDouble(marketRateParam.Value == System.DBNull.Value ? 0D : marketRateParam.Value);
            interestRate = Convert.ToDouble(interestRateParam.Value == System.DBNull.Value ? 0D : interestRateParam.Value);
            term = Convert.ToInt32(termParam.Value == System.DBNull.Value ? 0D : termParam.Value);
		}

		public ISnapShotAccount GetDebtCounsellingSnapShot(int debtCounsellingKey)
		{
			var snapShotAccount = SnapShotAccount_DAO.FindAllByProperty("DebtCounselling.Key", debtCounsellingKey).FirstOrDefault();
			IBusinessModelTypeMapper typeMapper = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
			return typeMapper.GetMappedType<ISnapShotAccount, SnapShotAccount_DAO>(snapShotAccount);
		}

		public void CancelDebtCounselling(IDebtCounselling debtCounselling, string userID, DebtCounsellingStatuses debtCounsellingStatus)
		{
			// Add the required parameters
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@AccountKey", debtCounselling.Account.Key));
			parameters.Add(new SqlParameter("@UserID", userID));
			var hasAcceptedProposal = debtCounselling.Proposals.Any(p => p.Accepted ?? false);
			if (hasAcceptedProposal)
			{
				this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DebtCounsellingRepository", "pDebtCounsellingOptOut", parameters);
			}

			// update the debtcounselling status
			debtCounselling.DebtCounsellingStatus = LookupRepo.DebtCounsellingStatuses[debtCounsellingStatus];
			this.SaveDebtCounselling(debtCounselling);
		}

		public void ConvertDebtCounselling(int accountKey, string userID)
		{
			// Add the required parameters
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@AccountKey", accountKey));
			parameters.Add(new SqlParameter("@UserID", userID));

			this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DebtCounsellingRepository", "pConvertDebtCounselling", parameters);
		}

		public void ProcessDebtCounsellingOptOut(int accountKey, string user)
		{
			this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DebtCounsellingRepository", "pProcessDebtCounsellingOptOut", new ParameterCollection{
				new SqlParameter("@AccountKey", accountKey),
				new SqlParameter("@userID", user)
			});
		}

		public bool UpdateDebtCounsellingDebtReviewArrangement(int accountKey, string user)
		{
			// Add the required parameters
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@AccountKey", accountKey));
			parameters.Add(new SqlParameter("@UserId", user));

			// Execute Halo API
			this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DebtCounsellingRepository", "PaymentReviewUpdate", parameters);

			return true;
		}

		public bool RollbackTransaction(int debtCounsellingKey)
		{
			// Add the required parameters
			ParameterCollection parameters = new ParameterCollection();
			parameters.Add(new SqlParameter("@debtCounsellingKey", debtCounsellingKey));

			// Execute Halo API
			this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.DebtCounsellingRepository", "RollBackTransaction", parameters);

			return true;
		}

		public IList<IDebtCounsellingGroup> GetRelatedDebtCounsellingGroupForLegalEntities(List<int> keys)
		{
			string delimitedKeys = String.Join(",", keys.Select((key) => key.ToString()).ToArray());

			//Get the UI Statement
			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetRelatedDebtCounsellingGroupForLegalEntities");
			query = String.Format(query, delimitedKeys);

			SimpleQuery<DebtCounsellingGroup_DAO> c = new SimpleQuery<DebtCounsellingGroup_DAO>(QueryLanguage.Sql, query);
			c.AddSqlReturnDefinition(typeof(DebtCounsellingGroup_DAO), "gd");

			DebtCounsellingGroup_DAO[] result = c.Execute();

			IList<IDebtCounsellingGroup> debtCounsellingGroups = null;
			if (result != null && result.Length > 0)
			{
				IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
				debtCounsellingGroups = new List<IDebtCounsellingGroup>();

				foreach (var dao in result)
				{
					IDebtCounsellingGroup debtCounsellingGroup = BMTM.GetMappedType<IDebtCounsellingGroup, DebtCounsellingGroup_DAO>(dao);
					debtCounsellingGroups.Add(debtCounsellingGroup);
				}
			}
			return debtCounsellingGroups;
		}

		public int GetRemainingTermPriorToProposalAcceptance(int debtCounsellingKey)
		{
			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetRemainingTermPriorToProposalAcceptance");
			ParameterCollection prms = new ParameterCollection();
			prms.Add(new SqlParameter("@DebtCounsellingKey", debtCounsellingKey));

			object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

			if (obj != DBNull.Value && obj != null)
				return Convert.ToInt32(obj);
			else
				return -1;
		}

		#region Graphs

		//public LoanCalculations.AmortisationScheduleDataTable AgregateAmortisingPointsByInterval(LoanCalculations.AmortisationScheduleDataTable AmortisingTable, Intervals Interval)
		//{
		//    LoanCalculations.AmortisationScheduleDataTable dt = new LoanCalculations.AmortisationScheduleDataTable();
		//    LoanCalculations.AmortisationScheduleRow row = dt.NewAmortisationScheduleRow();

		//    int seed = 0;
		//    int period = 1;
		//    int i = 0;

		//    while (i < AmortisingTable.Rows.Count)
		//    {
		//        i += 1;
		//        seed += 1;
		//        row.OpenBalance = AmortisingTable[i].OpenBalance;
		//        row.CloseBalance = AmortisingTable[i].CloseBalance;
		//        row.Capital += AmortisingTable[i].Capital;
		//        row.Interest += AmortisingTable[i].Interest;
		//        row.Instalment += AmortisingTable[i].Instalment;

		//        if (seed == (int)Interval || i >= AmortisingTable.Rows.Count)
		//        {
		//            row.Period = period;
		//            dt.AddAmortisationScheduleRow(row);
		//            period += 1;
		//            seed = 0;
		//            row = dt.NewAmortisationScheduleRow();
		//        }
		//    }
		//    return dt;
		//}

		//public LoanCalculations.AmortisationScheduleDataTable GetAmortisationSchedule(double LoanBalance, double Instalment, double InterestRate)
		//{
		//    return GenerateAmortisingScheduleInMonths(LoanBalance, Instalment, InterestRate, 1);
		//}

		/// <summary>
		/// The method does base calculations before calculating the amortising points.
		/// Gets the Loan Current Balance at a point in time.
		/// Calculates the Months Elapsed to determine the starting point.
		/// Calculates the Remaining Term.
		/// Calculates the Instalment.
		/// </summary>
		/// <param name="ProposalKey"></param>
		/// <returns></returns>
		public LoanCalculations.AmortisationScheduleDataTable GetAmortisationScheduleForAccountByProposalKey(int ProposalKey)
		{
			IProposal proposal = GetProposalByKey(ProposalKey);
			DateTime proposalStartDT = DateTime.Now;
			LoanCalculations.AmortisationScheduleDataTable dt = null;
			IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
			IAccount account = accRepo.GetAccountByKey(proposal.DebtCounselling.Account.Key);
			IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;

			double currentBalance = 0D;
			double interestRate = 0D;
			double installment = 0D;
			int remainingTerm = 0;
			int monthsElapsed = 0;
			bool interestOnly = false;
			DateTime? maturityDate = null;
			DateTime endDate = DateTime.MinValue;

			IEventList<IProposalItem> propItems = SortProposalItems(proposal);

			if (propItems.Count > 0 && mortgageLoanAccount != null)
			{
				//need something to check for snapshot data, and use that if it exists
				string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetSnapShotDataForGraph");
				ParameterCollection parameters = new ParameterCollection { new SqlParameter("@DebtCounsellingKey", proposal.DebtCounselling.Key) };

				DataSet dsSS = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

				if (dsSS != null && dsSS.Tables.Count > 0 && dsSS.Tables[0].Rows.Count > 0)
				{
					DataRow dr = dsSS.Tables[0].Rows[0];
					proposalStartDT = Convert.ToDateTime(dr["SnapShotDate"]);
					monthsElapsed = proposalStartDT.MonthDifference(account.OpenDate.Value, 1);
					remainingTerm = Convert.ToInt16(dr["RemainingInstallments"]);
					interestRate = Convert.ToDouble(dr["EffectiveRate"]);
					interestOnly = Convert.ToBoolean(dr["IsInterestOnly"]);
					var possibleMaturityDate = dr["MaturityDate"].ToString();

					DateTime temp;
					if (possibleMaturityDate != null && DateTime.TryParse(possibleMaturityDate, out temp))
					{
						maturityDate = temp;
					}
				}
				else
				{
					proposalStartDT = propItems[0].StartDate;

					// Calculate Months Elapsed since the Account was open based on the Proposal Start Date
					monthsElapsed = proposalStartDT.MonthDifference(account.OpenDate.Value, 1);

					// Calculate the remaining Term based on the Proposal Start Date
					// No. of installments since Proposal Start Date till now
					remainingTerm = DateTime.Now.MonthDifference(proposalStartDT, 1);

					// The current remaining installments
					remainingTerm += mortgageLoanAccount.SecuredMortgageLoan.RemainingInstallments;

					interestRate = mortgageLoanAccount.SecuredMortgageLoan.InterestRate;

					var hasInterestOnlyFinancialAdjustment = mortgageLoanAccount.SecuredMortgageLoan.FinancialAdjustments.Where(x => x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.InterestOnly &&
																							  x.Term == -1).FirstOrDefault() != null;

                    if (mortgageLoanAccount.SecuredMortgageLoan.HasInterestOnly()
                        && mortgageLoanAccount.SecuredMortgageLoan.InterestOnly != null)
                    {
                        maturityDate = mortgageLoanAccount.SecuredMortgageLoan.InterestOnly.MaturityDate;
                    }

					interestOnly = maturityDate.HasValue && hasInterestOnlyFinancialAdjustment;
				}

				var endTerm = remainingTerm + monthsElapsed;

				endDate = proposalStartDT.AddMonths(remainingTerm);
				var maturityPeriod = maturityDate.HasValue ? maturityDate.Value.MonthDifference(endDate, 1) : 0;
				if (maturityDate > endDate)
				{
					endTerm += maturityPeriod;
				}

				// Get the current balance at a point in time from the Loan Transaction Table
				currentBalance = mortgageLoanAccount.GetAccountBalanceByDate(proposalStartDT);

				installment = Helpers.LoanCalculator.CalculateInstallment(currentBalance, interestRate, remainingTerm, interestOnly);
				dt = GetAmortisationSchedule(currentBalance, installment, interestRate, monthsElapsed, endTerm, interestOnly);
			}

			return dt;
		}

		/// <summary>
		/// Calculates the amortising points
		/// </summary>
		/// <param name="loanBalance"></param>
		/// <param name="instalment"></param>
		/// <param name="interestRate"></param>
		/// <param name="startingPeriod"></param>
		/// <param name="endPeriod"></param>
		/// <param name="reduceBalanceAtEndOfPeriod"></param>
		/// <returns></returns>
		public LoanCalculations.AmortisationScheduleDataTable GetAmortisationSchedule(double loanBalance, double instalment, double interestRate, int startingPeriod, int endPeriod, bool reduceBalanceAtEndOfPeriod)
		{
			return GenerateAmortisingScheduleInMonths(loanBalance, instalment, interestRate, startingPeriod, endPeriod, reduceBalanceAtEndOfPeriod);
		}

		/// <summary>
		/// The method does base calculations before calculating the amortising points per proposal line item.
		/// Gets the Loan Current Balance at a point in time.
		/// Calculates the Months Elapsed to determine the starting point.
		/// </summary>
		/// <param name="proposalKey"></param>
		/// <param name="maxPeriods"></param>
		/// <returns></returns>
		public LoanCalculations.AmortisationScheduleDataTable GetAmortisationScheduleForProposalByKey(int proposalKey, int maxPeriods)
		{
			IProposal proposal = GetProposalByKey(proposalKey);

			IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
			IAccount account = accRepo.GetAccountByKey(proposal.DebtCounselling.Account.Key);
			IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;

			double totalPremium = 0D;
			double currentBalance = 0D;
			int monthsElapsed = 0;

			IEventList<IProposalItem> propItems = SortProposalItems(proposal);

			if (propItems.Count > 0 && mortgageLoanAccount != null)
			{
				// Get earliest Proposal Start Date
				DateTime proposalStartDT = propItems[0].StartDate;

				// Calculate Months Elapsed since the Account was open based on the Proposal Start Date
				monthsElapsed = proposalStartDT.MonthDifference(account.OpenDate.Value, 1);

				// Get the current balance at a point in time from the Loan Transaction Table
				currentBalance = mortgageLoanAccount.GetAccountBalanceByDate(proposalStartDT);

				// Find the HOC and Life Accounts
				IEventList<IAccount> list = account.GetNonProspectRelatedAccounts();
				list.Add(null, account);
				IAccountHOC accHOC = null;
				IAccountLifePolicy accLife = null;

				foreach (IAccount acc in list)
				{
					if (acc.Product.Key == (int)Products.HomeOwnersCover && acc.AccountStatus.Key == (int)AccountStatuses.Open)
					{
						accHOC = acc as IAccountHOC;
					}
					else if (acc.Product.Key == (int)Products.LifePolicy && acc.AccountStatus.Key == (int)AccountStatuses.Open)
					{
						accLife = acc as IAccountLifePolicy;
					}
				}

				double hocMonthlyPremium = 0;
				double lifeMonthlyPremium = 0;
				double monthlyServiceFee = 0;
				// Include the HOC Premium if requested by the proposal
				if (proposal.HOCInclusive.HasValue && proposal.HOCInclusive.Value && accHOC != null)
				{
					hocMonthlyPremium = accHOC.MonthlyPremium;
				}
				// Include the Life Premium if requested by the proposal
				if (proposal.LifeInclusive.HasValue && proposal.LifeInclusive.Value && accLife != null)
				{
					lifeMonthlyPremium = accLife.LifePolicy.MonthlyPremium;
				}

				if (proposal.MonthlyServiceFeeInclusive)
					monthlyServiceFee = account.InstallmentSummary.MonthlyServiceFee;

				totalPremium = hocMonthlyPremium + lifeMonthlyPremium + monthlyServiceFee;

				var proposalAmortisationSchedule = GetAmortisationScheduleForProposalItems(propItems, monthsElapsed, currentBalance, totalPremium, maxPeriods);
				foreach (var amortisationScheduleItem in proposalAmortisationSchedule)
				{
					amortisationScheduleItem.HOC = hocMonthlyPremium;
					amortisationScheduleItem.Life = lifeMonthlyPremium;
					amortisationScheduleItem.Fee = monthlyServiceFee;
					amortisationScheduleItem.Interest = amortisationScheduleItem.Interest - totalPremium;
					amortisationScheduleItem.Loan = amortisationScheduleItem.Payment - totalPremium;
				}
				return proposalAmortisationSchedule;
			}

			return null;
		}

		/// <summary>
		/// Concatenates all the points into one data table.
		/// </summary>
		/// <param name="propItems"></param>
		/// <param name="monthsElapsed"></param>
		/// <param name="openBalance"></param>
		/// <param name="totalPremium"></param>
		/// <param name="maxPeriods"></param>
		/// <returns></returns>
		private LoanCalculations.AmortisationScheduleDataTable GetAmortisationScheduleForProposalItems(IEventList<IProposalItem> propItems, int monthsElapsed, double openBalance, double totalPremium, int maxPeriods)
		{
			//go through the ordred list of ProposalItems building up a data row for each moonth of payment
			//collect the rows into a single data table to return

			double lineOpenBalance = openBalance;
			int startPeriod = monthsElapsed;
			LoanCalculations.AmortisationScheduleDataTable payments = new LoanCalculations.AmortisationScheduleDataTable();

			foreach (IProposalItem pi in propItems)
			{
				//build up a list of monthly payments to add to dt
				GenerateAmortisingScheduleInMonthsForPeriod(payments, lineOpenBalance, pi.TotalPayment, pi.TotalInterestRate, startPeriod, pi.Period, maxPeriods, monthsElapsed, pi.AnnualEscalation ?? 0, totalPremium);

				//if we are out of range, get out of dodge
				if (startPeriod > maxPeriods)
					break;

				//get the starting values for the next proposal item
				if (payments.Rows.Count > 0) //the first proposal item might have a term < 1
				{
					lineOpenBalance = payments[payments.Rows.Count - 1].Closing;
					startPeriod = payments[payments.Rows.Count - 1].Period + 1;
				}
			}

			return payments;
		}

		private Double CalcTotalPremium(IAccount acc)
		{
			double sum = 0D;
			foreach (IFinancialService fs in acc.FinancialServices)
			{
				if (fs.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open || fs.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Dormant)
				{
					sum += fs.Payment;
				}
			}

			return sum;
		}

		#region Helper Methods

		/// <summary>
		/// This method calculates the amortising points for a certain period.
		/// The starting point indicates at which point the loan is at.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="LoanBalance"></param>
		/// <param name="payment"></param>
		/// <param name="InterestRate"></param>
		/// <param name="StartingPeriod"></param>
		/// <param name="Periods"></param>
		/// <param name="maxPeriods"></param>
		/// <param name="monthsElapsed"></param>
		/// <param name="annualEscalation"></param>
		/// <param name="totalPremium">HOC, Life and the Administration Fee</param>
		private void GenerateAmortisingScheduleInMonthsForPeriod(LoanCalculations.AmortisationScheduleDataTable dt, double LoanBalance, double payment, double InterestRate, int StartingPeriod, int Periods, int maxPeriods, int monthsElapsed, double annualEscalation, double totalPremium)
		{
			double closeBalance = LoanBalance;
			double openBalance = LoanBalance;
			double instalment = payment;
			double interestRate = InterestRate;
			double nonCapitalAmount = 0;
			double capitalAmount = 0;
			int count = 0;

			while (count < Periods && StartingPeriod <= maxPeriods)
			{
				//Determine whether we should be escalating

				//StartingPeriod is the number of Months that has Elapsed since the Proposal Start Date
				//Months Elapsed is the number of Months that has elapsed for the Account
				//The Escalation includes the total Premium (HOC, Life and the Administration Fee)
				bool mustEscalate = ((StartingPeriod - (monthsElapsed - 1)) % 12.0) == 0 && count > 0;

				//Escalate the instalment by the annual escalation
				if (mustEscalate)
				{
					instalment = (instalment * annualEscalation) + instalment;
				}

				LoanCalculations.AmortisationScheduleRow row = dt.NewAmortisationScheduleRow();
				row.Opening = openBalance;

				nonCapitalAmount = CalculateInstalmentInterest(openBalance, interestRate) + totalPremium;

				capitalAmount = CalculateInstalmentCapital(instalment, nonCapitalAmount);
				if (openBalance > instalment)
				{
					closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
				}
				else
				{   //removed so lasts months value does not reflect on last years figure
					instalment = closeBalance + nonCapitalAmount;
					capitalAmount = CalculateInstalmentCapital(instalment, nonCapitalAmount);
					closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
				}

				row.Capital = capitalAmount;
				row.Closing = closeBalance;
				row.Payment = instalment;
				row.Interest = nonCapitalAmount;
				row.Period = StartingPeriod;
				openBalance = closeBalance;
				dt.AddAmortisationScheduleRow(row);
				StartingPeriod += 1;
				count += 1;
			}
		}

		/// <summary>
		/// This method calculates the points until the loan is amortised to zero.
		/// The starting point indicates at which point the loan is at i.e. Balance = 500K @ Month No. 10
		/// </summary>
		/// <param name="LoanBalance"></param>
		/// <param name="Instalment"></param>
		/// <param name="InterestRate"></param>
		/// <param name="StartingPeriod"></param>
		/// <param name="endPeriod"></param>
		/// <param name="reduceBalanceAtEndOfPeriod"></param>
		/// <returns></returns>
		private LoanCalculations.AmortisationScheduleDataTable GenerateAmortisingScheduleInMonths(double LoanBalance, double Instalment, double InterestRate, int StartingPeriod, int endPeriod, bool reduceBalanceAtEndOfPeriod)
		{
			LoanCalculations.AmortisationScheduleDataTable dt = new LoanCalculations.AmortisationScheduleDataTable();
			double closeBalance = LoanBalance;
			double openBalance = LoanBalance;
			double instalment = Instalment;
			double interestRate = InterestRate;
			double interestAmount = 0;
			double capitalAmount = 0;
			double instalmentDisplay = 0;

			while (closeBalance > 0 && (StartingPeriod < endPeriod))
			{
				LoanCalculations.AmortisationScheduleRow row = dt.NewAmortisationScheduleRow();
				row.Opening = openBalance;

				interestAmount = CalculateInstalmentInterest(openBalance, interestRate);
				capitalAmount = CalculateInstalmentCapital(instalment, interestAmount);

				instalmentDisplay = instalment;
				if (openBalance > instalment)
				{
					closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
				}
				else
				{   //removed so lasts months value does not reflect on last years figure
					instalment = closeBalance + interestAmount;
					capitalAmount = CalculateInstalmentCapital(instalment, interestAmount);
					closeBalance = CalculateCloseBalance(openBalance, capitalAmount);
				}

				row.Capital = capitalAmount;
				row.Closing = closeBalance;
				row.Payment = instalmentDisplay;
				row.Interest = interestAmount;
				row.Period = StartingPeriod;
				openBalance = closeBalance;
				dt.AddAmortisationScheduleRow(row);
				StartingPeriod += 1;

				//avoid excessive loops, this happens when the amounts are very low
				//if (StartingPeriod > 500)
				//    break;
			}
			if (reduceBalanceAtEndOfPeriod)
			{
				LoanCalculations.AmortisationScheduleRow row = dt.NewAmortisationScheduleRow();
				row.Payment = closeBalance;
				closeBalance = -0.001;

				row.Capital = 0;
				row.Closing = closeBalance;
				row.Interest = interestAmount;
				row.Period = StartingPeriod;
				openBalance = closeBalance;
				dt.AddAmortisationScheduleRow(row);
			}
			return dt;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="OpeningBalance"></param>
		/// <param name="Capital"></param>
		/// <returns></returns>
		private double CalculateCloseBalance(double OpeningBalance, double Capital)
		{
			double ClosingBalance = OpeningBalance - Capital;
			return ClosingBalance;
		}

		/// <summary>
		/// Calculate the Interest paid in a month for the Loan value at the annual interest rate
		/// </summary>
		/// <param name="LV"></param>
		/// <param name="AnnualInterestRate"></param>
		/// <returns></returns>
		private double CalculateInstalmentInterest(double LV, double AnnualInterestRate)
		{
			return LV * (AnnualInterestRate / 12);
		}

		/// <summary>
		/// Calculate the Capital portion of an instalment given the interest
		/// </summary>
		/// <param name="instalment"></param>
		/// <param name="instalmentInterest"></param>
		/// <returns></returns>
		private double CalculateInstalmentCapital(double instalment, double instalmentInterest)
		{
			return instalment - instalmentInterest;
		}

		#endregion Helper Methods

		#endregion Graphs

		#region Attorney

		/// <summary>
		/// Get Litigation Attorneys
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, string> GetLitigationAttorneys()
		{
			IDictionary<int, string> attorneys = new Dictionary<int, string>();

			string query = @"SELECT LE.LegalEntityKey, RegisteredName
									FROM [2am]..Attorney ATT (nolock)
									JOIN [2am]..LegalEntity LE (nolock) ON ATT.LegalEntityKey = LE.LegalEntityKey
									WHERE ATT.AttorneyLitigationInd = 1
									ORDER BY RegisteredName;";
			ParameterCollection parameters = new ParameterCollection();

			DataSet dsResults = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
			if (dsResults != null && dsResults.Tables.Count > 0)
			{
				foreach (DataRow dr in dsResults.Tables[0].Rows)
				{
					attorneys.Add((int)dr[0], dr[1].ToString());
				}
			}
			return attorneys;
		}
        
		#endregion Attorney

		#region Loss Control

		/// <summary>
		///
		/// </summary>
		/// <param name="AccountKey"></param>
		/// <param name="eStageName"></param>
		/// <param name="eFolderID"></param>
		/// <param name="adUser"></param>
		public void GetEworkDataForLossControlCase(int AccountKey, out string eStageName, out string eFolderID, out IADUser adUser)
		{
			eStageName = "";
			eFolderID = "";
			adUser = null;

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetEworkDataForLossControlCase");
			ParameterCollection prms = new ParameterCollection();
			prms.Add(new SqlParameter("@AccountKey", SqlDbType.VarChar));
			prms[0].Value = AccountKey;

			DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

			if (ds != null && ds.Tables.Count > 0)
			{
				if (ds.Tables[0].Rows.Count > 0)
				{
					eStageName = ds.Tables[0].Rows[0]["eStageName"].ToString();
					eFolderID = ds.Tables[0].Rows[0]["eFolderId"].ToString();
				}
			}

			adUser = GetEworkADUserForLossControlCase(eFolderID);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="eFolderID"></param>
		/// <returns></returns>
		public IADUser GetEworkADUserForLossControlCase(string eFolderID)
		{
			IADUser adUser = null;

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetEworkADUserForLossControlCase");
			ParameterCollection prms = new ParameterCollection();
			prms.Add(new SqlParameter("@eFolderId", SqlDbType.VarChar));
			prms[0].Value = eFolderID;

			object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);
			if (obj != null)
			{
				int adUserKey = Convert.ToInt32(obj);
				if (adUserKey > 0)
				{
					IOrganisationStructureRepository orgstructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
					adUser = orgstructRepo.GetADUserByKey(adUserKey);
				}
			}

			return adUser;
		}

		#endregion Loss Control

		#region Proposals

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public IProposal CreateEmptyProposal()
		{
			return base.CreateEmpty<IProposal, Proposal_DAO>();
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public IProposalItem CreateEmptyProposalItem()
		{
			return base.CreateEmpty<IProposalItem, ProposalItem_DAO>();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposal"></param>
		public void SaveProposal(IProposal proposal)
		{
			base.Save<IProposal, Proposal_DAO>(proposal);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposalKey"></param>
		/// <returns></returns>
		public IProposal GetProposalByKey(int proposalKey)
		{
			return base.GetByKey<IProposal, Proposal_DAO>(proposalKey);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposalItemKey"></param>
		/// <returns></returns>
		public IProposalItem GetProposalItemByKey(int proposalItemKey)
		{
			return base.GetByKey<IProposalItem, ProposalItem_DAO>(proposalItemKey);
		}

		/// <summary>
		/// Return a List of Debt Counselling proposals by generickey and generickeytype
		/// </summary>
		/// <param name="genericKey"></param>
		/// <param name="genericKeyTypeKey"></param>
		/// <returns></returns>
		public List<IProposal> GetProposalsByGenericKey(int genericKey, int genericKeyTypeKey)
		{
			List<IProposal> proposalList = new List<IProposal>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = "";

			switch (genericKeyTypeKey)
			{
				case (int)GenericKeyTypes.DebtCounselling2AM:
					query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByDebtCounsellingKey");
					break;
				case (int)GenericKeyTypes.Account:
				case (int)GenericKeyTypes.ParentAccount:
					query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByAccountKey");
					break;
				case (int)GenericKeyTypes.LegalEntity:
					query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByLegalEntityKey");
					break;
				default:
					break;
			}

			if (!String.IsNullOrEmpty(query))
			{
				SimpleQuery<Proposal_DAO> p = new SimpleQuery<Proposal_DAO>(QueryLanguage.Sql, query, genericKey);
				p.AddSqlReturnDefinition(typeof(Proposal_DAO), "p");

				Proposal_DAO[] res = p.Execute();

				if (res == null || res.Length == 0)
					return proposalList;

				foreach (Proposal_DAO pr in res)
				{
					proposalList.Add(BMTM.GetMappedType<IProposal, Proposal_DAO>(pr));
				}
			}

			return proposalList;
		}

		/// <summary>
		/// Get Proposal Detail Items by Key
		/// </summary>
		/// <param name="ProposalKey"></param>
		/// <returns></returns>
		public List<IProposalItem> GetProposalItemsByKey(int ProposalKey)
		{
			List<IProposalItem> proposalItemList = new List<IProposalItem>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
			const string HQL = "SELECT pitem from ProposalItem_DAO pitem WHERE pitem.Proposal.Key = ?";
			SimpleQuery<ProposalItem_DAO> q = new SimpleQuery<ProposalItem_DAO>(HQL, ProposalKey);
			ProposalItem_DAO[] res = q.Execute();
			if (res == null || res.Length == 0)
				return proposalItemList;

			foreach (ProposalItem_DAO pitem in res)
			{
				proposalItemList.Add(BMTM.GetMappedType<IProposalItem, ProposalItem_DAO>(pitem));
			}
			return proposalItemList;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCounsellingKey"></param>
		/// <param name="proposalType"></param>
		/// <returns></returns>
		public List<IProposal> GetProposalsByType(int debtCounsellingKey, ProposalTypes proposalType)
		{
			List<IProposal> proposalList = new List<IProposal>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByType");

			if (!String.IsNullOrEmpty(query))
			{
				SimpleQuery<Proposal_DAO> p = new SimpleQuery<Proposal_DAO>(QueryLanguage.Sql, query, (int)proposalType, debtCounsellingKey);
				p.AddSqlReturnDefinition(typeof(Proposal_DAO), "p");

				Proposal_DAO[] res = p.Execute();

				if (res == null || res.Length == 0)
					return proposalList;

				foreach (Proposal_DAO pr in res)
				{
					proposalList.Add(BMTM.GetMappedType<IProposal, Proposal_DAO>(pr));
				}
			}

			return proposalList;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCounsellingKey"></param>
		/// <param name="proposalStatus"></param>
		/// <returns></returns>
		public List<IProposal> GetProposalsByStatus(int debtCounsellingKey, ProposalStatuses proposalStatus)
		{
			List<IProposal> proposalList = new List<IProposal>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByStatus");

			if (!String.IsNullOrEmpty(query))
			{
				SimpleQuery<Proposal_DAO> p = new SimpleQuery<Proposal_DAO>(QueryLanguage.Sql, query, (int)proposalStatus, debtCounsellingKey);
				p.AddSqlReturnDefinition(typeof(Proposal_DAO), "p");

				Proposal_DAO[] res = p.Execute();

				if (res == null || res.Length == 0)
					return proposalList;

				foreach (Proposal_DAO pr in res)
				{
					proposalList.Add(BMTM.GetMappedType<IProposal, Proposal_DAO>(pr));
				}
			}

			return proposalList;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCounsellingKey"></param>
		/// <param name="proposalType"></param>
		/// <param name="proposalStatus"></param>
		/// <returns></returns>
		public List<IProposal> GetProposalsByTypeAndStatus(int debtCounsellingKey, ProposalTypes proposalType, ProposalStatuses proposalStatus)
		{
			List<IProposal> proposalList = new List<IProposal>();
			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetProposalsByTypeAndStatus");

			if (!String.IsNullOrEmpty(query))
			{
				SimpleQuery<Proposal_DAO> p = new SimpleQuery<Proposal_DAO>(QueryLanguage.Sql, query, (int)proposalType, (int)proposalStatus, debtCounsellingKey);
				p.AddSqlReturnDefinition(typeof(Proposal_DAO), "p");

				Proposal_DAO[] res = p.Execute();

				if (res == null || res.Length == 0)
					return proposalList;

				foreach (Proposal_DAO pr in res)
				{
					proposalList.Add(BMTM.GetMappedType<IProposal, Proposal_DAO>(pr));
				}
			}

			return proposalList;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposal"></param>
		public void DeleteProposal(IProposal proposal)
		{
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
			IRuleService svcRule = ServiceFactory.GetService<IRuleService>();

			bool canDelete =
				(svcRule.ExecuteRule(spc.DomainMessages, "DeleteDraftProposal", proposal) == 1) &&
				(svcRule.ExecuteRule(spc.DomainMessages, "DeleteProposal", proposal) == 1);

			if (canDelete)
			{
				Proposal_DAO dao = (Proposal_DAO)(proposal as IDAOObject).GetDAOObject();
				dao.DeleteAndFlush();
			}

			if (ValidationHelper.PrincipalHasValidationErrors())
				throw new DomainValidationException();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposal"></param>
		/// <param name="adUser"></param>
		public void CopyProposalToDraft(IProposal proposal, IADUser adUser)
		{
			// create a new Proposal
			IProposal newProposal = this.CreateEmptyProposal();

			// set its properties from the selected proposal
			newProposal.ProposalType = proposal.ProposalType;
			newProposal.ProposalStatus = LookupRepo.ProposalStatuses[SAHL.Common.Globals.ProposalStatuses.Draft];
			newProposal.DebtCounselling = proposal.DebtCounselling;
			newProposal.HOCInclusive = proposal.HOCInclusive;
			newProposal.LifeInclusive = proposal.LifeInclusive;
			newProposal.ADUser = adUser;
			newProposal.CreateDate = DateTime.Now;
			newProposal.MonthlyServiceFeeInclusive = proposal.MonthlyServiceFeeInclusive;

			// loop thru each proposalitem on the selected proposal and add to new draft proposal
			foreach (IProposalItem proposalItem in proposal.ProposalItems)
			{
				IProposalItem newProposalItem = this.CreateEmptyProposalItem();
				newProposalItem.Proposal = newProposal;
				newProposalItem.StartDate = proposalItem.StartDate;
				newProposalItem.EndDate = proposalItem.EndDate;
				newProposalItem.MarketRate = proposalItem.MarketRate;
				newProposalItem.InterestRate = proposalItem.InterestRate;
				newProposalItem.Amount = proposalItem.Amount;
				newProposalItem.AdditionalAmount = proposalItem.AdditionalAmount;
				newProposalItem.ADUser = adUser;
				newProposalItem.InstalmentPercent = proposalItem.InstalmentPercent;
				newProposalItem.AnnualEscalation = proposalItem.AnnualEscalation;
				newProposalItem.CreateDate = DateTime.Now;
				newProposalItem.StartPeriod = proposalItem.StartPeriod;
				newProposalItem.EndPeriod = proposalItem.EndPeriod;

				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

				newProposal.ProposalItems.Add(spc.DomainMessages, newProposalItem);
			}

			// save the new proposal
			this.SaveProposal(newProposal);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposal"></param>
		/// <param name="adUser"></param>
		/// <param name="proposalTypes"></param>
		public void CopyProposalToDraft(IProposal proposal, IADUser adUser, ProposalTypes proposalTypes)
		{
			ILookupRepository lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

			// create a new Proposal
			IProposal newProposal = this.CreateEmptyProposal();

			// set its properties from the selected proposal
			newProposal.ProposalType = lkRepo.ProposalTypes[proposalTypes];
			newProposal.ProposalStatus = LookupRepo.ProposalStatuses[SAHL.Common.Globals.ProposalStatuses.Draft];
			newProposal.DebtCounselling = proposal.DebtCounselling;
			newProposal.HOCInclusive = proposal.HOCInclusive;
			newProposal.LifeInclusive = proposal.LifeInclusive;
			newProposal.ADUser = adUser;
			newProposal.CreateDate = DateTime.Now;
			newProposal.MonthlyServiceFeeInclusive = proposal.MonthlyServiceFeeInclusive;

			// loop thru each proposalitem on the selected proposal and add to new draft proposal
			foreach (IProposalItem proposalItem in proposal.ProposalItems)
			{
				IProposalItem newProposalItem = this.CreateEmptyProposalItem();
				newProposalItem.Proposal = newProposal;
				newProposalItem.StartDate = proposalItem.StartDate;
				newProposalItem.EndDate = proposalItem.EndDate;
				newProposalItem.MarketRate = proposalItem.MarketRate;
				newProposalItem.InterestRate = proposalItem.InterestRate;
				newProposalItem.Amount = proposalItem.Amount;
				newProposalItem.AdditionalAmount = proposalItem.AdditionalAmount;
				newProposalItem.ADUser = adUser;
				newProposalItem.InstalmentPercent = proposalItem.InstalmentPercent;
				newProposalItem.AnnualEscalation = proposalItem.AnnualEscalation;
				newProposalItem.CreateDate = DateTime.Now;
				newProposalItem.StartPeriod = proposalItem.StartPeriod;
				newProposalItem.EndPeriod = proposalItem.EndPeriod;

				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
				newProposal.ProposalItems.Add(spc.DomainMessages, newProposalItem);
			}

			// save the new proposal
			this.SaveProposal(newProposal);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="proposal"></param>
		/// <param name="adUser"></param>
		public void SetProposalToActive(IProposal proposal, IADUser adUser)
		{
			// get a list of the "Active" proposals for the proposal type i am working with
			ProposalTypes proposalType = proposal.ProposalType.Key == (int)Globals.ProposalTypes.CounterProposal ? Globals.ProposalTypes.CounterProposal : Globals.ProposalTypes.Proposal;
			IList<IProposal> activeProposals = this.GetProposalsByTypeAndStatus(proposal.DebtCounselling.Key, proposalType, ProposalStatuses.Active);

			// set the status of any "Active" proposals to "Inactive"
			foreach (IProposal activeProposal in activeProposals)
			{
				activeProposal.ProposalStatus = LookupRepo.ProposalStatuses[SAHL.Common.Globals.ProposalStatuses.Inactive];
				SaveProposal(activeProposal);
			}

			// set the status of our proposal to "Active"
			proposal.ProposalStatus = LookupRepo.ProposalStatuses[SAHL.Common.Globals.ProposalStatuses.Active];
			base.Save<IProposal, Proposal_DAO>(proposal);
		}

		public IEventList<IProposalItem> SortProposalItems(IProposal proposal)
		{
			if (proposal != null && proposal.ProposalItems.Count > 0)
			{
				List<IProposalItem> listPI = new List<IProposalItem>(proposal.ProposalItems);
				listPI.Sort(delegate(IProposalItem pi1, IProposalItem pi2) { return pi1.StartDate.CompareTo(pi2.StartDate); });
				return new EventList<IProposalItem>(listPI);
			}
			return new EventList<IProposalItem>();
		}

		#endregion Proposals

		#region DebtCounsellor

		#region Legal Entity Specific

		/// <summary>
		/// Get an empty instance of debt counselling to work with
		/// </summary>
		/// <returns></returns>
		public IDebtCounsellorDetail CreateEmptyDebtCounsellorDetail()
		{
			return base.CreateEmpty<IDebtCounsellorDetail, DebtCounsellorDetail_DAO>();
		}

		/// <summary>
		/// Save a debt counsellor instance
		/// </summary>
		/// <param name="debtCounsellorDetail"></param>
		public void SaveDebtCounsellorDetail(IDebtCounsellorDetail debtCounsellorDetail)
		{
			base.Save<IDebtCounsellorDetail, DebtCounsellorDetail_DAO>(debtCounsellorDetail);
		}

		/// <summary>
		/// Gets the last active user of a given workflow role type which have been allocated to the case
		/// </summary>
		/// <param name="debtCounsellingKey"></param>
		/// <param name="workflowRoleType"></param>
		/// <returns>an IADuser or null</returns>
		public IADUser GetActiveDebtCounsellingUser(int debtCounsellingKey, WorkflowRoleTypes workflowRoleType)
		{
			string HQL = "SELECT wr from WorkflowRole_DAO wr WHERE wr.GenericKey = ? and wr.WorkflowRoleType.Key = ? and wr.GeneralStatus.Key = 1";
			SimpleQuery<WorkflowRole_DAO> q = new SimpleQuery<WorkflowRole_DAO>(HQL, debtCounsellingKey, (int)workflowRoleType);
			WorkflowRole_DAO[] res = q.Execute();
			if (res == null || res.Length == 0)
				return null;

			HQL = "SELECT ad from ADUser_DAO ad WHERE ad.LegalEntity.Key = ?";
			SimpleQuery<ADUser_DAO> qAD = new SimpleQuery<ADUser_DAO>(HQL, res[0].LegalEntityKey);
			ADUser_DAO[] rAD = qAD.Execute();
			if (rAD == null || rAD.Length == 0)
				return null;

			IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
			IADUser adUser = BMTM.GetMappedType<IADUser, ADUser_DAO>(rAD[0]);

			return adUser;
		}

		#endregion Legal Entity Specific

		public void SaveDebtCounsellorOrganisationStructure(IDebtCounsellorOrganisationNode eaos)
		{
			base.Save<IDebtCounsellorOrganisationNode, OrganisationStructure_DAO>(eaos);
		}

		public IDebtCounsellorOrganisationNode GetDebtCounsellorOrganisationNodeForKey(int Key)
		{
			OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);

			if (dao != null)
			{
				organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.DebtCounsellor;
				return (IDebtCounsellorOrganisationNode)organisationStructureFactory.GetLEOSNode(dao);
			}
			else
				return null;
		}

		public IDebtCounsellorOrganisationNode GetDebtCounsellorOrganisationNodeForLegalEntity(int key)
		{
			LegalEntityOrganisationStructure_DAO[] daoList = LegalEntityOrganisationStructure_DAO.FindAllByProperty("LegalEntity.Key", key);

			if (daoList != null && daoList.Length > 0)
			{
				organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.DebtCounsellor;
				return (IDebtCounsellorOrganisationNode)organisationStructureFactory.GetLEOSNode(daoList[0].OrganisationStructure);
			}
			else
				return null;
		}

		public IDebtCounsellorOrganisationNode GetTopDebtCounsellorCompanyNodeForDebtCounselling(int debtCouncellingKey)
		{
			IDebtCounsellorOrganisationNode dcNode = null;

			// get the debtcounselling object
			IDebtCounselling debtCounselling = this.GetDebtCounsellingByKey(debtCouncellingKey);

			// get the debtcounsellor
			ILegalEntity debtCounsellor = debtCounselling.DebtCounsellor;

			if (debtCounsellor != null)
			{
				// get the organisationstructure for the top level dc node
				IControl ct = ControlRepo.GetControlByDescription("DebtCounsellorRoot");
				int rootKey = Convert.ToInt32(ct.ControlNumeric.Value);
				IOrganisationStructure orgStructureDebtCounsellorRoot = OrganisationStructureRepo.GetOrganisationStructureForKey(rootKey);
				if (orgStructureDebtCounsellorRoot != null)
				{
					// get the IDebtCounsellorOrganisationNode for the dc
					IDebtCounsellorOrganisationNode dcOS = this.GetDebtCounsellorOrganisationNodeForLegalEntity(debtCounsellor.Key);

					// get top level company
					ILegalEntityOrganisationNode lon = dcOS.GetOrgstructureTopParentItem(OrganisationTypes.Company, rootKey);

					// cast the node to the correct type
					dcNode = this.GetDebtCounsellorOrganisationNodeForKey(lon.Key);
				}
			}

			return dcNode;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCouncellingKey"></param>
		/// <returns></returns>
		public ILegalEntity GetDebtCounsellorForDebtCounselling(int debtCouncellingKey)
		{
			ILegalEntity le = null;

			int debtCounsellingKey = (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM;
			int debtCounsellorKey = (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounsellor;
			int activeGeneralStatusKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;

			string sql = string.Format(@"SELECT le.* FROM DebtCounselling.DebtCounselling dc (NOLOCK) JOIN dbo.ExternalRole er (NOLOCK) ON er.GenericKey = dc.DebtCounsellingKey" +
				" AND er.GenericKeyTypeKey = " + debtCounsellingKey + " AND er.ExternalRoleTypeKey = " + debtCounsellorKey + " AND er.GeneralStatusKey = " + activeGeneralStatusKey +
				" JOIN dbo.LegalEntity le (NOLOCK) ON er.LegalEntityKey = le.LegalEntityKey" +
				" LEFT JOIN debtcounselling.DebtCounsellorDetail dcd (NOLOCK) ON le.LegalEntityKey = dcd.LegalEntityKey" +
				" WHERE dc.DebtCounsellingKey = {0}", debtCouncellingKey);

			SimpleQuery<LegalEntity_DAO> leQ = new SimpleQuery<LegalEntity_DAO>(QueryLanguage.Sql, sql);
			leQ.AddSqlReturnDefinition(typeof(LegalEntity_DAO), "LE");
			LegalEntity_DAO[] leRes = leQ.Execute();

			if (leRes.Length > 0)
				le = base.GetByKey<ILegalEntity, LegalEntity_DAO>(leRes[0].Key);

			return le;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCounsellorKey"></param>
		/// <returns></returns>
		public ILegalEntity GetDebtCounsellorCompanyForDebtCounsellor(int debtCounsellorKey)
		{
			ILegalEntity le = null;

			int orgTypeDesignationKey = (int)SAHL.Common.Globals.OrganisationTypes.Designation;
			int orgTypeCompanyKey = (int)SAHL.Common.Globals.OrganisationTypes.Company;
			int generalStatusActiveKey = (int)SAHL.Common.Globals.GeneralStatuses.Active;

			string sql = string.Format(@"SELECT le.* FROM dbo.LegalEntityOrganisationStructure leos1 (NOLOCK) JOIN dbo.OrganisationStructure os1 (NOLOCK) ON leos1.OrganisationStructureKey = os1.OrganisationStructureKey " +
				"AND os1.OrganisationTypeKey = " + orgTypeDesignationKey + " AND os1.GeneralStatusKey = " + generalStatusActiveKey +
				" JOIN dbo.OrganisationStructure os2 (NOLOCK) ON os1.ParentKey = os2.OrganisationStructureKey " +
				"AND os2.OrganisationTypeKey = " + orgTypeCompanyKey + " AND os2.GeneralStatusKey = " + generalStatusActiveKey +
				" JOIN dbo.LegalEntityOrganisationStructure leos2 (NOLOCK) ON os2.OrganisationStructureKey = leos2.OrganisationStructureKey " +
				"JOIN dbo.LegalEntity le (NOLOCK) ON leos2.LegalEntityKey = le.LegalEntityKey " +
				"LEFT JOIN debtcounselling.DebtCounsellorDetail dcd (NOLOCK) ON le.LegalEntityKey = dcd.LegalEntityKey " +
				"WHERE leos1.LegalEntityKey = {0}", debtCounsellorKey);

			SimpleQuery<LegalEntity_DAO> leQ = new SimpleQuery<LegalEntity_DAO>(QueryLanguage.Sql, sql);
			leQ.AddSqlReturnDefinition(typeof(LegalEntity_DAO), "LE");
			LegalEntity_DAO[] leRes = leQ.Execute();

			if (leRes.Length > 0)
				le = base.GetByKey<ILegalEntity, LegalEntity_DAO>(leRes[0].Key);

			return le;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="debtCouncellingKey"></param>
		/// <returns></returns>
		public IEventList<ILegalEntity> GetAllDebtCounsellorsForDebtCounselling(int debtCouncellingKey)
		{
			IEventList<ILegalEntity> debtCounsellors = new EventList<ILegalEntity>();

			// get debt counselling object
			IDebtCounselling dc = GetDebtCounsellingByKey(debtCouncellingKey);

			if (dc != null && dc.DebtCounsellor != null)
			{
				// get the debt counsellors node
				IDebtCounsellorOrganisationNode dcNode = GetDebtCounsellorOrganisationNodeForLegalEntity(dc.DebtCounsellor.Key);

				// if its a Company or Branch then show everything below this level
				// if its a Dept or Contact then go up to branch level (or company level if no branch) and show everything below
				int orgStructureKey = -1;
				switch (dcNode.OrganisationType.Key)
				{
					case (int)SAHL.Common.Globals.OrganisationTypes.Company:
					case (int)SAHL.Common.Globals.OrganisationTypes.Branch_Originator:
						orgStructureKey = dcNode.Key;
						break;
					default:
						ILegalEntityOrganisationNode branchNode = dcNode.GetOrgstructureParentItem(OrganisationTypes.Branch_Originator);
						if (branchNode != null)
							orgStructureKey = branchNode.Key;
						else
						{
							ILegalEntityOrganisationNode companyNode = dcNode.GetOrgstructureParentItem(OrganisationTypes.Company);
							if (companyNode != null)
								orgStructureKey = companyNode.Key;
						}
						break;
				}

				// find all debt counsellers belonging to this company/branch
				if (orgStructureKey > 0)
				{
					IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
					debtCounsellors = osRepo.GetLegalEntitiesForOrganisationStructureKey(orgStructureKey, true);
				}
			}
			return debtCounsellors;
		}

		public DataTable GetRelatedDebtCounsellingAccounts(int iDebtCounsellingKey)
		{
			string query = "", statement = "";

			query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetRelatedDebtCounsellingAccounts");
			statement = String.Format(query, iDebtCounsellingKey);

			DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(statement, typeof(GeneralStatus_DAO), null);

			if (ds != null && ds.Tables.Count > 0)
				return ds.Tables[0];

			return new DataTable();
		}

		public IList<IDebtCounselling> GetDebtCounsellingByLegalEntityKey(GenericKeyTypes genericKeyType, List<int> externalRoleTypeKeys, GeneralStatuses externalRoleGeneralStatus,
			List<int> roleTypeKeys, GeneralStatuses roleGeneralStatus, DebtCounsellingStatuses debtCounsellingStatus, int legalEntityKey)
		{
			string delimitedExternalRoleTypes = String.Join(",", externalRoleTypeKeys.ToArray());
			string delimitedRoleTypes = String.Join(",", roleTypeKeys.ToArray());

			string sql = @"select dc.*
							from [2am].DebtCounselling.DebtCounselling dc (nolock)
							join [2am].dbo.ExternalRole er (nolock) on er.GenericKey = dc.DebtCounsellingKey
									and er.GenericKeyTypeKey = ?
									and er.ExternalRoleTypeKey in (" + delimitedExternalRoleTypes + @")
									and er.GeneralStatusKey = ?
							join [2am].dbo.[Role] r (nolock) on r.LegalEntityKey = er.LegalEntityKey
									and r.AccountKey = dc.AccountKey
									and r.RoleTypeKey in (" + delimitedRoleTypes + @")
									and r.GeneralStatusKey = ?
							where dc.DebtCounsellingStatusKey = ?
							and er.LegalEntityKey = ?";

			SimpleQuery<DebtCounselling_DAO> p = new SimpleQuery<DebtCounselling_DAO>(QueryLanguage.Sql, sql, (int)genericKeyType, (int)externalRoleGeneralStatus, (int)roleGeneralStatus, (int)debtCounsellingStatus, legalEntityKey);
			p.AddSqlReturnDefinition(typeof(DebtCounselling_DAO), "dc");

			DebtCounselling_DAO[] res = p.Execute();

			if (res == null || res.Length == 0)
				return null;

			IList<IDebtCounselling> debtCounsellingList = new List<IDebtCounselling>();
			IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
			foreach (DebtCounselling_DAO pr in res)
			{
				debtCounsellingList.Add(BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(pr));
			}

			return debtCounsellingList;
		}

		#endregion DebtCounsellor

		#region Payment Distribution Agent

		public void SavePaymentDistributionAgentOrganisationStructure(IPaymentDistributionAgentOrganisationNode eaos)
		{
			base.Save<IPaymentDistributionAgentOrganisationNode, OrganisationStructure_DAO>(eaos);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="Key">The Organisation Structure Key</param>
		/// <returns></returns>
		public IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForKey(int Key)
		{
			OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);

			if (dao != null)
			{
				organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.PaymentDistributionAgency;
				return (IPaymentDistributionAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(dao);
			}
			else
				return null;
		}

		/// <summary>
		/// Get the IPaymentDistributionAgentOrganisationNode for the LegalEntityKey provided
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForLegalEntity(int key)
		{
			LegalEntityOrganisationStructure_DAO[] daoList = LegalEntityOrganisationStructure_DAO.FindAllByProperty("LegalEntity.Key", key);

			if (daoList != null && daoList.Length > 0)
			{
				organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.PaymentDistributionAgency;
				return (IPaymentDistributionAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(daoList[0].OrganisationStructure);
			}
			else
				return null;
		}

		#endregion Payment Distribution Agent

		#region Helper Methods

		private OrganisationStructureRepository.OrganisationStructureFactory _organisationStructureFactory;

		private OrganisationStructureRepository.OrganisationStructureFactory organisationStructureFactory
		{
			get
			{
				if (_organisationStructureFactory == null)
				{
					_organisationStructureFactory = new OrganisationStructureRepository.OrganisationStructureFactory();
					_organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.DebtCounsellor;
				}
				return _organisationStructureFactory;
			}
		}

		private ILookupRepository _lookupRepo;

		private ILookupRepository LookupRepo
		{
			get
			{
				if (_lookupRepo == null)
					_lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

				return _lookupRepo;
			}
		}

		private IControlRepository _controlRepo;

		private IControlRepository ControlRepo
		{
			get
			{
				if (_controlRepo == null)
					_controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

				return _controlRepo;
			}
		}

		private IOrganisationStructureRepository _osRepo;

		private IOrganisationStructureRepository OrganisationStructureRepo
		{
			get
			{
				if (_osRepo == null)
					_osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

				return _osRepo;
			}
		}

		#endregion Helper Methods

		#region Court / Hearing Details

		public ICourt GetCourtByKey(int key)
		{
			return base.GetByKey<ICourt, Court_DAO>(key);
		}

		public ICourtType GetCourtTypeByKey(int key)
		{
			return base.GetByKey<ICourtType, CourtType_DAO>(key);
		}

		public IHearingType GetHearingTypeByKey(int key)
		{
			return base.GetByKey<IHearingType, HearingType_DAO>(key);
		}

		public IHearingDetail CreateEmptyHearingDetail()
		{
			return base.CreateEmpty<IHearingDetail, HearingDetail_DAO>();
		}

		public IHearingAppearanceType GetHearingAppearanceTypeByKey(int key)
		{
			return base.GetByKey<IHearingAppearanceType, HearingAppearanceType_DAO>(key);
		}

		public bool CheckHearingDetailExistsForDebtCounsellingKey(int debtCounsellingKey, int HearingTypeKey, List<int> HearingAppearanceTypeKeys)
		{
			IList<IHearingDetail> hearingDetails = new List<IHearingDetail>();

			string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "GetActiveHearingDetailListForDebtCounsellingKey");
			query = string.Format(query, debtCounsellingKey, HearingTypeKey, String.Join(",", HearingAppearanceTypeKeys.Select((o) => o.ToString()).ToArray()));
			if (!String.IsNullOrEmpty(query))
			{
				SimpleQuery<HearingDetail_DAO> c = new SimpleQuery<HearingDetail_DAO>(QueryLanguage.Sql, query);
				c.AddSqlReturnDefinition(typeof(HearingDetail_DAO), "hd");

				HearingDetail_DAO[] res = c.Execute();

				if (res != null && res.Length > 0)
				{
					IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

					foreach (var dao in res)
					{
						IHearingDetail hearingDetail = BMTM.GetMappedType<IHearingDetail, HearingDetail_DAO>(dao);
						hearingDetails.Add(hearingDetail);
					}
				}
			}

			if (hearingDetails != null && hearingDetails.Count > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Get by the Primary key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public IHearingDetail GetHearingDetailByKey(int key)
		{
			return base.GetByKey<IHearingDetail, HearingDetail_DAO>(key);
		}

		/// <summary>
		/// Save a hearing detail instance
		/// </summary>
		/// <param name="hearingDetail"></param>
		public void SaveHearingDetail(IHearingDetail hearingDetail)
		{
			base.Save<IHearingDetail, HearingDetail_DAO>(hearingDetail);
		}

		#endregion Court / Hearing Details

		#region Dates

		/// <summary>
		/// Get the 60 days date
		/// This is x2 data so not exposed on the business model
		/// otherwise this will get run every time we load a DC case
		/// </summary>
		/// <param name="dcKey"></param>
		/// <returns></returns>
		public DateTime? Get60DaysDate(int dcKey)
		{
			IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

			//GetInstancesForGenericKey
			IList<SAHL.Common.X2.BusinessModel.Interfaces.IInstance> iList = x2Repo.GetInstancesForGenericKey(dcKey, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling);

			if (iList == null || iList.Count == 0)
				return null;

			//GetRelatedInstances
			DataTable i = x2Repo.GetRelatedInstances(iList[0].ID);

			//GetScheduledActivities
			DataTable a = x2Repo.GetScheduledActivities(i);

			DateTime? dt = null;

			foreach (DataRow dr in a.Rows)
			{
				//Type = 4 is a timer
				if (String.Compare(dr["Type"].ToString(), "4", true) == 0
					&& String.Compare(dr["Name"].ToString(), "60 days", true) == 0
					&& (!dt.HasValue || dt.Value > Convert.ToDateTime(dr["Time"].ToString())))
				{
					dt = Convert.ToDateTime(dr["Time"].ToString());
				}
			}

			return dt;
		}

		#endregion Dates

		#region Loan Details Types

		/// <summary>
		/// If a specific detail type is a added then raise an External Activity that willl cause a flag to fire in
		/// the Debt Counselling Workflow Map
		/// </summary>
		/// <param name="accountKey"></param>
		/// <param name="detailTypeKey"></param>
		public void RaiseActiveExternalActivityForAddDetailType(int accountKey, int detailTypeKey)
		{
            var account = accountRepository.GetAccountByKey(accountKey);
            if (!account.UnderDebtCounselling)
            {
                return;
            }

			string externalActivityName = string.Empty;
			switch (detailTypeKey)
			{
				case (int)DetailTypes.SequestrationOrLiquidation:
				case (int)DetailTypes.EstateLateSecured:
				case (int)DetailTypes.EstateLateUnsecured:
				case (int)DetailTypes.SuretyDeceased:
					//continue as normal for now
					break;
				case (int)DetailTypes.UnderCancellation:
					{
						externalActivityName = SAHL.Common.Constants.WorkFlowExternalActivity.UnderCancellation;
						break;
					}
				case (int)DetailTypes.CancellationRegistered:
					{
						externalActivityName = SAHL.Common.Constants.WorkFlowExternalActivity.CancellationRegistered;
						break;
					}
				default:
					{
						return;
					}
			}
			if (externalActivityName != string.Empty)
			{
				IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
				IInstance instance = x2Repo.GetDebtCousellingInstanceByAccountKey(accountKey);
				if (instance != null)
				{
					x2Repo.CreateAndSaveActiveExternalActivity(externalActivityName, instance.ID, SAHL.Common.Constants.WorkFlowName.DebtCounselling, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling, string.Empty);
				}
			}
			else
			{
				string eStageName = string.Empty, eFolderID = string.Empty;
				IADUser eADUserID;
				GetEworkDataForLossControlCase(accountKey, out eStageName, out eFolderID, out eADUserID);
				if (eFolderID != string.Empty)
				{
					string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "EWorkRaiseFlagGoToAllocator");

					ParameterCollection paramCollection = new ParameterCollection();

					SqlParameter folderParam = new SqlParameter("@efolderID", SqlDbType.NVarChar, 62);
					SqlParameter flagDataParam = new SqlParameter("@eFlagData", SqlDbType.NVarChar, 200);

					folderParam.Value = eFolderID;
					flagDataParam.Value = string.Format("GlobalGoToLossControlAllocator From Halo {0}", eFolderID);

					paramCollection.Add(folderParam);
					paramCollection.Add(flagDataParam);

					ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), paramCollection);
				}
			}
		}

		/// <summary>
		/// If a specific detail type is a removed then raise an External Activity that willl cause a flag to fire in
		/// the Debt Counselling Workflow Map
		/// </summary>
		/// <param name="accountKey"></param>
		/// <param name="detailTypeKey"></param>
		public void RaiseActiveExternalActivityForDeleteDetailType(int accountKey, int detailTypeKey)
		{
            var account = accountRepository.GetAccountByKey(accountKey);
            if (!account.UnderDebtCounselling)
            {
                return;
            }

			string externalActivityName = string.Empty;
			switch (detailTypeKey)
			{
				case (int)DetailTypes.UnderCancellation:
					{
						externalActivityName = SAHL.Common.Constants.WorkFlowExternalActivity.UnderCancellationRemoved;
						break;
					}
				default:
					{
						return;
					}
			}

			IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
			IInstance instance = x2Repo.GetDebtCousellingInstanceByAccountKey(accountKey);
			if (instance != null)
			{
				x2Repo.CreateAndSaveActiveExternalActivity(externalActivityName, instance.ID, SAHL.Common.Constants.WorkFlowName.DebtCounselling, SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling, string.Empty);
			}
		}

		#endregion Loan Details Types

		public void SendNotification(IDebtCounselling debtCounselling)
		{
			ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
			DataTable dt = debtCounselling.Account.GetAccountRoleNotificationByTypeAndDecription(ReasonTypes.DebtCounsellingNotification, ReasonDescriptions.NotificationofDeath);
			foreach (DataRow dr in dt.Rows)
			{
				if (Convert.ToBoolean(dr[1])) // notification of death exists, send the mail to
				{
					ICorrespondenceTemplate template = commonRepo.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.DeceasedEstatesDebtCounsellingArchived);

					//send internal mail
					string accKey = debtCounselling.Account.Key.ToString();
					string subject = String.Format(template.Subject, accKey);
					string desc = String.Format(template.Template, accKey, debtCounselling.Account.GetLegalName(LegalNameFormat.InitialsOnlyNoSalutation));

					// now send the mail
					IMessageService messageService = ServiceFactory.GetService<IMessageService>();
					messageService.SendEmailInternal("HALO@SAHomeloans.com", template.DefaultEmail, null, null, subject, desc);
					break;
				}
			}
		}

        public List<IDebtCounselling> SearchDebtCounsellingCasesForAttorney(int legalEntityKey, string IDNumber, int? accountKey, string legalEntityName)
        {
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "SearchDebtCounsellingForAttorney");

            SimpleQuery<DebtCounselling_DAO> debtCounselling = new SimpleQuery<DebtCounselling_DAO>(QueryLanguage.Sql, string.Format(query, legalEntityKey, IDNumber ?? "", accountKey.Value, legalEntityName ?? ""));
            debtCounselling.AddSqlReturnDefinition(typeof(DebtCounselling_DAO), "dc");

            DebtCounselling_DAO[] res = debtCounselling.Execute();

            if (res == null || res.Length == 0)
                return new List<IDebtCounselling>();

            return res.Select(x => BMTM.GetMappedType<IDebtCounselling, DebtCounselling_DAO>(x)).ToList();
        }
    }
}