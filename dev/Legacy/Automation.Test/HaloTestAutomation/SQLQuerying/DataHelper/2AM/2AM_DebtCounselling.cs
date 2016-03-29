using Common.Constants;
using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        ///<summary>
        /// This query will get a list of debt counselling test identifiers when provided with a test group
        ///</summary>
        ///<param name="testGroup">DebtCounselling.TestGroup</param>
        ///<returns></returns>
        public QueryResults GetDebtCounsellingTestIdentifiers(string testGroup)
        {
            string query =
                String.Format(@"select DebtCounsellingTestCases.TestIdentifier
								from test.DebtCounsellingTestCases
								where DebtCounsellingTestCases.TestGroup='{0}'", testGroup);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the row from the debtcounselling.DebtCounselling table for a given account
        /// </summary>
        /// <param name="accountKey">account</param>
        /// <param name="generalStatus">status of the debt counselling record</param>
        /// <returns>debtcounselling.debtCounselling.*</returns>
        public QueryResults GetDebtCounsellingRowByDebtCounsellingKey(int debtCounsellingKey, DebtCounsellingStatusEnum status)
        {
            string query = status == DebtCounsellingStatusEnum.None ?
                String.Format(@"select * from debtcounselling.debtcounselling dc where dc.debtcounsellingkey = {0}", debtCounsellingKey) :
                String.Format(@"select * from debtcounselling.debtCounselling dc where dc.debtCounsellingKey={0} and dc.debtcounsellingstatuskey={1}",
                    debtCounsellingKey, (int)status);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets the row from the debtcounselling.DebtCounselling table for a given account
        /// </summary>
        /// <param name="accountKey">account</param>
        /// <param name="generalStatus">status of the debt counselling record</param>
        /// <returns>debtcounselling.debtCounselling.*</returns>
        public QueryResults GetReferenceByDebtCounsellingKey(int debtCounsellingKey)
        {
            string query =
                String.Format(@"select * from debtcounselling.debtcounselling dc where dc.debtcounsellingkey = {0}", debtCounsellingKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Adds PDA test data in OrganisationStructure
        /// </summary>
        /// <returns>Success or failure indicator</returns>
        public void AddAndMaintainPDATestDataInOrganisationStructure()
        {
            var procedure = new SQLStoredProcedure
                {
                    Name = @"test.AddAndMaintainPDATestDataInOrganisationStructure"
                };
            dataContext.ExecuteStoredProcedure(procedure);
        }

        /// <summary>
        /// Gets the list of legal entities on the accounts that we want to create debt counselling cases for.
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>test.DebtCounsellingLegalEntities.*</returns>
        public QueryResults GetLegalEntitiesForDebtCounsellingCaseCreate(string testIdentifier)
        {
            string query =
                string.Format(@"select le.* from test.debtcounsellingtestcases t
								join test.DebtCounsellingAccounts a on t.testidentifier=a.testidentifier
								join test.DebtCounsellingLegalEntities le on a.accountkey=le.accountkey
								where t.testidentifier ='{0}'", testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns contents of the debtcounsellingtestcases table for a test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>debtcounsellingtestcases.*</returns>
        public QueryResults GetDebtCounsellingTestCases(string testIdentifier)
        {
            string query =
                string.Format(@"select * from test.debtcounsellingtestcases	where testidentifier = '{0}'", testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns contents of the DebtCounsellingAccounts table for a test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>DebtCounsellingAccounts.*</returns>
        public QueryResults GetAccountsForDCTestIdentifier(string testIdentifier)
        {
            string query =
                string.Format(@"select * from test.DebtCounsellingAccounts where testidentifier = '{0}'", testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// saves the debt counselling keys for a test case
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        public void SaveDebtCounsellingkeys(string testIdentifier)
        {
            string query =
                string.Format(@"update dc
								set dc.debtCounsellingKey=data.debtCounsellingKey
								from test.debtcounsellingAccounts dc
								join debtCounselling.debtCounselling data on dc.accountkey=data.accountkey and data.debtcounsellingstatuskey=1
								where testIdentifier = '{0}'", testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Returns the debt counselling key and legalentity for a test identifier
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <param name="underDebtCounselling">1=LE added as Client Role, 0=LE not added as client role</param>
        /// <returns></returns>
        public QueryResults GetLegalEntitiesForCaseCreationAssertion(string testIdentifier, int underDebtCounselling)
        {
            string query =
                string.Format(@"select isnull(debtcounsellingkey,0) as debtcounsellingkey, legalentitykey from test.debtcounsellingAccounts a
								join test.debtcounsellinglegalentities le on a.accountkey=le.accountkey
								where underdebtcounselling = {0}
								and testIdentifier = '{1}'", underDebtCounselling, testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// fetches the min value of the underDebtCounselling indicator for a given test case
        /// </summary>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns></returns>
        public int MinDebtCounselling(string testIdentifier)
        {
            string query =
                string.Format(@"select min(cast(underdebtcounselling as int))
								from test.debtcounsellingtestcases test
								join test.debtcounsellingaccounts acc on test.testidentifier=acc.testidentifier
								join test.debtcounsellinglegalentities le on acc.accountkey=le.accountkey
								where test.testidentifier= '{0}'", testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// This will update all debt counselling legal entities for a test case, given the parameters.
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="underDebtCounselling"></param>
        public void UpdateDebtCounsellingLegalEntity(string testIdentifier, bool underDebtCounselling)
        {
            int underDebt = 0;
            if (underDebtCounselling)
                underDebt = 1;
            string query =
              string.Format(@" update test.DebtCounsellingLegalEntities
							   set DebtCounsellingLegalEntities.UnderDebtCounselling={0}
							   where DebtCounsellingLegalEntities.LegalEntityKey in
							   (
								   select DebtCounsellingLegalEntities.LegalEntityKey from test.DebtCounsellingAccounts
									   inner join test.DebtCounsellingLegalEntities
										   on DebtCounsellingAccounts.AccountKey = DebtCounsellingLegalEntities.AccountKey
								   where DebtCounsellingAccounts.TestIdentifier='{1}'
							   )", underDebt, testIdentifier);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Returns a legal entity for whom a debt counselling case can be created
        /// </summary>
        /// <param name="bHasEWorkLossControlCase"></param>
        /// <param name="countOfCases"></param>
        /// <param name="hasArrearBalance"></param>
        /// <param name="product"></param>
        /// <param name="isInterestOnly"></param>
        /// <returns></returns>
        public QueryResults GetLegalEntityForDebtCounsellingCreate(bool bHasEWorkLossControlCase, int countOfCases, bool hasArrearBalance, ProductEnum product,
            bool isInterestOnly)
        {
            int iHasEWorkLossControlCase = bHasEWorkLossControlCase ? 1 : 0;
            int ihasArrearBalance = hasArrearBalance ? 1 : 0;
            int interestOnly = isInterestOnly ? 1 : 0;

            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.GetLEForDebtCounsellingCreate" };
            proc.AddParameter(new SqlParameter("@lossControlCase", iHasEWorkLossControlCase.ToString()));
            proc.AddParameter(new SqlParameter("@CountofAccounts", countOfCases.ToString()));
            proc.AddParameter(new SqlParameter("@HasArrearBalance", ihasArrearBalance.ToString()));
            proc.AddParameter(new SqlParameter("@ProductKey", ((int)product).ToString()));
            proc.AddParameter(new SqlParameter("@InterestOnly", interestOnly.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Get the DebtCounsellingKey from debtcounselling.DebtCounselling table by AccountKey
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>debtcounselling.DebtCounselling.DebtCounsellingKey</returns>
        public QueryResults GetDebtCounsellingByAccountKey(int accountKey)
        {
            string query =
                string.Format(@"select * from debtcounselling.debtCounselling where accountkey = {0}", accountKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the DebtCounsellingKey from debtcounselling.DebtCounselling table by AccountKey
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="status">DebtCounsellingStatus. Using NONE will bring back cases of any status back.</param>
        /// <returns>debtcounselling.DebtCounselling.DebtCounsellingKey</returns>
        public QueryResults GetDebtCounsellingByAccountKeyAndStatus(int accountKey, DebtCounsellingStatusEnum status = DebtCounsellingStatusEnum.Open)
        {
            string query =
                status == DebtCounsellingStatusEnum.None ? string.Format(@"select * from debtcounselling.debtCounselling where accountkey = {0}", accountKey)
                                                     : string.Format(@"select * from debtcounselling.debtcounselling where accountkey = {0}
																		and debtcounsellingstatuskey = {1}", accountKey, (int)status);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get Proposal data from debtcounselling.Proposal table by AccountKey and ProposalStatusKey
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>debtcounselling.Proposal</returns>
        public QueryResults GetLatestProposalRecordByAccountKey(int accountKey, DebtCounsellingStatusEnum status)
        {
            string query = string.Format(@"select top 1 p.*
										from debtcounselling.DebtCounsellingGroup AS dcg
										join debtcounselling.DebtCounselling dc ON dcg.DebtCounsellingGroupKey = dc.DebtCounsellingGroupKey
										join debtcounselling.Proposal AS p ON dc.DebtCounsellingKey = p.DebtCounsellingKey
										where dc.AccountKey = {0} and dc.debtcounsellingstatuskey = {1}
										order by p.CreateDate desc", accountKey, (int)status);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Updates the ework loss control user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userFullName"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public QueryResults InsertEWorkLossControlUser(string userName, string userFullName, string emailAddress)
        {
            string query = string.Format(@"Declare	@UserName varchar(50) ,
												@UserFullName varchar(100) ,
												@ReportsTo varchar(50) ,
												@Natal1Pwd varchar(100) ,
												@EmailAddress varchar(100),
												@RoleName varchar(50)

										Set @UserName =  '{0}'
										Set @UserFullName = '{1}'
										Set @ReportsTo = 'IT'
										Set @Natal1Pwd = '2FF391A6090D11041328F8C00B8664F1'
										Set @EmailAddress = '{2}'

										-- Add/Update the eUser record
										if (not exists(select * from [e-work]..eUser (nolock) where eUserName = @UserName))
											insert into [e-work]..eUser (eUserName,epassword,eReportsTo,eEMailAddress) values (@UserName,@Natal1Pwd,@ReportsTo,@EmailAddress)
										else
											update [e-work]..eUser set epassword = @Natal1Pwd, eReportsTo = @ReportsTo, eEMailAddress = @EmailAddress where eUserName = @UserName

										-- Add the eAssignment records
										set @RoleName = @UserName
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'everybody'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'LossControl'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'Collections Non Subsidy'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'Collections Subsidy'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'DebtCounsellingBackup'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'DebtCounsellingClerk'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'DebtCounsellingManager'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')
										set @RoleName = 'DebtCounsellingSupervisor'
										if (not exists(select * from [e-work]..eAssignment (nolock) where eUserName = @UserName and eRoleName = @RoleName))
											insert into [e-work]..eAssignment (eUserName,eRoleName,eFolderID) values (@UserName,@RoleName,'')

										-- Add/Update user in the [sahldb]..EWorkUserDetails table
										if (not exists(select * from [sahldb]..EWorkUserDetails (nolock) where UserName = @UserName))
											insert into [sahldb]..EWorkUserDetails (UserName, UserFullName,UserEmail, UserEmailAddress) values (@UserName,@UserFullName, @EmailAddress, @EmailAddress)
										else
											update [sahldb]..EWorkUserDetails set UserFullName = @UserFullName,UserEmail = @EmailAddress,UserEmailAddress = @EmailAddress  where UserName = @UserName", userName, userFullName, emailAddress);

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets a list of debt counselling
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public QueryResults GetDCAccountsByIDNumber(string idNumber)
        {
            string query =
                string.Format(@"select
	                                dc.accountKey as AccountKey,
	                                dc.debtCounsellingKey as DCKey,
	                                dc.debtCounsellingGroupKey as DCGKey,
	                                acc.rrr_productkey as ProductKey
                                from [2am].debtCounselling.debtCounselling dc with (nolock)
	                                join [2am].dbo.externalRole er with (nolock)
	                                on dc.debtcounsellingkey=er.genericKey
	                                join [2am].dbo.legalEntity le with (nolock)
	                                on er.legalEntityKey=le.legalEntityKey
	                                join [2am].dbo.account acc with (nolock)
	                                on acc.accountkey = dc.accountKey
                                where genericKeyTypeKey=27
                                and er.externalRoleTypeKey=1
								and le.idNumber='{0}' and dc.debtcounsellingstatuskey=1", idNumber);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Ensures that only test users are marked as active.
        /// </summary>
        public void MakeDebtCounsellingBusinessUsersInactive()
        {
            SQLStoredProcedure p = new SQLStoredProcedure { Name = "test.SetupDCTestUsers" };
            dataContext.ExecuteStoredProcedure(p);
        }

        /// <summary>
        /// Gets a debt counselling case in a particular state assigned to a user in the workflow role provided.
        /// </summary>
        /// <param name="workflowState">Workflow State</param>
        /// <param name="aduserName">ADUserName</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.DebtCounselling> GetDebtCounsellingCaseByStateAndWorkflowRoleType(string workflowState, string aduserName)
        {
            var query = string.Empty;
            if (workflowState == WorkflowStates.DebtCounsellingWF.DebtReviewApproved)
            {
                query =
                    string.Format(@"select d.debtcounsellingkey, i.id as InstanceID, d.accountkey, adusername, a.rrr_productKey as ProductKey, isnull(InterestOnly.Indicator, 0) as InterestOnly
                            from x2.x2data.debt_counselling d with (nolock)
                            join [2am].debtcounselling.DebtCounselling dc with (nolock) on d.debtCounsellingKey = dc.debtCounsellingKey
                            join [2am].dbo.Account a with (nolock) on dc.AccountKey=a.AccountKey
                            join x2.x2.instance i with (nolock) on d.instanceid=i.id
                            join x2.x2.state s with (nolock) on i.stateid=s.id
                            join x2.x2.workflowRoleAssignment w with (nolock) on i.id=w.instanceid and generalstatuskey=1
                            join [2am].dbo.workflowRoleTypeOrganisationStructureMapping map with (nolock) on
                            w.workflowRoleTypeOrganisationStructureMappingKey=map.workflowRoleTypeOrganisationStructureMappingKey
                            join [2am]..aduser ad on w.aduserkey=ad.aduserkey
                            left join (select 1 as Indicator, a.accountKey from dbo.Account a
                            join dbo.financialService fs on a.accountKey=fs.accountKey
	                            and fs.financialServiceTypeKey = 1
                            join fin.financialAdjustment fa on fs.financialServiceKey = fa.financialServiceKey
                            join product.FinancialServiceAttribute fsa on fa.financialservicekey=fsa.financialservicekey
                            join product.InterestOnly i on fsa.financialServiceAttributeKey = i.financialServiceAttributeKey
	                            and generalStatusKey=1
                            where fa.financialAdjustmentSourceKey = 6 and fa.financialAdjustmentStatusKey = 1 and a.accountStatusKey = 1
                            ) as interestOnly on a.accountKey = interestOnly.AccountKey
                            where s.name='{0}'
                            and ad.adusername='{1}'
                            and a.accountkey not in (select accountKey from debtcounselling.debtcounselling
                            where debtcounsellingstatuskey = 1 group by accountkey having count(accountKey) > 1)
                            order by newid()", workflowState, aduserName);
            }
            else
            {
                query =
                 string.Format(@"select d.debtcounsellingKey, i.id as InstanceID, d.accountKey, w.adusername, a.rrr_productKey as ProductKey, isnull(InterestOnly.Indicator, 0) as InterestOnly
                            from x2.x2data.debt_counselling d with (nolock)
                            join x2.x2.instance i with (nolock) on d.instanceid=i.id
                            join x2.x2.state s with (nolock) on i.stateid=s.id
                            join x2.x2.worklist w with (nolock) on i.id=w.instanceid
                            join [2am].debtcounselling.DebtCounselling dc with (nolock) on d.debtCounsellingKey = dc.debtCounsellingKey
                            join [2am].dbo.Account a with (nolock) on dc.AccountKey=a.AccountKey
                            left join (select 1 as Indicator, a.accountKey from dbo.Account a
                            join dbo.financialService fs on a.accountKey=fs.accountKey
	                            and fs.financialServiceTypeKey = 1
                            join fin.financialAdjustment fa on fs.financialServiceKey = fa.financialServiceKey
                            join product.FinancialServiceAttribute fsa on fa.financialservicekey=fsa.financialservicekey
                            join product.InterestOnly i on fsa.financialServiceAttributeKey = i.financialServiceAttributeKey
	                            and generalStatusKey=1
                            where fa.financialAdjustmentSourceKey = 6 and fa.financialAdjustmentStatusKey = 1 and a.accountStatusKey = 1
                            ) as interestOnly on a.accountKey = interestOnly.AccountKey
                            where s.name='{0}'
                            and w.adusername='{1}'
                            and a.accountkey not in (select accountKey from debtcounselling.debtcounselling
                            where debtcounsellingstatuskey = 1 group by accountkey having count(accountKey) > 1)
                            order by newid()", workflowState, aduserName);
            }
            return dataContext.Query<Automation.DataModels.DebtCounselling>(query);
        }

        /// <summary>
        /// Gets the contents of the debtCounsellorDetails table
        /// </summary>
        /// <param name="legalEntityKey">legalEntityKey of the debt counsellor.</param>
        /// <returns></returns>
        public QueryResults GetDebtCounsellorDetails(int legalEntityKey)
        {
            var q =
                string.Format(@"Select * from debtcounselling.DebtCounsellorDetail where legalEntityKey={0}", legalEntityKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Gets one or more debt counselling case for each particular workflowstate, and assigned to a user in the workflow role provided.
        /// </summary>
        /// <param name="workflowState">Workflow State</param>
        /// <param name="workflowRoleType">Workflow Role Type</param>
        /// <returns></returns>
        public QueryResults GetDebtCounsellingCaseByWorkflowRoleTypeForEachState(WorkflowRoleTypeEnum workflowRoleType, params string[] workflowState)
        {
            string workflowStateStr = "'";
            workflowStateStr += Helpers.GetDelimitedString<string>(workflowState, ",");
            workflowStateStr = workflowStateStr.Replace(",", "','");
            workflowStateStr += "'";

            var q =
                string.Format(@"select top {0} d.debtcounsellingKey, i.id, d.accountKey, w.adusername, s.Name as statename
								from x2.x2data.debt_counselling d with (nolock)
								join x2.x2.instance i with (nolock) on d.instanceid=i.id
								join x2.x2.state s with (nolock) on i.stateid=s.id
								join x2.x2.worklist w with (nolock) on i.id=w.instanceid
								where s.name in ({1})
								and w.adusername in (
								select aduserName from adUser a with (nolock)
								join userOrganisationStructure uos with (nolock)
								on a.aduserkey=uos.aduserKey and uos.generalStatusKey=1
								where uos.generickeytypekey=34
								)", workflowState.Length, workflowStateStr, (int)workflowRoleType);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Get a random account that is under debtcounselling.
        /// </summary>
        /// <param name="status">Debt Counselling Status</param>
        /// <returns></returns>
        public int GetRandomDebtCounsellingAccount(DebtCounsellingStatusEnum status)
        {
            string query = status == DebtCounsellingStatusEnum.None ?
                @"select top 01 dc.accountkey from [2am].debtcounselling.debtcounselling dc
				join [2am].dbo.Account a on dc.AccountKey = a.AccountKey and a.accountStatusKey = 1 order by newid()"
                : string.Format(@"select top 1 dc.accountkey from [2am].debtcounselling.debtcounselling dc
				join [2am].dbo.Account a on dc.AccountKey = a.AccountKey and a.accountStatusKey = 1 where dc.debtcounsellingStatusKey={0}
					order by newid()", (int)status);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return Int32.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// This method will get the account, financialservice and RateOverrides snapshot data that was created before opt in to standard.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        public QueryResults GetDebtCounsellingAccountSnapShot(int debtCounsellingKey)
        {
            var query = String.Format(@"select * from [debtcounselling].[SnapShotAccount]
				join [debtcounselling].[SnapShotFinancialService]
				on  [SnapShotAccount].[SnapShotAccountKey] = [SnapShotFinancialService].[SnapShotAccountKey]
				left join dbo.valuation on [SnapShotAccount].valuationkey = valuation.valuationkey
				left join [debtcounselling].[SnapShotFinancialAdjustment]
				on [SnapShotFinancialService].[SnapShotFinancialServiceKey] = [SnapShotFinancialAdjustment].[SnapShotFinancialServiceKey]
				where [debtcounselling].[SnapShotAccount].debtcounsellingkey = {0}", debtCounsellingKey);
            SQLStatement s = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Fetches a list of accounts under debt counselling linked to the specified debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <returns></returns>
        public QueryResults GetGroupedAccounts(int debtCounsellingKey)
        {
            var q = String.Format(@"select distinct dc2.accountKey
									from [2AM].debtcounselling.debtCounselling dc
									join [2AM].debtcounselling.debtCounselling dc2
										on dc.debtCounsellingGroupKey=dc2.debtCounsellingGroupKey
									where dc.debtCounsellingKey={0}
									and dc2.debtCounsellingStatusKey=1
									and dc2.accountKey<>dc.accountKey", debtCounsellingKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Fetches a list of accounts at a debt counselling stage within the e-work loss control.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetEworkCaseAtDebtCounsellingStage(string LossControlSubMap = "None")
        {
            string q = string.Empty;
            switch (LossControlSubMap)
            {
                case "Litigation":
                    q = String.Format(@"select lossControlEworkCases.accountKey from [2am].test.lossControlEworkCases
										join [2am].dbo.account a on lossControlEworkCases.accountKey = a.accountKey and a.accountStatusKey=1
										where userToDo in (
										select adusername from [2am].dbo.aduser a
										join [2am].dbo.userOrganisationStructure uos on a.aduserkey=uos.aduserkey
										where generickey=6
										and generickeytypekey=34
										) order by efoldername");
                    break;

                default:
                    q = String.Format(@"select lossControlEworkCases.accountKey from [2AM].test.lossControleWorkCases
									join [2am].dbo.account a on lossControlEworkCases.accountKey = a.accountKey and a.accountStatusKey=1
									where eStageName in (
									'Debt Counselling',
									'Debt Counselling (Arrears)',
									'Debt Counselling (Collections)',
									'Debt Counselling (Estates)',
									'Debt Counselling (Facilitation)',
									'Debt Counselling (Seq)')");
                    break;
            }
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Check for cases at Debt Review Approved where the arrears tolerance level has been breached and inserts a record into the ActiveExternalActivity table to
        /// move the case to Default in Payment. It also checks for cases at Default in Payment where the arrears have been paid and inserts a record into the
        /// ActiveExternalActivity table to move the case back to Debt Review Approved.
        /// </summary>
        public void DebtCounsellingArrears()
        {
            string query =
                string.Format(@"EXECUTE [2AM].dbo.pDebtCounsellingArrears");
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtcounsellingkey"></param>
        /// <returns></returns>
        public DateTime GetDebtCounsellingDiaryDate(int debtcounsellingkey)
        {
            string query = string.Format(@"select debtCounselling.diarydate from debtCounselling.debtCounselling
										   where debtCounsellingkey = {0}", debtcounsellingkey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return DateTime.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Fetches a list of accounts by product for debt counselling.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="isInterestOnly"></param>
        /// <returns></returns>
        public QueryResults GetAccountsForDebtCounsellingByProduct(ProductEnum product, bool isInterestOnly)
        {
            var interestOnly = isInterestOnly ? 1 : 0;
            var query = String.Format(@"select
									cases.accountKey
									from test.AutomationDebtCounsellingTestCases cases
									left join debtcounselling.debtCounselling dc
										on cases.accountKey=dc.accountKey
									where dc.debtCounsellingKey is null
									and cases.productKey={0} and cases.isInterestOnly = {1}", (int)product, interestOnly);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public List<int> AssignDebtCounsellingCasesToAttorney(int legalEntityKey)
        {
            var query = string.Format(@"declare @keys table (debtCounsellingKey int)
									insert into @keys
									select top 10 debtCounsellingKey from debtcounselling.debtcounselling
									where debtcounsellingstatuskey=1
									order by newid() desc
									--remove the existing attorney roles
									delete from [2am].dbo.ExternalRole
									where GenericKey in (select debtCounsellingKey from @keys)
									and externalRoleTypeKey=5
									--insert our new ones
									insert into [2am].dbo.ExternalRole
									(genericKey, genericKeyTypeKey, legalEntityKey,
									externalRoleTypeKey, generalStatusKey, ChangeDate)
									select debtCounsellingKey,{0},{1},{2},1,getdate() from @keys

									select accountKey from @keys k
									join debtCounselling.debtCounselling dc on k.debtCounsellingKey=dc.debtCounsellingKey
									and debtCounsellingStatusKey=1", (int)GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, legalEntityKey,
                                                                   (int)ExternalRoleTypeEnum.LitigationAttorney);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement).GetColumnValueList<int>("accountKey");
        }

        /// <summary>
        /// Fetches the proposed remaining term of the debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="proposalEndDate">Debt Counselling Proposal End Date</param>
        /// <returns>Proposed Debt Counselling Remaining Term</returns>
        public QueryResults GetDebtCounsellingRemainingTerm(int debtCounsellingKey, DateTime proposalEndDate)
        {
            var query = String.Format(@"select
									lb.remainingInstalments + datediff(mm, dateadd(mm, lb.remainingInstalments,getdate()),'{0}') as RemainingTerm
									from debtcounselling.debtCounselling dc
									join [2AM].dbo.account a
										on dc.accountKey=a.accountKey
									join [2AM].dbo.financialService fs
										on a.accountKey=fs.accountKey
										and fs.accountStatusKey=1
										and fs.financialServiceTypeKey=1
									join [2AM].fin.mortgageLoan ml
										on fs.financialServiceKey=ml.financialServiceKey
									join [2am].fin.loanbalance lb
										on lb.financialservicekey=ml.financialservicekey
									where dc.debtCounsellingKey={1}", proposalEndDate.ToString(Formats.DateTimeFormatSQL), debtCounsellingKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isArchivedCases"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.DebtCounselling> GetDebtCounsellingRecords(bool isArchivedCases = false, bool includeDebtReviewApproved = false, int accountKey = 0, int debtCounsellingKey = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@isArchivedCases", value: isArchivedCases, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@includeDebtReviewApproved", value: includeDebtReviewApproved, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@accountKey", value: accountKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@debtCounsellingKey", value: debtCounsellingKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.DebtCounselling,
                                     Automation.DataModels.Proposal,
                                     Automation.DataModels.Account,
                                     Automation.DataModels.SnapshotAccount,
                                     Automation.DataModels.DebtCounselling>("test.GetDebtCounsellingRecords",
                          (debtcounselling, proposal, account, snapshotaccount) =>
                          {
                              debtcounselling.Proposal = proposal;
                              debtcounselling.Account = account;
                              debtcounselling.SnapshotAccount = snapshotaccount;
                              return debtcounselling;
                          }, parameters: parameters, commandtype: System.Data.CommandType.StoredProcedure,
                          splitOn: "proposalkey,AccountKey,SnapShotAccountKey");
        }

        /// <summary>
        /// Gets the account key of debt counselling case with more than one LE on it.
        /// </summary>
        /// <param name="workflowStates"></param>
        /// <returns></returns>
        public int GetDebtCounsellingCaseWithMoreThanOneLegalEntity(string[] users, params string[] workflowStates)
        {
            string states = Helpers.GetDelimitedString<string>(workflowStates, ",");
            states = states.Replace(",", "','");
            string userList = Helpers.GetDelimitedString<string>(users, ",");
            userList = userList.Replace(",", "','");
            string query =
                string.Format(@"select top 1 dc.accountkey, count(er.externalRoleKey)
                                from [2am].debtcounselling.debtcounselling dc
								join [2am].dbo.externalRole er on dc.debtcounsellingKey=er.genericKey
								    and er.externalRoleTypeKey = 1
								join [x2].[x2data].debt_counselling data on dc.debtcounsellingKey=data.debtcounsellingKey
								join x2.x2.instance i on data.instanceid=i.id
                                    and parentinstanceid is null
								join x2.x2.state s on i.stateid=s.id
                                     and s.name in ('{0}')
								join x2.x2.workflowRoleAssignment wra on i.id=wra.instanceid
                                    and wra.generalstatuskey=1
                                join [2am].dbo.AdUser a on wra.aduserKey = a.aduserKey
                                    and a.adusername in ('{1}')
								where dc.debtCounsellingStatusKey = 1
								group by dc.accountkey, dc.debtCounsellingKey
								having count(er.externalRoleKey) > 1
                                order by newid()", states, userList);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column("accountkey").GetValueAs<int>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetNCRNumber()
        {
            string query =
                string.Format(@"select top 1 NCRDCRegistrationNumber
								from [2am].debtcounselling.debtcounsellordetail d
								join [2am].dbo.legalentityOrganisationStructure leos on d.legalentitykey=leos.legalentitykey
								where len(NCRDCRegistrationNumber) > 0
								order by newid()");
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Gets the legal entity key for our debt counsellor
        /// </summary>
        /// <param name="ncrdcNumber">NCRDCRegistrationNumber</param>
        /// <returns>debtcounsellordetail.LegalEntityKey</returns>
        public int GetDebtCounsellorLegalEntityKey(string ncrdcNumber)
        {
            string query =
                 string.Format(@"select legalentitykey from [2am].debtcounselling.debtcounsellordetail
								where NCRDCRegistrationNumber = '{0}'", ncrdcNumber);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return Int32.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Updates the currently assigned user on the test.DebtCounsellingTestCases table
        /// </summary>
        /// <param name="adUserName"></param>
        /// <param name="testIdentifier"></param>
        public void UpdateCurrentCaseOwner(string adUserName, string testIdentifier)
        {
            string query =
                string.Format(@"update test.debtcounsellingTestCases
									set CurrentCaseOwner = '{0}'
									where testIdentifier = '{1}'
									", adUserName, testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="debtcounsellingkey"></param>
        /// <returns></returns>
        public Automation.DataModels.DebtCounselling GetDebtCounsellingAccount(int debtcounsellingkey)
        {
            string query = String.Format(@"select * from debtCounselling.debtCounselling
								where debtCounsellingkey = {0}", debtcounsellingkey);
            var accountEnum = dataContext.Query<Automation.DataModels.DebtCounselling>(query).GetEnumerator();
            accountEnum.MoveNext();
            return accountEnum.Current;
        }

        /// <summary>
        /// Get [2am].debtcounselling.SnapShotAccount by AccountKey
        /// </summary>
        /// <param name="accountKey">filter by AccountKey</param>
        /// <returns>Single SnapShotAccount record or null if no records returned</returns>
        public Automation.DataModels.SnapshotAccount GetSnapShotAccountByAccountKey(int accountKey)
        {
            var sql = string.Format(@"Select * from [2am].debtcounselling.SnapShotAccount where AccountKey = {0}", accountKey);

            return dataContext.Query<Automation.DataModels.SnapshotAccount>(sql).SingleOrDefault();
        }

        /// <summary>
        /// Get [2am].debtcounselling.SnapShotAccount by DebtCounsellingKey
        /// </summary>
        /// <param name="debtCounsellingkey">filter by DebtCounsellingKey</param>
        /// <returns>Single SnapShotAccount record or null if no records returned</returns>
        public Automation.DataModels.SnapshotAccount GetSnapShotAccountByDebtCounsellingKey(int debtCounsellingkey)
        {
            var sql = string.Format(@"Select * from [2am].debtcounselling.SnapShotAccount where DebtCounsellingKey = {0}", debtCounsellingkey);

            return dataContext.Query<Automation.DataModels.SnapshotAccount>(sql).SingleOrDefault();
        }

        /// <summary>
        /// Fetches a list of accounts at a debt counselling stage within the e-work loss control.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetDebtCounsellingEworkCaseWithNoDebtCounsellingCase()
        {
            string query = String.Format(@"select distinct loannumber as accountkey
                            from [e-work]..losscontrol	lc
                            join [2am].dbo.account a on lc.loannumber = a.accountkey
	                            and a.accountstatuskey = 1
                            join [2am].dbo.Role r on a.accountKey = r.AccountKey
	                            and r.roleTypeKey = 2
	                            and r.generalstatusKey = 1
                            join [2am].dbo.LegalEntity le on r.legalEntityKey = le.legalEntityKey
	                            and le.legalEntityTypeKey = 2
	                            and idnumber is not null
                            join [e-work]..efolder f on lc.efolderid = f.efolderid
                            left join debtcounselling.debtcounselling dc on a.accountkey = dc.accountkey and dc.debtcounsellingstatuskey = 1
                            join (select distinct estagename from [e-work].dbo.eAction where eactionname = 'x2 debt counselling') ea on f.estagename = ea.estagename
                            where dc.accountkey is null");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns a set of test identifiers to be used for debt counselling create tests.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetDebtCounsellingCreateCases()
        {
            string query = @"select * from [2AM].test.debtcounsellingtestcases where testgroup = 'Create'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns a set of test identifiers to be used for debt counselling search tsets.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetDebtCounsellingSearchCases()
        {
            string query = @"select * from [2AM].test.debtcounsellingtestcases where testgroup = 'Search'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns a personal loan account under debt counselling.
        /// </summary>
        /// <returns></returns>
        public int GetPersonalLoanAccountUnderDebtCounselling()
        {
            string query = @"select top 1 d.accountkey from [2AM].debtcounselling.debtcounselling d
                            join [2AM].dbo.account a
	                            on d.accountkey=a.accountkey
	                            and a.rrr_productkey=12
                            where d.debtcounsellingstatuskey=1
                            and a.accountstatuskey=1
                            order by newid()";
            SQLStatement statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return Int32.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Updates the ework assigned user to a specific user.
        /// </summary>
        /// <param name="userToDo">User to update to</param>
        /// <param name="accountKey">accountKey</param>
        /// <param name="eStageName">Ework stage</param>
        public void UpdateEworkAssignedUser(int accountKey, string userToDo, string eStageName)
        {
            userToDo = userToDo.Replace(@"SAHL\", string.Empty);
            string query =
                string.Format(@"update loss
								set loss.userToDo = '{0}'
								from
								[e-work]..efolder e
								join [e-work]..lossControl loss on e.efolderid=loss.efolderid
								and e.eStageName='{1}'
								where eFolderName='{2}'", userToDo, eStageName, accountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}