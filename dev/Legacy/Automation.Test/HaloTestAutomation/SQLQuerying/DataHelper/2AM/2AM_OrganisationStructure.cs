using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Gets the Organisation Structure Description for the Designation Organisation Type for the ADUser's provided.
        /// </summary>
        /// <param name="aduserkey"></param>
        /// <returns></returns>
        public QueryResults GetUserOrganisationStructureDescription(params int[] aduserkey)
        {
            string aduserkeyStr = Helpers.GetDelimitedString<int>(aduserkey, ",");
            string query =
                 String.Format(@"
                        select uos.aduserkey, os.description
                        from dbo.userorganisationstructure uos
	                    inner join dbo.organisationstructure os
		                on uos.organisationstructurekey = os.organisationstructurekey
                        where os.OrganisationTypeKey = 7 and uos.aduserkey in ({0})", aduserkeyStr);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}