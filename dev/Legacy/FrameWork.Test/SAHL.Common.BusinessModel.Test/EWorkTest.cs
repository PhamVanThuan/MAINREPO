using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using System.Data;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class EWorkTest : TestBase
    {
        [NUnit.Framework.Test]
        public void EWorkRoundRobinTest()
        {
            string eMapName = "LC Arrears Non-Subsidy";
            string eAttribute = "LC Arrears Non-Subsidy";
            string eValue = "True";

            int assignEachUserCount = 10;
            
            using (new SessionScope())
            {
                IDbConnection con = Helper.GetSQLDBConnection("EWorkConnectionString");
                
                // get the number of users in the list
                string sql = String.Format("select count(1) as UserCount from [e-work].dbo.eAttribute a (nolock) join sahldb.dbo.TokenAssignment t (nolock) on t.eUserName = a.eUserName where eAttribute = '{0}' and t.eMapName = '{2}' and eValue = '{1}' and Enabled  = 'Y'", eAttribute, eValue, eMapName);
                int userCount = Convert.ToInt32(base.ExecuteScalar(con, sql));

                int loopCount = userCount * assignEachUserCount;
                // setup a dictionary to hold the users token count
                IDictionary<string, int> dicUserTokenCount = new Dictionary<string, int>(userCount);

                for (int i = 0; i < loopCount; i++)
                {
                    // execute the stored proc to do the token asignment
                    sql = String.Format("exec [e-work].[dbo].[NextTokenIssue] '{0}','{1}','{2}'", eMapName, eAttribute, eValue);
                    int result = base.ExecuteNonQuery(con, sql);

                    // get the user that has the token 
                    sql = String.Format("select a.eUserName from [e-work].dbo.eAttribute a (nolock) join sahldb.dbo.TokenAssignment t (nolock) on t.eUserName = a.eUserName where eAttribute = '{0}' and t.eMapName = '{1}' and eValue = '{2}' and Enabled  = 'Y' and Token = 1", eAttribute, eMapName, eValue);
                    DataTable dtToken = base.GetQueryResults(sql, con);
                    string user = dtToken.Rows[0]["eUserName"].ToString();

                    // add/update the user token count in the dictionary                
                    if (dicUserTokenCount.ContainsKey(user))
                        dicUserTokenCount[user] = dicUserTokenCount[user] + 1;
                    else
                        dicUserTokenCount.Add(user, 1);
                }

                // loop thru the dictionary and check that each person has the 'assignEachUserCount' number of tokens
                foreach (KeyValuePair<string,int> tokens in dicUserTokenCount)
                {
                    Assert.IsTrue(tokens.Value == assignEachUserCount);
                }                
            }
        }
    }
}
