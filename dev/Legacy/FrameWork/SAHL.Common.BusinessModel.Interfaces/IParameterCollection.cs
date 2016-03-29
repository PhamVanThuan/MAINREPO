using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IParameterCollection : IList<SqlParameter>
    {
        void TransferParameters(SqlParameterCollection Destination);

        void TransferParameters(IParameterCollection Destination);
    }
}
