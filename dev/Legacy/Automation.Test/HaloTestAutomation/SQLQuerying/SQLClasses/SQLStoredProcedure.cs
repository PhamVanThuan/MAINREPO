using System.Collections.Generic;
using System.Data.SqlClient;

namespace Automation.DataAccess
{
    public class SQLStoredProcedure
    {
        internal SQLStoredProcedure(params SqlParameter [] Params)
        {
            this.Parameters = new List<SqlParameter>();
            this.Parameters.AddRange(Params);
        }
        internal void AddParameter(SqlParameter Param)
        {
            this.Parameters.Add(Param);
        }
        internal List<SqlParameter> Parameters {get;set;}
        internal string Name { get; set; }
    }
}
