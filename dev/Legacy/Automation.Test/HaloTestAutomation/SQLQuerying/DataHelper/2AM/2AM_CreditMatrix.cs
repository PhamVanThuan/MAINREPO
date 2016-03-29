using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get the latest credit matrix key
        /// </summary>
        /// <returns></returns>
        public int GetLatestCreditMatrixKey()
        {
            var statement = new SQLStatement();
            var query =
                String.Format(
                    @"select max(creditmatrixKey) as CreditMatrixKey from [2am].dbo.CreditMatrix where newbusinessindicator = 'Y'");
            statement.StatementString = query;
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// Get a distinct list of link rates for a credit matrix
        /// </summary>
        /// <param name="creditMatrixKey"></param>
        /// <returns></returns>
        public List<Automation.DataModels.LinkRates> GetCreditMatrixMargins(int creditMatrixKey)
        {
            var query =
                String.Format(
                    @"select distinct m.marginKey, m.value from [2am].dbo.creditCriteria cc
                                        join [2am].dbo.margin m on cc.marginkey = m.marginkey
                                        where creditMatrixKey={0}", creditMatrixKey);
            return dataContext.Query<Automation.DataModels.LinkRates>(query).ToList();
        }

        public string GetCategoryDescription(int categoryKey)
        {
            var query =
                String.Format(@"select description from [2am].dbo.category where categorykey = {0}", categoryKey);
            return dataContext.Query<string>(query).FirstOrDefault();
        }
    }
}