using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public double GetCurrentPersonalLoanBalance(int accountKey)
        {
            string sql = string.Format(@"SELECT Amount
			FROM [2am].dbo.Account a
			JOIN [2am].dbo.FinancialService fs ON fs.AccountKey = a.AccountKey
			JOIN [2am].fin.Balance b ON fs.FinancialServiceKey = b.FinancialServiceKey
			WHERE RRR_ProductKey = 12
				AND a.AccountStatusKey = 1
				AND fs.FinancialServiceTypeKey = 10
				AND a.AccountKey={0}", accountKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("Amount").GetValueAs<double>();
        }

        public int GetPersonalLoanAccountWithStageTransition()
        {
            string sql = string.Format(@"select TOP 1 a.AccountKey
                                        from [2am].dbo.Account a
                                        Join [2am].dbo.StageTransition st ON st.GenericKey = a.AccountKey
                                        Where a.RRR_ProductKey = 12 AND a.AccountStatusKey = 1");
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("AccountKey").GetValueAs<int>();
        }

        public int GetPersonalLoanAccountWithoutStageTransition()
        {
            string sql = string.Format(@"select TOP 1 a.AccountKey
                                        from [2am].dbo.Account a
                                        Left Join [2am].dbo.StageTransition st ON st.GenericKey = a.AccountKey
                                        Where a.RRR_ProductKey = 12
                                        AND st.GenericKey is null AND a.AccountStatusKey = 1");
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("AccountKey").GetValueAs<int>();
        }

        public IEnumerable<Automation.DataModels.LegalEntity> GetXNumberOfLegalEntitiesWhoQualifyForPersonalLoans(int noOfCasesToFetch)
        {
            string sql = string.Format(@"SELECT TOP {0} le.IdNumber, le.legalEntityKey
                                        FROM [2am].dbo.legalentity as le
                                        inner join [2am].dbo.role as r on le.legalentitykey = r.legalentitykey
                                            and r.roletypekey = 2
                                            AND r.GeneralStatusKey = 1
                                        inner join [2am].dbo.account as a
                                            on r.accountkey = a.accountkey
                                            and a.accountstatuskey = 1
                                            AND a.RRR_ProductKey IN (1,2,5,6,9,11)
                                        left join [2am].dbo.externalrole as er on le.legalentitykey = er.legalentitykey
                                            and er.generickeytypekey = 2
                                        left join [2am].dbo.offer as o on er.generickey = o.offerkey
                                        LEFT JOIN [2am].debtcounselling.DebtCounselling dc ON a.AccountKey = dc.AccountKey
	                                        AND dc.DebtCounsellingStatusKey = 1
                                        where
                                        er.externalrolekey is null
                                        and o.offerkey is null
                                        AND dc.DebtCounsellingKey IS NULL
                                        AND le.LegalEntityTypeKey = 2", noOfCasesToFetch);
            var results = dataContext.Query<Automation.DataModels.LegalEntity>(sql);
            return results;
        }

        public IEnumerable<Automation.DataModels.GenericCuttlefishMessage> GetLatestBatchServiceGenericMessages()
        {
            string sql = @"select gm.ID, gm.MessageDate, gm.MessageContent, gm.GenericStatusID, gm.BatchID
                from [2am].dbo.BatchService bs
                join [Cuttlefish].dbo.GenericMessage gm on bs.BatchServiceKey = gm.BatchID
                where bs.BatchServiceKey =
                (select max(batchServiceKey) from [2am].dbo.BatchService)";
            return dataContext.Query<Automation.DataModels.GenericCuttlefishMessage>(sql);
        }

        public QueryResults GetPersonalLoanApplicationByApplicantsIdNumber(string idNumber, string workflowState)
        {
            string sql = string.Format(@"select * from x2.x2data.Personal_Loans pl
                join x2.x2.Instance i on pl.InstanceID = i.ID
                join x2.x2.State s on i.StateID = s.id
                join [2am].dbo.externalRole wr on pl.ApplicationKey = wr.GenericKey
	                and wr.externalRoleTypeKey = 1
                join [2am].dbo.legalentity le on wr.legalentitykey = le.legalentitykey
                join x2.x2.worklist wl on pl.InstanceID = wl.InstanceID
                where le.idnumber = '{0}' and s.name = '{1}'", idNumber, workflowState);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntityDetailsForCapitecApplication(int offerKey)
        {
            string sql = string.Format(@"SELECT
	                                        le.*
                                        FROM [2AM].[dbo].LegalEntity le
	                                        INNER JOIN dbo.OfferRole ofr ON ofr.LegalEntityKey = le.LegalEntityKey
	                                        INNER JOIN dbo.Offer o ON o.OfferKey = ofr.OfferKey
                                        WHERE o.OfferKey = {0}
                                        AND le.IDNumber IS NOT NULL", offerKey);
            var results = dataContext.Query<Automation.DataModels.LegalEntity>(sql);
            return results;
        }

        public IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntityDetailsForCapitecAccount(int accountKey)
        {
            string sql = string.Format(@"SELECT
	                                        le.*
                                        FROM [2AM].[dbo].LegalEntity le
	                                        INNER JOIN dbo.Role r ON r.LegalEntityKey = le.LegalEntityKey
	                                        INNER JOIN dbo.Account acc ON acc.AccountKey = r.AccountKey
                                        WHERE acc.AccountKey = {0}", accountKey);
            var results = dataContext.Query<Automation.DataModels.LegalEntity>(sql);
            return results;
        }
    }
}