using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ServiceStack.OrmLite;

namespace Database.Tests.Base
{
    public class TestBase
    {
        protected string RunTest(string testName, string testDBName)
        {
            if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(testDBName))
            {
                SqlParameter testSessionIdParam = new SqlParameter("@TestSessionId", SqlDbType.Int);
                testSessionIdParam.Direction = ParameterDirection.Output;

                SqlParameter testSessionPassedParam = new SqlParameter("@TestSessionPassed", SqlDbType.Bit);
                testSessionPassedParam.Direction = ParameterDirection.Output;

                using (IDbConnection db = DB.Factory.OpenDbConnection())
                {
                    db.Exec(cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300;
                        cmd.Parameters.Add(new SqlParameter("@TestDatabaseName", testDBName));
                        cmd.Parameters.Add(new SqlParameter("@TestName", testName));
                        cmd.Parameters.Add(new SqlParameter("@ResultsFormat", "None"));
                        cmd.Parameters.Add(new SqlParameter("@CleanTemporaryData", false));
                        cmd.Parameters.Add(testSessionIdParam);
                        cmd.Parameters.Add(testSessionPassedParam);
                        cmd.CommandText = "TST.Runner.RunTest";
                        cmd.ExecuteScalar();
                    });

                    bool testPassed = Convert.ToBoolean(testSessionPassedParam.Value);

                    if (!testPassed)
                    {
                        int testSessionId = Convert.ToInt32(testSessionIdParam.Value);

                        if (testSessionId > 0)
                        {
                            StringBuilder message = new StringBuilder();
                            List<string> testResults = db.List<string>(string.Format(@"select LogMessage from TST.Data.TSTResultsEx (nolock)
                                                                                            where TestSessionId = {0}
                                                                                            and SProcName = '{1}'
                                                                                            and TestStatus = 'F'",
                                                                                testSessionId,
                                                                                testName));
                            if (testResults != null && testResults.Count > 0)
                            {
                                foreach (string testResult in testResults)
                                {
                                    message.AppendLine(testResult);
                                }

                                db.Exec(cmd =>
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@TestSessionId", testSessionId));
                                    cmd.CommandText = "TST.Internal.CleanSessionData";
                                    cmd.ExecuteScalar();
                                });
                                //fail with log msg
                                return message.ToString();
                            }
                            else
                            {
                                return String.Format("{0} test failed with no log messages returned", testName);
                            }
                        }
                        else
                        {
                            return String.Format("An error occurred in the RunTest method when trying to retrieve the TestSessionId for TestName = {0} ", testName);
                        }
                    }
                    else
                    {
                        //pass
                        return string.Empty;
                    }
                }
            }
            else
            {
                return "Invalid TestName or TestDB provided";
            }
        }

        protected string RunAllTests(string testDBName)
        {
            if (!string.IsNullOrEmpty(testDBName))
            {
                SqlParameter testSessionIdParam = new SqlParameter("@TestSessionId", SqlDbType.Int);
                testSessionIdParam.Direction = ParameterDirection.Output;

                SqlParameter testSessionPassedParam = new SqlParameter("@TestSessionPassed", SqlDbType.Bit);
                testSessionPassedParam.Direction = ParameterDirection.Output;

                using (IDbConnection db = DB.Factory.OpenDbConnection())
                {
                    db.Exec(cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 54000; // 15 minutes
                        cmd.Parameters.Add(new SqlParameter("@TestDatabaseName", testDBName));
                        cmd.Parameters.Add(new SqlParameter("@ResultsFormat", "None"));
                        cmd.Parameters.Add(new SqlParameter("@CleanTemporaryData", false));
                        cmd.Parameters.Add(testSessionIdParam);
                        cmd.Parameters.Add(testSessionPassedParam);
                        cmd.CommandText = "TST.Runner.RunAll";
                        cmd.ExecuteScalar();
                    });

                    bool passed = Convert.ToBoolean(testSessionPassedParam.Value);

                    if (!passed)
                    {
                        int sessionId = Convert.ToInt32(testSessionIdParam.Value);

                        if (sessionId > 0)
                        {
                            StringBuilder message = new StringBuilder();
                            List<string> testResults = db.List<string>(string.Format(@"select LogMessage from TST.Data.TSTResultsEx (nolock)
                                                                                            where TestSessionId = {0}
                                                                                            and TestStatus = 'F'",
                                                                                sessionId));
                            if (testResults != null && testResults.Count > 0)
                            {
                                foreach (string testResult in testResults)
                                {
                                    message.AppendLine(testResult);
                                }

                                db.Exec(cmd =>
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@TestSessionId", sessionId));
                                    cmd.CommandText = "TST.Internal.CleanSessionData";
                                    cmd.ExecuteScalar();
                                });
                                //fail with log msg
                                return message.ToString();
                            }
                            else
                            {
                                return String.Format("Tests failed with no log messages returned");
                            }
                        }
                        else
                        {
                            return String.Format("An error occurred in the RunTest method when trying to retrieve the TestSessionId");
                        }
                    }
                    else
                    {
                        //pass
                        return string.Empty;
                    }
                }
            }
            else
            {
                return "Invalid TestDB provided";
            }
        }
    }
}