using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using System.Data;
using SAHL.Common.DataAccess;
using SAHL.Common.DataAccess.Exceptions;

namespace SAHL.Common.Test.DataAccess
{
    [TestFixture]
    public class UIStatementRepositoryTest : TestBase
    {
        [Test]
        public void ClearCache()
        {
            UIStatementRepository.ClearCache();
        }

        /// <summary>
        /// Tests the GetStatement method.
        /// </summary>
        [Test]
        public void GetStatement()
        {
            string query = "Select top 1 ApplicationName, StatementName, Statement, LastAccessedDate from [2AM].[dbo].[UIStatement] (nolock) where Version = (Select max(Version) from [2AM].[dbo].[UIStatement] (nolock))";
            DataTable dt = GetQueryResults(query);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            string appName = Convert.ToString(dt.Rows[0]["ApplicationName"]);
            string statementName = Convert.ToString(dt.Rows[0]["StatementName"]);
            string statement = Convert.ToString(dt.Rows[0]["Statement"]);
            DateTime lastAccessedDate = Convert.ToDateTime(dt.Rows[0]["LastAccessedDate"]);
            dt.Dispose();

            // now attempt to retrieve the statement and ensure the text is the same as above
            string statementText = UIStatementRepository.GetStatement(appName, statementName);
            Assert.AreEqual(statement, statementText);

            // retrieve the statement again via sql and confirm that the LastAccessedDate has 
            // been changed
            dt = GetQueryResults(query);
            DateTime lastAccessedDateCheck = Convert.ToDateTime(dt.Rows[0]["LastAccessedDate"]);
            dt.Dispose();
            Assert.Greater(lastAccessedDateCheck, lastAccessedDate);

            // test that not finding a statement throws an exception
            bool errorThrown = false;
            try
            {
                UIStatementRepository.GetStatement("XXXXXX", "XXXXXX");
            }
            catch (DataAccessException)
            {
                errorThrown = true;
            }
            Assert.IsTrue(errorThrown, "Attempt to retrieve a UIStatement that does not exist did not throw the expected exception");

        }
    }
}
