using Automation.DataAccess.Interfaces;
using Automation.DataModels;
using Common.Constants;
using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        private IDataContext dataContext;

        public _2AMDataHelper()
        {
            dataContext = new DataContext();
        }

        /// <summary>
        /// Gets the first open account that has a specific detail type stored against it.
        /// </summary>
        /// <param name = "detailTypeKey">DetailType</param>
        /// <returns>AccountKey</returns>
        public QueryResults GetOpenAccountByDetailType(int detailTypeKey)
        {
            SQLStatement statement = new SQLStatement();
            string query =
                String.Format(
                    @"select top 1 a.AccountKey from
                            [2am].dbo.detail d (NOLOCK)
							join [2am].dbo.Account a on d.AccountKey=a.AccountKey
							join [2am].dbo.FinancialService fs on a.AccountKey=fs.AccountKey and fs.parentFinancialServiceKey is null
							where DetailTypeKey = {0} and fs.AccountStatusKey = 1 and a.AccountStatusKey = 1
							and RRR_ProductKey in (1,9,4,5) and RRR_OriginationSourceKey = 1
							order by DetailDate desc",
                    detailTypeKey);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///  Returns a test account for the further lending LTP test
        /// </summary>
        /// <returns></returns>
        public int GetAccountForLTPTest()
        {
            SQLStatement statement = new SQLStatement();
            string query =
                @"select top 1 a.AccountKey, (b.amount/oml.purchaseprice) as LTP from [2AM].dbo.account a
                    join [2AM].dbo.financialservice fs
	                    on a.accountkey=fs.accountkey
	                    and fs.parentfinancialservicekey IS NULL
                    join [2AM].fin.mortgageloan ml
	                    on fs.financialservicekey=ml.financialservicekey
                    join [2AM].fin.balance b
	                    on ml.financialservicekey=b.financialservicekey
                    join [2AM].dbo.offer o
	                    on a.accountkey=o.accountkey
                    join [2AM].dbo.offermortgageloan oml
	                    on o.offerkey=oml.offerkey
                    left join [2AM].dbo.Offer fl on fs.accountKey = fl.accountKey
                        and fl.offertypekey in (2,3,4)
                        and fl.offerstatusKey = 1
                    where datediff(mm,a.opendate,getdate()) < 12
                    and (b.amount/oml.purchaseprice) > 0.85
                    and (b.amount/oml.purchaseprice) < 0.88
                    and ml.mortgageloanpurposekey=3
                    and fl.offerkey is null
                    order by newid()";
            statement.StatementString = query;
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return (results.Rows(0).Column("AccountKey").GetValueAs<int>());
        }

        /// <summary>
        /// This will retrieve a column value from the Account table when provided with the AccountKey
        /// and the column to retrieve
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="columnName">Column to return</param>
        /// <returns>Account.*</returns>
        public string GetAccountColumn(int accountKey, string columnName)
        {
            var statement = new SQLStatement();
            string query = String.Format(@"select {0} from [2am].dbo.Account where AccountKey={1}", columnName, accountKey);
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(columnName).Value;
        }

        /// <summary>
        /// This will retrieve the first Account provided an AccountStatus
        /// </summary>
        /// <param name="accountStatus"></param>
        /// <returns></returns>
        public string GetAccountByAccountStatus(AccountStatusEnum accountStatus)
        {
            SQLStatement statement = new SQLStatement();
            string query =
                String.Format(
                    @"select top 01 AccountKey
						from [2am].dbo.Account
						where
							AccountStatusKey={0}", (int)accountStatus);

            statement.StatementString = query;
            QueryResults r = dataContext.ExecuteSQLScalar(statement);
            return r.SQLScalarValue;
        }

        /// <summary>
        /// This methods only returns Natural Persons with an idnumber
        /// </summary>
        /// <returns></returns>
        public QueryResults GetPersonalLoanAccount()
        {
            var statement = new SQLStatement();
            string query =
                string.Format(@"select top 01 a.accountKey
                    from [2am].[dbo].[Account] a
                    left join [2am].debtcounselling.debtcounselling dc on a.accountKey = dc.accountKey
                        and dc.debtCounsellingStatusKey = 1
                    where a.AccountStatusKey = 1
                    and a.rrr_productKey = 12
                    and a.[RRR_OriginationSourceKey] = 1
                    and dc.debtcounsellingkey is null
                   ");
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This methods only returns Natural Persons with an idnumber
        /// </summary>
        /// <returns></returns>
        public QueryResults GetRandomVariableLoanAccountByMainApplicantCount(int roleCount, int recordCount, AccountStatusEnum status)
        {
            var statement = new SQLStatement();
            string query =
                string.Format(@"with accounts as (
                    select top 20 a.accountKey
                    from [2am].[dbo].[Account] a (nolock)
                    join [2am].[dbo].[Role] r (nolock) on r.AccountKey = a.AccountKey
                    join [2am].[dbo].[LegalEntity] le (nolock) on r.LegalEntityKey = le.LegalEntityKey
                    and le.LegalEntityTypeKey=2 and len(idnumber) = 13
                    where a.AccountStatusKey = {0} and r.RoleTypeKey = 2
                    and a.rrr_productKey in (1,5,6,9,10,11) and a.[RRR_OriginationSourceKey] = 1
                    group by a.AccountKey
                    having count(r.AccountRoleKey) = {1}
                    order by newid()
                    )
                    select top {2} r.accountKey, le.legalEntityKey, le.idnumber
                    from [2am].[dbo].[role] r
                    join [2am].[dbo].[LegalEntity] le (nolock) on r.legalEntityKey=le.legalEntityKey
                    --We still have very old accounts without offers
                    join [2am].[dbo].offer o on r.accountkey=o.accountkey
                    where r.accountkey in (select accountKey from accounts)
                    ", (int)status, roleCount, recordCount);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetLatestOpenAccountWithOneMainApplicantAndOneEmploymentRecord()
        {
            SQLStatement statement = new SQLStatement();
            string query =
                @"select top 1 a.AccountKey from [2am]..Account a (nolock)
					join [2am]..[Role] r (nolock) on r.AccountKey = a.AccountKey
					join [2am]..[LegalEntity] le (nolock) on le.LegalEntityKey = r.LegalEntityKey
					join [2am]..[Employment] e (nolock) on e.LegalEntityKey = r.LegalEntityKey
					where a.AccountStatusKey = 1 and r.RoleTypeKey = 2
					and le.Legalentitytypekey = 2
					group by a.AccountKey
					having count(r.AccountRoleKey) = 1 and count(e.LegalEntityKey) = 1";
            statement.StatementString = query;
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return (results.Rows(0).Column("AccountKey").GetValueAs<int>());
        }

        /// <summary>
        /// Tis will get a list of all the open mortgage accounts that the legalentity plays a role in.
        /// </summary>
        /// <param name="legalentityKey">Optional. Default is 0</param>
        /// <param name="legalentityId">Optional. Default is Empty String</param>
        /// <returns>list of accounts</returns>
        public List<int> GetMortgageAccountsByLegalEntity(ref int legalentityKey, ref string legalentityId)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(legalentityId))
                filter = string.Format("and le.IDNumber='{0}'", legalentityId);
            else if (legalentityKey > 0)
                filter = string.Format("and le.legalentityKey={0}", legalentityKey);
            string query =
                String.Format(
                  @"select distinct	a.AccountKey,le.LegalEntityKey,le.IDNumber
					from [2am].dbo.Account a
					inner join [2am].dbo.Role r on a.AccountKey = r.AccountKey
					inner join [2am].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
					where a.rrr_productkey in (1,2,5,6,9,11)
                    {0}
                    and a.AccountStatusKey=1", filter);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            legalentityId = results.Rows(0).Column("IDNumber").Value;
            legalentityKey = results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
            List<int> list = results.GetColumnValueList<int>("AccountKey");
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public QueryResults GetDebitOrderByAccountKey(int accountKey)
        {
            string q =
                string.Format(@"select top 1 financialservicebankaccountkey, debitorderday from [2am].dbo.account a with (nolock)
								join [2am].dbo.financialService fs with (nolock)
								on a.accountKey=fs.accountKey
								join [2am].dbo.financialServiceBankAccount b with (nolock)
								on fs.financialServiceKey=b.financialServiceKey
								where a.accountkey={0}
								order by financialservicebankaccountkey desc", accountKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialServiceBankAccountKey"></param>
        /// <returns></returns>
        public int GetDebitOrderDayFromFutureDatedChange(int financialServiceBankAccountKey)
        {
            string q =
                string.Format(@"select value from [2am].dbo.FutureDatedChange f with (nolock)
								join [2am].dbo.FutureDatedChangeDetail d with (nolock)
								on f.FutureDatedChangeKey=d.FutureDatedChangeKey
								where d.referenceKey={0}
								and ColumnName='DebitOrderDay' and FutureDatedChangeTypeKey=2 and effectivedate > getdate()
								order by 1 desc", financialServiceBankAccountKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            var r = dataContext.ExecuteSQLQuery(s);
            int debitOrderDay = r.HasResults ? r.Rows(0).Column("value").GetValueAs<int>() : 0;
            return debitOrderDay;
        }

        /// <summary>
        /// This will get the account, financialservices and financial adjustments.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public QueryResults GetAccountFinancialServiceFinancialAdjustments(int accountkey)
        {
            string q =
               string.Format(@"select top 1 * from [2am].dbo.account a
                join [2am].dbo.financialservice fs with (nolock) on a.accountkey = fs.accountkey
                    and a.accountstatuskey in (1, 4, 5)
                    and fs.parentFinancialServiceKey is null
                    and fs.accountstatuskey = 1
                join [2am].fin.mortgageloan ml with (nolock) on fs.financialservicekey = ml.financialservicekey
                join [2am].fin.LoanBalance lb with (nolock) on ml.financialServiceKey = lb.financialServiceKey
                join [2am].dbo.rateconfiguration rc with (nolock) on lb.rateconfigurationkey = rc.rateconfigurationkey
                join [2am].dbo.resetconfiguration rec with (nolock) on lb.resetconfigurationkey = rec.resetconfigurationkey
                join [2am].dbo.margin m with (nolock) on rc.marginkey = m.marginkey
                join [2am].dbo.valuation v with (nolock) on ml.propertykey = v.propertykey
                    and v.isactive = 1
                left join fin.FinancialAdjustment fa with (nolock) on fa.financialservicekey = fs.financialservicekey
                    and fa.FinancialAdjustmentStatusKey = 1
                left join fin.FinancialAdjustmentTypeSource fats with (nolock) on fa.FinancialAdjustmentTypeKey=fats.FinancialAdjustmentTypeKey
                    and fa.FinancialAdjustmentSourceKey=fats.FinancialAdjustmentSourceKey
                where a.accountKey= {0}", accountkey);
            var statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the financial service payment type for the acount
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public int GetFinancialServicePaymentTypeByAccountKey(int accountKey)
        {
            var query = string.Format(@"
                        select fsba.financialServicePaymentTypeKey
						from [2am].dbo.financialService fs
                        join [2am].dbo.financialServiceBankAccount fsba on fs.financialservicekey=fsba.financialservicekey
						where fs.accountkey={0} and fsba.generalstatuskey=1", accountKey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// This will get the first valid account that is not under debtcounselling
        /// </summary>
        /// <returns></returns>
        public int GetAccountNotUnderDebtCounselling()
        {
            var query =
                    @"select top 1 * from dbo.account a
                    inner join dbo.financialservice fs
                    on a.accountkey = fs.accountkey
                    and fs.accountstatuskey = 1 and fs.parentFinancialServiceKey is null
                    left join debtcounselling.debtcounselling dc
                    on dc.accountkey = a.accountkey
                    where a.rrr_productkey in (1,2,5,9,11)  and a.accountstatuskey=1
                    and dc.accountkey is null";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return Int32.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Fetches the accounts financial service record for the specified financial service type
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="financialServiceTypeKey">financialServiceTypeKey</param>
        /// <returns></returns>
        public QueryResults GetOpenFinancialServiceRecordByType(int accountKey, FinancialServiceTypeEnum financialServiceTypeKey)
        {
            string query =
                string.Format(@"select * from [2AM].dbo.financialService
								where accountKey={0} and accountStatusKey=1 and financialServiceTypeKey={1}", accountKey, (int)financialServiceTypeKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns open related accounts by their product type
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="product">Product</param>
        /// <returns></returns>
        public QueryResults GetOpenRelatedAccountsByProductKey(int accountKey, ProductEnum product)
        {
            string query =
                string.Format(@"select *, a.accountKey as RelatedAccountKey
                                            from [2am].dbo.Account a
                                            where parentAccountKey={0}
                                            and a.accountStatusKey=1 and rrr_productKey = {1}", accountKey, (int)product);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This query returns an account with multiple open further lending offers where at least one of the offers is in the Manage Application state.
        /// </summary>
        /// <returns></returns>
        public QueryResults ReassignMultipleApps()
        {
            string query =
                String.Format(@"declare @accountkey int;
                            with accounts as (
                            select accountkey from [2am]..offer o
                            where offertypekey in (2,3,4)
                            and offerstatuskey=1
                            group by accountkey
                            having count(o.offerkey) > 1
                            )

                            select top 1 @accountkey = o.accountKey
                            from accounts a
                            join offer o on a.accountkey=o.accountkey and offertypekey in (2,3,4) and offerstatuskey=1
                            join x2.x2data.application_management am on o.offerkey=am.applicationkey
                            join x2.x2.instance i on am.instanceid=i.id
                            join x2.x2.state s on i.stateid=s.id
                            where s.name='Manage Application'
                            order by o.offerTypeKey asc

                            select o.offerkey, o.accountkey, s.name
                            from [2am]..offer o
                            left join x2.x2data.application_management am on o.offerkey=am.applicationkey
                            left join x2.x2.instance i on am.instanceid=i.id
                            left join x2.x2.state s on i.stateid=s.id
                            where accountkey=@accountkey and o.offertypekey in (2,3,4)
                            and offerstatuskey=1 and parentinstanceid is null");
            SQLStatement s = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Gets a variable loan when provided when provided with the account number
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetAccountByKeySQL(int accountKey)
        {
            string query = string.Format(@"
                declare @productKey int
                declare @financialServiceGroupKey int
                set @financialServiceGroupKey = 1
                select @ProductKey = rrr_productKey from [2am].dbo.Account where accountKey = {0}
                if @ProductKey = 13
					begin
						set @financialServiceGroupKey = 4
					end
                select distinct acc1.accountKey, acc1.FixedPayment, acc1.AccountStatusKey, acc1.rrr_ProductKey as ProductKey, acc1.SPVKey, acc1.OpenDate,
                sum(acc.CurrentBalance) as CurrentBalance, sum(acc.ArrearBalance) as ArrearBalance, dbo.AccountLegalName(acc1.AccountKey,0) as AccountLegalName
                from (
                select
                a.accountKey,
                case when b.balanceTypeKey = 1 then sum(b.Amount) else 0.00 end as CurrentBalance,
                case when b.balanceTypeKey = 4 then sum(b.Amount) else 0.00 end as ArrearBalance
                from [2am].dbo.Account a
                inner join [2am].dbo.FinancialService fs on a.accountKey = fs.accountKey
                    and fs.accountStatusKey=1
                inner join [2am].dbo.FinancialServiceType fst on fs.financialServiceTypeKey = fst.financialServiceTypeKey
	                and fst.financialServiceGroupKey=@financialServiceGroupKey
                inner join [2am].fin.Balance b on fs.financialServiceKey = b.financialServiceKey
                where a.accountkey = {1}
                group by a.accountKey, b.balanceTypeKey
                ) as acc
                inner join [2am].dbo.account acc1 on acc.accountKey = acc1.accountKey
                group by acc1.accountKey, acc1.FixedPayment, acc1.AccountStatusKey, acc1.rrr_ProductKey, acc1.SPVKey, acc1.OpenDate", accountKey, accountKey);
            var account = dataContext.Query<Automation.DataModels.Account>(query).First();
            return account;
        }

        public Automation.DataModels.Account GetAccountByKeySimpleSQL(int accountKey)
        {
            string query = string.Format(@"select * from [2am].dbo.Account where accountKey = {0}", accountKey);
            var account = dataContext.Query<Automation.DataModels.Account>(query).First();
            return account;
        }

        /// <summary>
        /// Returns an indication if the account or any of the legal entities on the account are under debt counselling
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns>true = under debt counselling, false = not under debt counselling</returns>
        public bool fIsAccountUnderDebtCounselling(int accountKey)
        {
            string query = string.Format(@"select dbo.fIsAccountUnderDebtCounselling({0})", accountKey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<bool>();
        }

        /// <summary>
        /// Gets accounts with their current balance
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Account> GetAccountsWithCurrentBalance()
        {
            //revamp
            string query = @"select *
                from (
	                select distinct a.*, cast(bal.CurrentBalance as decimal(22,10)) CurrentBalance, case when s.subsidykey is not null then 1 else 0 end as SubsidyClient
	                from [2am].dbo.Account a
		                join [2am].dbo.vMortgageLoanCurrentBalance bal on a.accountKey = bal.accountKey
		                join [2am].dbo.Role r on a.AccountKey = r.AccountKey
			                and r.RoleTypeKey in (2,3)
		                join [2am].dbo.LegalEntityBankAccount leba on r.legalentitykey = leba.legalentitykey
			                and leba.GeneralStatusKey = 1
		                left join [2am].debtcounselling.debtCounselling dc on a.accountKey = dc.accountKey
			                and dc.debtCounsellingStatusKey = 1
                        left join AccountSubsidy sub on a.accountKey = sub.AccountKey
                        left join subsidy s on sub.subsidykey=s.subsidykey
	                        and s.generalStatusKey = 1
	                where a.accountStatusKey = 1 and dc.debtcounsellingkey is null
	                ) as x
                order by newid()";
            return dataContext.Query<Automation.DataModels.Account>(query);
        }

        public IEnumerable<Automation.DataModels.Account> GetRandomMortgageAccountFinancialServicesWithRateAdjustments()
        {
            return dataContext.Query<Automation.DataModels.Account, Automation.DataModels.LoanFinancialService, Automation.DataModels.Account>("test.GetRandomMortgageAccountFinancialServicesWithRateAdjustments", (acc, fs) =>
            {
                acc.FinancialService = fs;
                return acc;
            }, splitOn: "FinancialServiceKey", commandtype: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Inserts a new role against the account.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="legalEntityKey"></param>
        /// <param name="roleType"></param>
        /// <param name="status"></param>
        public void InsertRole(int accountKey, int legalEntityKey, RoleTypeEnum roleType, GeneralStatusEnum status)
        {
            string sql =
                string.Format(@"if not exists
                    (select 1 from [2am].dbo.Role where accountKey={0} and legalEntityKey = {1} and RoleTypeKey = {2} and GeneralStatusKey = {3})
                                begin
                                    insert into [2am].dbo.Role
                                    (LegalEntityKey,AccountKey,RoleTypeKey,GeneralStatusKey,StatusChangeDate)
                                    values
                                    ({4}, {5}, {6}, {7}, getdate())
                                end", accountKey, legalEntityKey, (int)roleType, (int)status, legalEntityKey, accountKey, (int)roleType,
                                    (int)status);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets accounts that have subsidies
        /// </summary>
        public IEnumerable<Automation.DataModels.Account> GetAccountWithSubsidyStopOrders(bool fixedPayments)
        {
            string filter = fixedPayments ? "and fixedPayment > 0" : "and fixedPayment = 0";
            string sql = string.Format(@"
                select top 20
                a.accountkey,
                max(a.fixedpayment) as fixedpayment,
                sum(s.stoporderamount) as SubsidyAmount
                into #accounts
                from [2am].dbo.Account a with (nolock)
                join [2am].dbo.AccountSubsidy sub  with (nolock) on a.accountKey = sub.accountKey
                join [2am].dbo.Subsidy s  with (nolock) on sub.subsidyKey = s.subsidyKey
                    and s.generalStatusKey=1
                where a.accountStatusKey = 1
                {0}
                group by a.accountkey

                select *, instalments.Instalment as TotalInstalment
                from #accounts a
                cross apply [2am].test.getTotalInstalmentForAccount(a.accountKey)
                as instalments

                drop table #accounts", filter);
            var accounts = dataContext.Query<Automation.DataModels.Account>(sql);
            return accounts;
        }

        /// <summary>
        /// Gets the total instalment for a mortgage loan account. includes the life, HOC and monthly service fees.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public double GetTotalInstalment(int accountKey)
        {
            string sql = string.Format(@"select * from [2am].test.[getTotalInstalmentForAccount]({0})", accountKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("Instalment").GetValueAs<double>();
        }

        /// <summary>
        /// Gets a mortgage account that have no recent correspondence and that only have open 1 HOC and Life account
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Account> GetUnusedUnsecuredLendingAccountForCorrespondenceTests()
        {
            var sql = @"select distinct top 2
	                            a.accountkey
                            from dbo.account a
	                            join financialService unsecuredFS
		                            on a.accountKey=unsecuredFS.accountKey
	                            left join dbo.correspondence c
		                            on a.accountKey = c.genericKey
                            where
	                            unsecuredFS.accountstatuskey = 1
	                            and a.rrr_productkey = 12
	                            and c.genericKey is null
	                            and a.accountkey not in
	                            (
		                            select accountkey from dbo.role where accountkey = a.accountkey
		                            group by accountkey having count(accountkey) > 1
		                            )
		                            and a.accountkey in (
		                            select accountkey from dbo.role as r
		                            inner join dbo.legalentity on r.legalentitykey = legalentity.legalentitykey
		                            where  len(legalentity.emailaddress) > 0
		                            and accountkey = unsecuredFS.accountkey
		                            group by accountkey
		                            having count(accountkey) = 1
	                            )";
            return dataContext.Query<Automation.DataModels.Account>(sql);
        }

        /// <summary>
        /// Gets an unsecured lending account that have no correspondence
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Account> GetUnusedMortgageAccountForCorrespondenceTests()
        {
            var sql = @"select distinct top 2 a.accountkey, a_life.accountkey as LifeAccountKey, a_HOC.accountKey as HOCAccountKey
                    from dbo.account a
                    join account a_life on a.accountkey=a_life.parentaccountkey
	                    and a_life.rrr_productkey = 4
	                    and a_life.accountstatuskey=1
                    join account a_HOC on a.accountkey=a_HOC.parentaccountkey
	                    and a_HOC.rrr_productkey = 3
	                    and a_HOC.accountstatuskey = 1
                    join financialService mortgageFS on a.accountKey=mortgageFS.accountKey
	                    and mortgageFS.accountstatuskey = 1
                    left join dbo.correspondence c on a.accountKey = c.genericKey
                    where
                    c.genericKey is null
                    and a.accountkey not in (
                    select accountkey from dbo.role where accountkey = a.accountkey
                    group by accountkey having count(accountkey) > 1
                    )
                    and a.accountkey in (
                    select accountkey from dbo.role as r
                    inner join dbo.legalentity on r.legalentitykey = legalentity.legalentitykey
                    where  len(legalentity.emailaddress) > 0
                    and accountkey = mortgageFS.accountkey
                    group by accountkey
                    having count(accountkey) = 1
                    )";
            return dataContext.Query<Automation.DataModels.Account>(sql);
        }

        public IEnumerable<Automation.DataModels.LifeAccountModel> GetLifePolicyAccounts(int noAssureLifeRoles, LifePolicyStatusEnum lifePolicyStatus, bool calculateToPremiumIncrease, bool calculateToPremiumDecrease)
        {
            var p = new DynamicParameters();
            p.Add("@noRoles", value: noAssureLifeRoles, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@lifePolicyStatus", value: (int)lifePolicyStatus, dbType: DbType.Int32, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.LifeAccountModel>("test.GetLifePolicyAccounts", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public IEnumerable<Automation.DataModels.AccountKeyWithIndicator> GetAccountMailingAddressInfo()
        {
            var statement = String.Format(@"
                        with mailinginfo as
                        (
	                        select top 1000 a.accountkey, le.legalentitykey, le.emailaddress, le.faxnumber, lea.addresskey
	                        from
	                            [2am].dbo.account a
	                                join [2am].dbo.role r on a.accountkey = r.accountkey
	                                join [2am].dbo.legalentity le on r.legalentitykey = le.legalentitykey
	                                join [2am].dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
	                        where
	                            a.accountstatuskey = 1
	                            and r.generalstatuskey = 1
	                            and le.legalentitystatuskey = 1
	                            and lea.generalstatuskey = 1
                                and le.emailaddress is not null
                                and le.emailaddress <> ''

                                and a.accountkey in (select distinct a.accountkey from [2am].dbo.account a
								    join [2am].dbo.role r on a.accountkey = r.accountkey where le.EmailAddress like '%@%'
								    group by a.accountkey having Count (r.LegalEntitykey) > 1)

								and a.accountkey in (select distinct a.accountkey from [2am].dbo.account a
								    join [2am].dbo.role r on a.accountkey = r.accountkey
								    join [2am].dbo.legalentity le on r.legalentitykey = le.legalentitykey
								    group by a.accountkey having Count (Distinct le.EmailAddress) > 1)

                                and a.accountkey in (select distinct a.accountkey from [2am].dbo.account a
								    join [2am].dbo.role r on a.accountkey = r.accountkey
								    join [2am].dbo.legalentity le on r.legalentitykey = le.legalentitykey
								    join [2am].dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
								    group by a.accountkey having Count (Distinct lea.AddressKey) > 1)
                        )
                        select top 1 AccountKey, 'Post' as 'Indicator', count(addresskey) as 'Count'
                        from mailinginfo
                        group by accountkey
                        having count(addresskey) > 1
                        union all
                        select top 1 accountkey, 'Email', count(emailaddress)
                        from mailinginfo
                        where isnull(emailaddress, '') <> ''
                        group by accountkey, legalentitykey
                        having count(emailaddress) > 1
                        union all
                        select top 1 accountkey, 'Fax', count(faxnumber)
                        from mailinginfo
                        where isnull(faxnumber, '') <> ''
                        group by accountkey, legalentitykey
                        having count(faxnumber) > 1");
            return dataContext.Query<Automation.DataModels.AccountKeyWithIndicator>(statement);
        }

        /// <summary>
        /// Gets all active interest only accounts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Account> GetInterestOnlyAccounts()
        {
            string query = @"select * from (
                                select distinct a.*, bal.currentbalance from [2AM].dbo.account a
                                join [2AM].dbo.vmortgageloancurrentbalance bal
                                    on a.accountkey = bal.accountkey
                                left join [2AM].debtcounselling.debtcounselling dc
                                    on a.accountkey=dc.accountkey
                                join [2AM].dbo.financialservice fs
                                    on a.accountkey=fs.accountkey
                                    and fs.parentfinancialservicekey IS NULL
                                    and fs.financialservicetypekey=1
                                join [2AM].fin.financialadjustment fa
                                    on fs.financialservicekey=fa.financialservicekey
                                join [2AM].fin.financialadjustmenttypesource fats
                                    on fa.financialadjustmenttypekey=fats.financialadjustmenttypekey
                                    and fa.financialadjustmentsourcekey=fats.financialadjustmentsourcekey
                                where a.accountstatuskey=1
                                and dc.debtcounsellingkey IS NULL
                                and fats.financialadjustmenttypesourcekey=5
                                and fa.financialadjustmentstatuskey=1
                                ) as accounts
                                order by newid()";
            return dataContext.Query<Automation.DataModels.Account>(query);
        }

        /// <summary>
        /// Updates the balance record on a financial service
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="financialServiceType"></param>
        /// <param name="newBalance"></param>
        public void UpdateFinancialServiceBalance(int accountKey, FinancialServiceTypeEnum financialServiceType, decimal newBalance)
        {
            string query = String.Format(@"declare @lifeFinancialservicekey int
                                             select top 01
                                                    @lifeFinancialservicekey = financialservice.financialservicekey
                                             from dbo.financialservice
                                             where financialservicetypekey = {0} and accountkey = {1}
                                             update b
                                             set amount = {2}
                                             from [2am].fin.Balance b
                                             where FinancialServiceKey = ISNULL(@lifeFinancialservicekey,1)",
                                                (int)financialServiceType, accountKey, newBalance);
            dataContext.Execute(query);
        }

        /// <summary>
        /// Gets an account for MarkNonPerforming tests
        /// </summary>
        /// <param name="isNonPerformingLoan">check for a FinancialAttribute record of Source: Suspended Interest and Type: Reversal Provision</param>
        /// <param name="hasFurtherLendingOffer">check for an open Further Lending Offer</param>
        /// <param name="hasDetails">check for Detail record of Type (11,180,275,299,592,227,581,582,583,584,590)</param>
        /// <param name="productKeys">filter loans by ProductKey</param>
        /// <returns>Account record</returns>
        public Automation.DataModels.Account GetAccountForNonPerformingLoanTests(bool isNonPerformingLoan, bool hasFurtherLendingOffer, bool hasDetails, params int[] productKeys)
        {
            string sIsNonPerformingLoan;
            string sHasFurtherLendingOffer;
            string sHasDetails;
            string sProductKeys = string.Empty;

            if (isNonPerformingLoan)
                sIsNonPerformingLoan = "is not null";
            else
                sIsNonPerformingLoan = "is null";

            if (hasFurtherLendingOffer)
                sHasFurtherLendingOffer = "is not null";
            else
                sHasFurtherLendingOffer = "is null";

            if (hasDetails)
                sHasDetails = "is not null";
            else
                sHasDetails = "is null";

            foreach (int productkey in productKeys)
                sProductKeys = sProductKeys + productkey.ToString() + ",";

            sProductKeys = sProductKeys.Remove(sProductKeys.Length - 1);

            string query = string.Format(@"
                    select top 1 a.* from [2am].dbo.Account a
                        join [2am].[dbo].FinancialService fs on a.AccountKey = fs.AccountKey
                            and fs.FinancialServiceTypeKey = 1
                        left join [2am].[fin].FinancialAdjustment fa on fs.financialservicekey = fa.financialservicekey
                            and fa.financialAdjustmentSourcekey = 2
                            and fa.FinancialAdjustmentStatusKey = 1
                        left join [2am].dbo.financialService fs_susp on fs.financialservicekey = fs_susp.parentfinancialservicekey
                            and fs_susp.financialservicetypekey = 7
                            and fs_susp.accountstatuskey = 1
                        left join fin.balance b_susp on fs_susp.financialservicekey = b_susp.financialservicekey
                            and b_susp.Amount > 1
                        left join [2am].dbo.Offer o on a.accountkey = o.accountkey
                            and o.OfferTypeKey in (2,3,4) and o.OfferStatusKey = 1
                        left join [2am].dbo.detail d on a.accountkey = d.accountkey
                            and detailtypekey in (11,180,275,299,592,227,581,582,583,584,590)
                    where fs.AccountStatusKey = 1
                        and a.rrr_ProductKey in ({0})
                        and fa.financialservicekey {1}
                        and b_susp.financialservicekey {1}
                        and o.Offerkey {2}
                        and d.detailkey {3}
                    order by newid()", sProductKeys, sIsNonPerformingLoan, sHasFurtherLendingOffer, sHasDetails);

            return dataContext.Query<Automation.DataModels.Account>(query).FirstOrDefault();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="financialServiceKey"></param>
        /// <returns></returns>
        public int GetAccountKeyByFinancialServiceKey(int financialServiceKey)
        {
            var sql = string.Format(@"select accountKey from dbo.FinancialService where financialServiceKey = {0}", financialServiceKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<int>();
        }

        public QueryResults GetMailingAddress(int accountKey)
        {
            var sql = string.Format(@"select * from dbo.MailingAddress where accountKey = {0}", accountKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }

        public void UpdateOpenDate(int accountkey, DateTime dateTime)
        {
            var sql = string.Format(@"update dbo.account
                                      set opendate = '{0}'
                                      where accountKey = {1}",
                                      dateTime.ToString(Formats.DateTimeFormatSQL), accountkey);

            var statement =
                new SQLStatement
                {
                    StatementString = sql
                };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public DateTime GetOpenDate(int accountkey)
        {
            var sql = string.Format(@"select opendate from dbo.account
                                      where accountKey = {0}", accountkey);
            var statement =
               new SQLStatement
               {
                   StatementString = sql
               };
            var dateStr = dataContext.ExecuteSQLQuery(statement).FirstOrDefault().Column("opendate").GetValueAs<string>();
            return DateTime.Parse(dateStr);
        }

        /// <summary>
        /// GEts idnumber from offerkey.
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        public string GetIDNumberforExternalRoleOnOffer(int offerkey)
        {
            var sql = string.Format(@"select top 1 le.idnumber from offer o
                join dbo.externalrole er
                on er.generickey=o.offerkey
                join legalentity le
                on le.legalentitykey=er.legalentitykey
                where o.offerkey={0}", offerkey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("IDNumber").Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <param name="emailAddress"></param>
        public void UpdateLegalEntityEmailAddress(int legalentitykey, string emailAddress)
        {
            var sql = string.Format(@"update dbo.legalentity
                                      set emailaddress = '{0}'
                                      where legalentitykey = {1}", emailAddress, legalentitykey);
            dataContext.ExecuteNonSQLQuery(new SQLStatement() { StatementString = sql });
        }

        /// <summary>
        /// Returns accounts which have an In force Credit Life policy
        /// </summary>
        /// <returns>QueryResults</returns>
        public QueryResults GetPersonalLoanAccountWithACreditLifePolicy()
        {
            var sql = string.Format(@"SELECT
                                        parentAcc.AccountKey
                                    FROM dbo.Account parentAcc
                                        INNER JOIN dbo.Account acc ON
                                        acc.ParentAccountKey = parentAcc.AccountKey
                                        INNER JOIN dbo.FinancialService fs ON
                                        fs.AccountKey = acc.AccountKey
                                        AND fs.FinancialServiceTypeKey = 11
                                    WHERE parentAcc.RRR_ProductKey = 12
                                    AND	acc.RRR_ProductKey = 13
                                    AND acc.AccountStatusKey = 1
                                    AND parentAcc.AccountStatusKey = 1
                                    AND fs.AccountStatusKey = 1 order by newid()");

            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }

        public QueryResults GetPersonalLoanAccountWithoutACreditLifePolicy()
        {
            var sql = string.Format(@"SELECT parentAcc.AccountKey
                                    FROM dbo.Account parentAcc
                                        Left JOIN dbo.Account acc
	                                    ON acc.ParentAccountKey = parentAcc.AccountKey AND	acc.RRR_ProductKey = 13
                                    WHERE parentAcc.RRR_ProductKey = 12
                                    AND parentAcc.AccountStatusKey = 1
                                    AND acc.AccountKey is  null order by newid()");
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }

        /// <summary>
        /// Gets all of the balances for all of the financial services for an account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Balance> GetAccountBalances(int accountKey)
        {
            return dataContext.Query<Automation.DataModels.Balance>(string.Format(@"select fs.financialServiceKey, v.balanceTypeKey, v.Amount
                    from [2am].dbo.financialservice fs
                    join [2am].fin.balance v on fs.financialServiceKey=v.financialServiceKey
                    where fs.accountkey={0}", accountKey));
        }

        public Automation.DataModels.Account GetOpenMortgageLoanAccountInSPV(int SPVKey)
        {
            return dataContext.Query<Automation.DataModels.Account>(string.Format(@"select top 1 * from [2am].dbo.Account a where a.spvKey = {0} and accountStatusKey = 1 and rrr_ProductKey in (1,5,6,9,11)", SPVKey))
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns an account which has no SAHL HOC and has a detail type record of type param [detailType]
        /// </summary>
        /// <param name="detailType"></param>
        /// <returns></returns>
        public Automation.DataModels.Account GetNonSAHLAccountWithDetailType(DetailTypeEnum detailType)
        {
            string query = string.Format(@"
                SELECT
                    acc.ParentAccountKey AS 'AccountKey'
                FROM dbo.Account acc
                    INNER JOIN dbo.FinancialService fs ON
                    fs.AccountKey = acc.AccountKey
                    INNER JOIN dbo.HOC hoc ON
                    hoc.FinancialServiceKey = fs.FinancialServiceKey
                    INNER JOIN dbo.Account acc1 ON
                    acc1.AccountKey = acc.ParentAccountKey
                    INNER JOIN dbo.Detail de ON
                    de.AccountKey = acc1.AccountKey
	                INNER JOIN dbo.FinancialService parentFs ON
	                parentFs.FinancialServiceKey = fs.ParentFinancialServiceKey
	                INNER JOIN fin.MortgageLoan mo ON
	                mo.FinancialServiceKey = parentFs.FinancialServiceKey
	                INNER JOIN dbo.Property p ON
	                p.propertyKey = mo.PropertyKey
	                INNER JOIN dbo.TitleType t ON
	                t.TitleTypeKey = p.TitleTypeKey
                WHERE acc.AccountStatusKey = 1
                AND HOCInsurerKey <> 2
                AND t.TitleTypeKey <> 3
                AND de.DetailTypeKey ={0}", (int)detailType);
            return dataContext.Query<Automation.DataModels.Account>(query).FirstOrDefault();
        }

        public Automation.DataModels.Account GetAccountByPropertyKey(int propertykey, AccountStatusEnum status, OriginationSourceEnum originationSource)
        {
            string query = string.Format(@"select acc.* from dbo.account as acc
                                                    join dbo.financialservice as fs
                                                        on acc.accountkey=fs.accountkey
                                                    join fin.mortgageloan as ml
		                                                on fs.financialservicekey=ml.financialservicekey
                                                where ml.propertykey= {0} and acc.accountstatuskey={1} and rrr_originationsourcekey={2}", propertykey, (int)status, (int)originationSource);
            return dataContext.Query<Automation.DataModels.Account>(query).FirstOrDefault();
        }

        public void RemoveRoleFromAccount(int accountKey, RoleTypeEnum roleType, int legalEntityKey)
        {
            string query = string.Format(@"Update [2am].dbo.Role set GeneralStatusKey = 2 where accountKey = {0} and RoleTypeKey = {1} and LegalEntityKey = {2}", accountKey, (int)roleType, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateDebtCounsellingCompany(int debtCounsellorLegalEntitKey, int accountKey)
        {
            var sql = string.Format(@"
            UPDATE er
            SET er.LegalEntityKey = {0}
            FROM dbo.ExternalRole er
	            INNER JOIN DebtCounselling.DebtCounselling dc ON dc.DebtCounsellingKey = er.GenericKey
            WHERE dc.AccountKey = {1}
            and er.ExternalRoleTypeKey = 2
            ", debtCounsellorLegalEntitKey, accountKey);
            dataContext.ExecuteNonSQLQuery(new SQLStatement() { StatementString = sql });
        }

        public void AddRecuringDiscountAttributeToAccountsForLegalEntity(int legalEntityKey)
        {
            string query = string.Format(@"INSERT INTO [2AM].dbo.OfferAttribute
                SELECT o.OfferKey,
	                29
                FROM [2AM].dbo.LegalEntity le (NOLOCK)
	                JOIN [2AM].dbo.Role r (NOLOCK) ON le.LegalEntityKey = r.LegalEntityKey
	                JOIN [2AM].dbo.Account a (NOLOCK) ON r.AccountKey = a.AccountKey
	                JOIN [2AM].dbo.Offer o (NOLOCK) ON a.AccountKey = o.AccountKey AND o.OfferTypeKey IN (6,7,8)
	                LEFT JOIN [2AM].dbo.OfferAttribute oa (NOLOCK) ON o.OfferKey = oa.OfferKey AND oa.OfferAttributeTypeKey = 29
                WHERE le.LegalEntityKey = {0}
	                AND oa.OfferAttributeKey IS NULL", legalEntityKey);
            dataContext.Execute(query);
        }

        public QueryResults GetFinancialServiceRecordByType(int accountKey, FinancialServiceTypeEnum financialServiceTypeKey)
        {
            string query =
                string.Format(@"select * from [2AM].dbo.financialService
								where accountKey={0} and financialServiceTypeKey={1}", accountKey, (int)financialServiceTypeKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public QueryResults GetRoundRobinConfiguration(OfferRoleTypeEnum offerRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            string query = string.Format(@"USE [2AM]
                                            SELECT
	                                            ROW_NUMBER() OVER(ORDER BY a.ADUserKey) AS RRNumber,
	                                            a.ADUserName,
                                                rrp.RoundRobinPointerIndexID,
												rrs.CapitecGeneralStatusKey,
												a.GeneralStatusKey
                                            FROM [2AM].[dbo].ADUser a (NOLOCK)
	                                            INNER JOIN [2AM].[dbo].UserOrganisationStructure uos (NOLOCK) ON a.ADUserKey = uos.ADUserKey
	                                            INNER JOIN [2AM].[dbo].UserOrganisationStructureRoundRobinStatus rrs (NOLOCK) ON uos.UserOrganisationStructureKey = rrs.UserOrganisationStructureKey
	                                            INNER JOIN [2AM].[dbo].OfferRoleTypeOrganisationStructureMapping smap (NOLOCK) ON uos.OrganisationStructureKey = smap.OrganisationStructureKey
	                                            INNER JOIN [2AM].[dbo].RoundRobinPointerDefinition rrpd (NOLOCK) ON smap.OfferRoleTypeOrganisationStructureMappingKey = rrpd.GenericKey
	                                            LEFT JOIN [2AM].[dbo].RoundRobinPointer rrp (NOLOCK) ON rrpd.RoundRobinPointerKey = rrp.RoundRobinPointerKey
                                            WHERE smap.OfferRoleTypeKey = {0}
                                            AND rrp.RoundRobinPointerKey = {1}", (int)offerRoleTypeKey, (int)roundRobinPointer);
            var statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }

        public QueryResults GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(OfferTypeEnum offerType, ProductEnum productType)
        {
            var query = string.Format(@"SELECT TOP 1
	                                         parentAcc.AccountKey,
	                                         o.OfferKey
                                        FROM [2AM].[dbo].Account parentAcc
	                                        INNER JOIN [2AM].[dbo].Account childAcc ON childAcc.ParentAccountKey = parentAcc.AccountKey
	                                        INNER JOIN [2AM].[dbo].Offer o ON o.AccountKey = parentAcc.AccountKey
                                        WHERE parentAcc.AccountStatusKey = 1
                                        AND parentAcc.AccountKey NOT IN
                                        (
	                                        SELECT AccountKey FROM debtcounselling.DebtCounselling
                                        )
                                        AND parentAcc.RRR_ProductKey = 1
                                        AND childAcc.RRR_ProductKey <> {0}
                                        AND o.OfferTypeKey = {1}
                                        AND o.OfferStatusKey = 3
                                        ORDER BY o.OfferStartDate DESC", (int)productType, (int)offerType);
            var statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }

        public int GetParentAccountKey(int accountKey)
        {
            return dataContext.Query<int>(String.Format("select ParentAccountKey from [2AM].dbo.Account where AccountKey = {0}", accountKey)).FirstOrDefault();
        }

        public QueryResults GetOpenLifeAccountWithAssuredLife()
        {
            var query = string.Format(@"select top 1
                                        c.AccountKey as LifeAccountKey, le.LegalEntityKey
                                        from [2AM].dbo.Account p
                                        join [2AM].dbo.Account c on p.AccountKey = c.ParentAccountKey
                                        join [2AM].dbo.Role r on c.AccountKey = r.AccountKey
                                        join [2AM].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
                                        where p.AccountStatusKey = 1
                                        and c.AccountStatusKey = 1 and c.RRR_ProductKey = 4
                                        and r.RoleTypeKey = 1 and r.GeneralStatusKey = 1
                                        and c.AccountKey not in (select AccountKey from [2AM].dbo.DisabilityClaim where DisabilityClaimStatusKey in (1,3))
                                        order by newid()");
            var statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results;
        }


        public QueryResults GetRemainingInstalmentsOnLoan(int financialServiceKey)
        {
            string q = string.Format(@"SELECT TOP 1
                                        lb.RemainingInstalments
                                       FROM [fin].LoanBalance lb
                                       WHERE lb.FinancialServiceKey = {0}
                                       ORDER BY newid()", financialServiceKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r;
        }

        public void UpdateRemainingInstalmentsOnLoan(int financialServiceKey, int numberOfInstalments)
        {
            var sql = string.Format(@"UPDATE [fin].LoanBalance
                                      SET RemainingInstalments = {0}
                                      WHERE FinancialServiceKey = {1}", numberOfInstalments, financialServiceKey);
            dataContext.ExecuteNonSQLQuery(new SQLStatement() { StatementString = sql });
        }

        public Automation.DataModels.Account GetAccountsWithActiveSubsidyAndAcceptedOfferWithLoanConditions222Or223()
        {
            string query = string.Format(@" SELECT TOP 10
	                                        acc.AccountKey
                                        FROM [2AM].[dbo].Account acc
	                                        INNER JOIN [2AM].[dbo].AccountSubsidy accs ON accs.AccountKey = acc.AccountKey 
	                                        INNER JOIN [2AM].[dbo].Subsidy s ON s.SubsidyKey = accs.SubsidyKey AND s.GeneralStatusKey = 1	
	                                        INNER JOIN [2AM].[dbo].Offer o ON o.AccountKey = acc.AccountKey AND o.OfferStatusKey = 3
	                                        INNER JOIN [2AM].[dbo].OfferCondition oc ON oc.OfferKey = o.OfferKey
	                                        INNER JOIN [2AM].[dbo].Condition c ON c.ConditionKey = oc.ConditionKey AND c.ConditionName In (222, 223)
                                        WHERE acc.AccountStatusKey = 1");
            var account = dataContext.Query<Automation.DataModels.Account>(query).First();
            return account;
        }

        
    }
}