using Common.Enums;
using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="sdsdgKey"></param>
        public bool InsertStageTransition(int genericKey, StageDefinitionStageDefinitionGroupEnum sdsdgKey, string testUser)
        {
            var query =
                String.Format(@"insert into [2AM].[dbo].[StageTransition]
                                   ([GenericKey]
                                   ,[ADUserKey]
                                   ,[TransitionDate]
                                   ,[Comments]
                                   ,[StageDefinitionStageDefinitionGroupKey]
                                   ,[EndTransitionDate])
                                select top 01 {0},aduser.aduserkey,getdate(),'',{1},getdate() from dbo.aduser
                                where adusername = '{2}'", genericKey, (int)sdsdgKey, testUser);
            var statement = new SQLStatement() { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}