using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will execute sc
        /// </summary>
        /// <returns></returns>
        public string GetActiveEstateAgencyTradingName()
        {
            string query =
                String.Format(@"select top 01
                                    case
		                                when RTrim(le.tradingname) != RTrim(le.registeredname)
			                                then RTrim(le.registeredname) + ' trading as ' + RTrim(le.tradingname)
		                                else
			                                RTrim(le.registeredname)
	                                end
                                from dbo.OfferOriginator oo
	                                inner join dbo.legalentity le
		                                on oo.legalentitykey = le.legalentitykey
                                where oo.originationsourcekey = 6 and oo.generalstatuskey = 1
                                order by newid()");
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }
    }
}