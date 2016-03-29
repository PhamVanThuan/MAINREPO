using Automation.DataAccess.Interfaces;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Automation.DataAccess.DataHelper
{
    /// <summary>
    /// Contains SQL Queries for CAP 2 Workflow
    /// </summary>
    public class _2AM_CAP2
    {
        private IDataContext dataContext;

        public _2AM_CAP2()
        {
            dataContext = new DataContext();
        }

        /// <summary>
        /// Gets the latest CAPOffer record for a given account
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>CapOffer.*, CapStatus.Description</returns>
        public QueryResults GetLatestCapOfferByAccountKey(int accountKey)
        {
            var q =
                string.Format(@"select top 1 co.*, c.description as [CapStatus]
                                from [2am].dbo.CapOffer co (nolock)
                                join [2am].dbo.CapStatus c on co.CapStatusKey=c.CapStatusKey
                                where AccountKey= {0}
                                order by OfferDate desc", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the X2Data.Cap2_Offers record when provided with a CapOfferKey and an AccountKey
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="capOfferKey">CapOfferKey</param>
        /// <returns>x2data.Cap2_Offers.*, State.Name</returns>
        public QueryResults GetLatestCap2X2DataByAccountKeyAndCapOfferKey(int accountKey, string capOfferKey)
        {
            var q =
                string.Format(@"select top 1 data.*, s.name as [StateName]
                                from x2.x2data.cap2_offers data (nolock)
                                join x2.x2.instance i (nolock) on data.instanceid=i.id
                                join x2.x2.state s on i.stateid=s.id
                                where accountkey= {0} and data.CapOfferKey= {1}
                                order by InstanceID desc", accountKey, capOfferKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the CAP NTU/Decline Reasons
        /// </summary>
        /// <param name="type">The type to fetch. "Decline" or "NTU"</param>
        /// <returns></returns>
        public QueryResults GetCapNTUReasons(string type)
        {
            string query = type == "NTU" ?
                " select * from [2am].dbo.capNTUReason where description not like 'Decline%'" :
                " select * from [2am].dbo.capntureason where description like 'Decline%'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the CapPaymentOption records for a particular CapPaymentOptionKey
        /// </summary>
        /// <param name="capPaymentOptionKey">CapPaymentOptionKey</param>
        /// <returns>CapPaymentOption.*</returns>
        public QueryResults GetCapPaymentOptionByCapPaymentOptionKey(string capPaymentOptionKey)
        {
            var q =
                string.Format(@"select * from [2am].dbo.CapPaymentOption where CapPaymentOptionKey={0}", capPaymentOptionKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the employment types of the Legal Entities that play a role on the Mortgage Loan Account. Used to
        /// determine if we can select the D/O cap payment option.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>EmploymentType.Description</returns>
        public QueryResults GetAccountEmploymentType(int accountKey)
        {
            var q =
                string.Format(@"select et.description
                                from [2am].dbo.role r (nolock)
                                join [2am].dbo.legalentity le (nolock) on r.legalentitykey=le.legalentitykey
                                join [2am].dbo.employment e (nolock) on le.legalentitykey=e.legalentitykey
                                join [2am].dbo.employmenttype et (nolock) on e.employmenttypekey=et.employmenttypekey
                                where accountkey={0}
                                and e.employmentstatuskey=1", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the CapOfferDetail records for the latest CAP Offer linked to the Account
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>CapOfferDetail.*, CapType.CapCapTypeKey</returns>
        public QueryResults GetCapOfferDetailByAccountKey(int accountKey)
        {
            string q =
                string.Format(@"select cod.*,ct.captypekey as CapTypeDescription
                                from [2am].dbo.account a (nolock)
                                join (select max(CapOfferKey) CapOfferKey,
                                AccountKey from [2am].dbo.CapOffer (nolock) group by AccountKey)
                                as LatestCapOffer on  a.AccountKey=LatestCapOffer.AccountKey
                                join [2am].dbo.CapOffer co  (nolock) on LatestCapOffer.CapOfferKey=co.CapOfferKey
                                join [2am].dbo.CapOfferDetail cod (nolock) on co.CapOfferKey=cod.CapOfferKey
                                join CapTypeConfigurationDetail ctcd
                                on cod.CapTypeConfigurationDetailKey=ctcd.CapTypeConfigurationDetailKey
                                join [2am].dbo.CapType ct on ctcd.CapTypeKey=ct.CapTypeKey
                                where a.AccountKey={0}
                                order by ct.CapTypeKey asc", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        ///<summary>
        ///</summary>
        ///<param name="accountKey"></param>
        ///<param name="capStatusKey"></param>
        ///<returns></returns>
        public QueryResults GetCapOfferDetailByAccountAndStatus(int accountKey, int capStatusKey)
        {
            var q =
                string.Format(@"select cod.*,ct.captypekey as CapTypeDescription
                                from [2am].dbo.account a (nolock)
                                join (select max(CapOfferKey) CapOfferKey,
                                AccountKey from [2am].dbo.CapOffer (nolock) group by AccountKey)
                                as LatestCapOffer on  a.AccountKey=LatestCapOffer.AccountKey
                                join [2am].dbo.CapOffer co  (nolock) on LatestCapOffer.CapOfferKey=co.CapOfferKey
                                join [2am].dbo.CapOfferDetail cod (nolock) on co.CapOfferKey=cod.CapOfferKey
                                join [2am].dbo.CapTypeConfigurationDetail ctcd
                                on cod.CapTypeConfigurationDetailKey=ctcd.CapTypeConfigurationDetailKey
                                join [2am].dbo.CapType ct on ctcd.CapTypeKey=ct.CapTypeKey
                                where a.AccountKey={0} and cod.CapStatusKey={1}
                                order by ct.CapTypeKey asc", accountKey, capStatusKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the CAP Payment Option selected for the CAP Offer
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>CAPPaymentOption.Description</returns>
        public QueryResults GetCapPaymentOptionByAccountKey(int accountKey)
        {
            var q =
                string.Format(@"select cpo.description as CapTypeDescription
                                from [2am].dbo.account a (nolock)
                                join (select max(CapOfferKey) CapOfferKey,
                                AccountKey from [2am].dbo.CapOffer (nolock) group by AccountKey)
                                as LatestCapOffer on  a.AccountKey=LatestCapOffer.AccountKey
                                join [2am].dbo.CapOffer co  (nolock) on LatestCapOffer.CapOfferKey=co.CapOfferKey
                                join [2am].dbo.CapPaymentOption cpo on co.cappaymentoptionkey=cpo.cappaymentoptionkey
                                where co.accountkey={0}", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the x2Data.Cap2_Offers record for the latest CAP offer linked to an account
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>x2Data.Cap2_Offers.*</returns>
        public QueryResults GetCap2X2Data(int accountKey)
        {
            var q =
                string.Format(@"select c.* from [2am].dbo.Account a join
                                (select max(CapOfferKey) as capofferkey, AccountKey
                                from [2am].dbo.CapOffer (nolock) group by AccountKey)
                                as latestCapOffer on a.accountkey=latestCapOffer.accountkey
                                join x2.x2data.cap2_offers c (nolock)
                                on latestCapOffer.capofferkey=c.capofferkey
                                where a.accountkey={0}", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Used to setup CAP Broker and CAP Credit Brokers prior to the running of the CAP automation suite
        /// </summary>
        /// <param name="brokerAdUserName">CAP Consultant ADUserName</param>
        /// <param name="creditAdUserName">Credit Broker ADUserName</param>
        /// <returns>QueryResults</returns>
        public void CAP2AutomationSetup(string brokerAdUserName, string creditAdUserName)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.CAP2AutomationSetup" };
            proc.AddParameter(new SqlParameter("@BrokerAdUserName", brokerAdUserName));
            proc.AddParameter(new SqlParameter("@CreditAdUserName", creditAdUserName));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// This is used at the end of the CAP 2 test suite to create financial adjustment records for our CAP2 offers
        /// </summary>
        public void OptIntoCAP2()
        {
            var proc = new SQLStoredProcedure { Name = "test.CapOptIn" };
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Retrieves the end date of the associated CapTypeConfiguration for a CAP Offer. This is the date that the Cap Offer
        /// should expire.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>CapTypeConfiguration.offerenddate</returns>
        public QueryResults GetCapTypeConfigurationEndDateForCapOffer(int accountKey)
        {
            var q =
                string.Format(@"select ctc.offerenddate, ctc.capeffectivedate
                                from [2am].dbo.account a (nolock)
                                join (select max(capofferkey) as CapOfferKey, accountkey from CapOffer (nolock) group by accountkey)
                                as maxcap on a.accountkey=maxcap.accountkey join
                                [2am].dbo.capoffer co (nolock) on maxcap.CapOfferKey=co.capofferkey
                                join captypeconfiguration ctc (nolock)
                                on co.captypeconfigurationkey=ctc.captypeconfigurationkey
                                where a.accountkey={0}", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the active CAP 2 Rate Override record for an Account
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns></returns>
        public QueryResults GetCap2FinancialAdjustmentDetailByAccountKey(int accountKey, FinancialAdjustmentStatusEnum financialAdjustmentStatus)
        {
            var q =
                string.Format(@"select top 1 fats.FinancialAdjustmentTypeSourceKey, fa.FromDate, fix.Rate as CapRate,
                    datediff(mm, fromdate, endDate) as Term,
                    CAPBalance as CapBalance,
                    CAPPaymentOptionKey as CapPaymentOptionKey,
                    fa.FinancialAdjustmentStatusKey
                    from [2am].dbo.financialservice fs
                    join [2am].fin.FinancialAdjustment fa
                    on fs.financialservicekey=fa.financialservicekey
                    join [2am].fin.FixedRateAdjustment fix
                    on fa.FinancialAdjustmentKey=fix.FinancialAdjustmentKey
                    join [2am].fin.FinancialAdjustmentTypeSource fats on
                    fa.FinancialAdjustmentSourceKey = fats.FinancialAdjustmentSourceKey and
                    fa.FinancialAdjustmentTypeKey = fats.FinancialAdjustmentTypeKey
                    JOIN product.FinancialServiceAttribute att
                    ON fs.FinancialServiceKey = att.FinancialServiceKey
                    JOIN product.CAP cap
                    ON att.FinancialServiceAttributeKey = cap.FinancialServiceAttributeKey
                    where
                    accountkey={0} and fa.FinancialAdjustmentSourceKey=1
                    and fa.FinancialAdjustmentTypeKey=1
                    and fa.FinancialAdjustmentStatusKey={1}
                    order by 1 desc", accountKey, (int)financialAdjustmentStatus);
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves test cases from the AutomationCAP2TestCases table
        /// </summary>
        /// <returns></returns>
        public QueryResults GetCap2TestCases()
        {
            var q =
                string.Format(@"select * from test.AutomationCAP2TestCases where TestType='Create'");
            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the CAP test account key when provided with an identifier
        /// </summary>
        /// <param name="identifier">Identifier from the AutomationCAP2TestCases table</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.CapOffer> GetTestCapOffer(string identifier)
        {
            var sql =
                string.Format(@"select AccountKey, CapOfferKey from test.automationcap2testcases where TestIdentifier='{0}'", identifier);
            return dataContext.Query<Automation.DataModels.CapOffer>(sql);
        }

        /// <summary>
        /// get the latest instance id for a cap offer on an account
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <returns>InstanceID</returns>
        public int GetCAP2InstanceID(int accountKey)
        {
            var q =
                string.Format(@"select max(instanceid) as InstanceID
                                from x2.x2data.cap2_offers c
                                join x2.x2.instance i on c.instanceid=i.id and parentInstanceID is null
                                where accountkey={0}", accountKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            var r = dataContext.ExecuteSQLQuery(statement);
            return Convert.ToInt32(r.Rows(0).Column("InstanceID").Value);
        }

        /// <summary>
        /// Gets the first 2 cap 2 test cases for legal entities that have more than one cap test case.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetLegalEntitiesWithMoreThanOneCAPTestCase()
        {
            var query =
                string.Format(@"    --Get all the legal entities with more than 1 account
                                    select r.LegalEntityKey, count(r.AccountKey) as NumberOfAccounts
                                    into
                                    #PossibleCapLegalEntities
                                    from  [2am].dbo.Role r
                                          join [2am].dbo.Account a on r.accountkey = a.accountkey
		                                    and a.AccountStatusKey = 1
		                                    and RRR_ProductKey in (1,9)
                                    where r.RoleTypeKey in (2,3)
                                    and r.GeneralStatusKey = 1
                                    and r.AccountKey in (select t.AccountKey from [2am].test.captestcases t where currentBalance > 150000 and capQualifyOnePerc = 'Qualify')
                                    group by r.LegalEntityKey
                                    having count(r.AccountKey) > 1

                                    -- Get the account keys for the entities with more than 1 account
                                    select AccountKey, c.LegalEntityKey
                                    into
                                    #PossibleCapAccounts
                                    from #PossibleCapLegalEntities c with(nolock)
                                    join [2am].dbo.Role r with(nolock) on  c.LegalEntityKey = r.LegalEntityKey

                                    -- Show the Accounts
                                    select top 2 p.LegalEntityKey, p.AccountKey, t.*
                                    from  #PossibleCapAccounts p with(nolock)
                                          join [2am].test.captestcases t with(nolock) on  p.AccountKey = t.AccountKey
                                    order by p.LegalEntityKey, p.AccountKey

                                    drop table #PossibleCapLegalEntities
                                    drop table #PossibleCapAccounts"
                );
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}