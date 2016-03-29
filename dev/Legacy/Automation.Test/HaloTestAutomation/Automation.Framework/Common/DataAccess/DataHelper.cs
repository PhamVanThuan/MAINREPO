using Automation.Framework.Models;
using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using FrameworkModels = Automation.Framework.Models;

namespace Automation.Framework.DataAccess
{
    public static class DataHelper
    {
        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            }
        }

        /// <summary>
        /// Gets the x2 data table for instance provided, returning the columns as a dictionary containing the column names and values.
        /// </summary>
        /// <param name="instanceID">InstanceID</param>
        /// <param name="x2DataTable">Data table name</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetX2DataTable(long instanceID, string x2DataTable)
        {
            using (var sqlconnection = new SqlConnection(ConnectionString))
            {
                sqlconnection.Open();
                var dictionary = new Dictionary<string, string>();
                SqlCommand command = sqlconnection.CreateCommand();
                command.CommandText = string.Format(@"select * from {0} where instanceid = {1}", x2DataTable, instanceID);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable();
                while (reader.Read())
                {
                    //For each field in the table...
                    foreach (DataRow myField in schemaTable.Rows)
                    {
                        //For each property of the field...
                        foreach (DataColumn myProperty in schemaTable.Columns)
                        {
                            if (myProperty.ColumnName == "ColumnName")
                            {
                                //we need the data type
                                dictionary.Add(myField[myProperty].ToString(), reader[myField[myProperty].ToString()].ToString());
                                break;
                            }
                        }
                    }
                }
                reader.Close();
                sqlconnection.Close();
                return dictionary;
            }
        }

        /// <summary>
        /// Gets an instance that is linked to the storage key provided where the activity that we want to perform has been setup in the InstanceActivitySecurity table
        /// </summary>
        /// <param name="keyValue">StorageKey Value</param>
        /// <param name="workflowID">WorkflowID</param>
        /// <param name="activityID">ActivityID</param>
        /// <param name="storageKey">Storage Key Name</param>
        /// <param name="x2DataTable">Data table to be used</param>
        /// <returns></returns>
        public static Instance GetInstanceForActivity(int keyValue, int workflowID, string activity, string storageKey, string x2DataTable, int activityType)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                bool activityExists = false;
                var endTime = DateTime.Now.AddSeconds(30);
                var instance = default(Instance);
                //give the engine 30 seconds to process the case to the next state that we are expecting it to be at
                while (DateTime.Now <= endTime)
                {
                    connection.Open();
                    string query = string.Format(@" select i.* from {0} data
                        join x2.x2.instance i on data.instanceid = i.id
                        join x2.x2.state s on i.stateid = s.id
                        where {1} = {2} and i.workflowID = {3} and i.parentInstanceID is null and s.type <> 5", x2DataTable, storageKey.Replace("@", ""), keyValue, workflowID);
                    instance = connection.Query<FrameworkModels.Instance>(query).FirstOrDefault();
                    if (activityType == (int)ActivityTypeEnum.External)
                    {
                        //check external activity applied to instance's current state
                        activityExists = CheckIfExternalActivityExistsForCurrentState(workflowID, activity, instance.StateID);
                    }
                    else if (activityType == (int)ActivityTypeEnum.Timed && !activityExists)
                    {
                        activityExists = CheckIfScheduledActivityExistsForInstance(activity, instance.ID);
                        if (activityExists)
                            return instance;
                        else
                        {
                            //we should check for timers against both the child and the parent
                            query = string.Format(@"select sa_child.*, a_child.name as Activity
                                        from x2.x2.instance c
                                        join x2.x2.scheduledACtivity sa_child on c.id=sa_child.instanceid
                                        join x2.x2.activity a_child on sa_child.activityid=a_child.ID
	                                        and a_child.name = '{0}'
                                        where c.parentinstanceid = {1}", activity, instance.ID);
                            instance.ClonedInstanceTimers = connection.Query<FrameworkModels.Timers>(query).ToList();
                            //check for the timer
                            var timer = (from t in instance.ClonedInstanceTimers where t.Activity.ToUpper() == activity.ToUpper() select t).FirstOrDefault();
                            if (timer != null)
                            {
                                //we have found a timer on the child so return that instance
                                query = string.Format(@"select i.* from {0} data
                                                    join x2.x2.instance i on data.instanceid = i.id
                                                    join x2.x2.scheduledactivity sa on i.id=sa.instanceid
                                                    join x2.x2.activity a on sa.activityid=a.id
                                                        and a.name = '{4}'
                                                    where {1} = {2} and i.workflowID = {3} 
                                                    and i.parentInstanceID is not null", x2DataTable, storageKey.Replace("@", ""), keyValue, workflowID, activity);
                                instance = connection.Query<FrameworkModels.Instance>(query).FirstOrDefault();
                                return instance;
                            }
                        }
                    }
                    else //we are dealing with a user activity that should have activity security
                    {
                        //get the security records
                        string iasQuery = string.Format(@"select a.* from x2.x2.instanceActivitySecurity ias
                                                    join x2.x2.activity a on ias.activityid = a.id
                                                    where ias.instanceID = {0}", instance.ID);
                        instance.ActivitySecurity = connection.Query<FrameworkModels.Activity>(iasQuery).ToList();
                        activityExists = (from a in instance.ActivitySecurity
                                          where a.Name.ToUpper() == activity.ToUpper()
                                          select a).FirstOrDefault() == null ? false : true;
                    }
                    if (activityExists)
                        return instance;
                }
                return instance;
            }
        }

        private static bool CheckIfScheduledActivityExistsForInstance(string activity, long instanceID)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"select i.* from x2.x2.scheduledActivity sa
                                                join x2.x2.activity a on sa.activityid=a.id
                                                join x2.x2.instance i on sa.instanceid = i.id
                                                where a.name='{0}' and sa.instanceid={1}", activity, instanceID);
                var instance = connection.Query<Instance>(query).FirstOrDefault();
                return instance == null ? false : true;
            }
        }

        /// <summary>
        /// Gets the latest workflow ID by name
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static int GetWorkflowID(string workflowName)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"select max(id) as ID, Name from x2.x2.workflow where name = '{0}' group by name", workflowName);
                return connection.Query<FrameworkModels.Activity>(query).FirstOrDefault().ID;
            }
        }

        /// <summary>
        /// Gets the latest activity by name
        /// </summary>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public static Activity GetActivity(string activityName, string workflowName)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"
                        select max(id) as ID, Name, Type as ActivityType
                        from x2.x2.activity
                        where ltrim(rtrim(activity.name)) = ltrim(rtrim('{0}'))
                        and activity.Type not in (2)
                        and activity.workflowid = (select max(id) from x2.x2.workflow where name = '{1}')
                        group by name, Type", activityName, workflowName);
                return connection.Query<FrameworkModels.Activity>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="procToRun">Stored Proc Name with schema</param>
        /// <param name="parameters">List of parameters</param>
        /// <returns></returns>
        public static bool ExecuteProcedure(string procToRun, Dictionary<string, string> parameters)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                try
                {
                    var p = new DynamicParameters();
                    foreach (var item in parameters)
                    {
                        p.Add(item.Key, item.Value);
                    }
                    connection.Execute(procToRun, p, commandType: CommandType.StoredProcedure);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlToRun"></param>
        /// <returns></returns>
        public static bool ExecuteAdHocSQL(string sqlToRun)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Execute(sqlToRun);
                return true;
            }
        }

        /// <summary>
        /// Returns an account when provided with a capOffer
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <returns></returns>
        internal static FrameworkModels.Account GetAccountByCapOfferKey(int capOfferKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"select accountKey, dbo.AccountLegalName(accountKey, 1) as AccountName
                                                from dbo.CapOffer where CapOfferKey = {0}", capOfferKey);
                return connection.Query<FrameworkModels.Account>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns the Cap Expiry Date for a capOffer
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <returns></returns>
        internal static FrameworkModels.CapOffer GetCapOfferDetails(int capOfferKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"
                   select co.capOfferKey, ctc.offerEndDate as ExpiryDate
                    from dbo.capOffer co
                    inner join dbo.capTypeConfiguration ctc on co.capTypeConfigurationKey = ctc.capTypeConfigurationKey
                    where co.capofferKey = {0}", capOfferKey);
                return connection.Query<FrameworkModels.CapOffer>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all of the instances from the x2 data table linked to the key provided.
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="dataTable"></param>
        /// <param name="storageKey"></param>
        /// <returns>IEnumerable of Instance</returns>
        public static IEnumerable<Instance> GetInstances(int keyValue, string dataTable, string storageKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                if (storageKey == "OfferKey")
                    storageKey = "ApplicationKey";
                connection.Open();
                var query = string.Format(@"
                        select i.*, s.name as StateName from {0} data with (nolock)
                        join x2.x2.instance i with (nolock) on data.instanceid = i.id
                        join x2.x2.state s with (nolock) on i.stateid=s.id
                        where {1} = {2}", dataTable, storageKey, keyValue);
                return connection.Query<Instance>(query);
            }
        }

        /// <summary>
        /// Retrieve the account by using the debt counselling key
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        internal static FrameworkModels.Account GetAccountByDebtCounsellingKey(int debtCounsellingKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@" select acc.accountKey, acc.rrr_productKey as ProductKey
                                                from [2am].debtcounselling.debtCounselling dc
                                                join [2am].dbo.Account acc on dc.accountKey = acc.accountKey
                                                where debtCounsellingKey = {0}", debtCounsellingKey);
                return connection.Query<FrameworkModels.Account>(query).FirstOrDefault();
            }
        }

        public static List<Automation.DataModels.TestCase> GetDisabilityClaimTestCases()
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query = @"SELECT
                                LifeAccountKey,                
	                            LegalEntityKey,
	                            DisabilityClaimKey AS IdentityKey,
	                            ExpectedEndState,
	                            ScriptToRun,
                                ScriptFile,
	                            'X2.X2Data.Disability_Claim' AS DataTable,
                                'DisabilityClaimKey' AS KeyType
                            FROM [2AM].test.DisabilityClaimAutomationCases
                            WHERE DisabilityClaimKey IS NULL";
                return connection.Query<Automation.DataModels.TestCase>(query).ToList();
            }
        }

        /// <summary>
        /// Retrieves a list of cases to fetch for the debt counselling case create
        /// </summary>
        /// <returns></returns>
        public static List<Automation.DataModels.TestCase> GetDebtCounsellingTestCases()
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query = @"select top 15 test.accountKey, test.debtcounsellingkey as IdentityKey, expectedEndState, ScriptToRun, ScriptFile,
                             'x2.x2data.debt_counselling' as DataTable, 'DebtCounsellingKey' as KeyType
                            from [2am].test.AutomationDebtCounsellingTestCases test
                            left join debtcounselling.debtcounselling dc with (nolock) on test.accountkey=dc.accountkey
                                and dc.DebtCounsellingStatusKey=1
                            where test.accountkey not in (select accountkey from test.DebtCounsellingAccounts)
                            and dc.accountkey is null
                            and eStageName in ( select distinct eStageName from [e-work]..eAction
                                                where eactionname='x2 debt counselling'
			                                    and emapname ='losscontrol' )
                            and test.ArrearTransactionNewBalance > 500 and len(test.userToDo) > 0
                            and productKey in (1,9) and isInterestOnly = 0
                            group by test.accountKey, test.debtcounsellingkey, expectedEndState, ScriptToRun, ScriptFile
                            having count(test.accountKey) = 1
                            order by test.accountkey";
                return connection.Query<Automation.DataModels.TestCase>(query).ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static int GetIdentityKeyDebtCounselling(int accountKey)
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query =
                    string.Format(@"select top 1 debtCounsellingKey from [2am].debtcounselling.debtCounselling where AccountKey = {0} and debtCounsellingStatusKey = 1", accountKey);
                var debtCounselling = connection.Query<Models.DebtCounselling>(query).FirstOrDefault();
                return debtCounselling.DebtCounsellingKey;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static int GetIdentityKeyHelpDesk(int legalEntityKey)
        {
            return legalEntityKey;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static int GetIdentityKeyDisabilityClaim(int legalEntityKey)
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                var query =
                    string.Format(@"select top 1 DisabilityClaimKey from [2am].[dbo].DisabilityClaim where LegalEntityKey = {0} ", legalEntityKey);
                var disabilityClaim = connection.Query<Models.DisabilityClaim>(query).FirstOrDefault();
                return disabilityClaim.DisabilityClaimKey;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Automation.DataModels.TestCase> GetCAP2OffersTestCases()
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query = @"select AccountKey, CapOfferKey as IdentityKey, ExpectedEndState, scriptToRun, ScriptFile, 'x2.x2data.cap2_offers' as DataTable, 'CapOfferKey' as KeyType
                            from [2am].test.AutomationCap2TestCases
                            where testType = 'Create' and capOfferKey = 0";
                return connection.Query<Automation.DataModels.TestCase>(query).ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Automation.DataModels.TestCase> GetHelpDeskTestCases()
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query = @"select AccountKey, 0 as IdentityKey, LegalEntityKey, ExpectedEndState, scriptToRun, ScriptFile, 'x2.x2data.Help_Desk' as DataTable, 'LegalEntityKey' as KeyType
                            from [2am].test.AutomationHelpDeskTestCases";
                return connection.Query<Automation.DataModels.TestCase>(query).ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static int GetIdentityKeyCAP2Offers(int accountKey)
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query =
                    string.Format(@"select top 1 capOfferKey from [2am].dbo.CapOffer where AccountKey = {0} and CapStatusKey = 1", accountKey);
                var capOffer = connection.Query<Models.CapOffer>(query).FirstOrDefault();
                return capOffer.CapOfferKey;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Automation.DataModels.TestCase> GetPersonalLoansTestCases()
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query = @"select 0 as AccountKey, OfferKey as IdentityKey, ExpectedEndState,
                    scriptToRun, ScriptFile, 'x2.x2data.personal_loans' as DataTable,
                    'OfferKey' as KeyType, legalEntityKey as LegalEntityKey
                    from [2am].test.personalLoanAutomationCases
                    where offerKey is null";
                return connection.Query<Automation.DataModels.TestCase>(query).ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static int GetIdentityKeyPersonalLoans(int accountKey)
        {
            using (var connection = new DapperSqlConnection(DataHelper.ConnectionString))
            {
                //we need to get the cases we want
                var query =
                    string.Format(@"select top 1 o.offerKey from [2am].dbo.Offer o
                        join [2am].dbo.externalRole er on o.offerKey = er.genericKey
                        and er.genericKeyTypeKey = 2
                        and er.externalRoleTypeKey = 1
                        where o.offerStatusKey = 1
                        and er.legalEntityKey = {0}", accountKey);
                var offer = connection.Query<Models.Offer>(query).FirstOrDefault();
                return offer.OfferKey;
            }
        }

        /// <summary>
        /// Returns the offer information key when provided with an offer key
        /// </summary>
        /// <param name="capOfferKey"></param>
        /// <returns></returns>
        internal static int GetMaxOfferInformationKey(int offerKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string query = string.Format(@"select offerKey, max(offerInformationKey) as OfferInformationKey
                    from [2am].dbo.OfferInformation
                    where offerKey = {0}
                    group by offerKey", offerKey);
                return connection.Query<Models.Offer>(query).FirstOrDefault().OfferInformationKey;
            }
        }

        internal static bool CheckIfExternalActivityExistsForCurrentState(int workflowID, string activity, int stateID)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = string.Format(@"select a.* from x2.x2.activity a where a.name = '{0}' and StateID = {1}
                                            and workflowid={2} and Type = 3 ", activity, stateID, workflowID);
                var externalActivity = connection.Query<Models.Activity>(sql).FirstOrDefault();
                return externalActivity == null ? false : true;
            }
        }

        internal static void UpdateX2Valuation(int applicationKey, int propertyKey, string requestingAdUser, string lightstonePropertyID, int valuationKey, int valuationDataProviderDataServiceKey)
        {
            using (var connection = new DapperSqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = string.Format(@"update x2.x2data.valuations
                                              set propertyKey={0},
                                                  requestingaduser='{1}',
                                                  lightstonePropertyID='{2}',
                                                  valuationKey={3},
                                                  valuationDataProviderDataServiceKey={4}
                                              where applicationKey={5}", propertyKey, requestingAdUser, lightstonePropertyID, valuationKey, valuationDataProviderDataServiceKey, applicationKey);
                connection.Execute(sql);
            }
        }

    }
}