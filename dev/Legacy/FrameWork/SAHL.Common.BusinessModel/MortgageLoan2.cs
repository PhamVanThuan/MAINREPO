using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel
{
	public partial class MortgageLoan : FinancialService, IMortgageLoan
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="Rules"></param>
		public override void OnPopulateRules(List<string> Rules)
		{
			base.OnPopulateRules(Rules);
		}

		public void OnConstruction()
		{
			if (null == this.FinancialServiceType)
			{
				//ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
				//string FSGKey = Convert.ToString((int)SAHL.Common.Globals.FinancialServiceGroups.MortgageLoan);

				//IFinancialServiceType FST = LR.FinancialServiceGroups.ObjectDictionary[FSGKey].FinancialServiceTypes[0];
				//IDAOObject obj = FST as IDAOObject;

				//if (obj != null)
				//    this._DAO.FinancialServiceType = (FinancialServiceType_DAO)obj.GetDAOObject();
				//else
				//    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");

				this._DAO.FinancialServiceType = FinancialServiceType_DAO.Find((int)FinancialServiceTypes.VariableLoan);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public IBond GetLatestRegisteredBond()
		{
			if (this.Bonds == null || this.Bonds.Count == 0)
				return null;

			DateTime latestDate = new DateTime(1, 1, 1);
			IBond bond = this.Bonds[0];

			for (int i = 0; i < this.Bonds.Count; i++)
			{
				DateTime? date = this.Bonds[i].BondRegistrationDate;

				if (date != null && date > latestDate)
				{
					latestDate = (DateTime)date;
					bond = this.Bonds[i];
				}
			}

			return bond;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public DateTime? GetLatestBondRegistrationDate()
		{
			IBond bond = GetLatestRegisteredBond();

			if (bond != null)
				return bond.BondRegistrationDate;

			return null;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public double SumBondRegistrationAmounts()
		{
			if (this.Bonds == null || this.Bonds.Count == 0)
				return 0;

			double sum = 0;

			for (int i = 0; i < this.Bonds.Count; i++)
			{
				sum += (double)this.Bonds[i].BondRegistrationAmount;
			}

			return sum;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public double SumBondLoanAgreementAmounts()
		{
			if (this.Bonds == null || this.Bonds.Count == 0)
				return 0;

			double sum = 0D;

			for (int i = 0; i < this.Bonds.Count; i++)
			{
				sum += (double)this.Bonds[i].BondLoanAgreementAmount;
			}

			return sum;
		}

		//public IValuation GetLatestPropertyValuation()
		//{
		//    if (this.Properties.Count > 1)
		//        throw new Exception("There is more than one Property attached to this MortgageLoan");

		//    string HQL = "FROM Valuation_DAO v WHERE v.Property.Key = ? Order By v.ValuationDate desc";
		//    SimpleQuery<Valuation_DAO> q = new SimpleQuery<Valuation_DAO>(HQL, this.Properties[0].Key);
		//    Valuation_DAO[] v = q.Execute();

		//    if (v == null || v.Length == 0)
		//        return null;

		//    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
		//    return BMTM.GetMappedType<IValuation, Valuation_DAO>(v[0]);
		//}

		//public DateTime? GetLatestPropertyValuationDate()
		//{
		//    IValuation v = GetLatestPropertyValuation();

		//    if (v == null)
		//        return null;

		//    return v.ValuationDate;
		//}

		//public double GetLatestPropertyValuationAmount()
		//{
		//    IValuation v = GetLatestPropertyValuation();

		//    if (v == null)
		//        return 0;

		//    return v.ValuationAmount.HasValue ? v.ValuationAmount.Value : 0;
		//}

		/// <summary>
		///  Private, QUICK method to get valuation details.
		/// </summary>
		/// <returns></returns>
		private DataTable GetActiveValuationInternal()
		{
			DataTable dt = new DataTable();

			string query = UIStatementRepository.GetStatement("FinancialService", "GetActiveValuationByFinancialServiceKey");

			// Create a collection
			ParameterCollection parameters = new ParameterCollection();

			// Add the required parameters
			parameters.Add(new SqlParameter("@fsKey", this.Key));

			// Execute Query
			DataSet dsResults = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
			if (dsResults != null && dsResults.Tables.Count > 0)
				dt = dsResults.Tables[0];

			return dt;
		}

		/// <summary>
		/// Gets the valuation with IsActive = true.
		/// This method should be used instead of GetLatestPropertyValuation()
		/// </summary>
		/// <returns>The active valuation or null if not found</returns>
		public IValuation GetActiveValuation()
		{
			DataTable dt = GetActiveValuationInternal();
			if (dt.Rows.Count == 0)
				return null;
			else
			{
				int key = Convert.ToInt32(dt.Rows[0]["ValuationKey"]);
				IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
				return propRepo.GetValuationByKey(key);
			}
		}

		public DateTime? GetActiveValuationDate()
		{
			DataTable dt = GetActiveValuationInternal();

			if (dt.Rows.Count == 0)
				return null;
			else
			{
				DataRow row = dt.Rows[0];
				if (row.IsNull("ValuationDate"))
					return new DateTime?();
				else
					return Convert.ToDateTime(row["ValuationDate"]);
			}
		}

		public double GetActiveValuationAmount()
		{
			DataTable dt = GetActiveValuationInternal();

			if (dt.Rows.Count == 0)
				return 0D;
			else
			{
				DataRow row = dt.Rows[0];
				if (row.IsNull("ValuationAmount"))
					return 0D;
				else
					return Convert.ToDouble(row["ValuationAmount"]);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public IEventList<IMargin> GetLoanAttributeBasedMargins()
		{
			//Add the current NewBusiness CM to return those link Rates also, so that the account can be re-priced to the current CM
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

			ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();
			var isAlphaHousing = Account.Details.Any(x => x.DetailType.Key == (int)DetailTypes.AlphaHousing);
			IReadOnlyEventList<ICreditCriteria> creditCriteriaList = ccRepo.GetCreditCriteria(spc.DomainMessages, this.Account.OriginationSource.Key, this.Account.Product.Key, (int)MortgageLoanPurposes.Newpurchase, (int)EmploymentTypes.Salaried, 0D);

			var creditCriteria = creditCriteriaList.FirstOrDefault();
			var newCreditMatrixKey = creditCriteria == null ? -1 : creditCriteria.CreditMatrix.Key;

			//Get Margins from CM for account
			var sqlQuery = String.Format(@"select distinct 
													m.MarginKey,  
													m.Value,
													m.Description
											from
													[2am].dbo.CreditCriteria cc 
											join	[2am].dbo.CreditMatrix cm on cm.CreditMatrixKey = cc.CreditMatrixKey
											join	[2am].dbo.CreditCriteriaAttribute cca on cc.CreditCriteriaKey = cca.CreditCriteriaKey 
											join	[2am].dbo.Margin m on cc.MarginKey = m.MarginKey
											where   cca.CreditCriteriaAttributeTypeKey = {0}
											and cm.CreditMatrixKey in ({1}, {2})", isAlphaHousing ? (int)CreditCriteriaAttributeTypes.FurtherLendingAlphaHousing : (int)CreditCriteriaAttributeTypes.FurtherLendingNonAlphaHousing, newCreditMatrixKey, this.CreditMatrix.Key);
			var margins = CastleTransactionsServiceHelper.Many<IMargin>(QueryLanguages.Sql, sqlQuery, "m", Databases.TwoAM);

			//Get the current Margin in case we need to add it to the list
			IMargin currentMargin = this.Balance.LoanBalance.RateConfiguration.Margin;
			int currentMarginKey = currentMargin != null ? currentMargin.Key : -1;
			bool exists = false;

			//Create a list of CM Margins
			List<IMargin> listOfMargins = new List<IMargin>();
			foreach (IMargin margin in margins)
			{
				if (currentMarginKey == margin.Key)
					exists = true;

				listOfMargins.Add(margin);
			}

			if (!exists)
				listOfMargins.Add(currentMargin);

			listOfMargins.Sort(delegate(IMargin m1, IMargin m2) { return m1.Value.CompareTo(m2.Value); });

			return new EventList<IMargin>(listOfMargins);
		}

		public IEventList<IMargin> GetAllMargins()
		{
			SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
			int newCreditMatrixKey = 0;
			ICreditCriteriaRepository ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();

			IReadOnlyEventList<ICreditCriteria> creditCriteriaList = ccRepo.GetCreditCriteria(principalCache.DomainMessages, this.Account.OriginationSource.Key, this.Account.Product.Key, (int)MortgageLoanPurposes.Newpurchase, (int)EmploymentTypes.Salaried, 0D);

			newCreditMatrixKey = creditCriteriaList.First().CreditMatrix.Key;

			string sql = String.Format(@"select distinct 
											  margin.*
										from 
											  [2am].dbo.CreditCriteria cc 
										join  [2am].dbo.Margin margin  on cc.MarginKey=margin.MarginKey 
										where cc.CreditMatrixKey in ({0} , {1})", this.CreditMatrix.Key, newCreditMatrixKey);

			var margins = CastleTransactionsServiceHelper.Many<IMargin>(SAHL.Common.Globals.QueryLanguages.Sql, sql, "margin", SAHL.Common.Globals.Databases.TwoAM);

			//if the current margin does not exist, add it
			if (!margins.Any(x => x.Key == this.Balance.LoanBalance.RateConfiguration.Margin.Key))
			{
				margins.Add(this.Balance.LoanBalance.RateConfiguration.Margin);
			}

			return new EventList<IMargin>(margins.OrderBy(x => x.Value));
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public bool HasInterestOnly()
		{
			bool bInterestOnly = false;
			foreach (IFinancialAdjustment fa in this.FinancialAdjustments)
			{
				if (fa.FinancialAdjustmentType.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentTypes.InterestOnly)
					&& fa.FinancialAdjustmentStatus.Key == Convert.ToInt32(SAHL.Common.Globals.FinancialAdjustmentStatuses.Active))
				{
					bInterestOnly = true;
					break;
				}
			}
			return bInterestOnly;
		}

		protected void OnMortgageLoanInfoes_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoanInfoes_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnBonds_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceConditions_BeforeAddOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceConditions_BeforeRemoveOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceRecurringTransactions_BeforeAddOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceRecurringTransactions_BeforeRemoveOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnHOCs_BeforeAddOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnHOCs_BeforeRemoveOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnLifePolicies_BeforeAddOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnLifePolicies_BeforeRemoveOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoans_BeforeAddOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoans_BeforeRemoveOnBonds_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnProperties_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnProperties_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoanInfoes_AfterAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoanInfoes_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnBonds_AfterAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceConditions_AfterAddOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceConditions_AfterRemoveOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceRecurringTransactions_AfterAddOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnFinancialServiceRecurringTransactions_AfterRemoveOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnHOCs_AfterAddOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnHOCs_AfterRemoveOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnLifePolicies_AfterAddOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnLifePolicies_AfterRemoveOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoans_AfterAddOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnMortgageLoans_AfterRemoveOnBonds_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnProperties_AfterAdd(ICancelDomainArgs args, object Item)
		{
		}

		protected void OnProperties_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		protected override void OnFinancialServiceBankAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
		{
			if (Item is FinancialServiceBankAccount)
			{
				//The current Debit Orders (For a mortgage loan) are never to be deleted.
				FinancialServiceBankAccount financialServiceBankAccount = Item as FinancialServiceBankAccount;
				if (financialServiceBankAccount.GeneralStatus.Key == (int)GeneralStatuses.Active
					&& financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment)
				{
					SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

					spc.DomainMessages.Add(new Error("The current Debit Orders are never to be deleted.", "The current Debit Orders are never to be deleted."));
					args.Cancel = true;
				}

				if (financialServiceBankAccount.GeneralStatus.Key == (int)GeneralStatuses.Active
					&& financialServiceBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DirectPayment)
				{
					SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

					spc.DomainMessages.Add(new Error("The current Debit Orders are never to be deleted.", "The current Debit Orders are never to be deleted."));
					args.Cancel = true;
				}
			}

			//If we are cancelling a request, throw an error so that we know it has been canccelled.
			//This needs to be done after all items have been validated so that multiple/all messages are
			//reported to the UI
			if (args.Cancel)
				throw new SAHL.Common.Exceptions.DomainValidationException();
		}

		protected override void OnFinancialServiceConditions_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		/// <summary>
		/// Remaining Installments
		/// </summary>
		public int RemainingInstallments
		{
			get
			{
				return this.Balance.LoanBalance.RemainingInstalments;
			}
		}

		/// <summary>
		/// Current Balance
		/// </summary>
		public double CurrentBalance
		{
			get
			{
				return this.Balance.Amount;
			}
		}

		/// <summary>
		/// Arrear Balance
		/// </summary>
		public double ArrearBalance
		{
			get
			{
				//get arrears balances
				IFinancialService parentFinancialService = this.FinancialServiceParent;
				if (parentFinancialService == null)
				{
					parentFinancialService = this;
				}
				var arrearBalance = (from fs in parentFinancialService.FinancialServices
									 where fs.Account.Key == this.Account.Key &&
										   fs.AccountStatus.Key == (int)AccountStatuses.Open &&
										   fs.Balance.BalanceType.Key == (int)BalanceTypes.Arrear
									 select fs.Balance.Amount).Sum();
				return arrearBalance;
			}
		}

		/// <summary>
		/// Interest Rate
		/// </summary>
		public double InterestRate
		{
			get
			{
				return this.Balance.LoanBalance.InterestRate;
			}
		}

		/// <summary>
		/// Initial Instalments
		/// </summary>
		public int InitialInstallments
		{
			get
			{
				return this.Balance.LoanBalance.Term;
			}
		}

		/// <summary>
		/// Reset Configuration
		/// </summary>
		public IResetConfiguration ResetConfiguration
		{
			get
			{
				return this.Balance.LoanBalance.ResetConfiguration;
			}
		}

		/// <summary>
		/// Rate Configuration
		/// </summary>
		public IRateConfiguration RateConfiguration
		{
			get
			{
				return this.Balance.LoanBalance.RateConfiguration;
			}
		}

		/// <summary>
		/// Active Market Rate
		/// </summary>
		public double ActiveMarketRate
		{
			get
			{
				return this.Balance.LoanBalance.ActiveMarketRate;
			}
		}

		/// <summary>
		/// Accrued Interest MTD
		/// </summary>
		public double? AccruedInterestMTD
		{
			get
			{
				return this.Balance.LoanBalance.MTDInterest;
			}
		}

		/// <summary>
		/// Rate Adjustment
		/// </summary>
		public double RateAdjustment
		{
			get
			{
				return this.Balance.LoanBalance.RateAdjustment;
			}
		}

		/// <summary>
		///
		/// </summary>
		public ICAP CAP
		{
			get
			{
				string hql = @"select c from CAP_DAO c where c.FinancialServiceAttribute.FinancialService.Key = ? and c.FinancialServiceAttribute.GeneralStatus.Key = ? and c.FinancialServiceAttribute.FinancialServiceAttributeType.Key = ?";
				SimpleQuery<CAP_DAO> query = new SimpleQuery<CAP_DAO>(hql, this.Key, (int)GeneralStatuses.Active, (int)FinancialServiceAttributeTypes.CAP);
				CAP_DAO[] result = query.Execute();
				if (result != null && result.Length > 0)
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICAP, CAP_DAO>(result[0]);
				}
				return null;
			}
		}

		public IInterestOnly InterestOnly
		{
			get
			{
				string hql = @"select i from InterestOnly_DAO i where i.FinancialServiceAttribute.FinancialService.Key = ? and i.FinancialServiceAttribute.GeneralStatus.Key = ? and i.FinancialServiceAttribute.FinancialServiceAttributeType.Key = ?";
				SimpleQuery<InterestOnly_DAO> query = new SimpleQuery<InterestOnly_DAO>(hql, this.Key, (int)GeneralStatuses.Active, (int)FinancialServiceAttributeTypes.InterestOnly);
				InterestOnly_DAO[] result = query.Execute();
				if (result != null && result.Length > 0)
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IInterestOnly, InterestOnly_DAO>(result[0]);
				}
				return null;
			}
		}
	}
}