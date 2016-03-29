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
        /// <summary>
        ///   This will get the policy status for a life account.
        /// </summary>
        /// <param name = "lifeAccountKey">Life account created for a life offer, after policy documents have been send off.</param>
        /// <returns></returns>
        public QueryResults GetLifePolicyStatus(int lifeAccountKey)
        {
            string query = string.Format(@"select lp.* from dbo.lifepolicy lp with (nolock)
                            inner join dbo.financialservice fs with (nolock)
                            on lp.financialservicekey = fs.financialservicekey
                            and fs.accountkey = {0}
                            inner join dbo.lifepolicystatus lps with (nolock)
                            on lp.policystatuskey = lps.policystatuskey", lifeAccountKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Will get the life policy type of a life offer.
        /// </summary>
        public QueryResults GetLifePolicyType(int offerKey)
        {
            string query = string.Format(@"select lpt.description as PolicyType
                        from [2am].dbo.offerlife ol  with (nolock)
                        inner join [2am].dbo.lifepolicytype lpt with (nolock)
                        on ol.lifepolicytypekey = lpt.lifepolicytypekey
                        where ol.offerkey = {0}", offerKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves all the addresses of all the legalentities that plays a role in the given account.
        /// </summary>
        /// <param name = "offerKey">AccountKey</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetLifeLegalEntityAddresses(int offerKey)
        {
            string query = string.Format(@"select distinct a.* from x2.x2data.lifeorigination lo with (nolock)
                        inner join dbo.role r with (nolock)
                        on	lo.loannumber = r.accountkey
                        inner join dbo.legalentityaddress lea  with (nolock)
                        on r.legalentitykey = lea.legalentitykey
                        inner join dbo.address a with (nolock)
                        on lea.addresskey = a.addresskey
                        where lo.offerkey = {0}", offerKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Get random address(s) that is not linked to the legal entities, that plays a role in the life account.
        /// </summary>
        /// <param name = "offerKey">AccountKey</param>
        /// <param name = "noResults">NoResults i.e. top 10 results</param>
        /// <param name = "addressFormat">Street, Box, PostNet, Suite, Private Bag, Free Text, Cluster Box</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetRandomAddresses(int offerKey, int noResults, string addressFormat)
        {
            string query = string.Format(@"select top {0}
                            af.description, a.* from dbo.address a  with (nolock)
                            inner join dbo.addressformat af  with (nolock)
                            on a.addressformatkey = af.addressformatkey
                            where a.addresskey not in
                            (select lea.addresskey from x2.x2data.lifeorigination lo  with (nolock)
                            inner join dbo.role r with (nolock)
                            on lo.loannumber = r.accountkey
                            inner join dbo.legalentityaddress lea with (nolock)
                            on r.legalentitykey = lea.legalentitykey
                            where lo.offerkey = {1}
                            ) and af.description = '{2}'
                            and isnumeric(a.BuildingNumber) = 1
                            and isnumeric(a.StreetNumber) = 1", noResults, offerKey, addressFormat);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Get all the Legal Entities that plays a role in the life account for the given life offer.
        /// </summary>
        /// <param name = "offerKey">Life lead offerkey.</param>
        /// <returns></returns>
        public QueryResults GetLegalEntitiesFromLifeAccountRoles(int offerKey)
        {
            string query = string.Format(@"select le.* from x2.x2data.lifeorigination lo  with (nolock)
                            inner join dbo.role r with (nolock)
                            on	 lo.loannumber = r.accountkey
                            inner join dbo.legalentity le
                            on le.legalentitykey = r.legalentitykey
                            where lo.offerkey = {0}", offerKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<LifeLead> GetLifeLeads(int mortgageLoanAccountKey,string workflowState,string cloneTimerName, LifePolicyTypeEnum policyTypeKey)
        {
            lock (this)
            {
                var p = new DynamicParameters();
                p.Add("@stateName", value: workflowState, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@cloneTimerName", value: cloneTimerName, dbType: DbType.String, direction: ParameterDirection.Input);
                p.Add("@policyTypeKey", value: (int)policyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
                p.Add("@loanNumber", value: mortgageLoanAccountKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
                return dataContext.Query<Automation.DataModels.LifeLead>("test.GetLifeLeads", parameters: p, commandtype: CommandType.StoredProcedure);
            }
        }
        
        public void ClearCreditLifePolicyClaims(int creditLifePolicyFinancialServiceKey)
        {
            dataContext.Execute(String.Format(@"delete from [2am].dbo.LifePolicyClaim where financialservicekey = {0}", creditLifePolicyFinancialServiceKey));
        }

        public LifePolicyClaim InsertCreditLifePolicyClaim(Automation.DataModels.LifePolicyClaim newClaim)
        {
            var param = new DynamicParameters();
            param.Add("@lifeFinancialServiceKey", value: newClaim.FinancialServiceKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@lifePolicyClaimKey", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@claimStatusKey", value: newClaim.ClaimStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@claimTypeKey", value: newClaim.ClaimTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@claimDate", value: newClaim.ClaimDate, dbType: DbType.Date, direction: ParameterDirection.Input);
            dataContext.Execute("test.InsertLifePolicyClaim", parameters: param, commandtype: CommandType.StoredProcedure);

            return this.GetCreditLifePolicyClaims(newClaim.FinancialServiceKey).Where(x => x.LifePolicyClaimKey == param.Get<int>("@lifePolicyClaimKey")).FirstOrDefault();
        }

        public FinancialService GetCreditLifePolicyFinancialService(int personalLoanAccountKey)
        {
            return dataContext.Query<Automation.DataModels.FinancialService>(String.Format(@"select childFS.* from [2am].dbo.financialservice parentFS
                                                                            join [2am].dbo.financialservice childFS
                                                                                on parentFS.financialservicekey=childFS.parentfinancialservicekey
                                                                       where parentFS.accountkey = {0}
                                                                       and childFS.financialservicetypekey=11", personalLoanAccountKey)).FirstOrDefault();
        }

        public IEnumerable<LifePolicyClaim> GetCreditLifePolicyClaims(int creditLifePolicyFinancialService)
        {
            return dataContext.Query<Automation.DataModels.LifePolicyClaim>(String.Format("select * from [2am].dbo.lifepolicyclaim where FinancialServiceKey = {0}", creditLifePolicyFinancialService));
        }

        public IEnumerable<ExternalLifePolicy> GetExternalLifePolicyByAccount(int accountKey)
        {
            var query = string.Format(@"select * from [2am].dbo.accountexternallife where accountkey = {0}", accountKey);
            return dataContext.Query<Automation.DataModels.ExternalLifePolicy>(query);
        }

        public void ReAssignLeadToConsultant(int mortgageLoanAccountKey, string consultantName)
        {
            var param = new DynamicParameters();
            param.Add("@adusername", value: consultantName, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@loanNumber", value: mortgageLoanAccountKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[test].[ReAssignLifeLeads]", parameters: param, commandtype: CommandType.StoredProcedure);
        }

        public IEnumerable<QualifyingAccount> GetQualifyingMortgageAccountsForLife(int count)
        {
            var sql = String.Format("select top {0} * from [2am].test.qualifyingaccounts", count);
            return dataContext.Query<QualifyingAccount>(sql);
        }

        public IEnumerable<LifeOfferAssignment> GetLifeOfferAssignments(int mortgageLoanAccountKey)
        {
            string sql = String.Format(@"select * from [2am].dbo.LifeOfferAssignment 
                                         where LoanAccountKey = {0}", mortgageLoanAccountKey);
            return dataContext.Query<LifeOfferAssignment>(sql);
        }

        public IEnumerable<LifeLead> GetCreatedLifeLeads()
        {
            string sql = String.Format(@"select 
	                                        cll.instanceid,
	                                        lo.LoanNumber as AccountKey,
	                                        lo.OfferKey,
	                                        lo.ConfirmationRequired,
	                                        cll.CreationDate,
	                                        lo.AssignTo as AssignedConsultant,
	                                        s.name as StateName,
	                                        i.statechangedate,
	                                        i.ParentInstanceID
                                        from [2am].test.CreatedLifeLeads cll with (nolock)
	                                        join [x2].[X2DATA].LifeOrigination lo with (nolock)
		                                        on cll.instanceid =lo.InstanceID
	                                        join [X2].[X2].Instance i with (nolock)
		                                        on lo.InstanceID = i.ID
	                                        join x2.x2.State s with (nolock)
		                                        on i.StateID = s.ID");
            return dataContext.Query<LifeLead>(sql);
        }
    }
}