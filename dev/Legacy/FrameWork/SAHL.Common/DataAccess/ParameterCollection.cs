using System.Collections.Generic;
using System.Data.SqlClient;

namespace SAHL.Common.DataAccess
{
    public class ParameterCollection : List<SqlParameter>
    {
        public void TransferParameters(SqlParameterCollection Destination)
        {
            foreach (SqlParameter P in this)
            {
                Destination.Add(P);
            }
        }

        public void TransferParameters(ParameterCollection Destination)
        {
            foreach (SqlParameter P in this)
            {
                Destination.Add(P);
            }
        }
    }
}