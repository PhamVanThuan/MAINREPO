using Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Updates the value of proposal end date
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="term"></param>
        /// <param name="proposalTypeKey"></param>
        /// <param name="proposalStatusKey"></param>
        public void UpdateProposalEndDate(int accountKey, int term, int proposalTypeKey, int proposalStatusKey)
        {
            string query = string.Format(@"Update debtcounselling.proposalitem set enddate = dateadd(mm, {0}, a.opendate) from
										[2am].debtcounselling.proposalitem pp
										join (Select a.opendate, p.proposalkey, max(pp.startdate) as startdate
										from [2am].debtcounselling.debtcounselling dc
										join [2am].debtcounselling.proposal p on dc.debtcounsellingkey = p.debtcounsellingkey
										join [2am].debtcounselling.proposalitem pp on p.proposalkey = pp.proposalkey
										join [2am].dbo.account a on dc.accountkey = a.accountkey
										where dc.accountkey = {1} and p.proposalstatuskey = {2} and p.proposaltypekey = {3}
										group by a.opendate, p.proposalkey) a on pp.proposalkey = a.proposalkey and pp.startdate = a.startdate", term, accountKey, proposalStatusKey, proposalTypeKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }/// <summary>

        /// Gets the Accepted Proposal details
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="proposalStatus"></param>
        /// <param name="proposalAccepted"></param>
        /// <returns></returns>
        public QueryResults GetProposalDetails(int debtCounsellingKey, ProposalStatusEnum proposalStatus, ProposalAcceptedEnum proposalAccepted, ProposalTypeEnum proposalType)
        {
            string query = string.Format(@"SELECT p.ProposalKey, p.ProposalTypeKey, p.ProposalStatusKey, p.DebtCounsellingKey, p.HOCInclusive, p.LifeInclusive,
	                                        p.ADUserKey, p.CreateDate, p.ReviewDate, p.Accepted
                                        FROM [2am].debtcounselling.DebtCounselling AS dc
                                        INNER JOIN [2am].debtcounselling.Proposal AS p ON dc.DebtCounsellingKey = p.DebtCounsellingKey
                                        WHERE (p.ProposalStatusKey = {0}) AND (isnull(p.Accepted,0) = {1}) AND (dc.DebtCounsellingKey = {2})
                                        AND (ProposalTypeKey={3})", (int)proposalStatus, (int)proposalAccepted, debtCounsellingKey,
                                                                (int)proposalType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts a counter proposal record and the proposal items for a debt counselling case
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalStatusKey">proposalStatusKey</param>
        /// <param name="proposalItems">proposalItems</param>
        public void InsertCounterProposalByStatus(int debtCounsellingKey, ProposalStatusEnum proposalStatusKey, int proposalItems)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "[2am].test.InsertCounterProposalByStatus" };
            proc.AddParameter(new SqlParameter("@debtCounsellingKey", debtCounsellingKey.ToString()));
            proc.AddParameter(new SqlParameter("@proposalStatusKey", ((int)proposalStatusKey).ToString()));
            proc.AddParameter(new SqlParameter("@proposalitems", proposalItems.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Deletes a proposal record and the related proposal items
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalTypeKey">proposalTypeKey</param>
        /// <param name="proposalStatusKey">proposalStatusKey</param>
        public void DeleteProposal(int debtCounsellingKey, ProposalTypeEnum proposalTypeKey, ProposalStatusEnum proposalStatusKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "[2am].test.DeleteProposal" };
            proc.AddParameter(new SqlParameter("@debtCounsellingKey", debtCounsellingKey.ToString()));
            proc.AddParameter(new SqlParameter("@proposalTypeKey", ((int)proposalTypeKey).ToString()));
            proc.AddParameter(new SqlParameter("@proposalStatusKey", ((int)proposalStatusKey).ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Fethces the proposed end date for the debt counselling case.
        /// </summary>
        /// <param name="proposalKey">proposalKey</param>
        /// <returns>Debt Counselling Proposal End Date</returns>
        public DateTime GetProposalEndDate(int proposalKey)
        {
            var q = String.Format(@"select max(enddate) as ProposalEndDate from [2AM].debtcounselling.proposalItem where proposalKey={0}", proposalKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s).Rows(0).Column("ProposalEndDate").GetValueAs<DateTime>();
        }

        /// <summary>
        /// Gets all the proposal/counter proposal keys linked to a debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public List<int> GetAllProposalsAndCounterProposalsByDebtCounsellingKey(int debtCounsellingKey)
        {
            var q = string.Format(@"select proposalKey,* from [2AM].debtcounselling.proposal where debtCounsellingKey={0}", debtCounsellingKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s).GetColumnValueList<int>("proposalKey");
        }

        /// <summary>
        ///  Gets all proposal items for a proposal.
        /// </summary>
        /// <param name="proposalkey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.ProposalItem> GetProposalItems(int proposalkey)
        {
            var query = string.Format(@"select distinct proposalItem.*,MonthlyServiceFee from [2AM].debtcounselling.proposal
											inner join [2AM].debtcounselling.proposalItem
												on proposal.proposalkey =proposalItem.proposalkey
										    where proposal.proposalkey = {0}", proposalkey);
            return dataContext.Query<Automation.DataModels.ProposalItem>(query);
        }

        /// <summary>
        ///  Gets all proposals for a debtcounselling case.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Proposal> GetProposals(params int[] debtCounsellingKey)
        {
            var keys = Helpers.GetDelimitedString<int>(debtCounsellingKey, ",");
            var query = string.Format(@"select * from [2AM].debtcounselling.proposal
										    where proposal.debtcounsellingkey in ({0})", keys);
            return dataContext.Query<Automation.DataModels.Proposal>(query);
        }

        /// <summary>
        /// If a matching Proposal exists, delete the Propsoal and Proposal Items.
        /// Insert a new Proposal according to the existing Remaining Installment, Interest Rate and Installment values of that account
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalStatusKey">proposalStatusKey</param>
        /// <param name="numberOfProposalItems">numberOfProposalItems</param>
        /// <param name="aduserName">aduserName</param>
        /// <param name="hocInclusive">HOC Inclusive = 1 Exclusive = 0</param>
        /// <param name="lifeInclusive">Life Inclusive = 1 Exclusive = 0</param>
        public void InsertProposal(int debtCounsellingKey, ProposalStatusEnum proposalStatusKey, int numberOfProposalItems, string aduserName, int hocInclusive,
            int lifeInclusive)
        {
            InsertProposal(debtCounsellingKey, proposalStatusKey, numberOfProposalItems, aduserName, hocInclusive, lifeInclusive, 0);
        }

        /// <summary>
        /// If a matching Proposal exists, delete the Propsoal and Proposal Items.
        /// Insert a new Proposal according to the existing Remaining Installment, Interest Rate and Installment values of that account
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalStatusKey">proposalStatusKey</param>
        /// <param name="numberOfProposalItems">numberOfProposalItems</param>
        /// <param name="aduserName">aduserName</param>
        /// <param name="hocInclusive">HOC Inclusive = 1 Exclusive = 0</param>
        /// <param name="lifeInclusive">Life Inclusive = 1 Exclusive = 0</param>
        public void InsertProposal(int debtCounsellingKey, ProposalStatusEnum proposalStatusKey, int numberOfProposalItems, string aduserName, int hocInclusive,
            int lifeInclusive, int monthlyServiceFeeInclusive)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "[2AM].test.InsertProposal" };
            proc.AddParameter(new SqlParameter("@debtcounsellingkey", debtCounsellingKey.ToString()));
            proc.AddParameter(new SqlParameter("@proposalstatuskey", ((int)proposalStatusKey).ToString()));
            proc.AddParameter(new SqlParameter("@adusername", aduserName));
            proc.AddParameter(new SqlParameter("@proposalitems", numberOfProposalItems.ToString()));
            proc.AddParameter(new SqlParameter("@hocinclusive", hocInclusive.ToString()));
            proc.AddParameter(new SqlParameter("@lifeinclusive", lifeInclusive.ToString()));
            proc.AddParameter(new SqlParameter("@feesinclusive", monthlyServiceFeeInclusive.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        public void UpdateReviewDateOfActiveAcceptedProposal(int debtCounsellingKey)
        {
            string query = string.Format(@" Update [2am].debtCounselling.Proposal
                                            Set ReviewDate = dateadd(yy, +1, getdate())
                                            where debtCounsellingKey = {0}
                                            and ProposalTypeKey = 1
                                            and ProposalStatusKey = 1
                                            and Accepted = 1", debtCounsellingKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}