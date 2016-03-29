using Common.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public void InsertITC(int offerKey, int empiricaUpperBound, int empiricaLowerBound)
        {
            InsertITCv4(offerKey, empiricaUpperBound, empiricaLowerBound);
        }

        /// <summary>
        ///   Executes the GetNextRRAssignmentByORTKey SP to retrieve the next user we expect to receive a case in the round robin
        ///   assignment for an Offer Role Type
        /// </summary>
        /// <param name = "offerRoleTypeKey">OfferRoleTypeKey</param>
        /// <returns>The next user we expect to get RR assigned</returns>
        public QueryResults GetNextRoundRobinAssignment(OfferRoleTypeEnum offerRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.GetNextRRAssignmentByORTKey" };
            proc.AddParameter(new SqlParameter("@OfferRoleTypeKey", ((int)offerRoleTypeKey).ToString()));
            proc.AddParameter(new SqlParameter("@RoundRobinPointerKey", ((int)roundRobinPointer).ToString()));
            QueryResults results = dataContext.ExecuteStoredProcedureWithResults(proc);
            return results;
        }

        /// <summary>
        ///   Returns a set of FL Offers for the automated tests
        /// </summary>
        /// <returns>AutomationFLTestCases.*</returns>
        public QueryResults GetFLAutomationOffers()
        {
            string query = @"SELECT * FROM test.AutomationFLTestCases where testGroup not like '%existing%'
                            and testIdentifier not like '%SuperLoSPVChange%'
                            and testIdentifier not like '%SuperLoNoOptOut%'
                            and testIdentifier not like '%SuperLoGreaterThan85Percent%'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns a set of FL Offers for the automated tests required for the Super Lo Opt Out Tests
        /// </summary>
        /// <returns>AutomationFLTestCases.*</returns>
        public QueryResults GetSuperLoOptFLAutomationOffers()
        {
            string query = @"SELECT * FROM test.AutomationFLTestCases where
                            testIdentifier like '%SuperLoSPVChange%'
                            or testIdentifier like '%SuperLoNoOptOut%'
                            or testIdentifier like '%SuperLoGreaterThan85Percent%'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns a single record from the AutomationFLTestCases when provided with a Test Identifier
        /// </summary>
        /// <param name = "identifier">Test Identifier</param>
        /// <returns>AutomationFLTestCases.*</returns>
        public QueryResults GetFLAutomationRowByTestIdentifier(string identifier)
        {
            string query = @"SELECT * FROM test.AutomationFLTestCases with (nolock) WHERE TestIdentifier ='" +
                           identifier + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Updates the test.AutomationFLTestCases table
        /// </summary>
        /// <param name = "colToUpdate">Column to be updated</param>
        /// <param name = "newValue">New Value</param>
        /// <param name = "identifier">Row Identifier</param>
        public void UpdateFLAutomation(string colToUpdate, string newValue, string identifier)
        {
            string sql = string.Format(@"Update [2AM].test.AutomationFLTestCases Set {0} = '{1}' where testidentifier = '{2}'", colToUpdate, newValue, identifier);
            SQLStatement st = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(st);
        }

        /// <summary>
        ///   Updates the OfferKeys for our Test Case
        /// </summary>
        /// <param name = "accountKey">Mortgage Loan Account Key</param>
        public void InsertFLOfferKeys(int accountKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "[2am].[test].UpdateFLAutomationOfferKeys" };
            proc.AddParameter(new SqlParameter("@AccountKey", accountKey));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        ///   Retrieves the contents of the OffersAtApplicationCapture table
        /// </summary>
        /// <returns>OffersAtApplicationCapture.*</returns>
        public QueryResults GetTestData()
        {
            const string query = @"Select * from [2am].test.OffersAtApplicationCapture where TestGroup not in ('CreditScoring','CreditScoringRules') order by Username";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves a row from the OffersAtApplicationCapture table using the TestIdentifier
        /// </summary>
        /// <param name = "testIdentifier">Test Identifier</param>
        /// <returns>OffersAtApplicationCapture.*</returns>
        public Automation.DataModels.OriginationTestCase GetTestDataByTestIdentifier(string testIdentifier)
        {
            string query = string.Format(@"Select * from [2am].test.OffersAtApplicationCapture with (nolock) where testIdentifier = '{0}'", testIdentifier);
            return dataContext.Query<Automation.DataModels.OriginationTestCase>(query).First();
        }

        /// <summary>
        ///   Retrieves the contents of the AutomationLeads table
        /// </summary>
        /// <returns>AutomationLeads.*</returns>
        public QueryResults GetAutomationLeads(string tableName)
        {
            string query = @"Select * from [2am].test." + tableName;
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Updates the AutomationLeads table
        /// </summary>
        /// <param name = "testIdentifier">TestIdentifier of row to update</param>
        /// <param name = "columnName">Column to Update</param>
        /// <param name = "value">New Value</param>
        /// <param name="tableName"></param>
        public void CommitTestDataAutomationLeads(string testIdentifier, string columnName, string value,
                                                  string tableName)
        {
            string query = @"Update [2am].test." + tableName + " Set " + columnName + " = '" + value +
                           "' where testIdentifier = '" + testIdentifier + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves a row from the AutomationLeads table using the TestIdentifier
        /// </summary>
        /// <param name = "testIdentifier">Identifier</param>
        /// <returns>AutomationLeads.*</returns>
        public Automation.DataModels.OriginationTestCase GetTestDataAutomationLeads(string testIdentifier)
        {
            string query = string.Format(@"Select origConsultant as Username, * from [2am].test.AutomationLeads where testIdentifier = '{0}'", testIdentifier);
            return dataContext.Query<Automation.DataModels.OriginationTestCase>(query).First();
        }

        /// <summary>
        ///   Returns a row from a test data table when provided with the table name (not including the test schema)
        ///   and the TestIdentifier of the row
        /// </summary>
        /// <param name = "testIdentifier">TestIdentifier</param>
        /// <param name = "tableName">TableName</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetTestData(string testIdentifier, string tableName)
        {
            string query = @"Select * from [2am].test." + tableName + " where testIdentifier = '" + testIdentifier + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns a row from a test data table when provided with the table name (not including the test schema)
        ///   ,the Column on which a condition will be applied and the Expected Value of the condition
        /// </summary>
        /// <param name = "tableName">TableName</param>
        /// <param name="conditionColumnName">Column name of the column on which a condition will be applied</param>
        /// <param name="conditionColumnValue">The expected value of the condition</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetTestData(string tableName, string conditionColumnName, string conditionColumnValue)
        {
            string query = @"Select * from [2am].test." + tableName + " where " + conditionColumnName + " = '" + conditionColumnValue + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves a row from the OffersAtApplicationCapture table using the TestIdentifier
        /// </summary>
        /// <param name="testIdentifierColumnName">OffersAtApplicationCapture Column Name</param>
        /// <param name="testIdentifierValue">OffersAtApplicationCapture Column Value</param>
        /// <returns>OffersAtApplicationCapture.*</returns>
        public QueryResults GetTestDataByTestIdentifier(string testIdentifierColumnName, string testIdentifierValue)
        {
            string query = @"Select * from [2am].test.OffersAtApplicationCapture with (nolock) where " +
                           testIdentifierColumnName + " = '" + testIdentifierValue + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Executes the test.CleanUpFlOfferData that will clean up the marital status, employment records, application declarations
        ///   and the Inspection Contact Detail for the property
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        public void CleanUpOfferData(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.CleanUpFLOfferData" };
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        ///   Retrieves the contents of the OffersAtApplicationCapture table
        /// </summary>
        /// <param name = "tableName">Name of table to query</param>
        /// <returns>[TableName].*</returns>
        public QueryResults GetTestData(string tableName)
        {
            string query = @"Select * from [2am].test." + tableName;
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   This will call on the test.GetProcessAtQAData proc that will return a single data row given the TestIdentifier
        /// </summary>
        /// <param name = "testIdentifier">ApplicationManagementTestID passed as a parameter to the proc to get the correct data row</param>
        /// <param name = "reasonTypeKey">ReasonKey passed as a parameter to the proc to return a reasondescription</param>
        /// <returns>Returns the whole resultsset (QueryResults)</returns>
        public QueryResults GetProcessAtQAData(string testIdentifier, int reasonTypeKey)
        {
            var sqlParam1 = new SqlParameter("@ReasonTypeKey", reasonTypeKey)
                {
                    Direction = ParameterDirection.Input
                };
            var sqlParam2 = new SqlParameter("@TestIdentifier", testIdentifier)
                {
                    Direction = ParameterDirection.Input
                };
            var procedure = new SQLStoredProcedure(sqlParam1, sqlParam2) { Name = "test.GetProcessAtQAData" };
            return dataContext.ExecuteStoredProcedureWithResults(procedure);
        }

        /// <summary>
        ///   Inserts a record into the test.TestMethod table.
        /// </summary>
        /// <param name = "testMethodName">TestMethodName</param>
        /// <param name = "testIdentifier">TestIdentifier</param>
        /// <param name = "testFixture">TestFixture</param>
        public void InsertTestMethod(string testMethodName, string testIdentifier, string testFixture)
        {
            var proc = new SQLStoredProcedure { Name = "test.InsertTestMethod" };
            proc.AddParameter(new SqlParameter("@TestMethodName", testMethodName));
            proc.AddParameter(new SqlParameter("@TestIdentifier", testIdentifier));
            proc.AddParameter(new SqlParameter("@TestFixture", testFixture));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        ///   Inserts the Parameters in into TestMethodParameter table
        /// </summary>
        /// <param name = "testMethodName">TestMethodName</param>
        /// <param name = "testIdentifier">TestIdentifier</param>
        /// <param name = "parameterDescription">ParameterDescription</param>
        /// <param name = "parameterValue">ParameterValue</param>
        public void SaveTestMethodParameters(string testMethodName, string testIdentifier, ParameterTypeEnum parameterDescription,
                                             string parameterValue)
        {
            var proc = new SQLStoredProcedure { Name = "test.SaveTestMethodParameters" };
            proc.AddParameter(new SqlParameter("@TestMethodName", testMethodName));
            proc.AddParameter(new SqlParameter("@TestIdentifier", testIdentifier));
            proc.AddParameter(new SqlParameter("@ParameterDescription", ((int)parameterDescription).ToString()));
            proc.AddParameter(new SqlParameter("@ParameterValue", parameterValue));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        ///   Retrieves test parameters for a given test and identifier
        /// </summary>
        /// <param name = "testMethodName">TestMethodName</param>
        /// <param name = "testIdentifier">TestIdentifier</param>
        /// <returns>pt.ParameterTypeKey, pt.Description, p.ParameterValue</returns>
        public QueryResults GetTestMethodParameters(string testMethodName, string testIdentifier)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"
                    SELECT pt.ParameterTypeKey, pt.Description, p.ParameterValue
                    FROM test.testmethod tm
                    JOIN test.testmethodparameter p ON p.testmethodkey=tm.testmethodkey
                    JOIN test.parametertype pt ON p.parametertypekey=pt.parametertypekey
                    WHERE tm.TestMethodName='{0}'
                    AND tm.TestIdentifier='{1}'", testMethodName, testIdentifier);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Removes the TestData from the TestMethod and TestMethodParameter tables for your TestMethod and TestIdentifier
        /// </summary>
        /// <param name="testMethodKey">TestMethod.TestMethodKey</param>
        public void DeleteTestMethodData(string testMethodKey)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"DELETE FROM test.testmethod where testMethodKey={0}", testMethodKey);
            statement.StatementString = query;
            dataContext.ExecuteNonSQLQuery(statement);
            query = string.Format(@"DELETE FROM test.testmethodParameter where testMethodKey={0}", testMethodKey);
            statement.StatementString = query;
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the TestMethodKey for a TestMethod and a TestIdentifier
        /// </summary>
        /// <param name = "testMethod">TestMethodName</param>
        /// <param name = "testIdentifier">TestIdentifier</param>
        /// <returns>TestMethodKey</returns>
        public string GetTestMethodKey(string testMethod, string testIdentifier)
        {
            string query = string.Format(@"SELECT TestMethodKey FROM test.TestMethod with (nolock) where TestMethodName='{0}' and TestIdentifier='{1}'", testMethod, testIdentifier);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).Value;
        }

        /// <summary>
        ///   This will remove all of the data for in the TestMethod and TestMethodParameter tables for a given TestFixture
        /// </summary>
        /// <param name = "testFixture">TestFixture</param>
        public void DeleteTestMethodDataForFixture(string testFixture)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"DELETE FROM test.testmethodParameter where testMethodKey in (Select testMethodKey from test.TestMethod where testFixture= '{0}')", testFixture);
            statement.StatementString = query;
            dataContext.ExecuteNonSQLQuery(statement);
            query = string.Format(@"DELETE FROM test.testmethod where testFixture= '{0}'", testFixture);
            statement.StatementString = query;
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///   Gets the top record from the test.BankAccountDetails table with a status of 0, ordered by AccountNumber
        /// </summary>
        /// <returns>ACBBank, ACBBranchCode, ACBBranchDescription, AccountNumber, ACBTypeNumber, ACBTypeDescriptionn, Status</returns>
        public QueryResults GetUnusedBankAccountDetails()
        {
            SQLStatement statement = new SQLStatement();
            const string query = @" select top 1 tests.* from [2am].[test].[BankAccountDetails] tests
                                    left join [2am].dbo.bankAccount ba on tests.accountnumber=ba.accountnumber
                                    where Status = 0 and ba.bankaccountkey is null order by tests.AccountNumber";
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Updates the Status of a record in the test.BankAccountDetails table identified by AccountNumber
        ///   Meant to be used after calling GetUnusedBankAccountDetails method to set the Account to Used
        /// </summary>
        /// <param name = "accountNumber">test.BankAccountDetails.AccountNumber</param>
        /// <param name = "status">test.BankAccountDetails.Status (0=Unused, 1=Used)</param>
        /// <returns></returns>
        public void UpdateBankAccountDetailsStatus(string accountNumber, int status)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"Update [2am].[test].[BankAccountDetails] Set Status = {0} where AccountNumber = {1}", status, accountNumber);
            statement.StatementString = query;
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves data for a test method from the test.TestMethod table when provided with a Test Fixture
        /// </summary>
        /// <param name = "fixture">Test Fixture</param>
        /// <returns>testmethod.testmethodname, testmethod.testidentifier </returns>
        public QueryResults GetTestMethodDataByFixture(string fixture)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"select testmethodname, testidentifier from [2am].test.testmethod where testfixture= '{0}'", fixture);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   This runs the test.GetNewBalanceLAADiffInclTolerance stored procedure which will calculate by how much the further advance
        ///   currently exceeds the LAA including the 2% tolerance applied to the LAA's against the Mortgage Loan
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>LAA, RegAmt, Diff</returns>
        public QueryResults GetNewBalanceLAADiffInclTolerance(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.GetNewBalanceLAADiffInclTolerance" };
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        ///   Updates the OffersAtApplicationCapture table
        /// </summary>
        /// <param name = "testIdentifier">TestIdentifier of row to update</param>
        /// <param name = "columnName">Column to Update</param>
        /// <param name = "value">New Value</param>
        public void CommitTestDataForTestIdentifier(string identifierColumn, string testIdentifier, string columnName, string value)
        {
            string query = string.Format(@"Update [2am].test.OffersAtApplicationCapture Set {0} = '{1}' where {2} = '{3}'",
                columnName, value, identifierColumn, testIdentifier);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public void InsertITCv4(int offerKey, int empiricaUpperBound, int empiricaLowerBound)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.InsertITCv4" };
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey.ToString()));
            if (empiricaUpperBound != -1 || empiricaLowerBound != -1)
            {
                int empiricaScore = new Random().Next(empiricaLowerBound, empiricaUpperBound);
                proc.AddParameter(new SqlParameter("@EmpiricaScore", empiricaScore.ToString()));
            }
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Cleans up Offer Debit Order data for an Offer
        /// </summary>
        /// <param name="offerKey"></param>
        public void CleanUpOfferDebitOrder(int offerKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.CleanUpOfferDebitOrder" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Cleans up Legal Entity Data for an Offer
        /// </summary>
        /// <param name="offerKey"></param>
        public void CleanUpLegalEntityData(int offerKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.CleanUpLegalEntityData" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Update the test.TermChange table
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="openOffer"></param>
        /// <returns></returns>
        public bool UpdateTermChangeTest(string testIdentifier, bool openOffer)
        {
            int value = openOffer ? 1 : 0;
            string query = String.Format(@"update test.TermChange
                                        set TermChange.OpenOffer = {0}
                                        where TermChange.TestIdentifier = '{1}'", value, testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Returns a set of transitions that exist against a generic key
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.StageTransition> GetStageTransitionsByGenericKey(int genericKey)
        {
            string sql = string.Format(@"SELECT   st.generickey as [GenericKey],
			                                 sdg.DESCRIPTION as [StageDefinitionGroup],
			                                 sdsdg.stagedefinitionstagedefinitiongroupkey as [SDSDGKey],
			                                 sd.DESCRIPTION as [StageDefinition],
			                                 st.transitiondate as [TransitionDate],
			                                 st.endtransitiondate as [EndTransitionDate],
			                                 sdg.StageDefinitionGroupKey,
			                                 sd.stageDefinitionKey,
			                                 st.stageTransitionKey,
			                                 Comments
	                                FROM     [2am].dbo.StageTransition (NOLOCK) st
			                                 JOIN [2am].dbo.StageDefinitionStageDefinitionGroup (NOLOCK) sdsdg
			                                   ON st.StageDefinitionStageDefinitionGroupKey = sdsdg.StageDefinitionStageDefinitionGroupKey
			                                 JOIN [2am].dbo.StageDefinition (NOLOCK) sd
			                                   ON sdsdg.StageDefinitionKey = sd.StageDefinitionKey
			                                 JOIN [2am].dbo.StageDefinitionGroup (NOLOCK) sdg
			                                   ON sdsdg.StageDefinitionGroupKey = sdg.StageDefinitionGroupKey
	                                WHERE    st.generickey = {0}
	                                GROUP BY st.generickey,
			                                 sdsdg.stagedefinitionstagedefinitiongroupkey,
			                                 sdg.DESCRIPTION,
			                                 sd.DESCRIPTION,
			                                 st.transitiondate,
			                                 st.endtransitiondate,
			                                 st.stagetransitionkey,
			                                 sdg.StageDefinitionGroupKey,
			                                 sd.stageDefinitionKey,
			                                 st.stageTransitionKey,
			                                 Comments
	                                ORDER BY st.stagetransitionkey desc", genericKey);
            var result = dataContext.Query<Automation.DataModels.StageTransition>(sql);
            return result;
        }

        /// <summary>
        /// Takes an account and inserts a debt counselling record and external role records in order to create a debt counselling case in the 2am data structures.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <returns>DebtCounsellingKey, DebtCounsellingGroupKey</returns>
        public QueryResults AddAccountUnderDebtCounselling(int accountKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.AddAccountUnderDebtCounselling" };
            proc.AddParameter(new SqlParameter("@AccountKey", accountKey.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Should be used in conjunction with AddAccountUnderDebtCounselling in order to remove the test data just added.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="debtCounsellingGroupKey"></param>
        /// <returns></returns>
        public QueryResults RemoveDebtCounsellingCase(int debtCounsellingKey, int debtCounsellingGroupKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.RemoveDebtCounsellingCase" };
            proc.AddParameter(new SqlParameter("@DebtCounsellingKey", debtCounsellingKey.ToString()));
            proc.AddParameter(new SqlParameter("@DebtCounsellingGroupKey", debtCounsellingGroupKey.ToString()));
            return dataContext.ExecuteStoredProcedureWithResults(proc);
        }

        /// <summary>
        /// Inserts a new Test DC
        /// </summary>
        /// <param name="name">DC Name</param>
        /// <param name="regNumber">Registration Number</param>
        /// <param name="ncrNumber">NCR Number</param>
        public void InsertNewTestDC(string name, string regNumber, string ncrNumber)
        {
            var proc = new SQLStoredProcedure { Name = "test.InsertNewTestDC" };
            proc.AddParameter(new SqlParameter("@DebtCounsellorName", name));
            proc.AddParameter(new SqlParameter("@regNumber", regNumber));
            proc.AddParameter(new SqlParameter("@NCRNumber ", ncrNumber));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts a new Test PDA
        /// </summary>
        /// <param name="name">PDA Name</param>
        /// <param name="regNumber">Registration Number</param>
        public void InsertNewTestPDA(string name, string regNumber)
        {
            var proc = new SQLStoredProcedure { Name = "test.InsertNewTestPDA" };
            proc.AddParameter(new SqlParameter("@PDAName", name));
            proc.AddParameter(new SqlParameter("@regNumber", regNumber));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Returns a legal entity who has more than account that qualifies for a further lending application.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetLegalEntityWhoQualifiesForMoreThanOneFLApplication()
        {
            var query = @"SELECT NonPerforming.AccountKey
                            INTO
                            #NonPerforming
                            FROM
                            (
                            select a.accountKey from [2am].dbo.Account a
                            join [2am].dbo.financialService fs on a.accountKey=fs.accountKey and parentFinancialServiceKey is null
                            join [2am].fin.financialAdjustment fa on fs.financialServiceKey = fa.financialServiceKey
                            and (financialadjustmentsourceKey=2 and financialAdjustmentTypeKey=5)
                            where fa.financialAdjustmentStatusKey=1
                            ) as NonPerforming

                            select
                            r.LegalEntityKey,
                            count(r.AccountKey) as NumberOfAccounts
                            into
                            #PossibleFurtherLendingLegalEntities
                            from [2am].dbo.Role r
                            join [2am].dbo.Account a on  r.accountkey = a.accountkey
	                            and  a.AccountStatusKey = 1
                            where r.RoleTypeKey in (2,3) and  r.GeneralStatusKey = 1 and
                            r.AccountKey in (
                            select t.AccountKey from test.furtherlendingtestcases t
                            left join [2am].dbo.Offer o on t.accountKey=o.AccountKey
	                            and o.offerStatusKey=1
	                            and o.offerTypeKey in (2,3,4)
                            left join (
                            SELECT accountkey FROM [2am].dbo.detailtype dt
                            JOIN [2am].dbo.detail d ON dt.detailtypekey=d.detailtypekey
                            WHERE dt.description like '%foreclosure%' or dt.description like '%suspended%'
                            or (dt.detailtypekey in (5, 9, 11, 14, 88, 99, 100, 104, 106, 117, 150, 180, 213, 214, 217, 227,
                            241, 242, 251, 279, 293, 294, 296, 299, 302, 453, 454, 455, 456, 457, 459, 461, 464, 493)
                            )
                            ) AS foreclosure_check ON t.accountkey=foreclosure_check.accountkey
                            left join #NonPerforming np on t.accountKey = np.accountKey
                            where  furtherAdvanceAmount > 0  and o.accountkey is null and foreclosure_check.accountKey is null and np.accountKey is null
                            )
                            group by r.LegalEntityKey
                            having  count(r.AccountKey) > 1

                            -- Get the account keys for the entities with more than 1 account
                            select AccountKey, c.LegalEntityKey
                            into #PossibleFurtherLendingAccounts
                            from #PossibleFurtherLendingLegalEntities c with(nolock)
                            join Role r with(nolock) on  c.LegalEntityKey = r.LegalEntityKey

                            -- Show the Accounts
                            select top 2 p.LegalEntityKey, p.AccountKey, t.*
                            from #PossibleFurtherLendingAccounts p
                            join test.furtherlendingtestcases t on  p.AccountKey = t.AccountKey
                            where t.UnderDebtCounselling = 0
                            order by p.LegalEntityKey, p.AccountKey

                            drop table #PossibleFurtherLendingLegalEntities
                            drop table #PossibleFurtherLendingAccounts
                            drop table #NonPerforming";
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Executes the GetNextRRAssignmentByWRTKey SP to retrieve the next user we expect to receive a case in the round robin
        ///   assignment for an Workflow Role Type
        /// </summary>
        /// <param name = "workflowRoleTypeKey">workflowRoleTypeKey</param>
        /// <param name="roundRobinPointer">Pointer that is being used</param>
        /// <returns>The next user we expect to get RR assigned</returns>
        public QueryResults GetNextRoundRobinAssignmentForWorkflowRole(WorkflowRoleTypeEnum workflowRoleTypeKey, RoundRobinPointerEnum roundRobinPointer)
        {
            var proc = new SQLStoredProcedure { Name = "test.GetNextRRAssignmentByWRTKey" };
            proc.AddParameter(new SqlParameter("@workflowRoleTypeKey", ((int)workflowRoleTypeKey).ToString()));
            proc.AddParameter(new SqlParameter("@RoundRobinPointerKey", ((int)roundRobinPointer).ToString()));
            QueryResults results = dataContext.ExecuteStoredProcedureWithResults(proc);
            return results;
        }

        /// <summary>
        /// reloads the config for the personal loan data setup
        /// </summary>
        public void CreatePersonalLoanTestCases()
        {
            var proc = new SQLStoredProcedure { Name = "test.createPersonalLoanAutomationCases" };
            dataContext.ExecuteStoredProcedure(proc);
        }

        public IEnumerable<Automation.DataModels.OriginationTestCase> GetOriginationTestCases()
        {
            return dataContext.Query<Automation.DataModels.OriginationTestCase>("Select * from [2am].test.OffersAtApplicationCapture where TestGroup not in ('CreditScoring','CreditScoringRules') order by Username");
        }

        public IEnumerable<Automation.DataModels.LeadTestCase> GetAutomationTestLeads()
        {
            return dataContext.Query<Automation.DataModels.LeadTestCase>("Select * from [2am].test.AutomationLeads");
        }

        public void InsertIDNumberIntoTestTable(string idNumber)
        {
            string query = String.Format(@"insert into test.idNumbers (idNumber) values ('{0}')", idNumber);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public void InsertAffordabilityAssessment(int affordabilityAssessmentStatusKey, int offerKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.InsertAffordabilityAssessment" };
            proc.AddParameter(new SqlParameter("@AffordabilityAssessmentStatusKey", affordabilityAssessmentStatusKey));
            proc.AddParameter(new SqlParameter("@OfferKey", offerKey));
            dataContext.ExecuteStoredProcedure(proc);
        }
    }
}