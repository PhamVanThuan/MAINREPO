using System;
using System.Linq;

namespace SAHL.Core.Data.Models.Cuttlefish.SqlStatements
{
    public class GenericMessageSelectByGenericStatusStatement : ISqlStatement<GenericMessageDataModel>
    {
        public GenericMessageSelectByGenericStatusStatement(int genericStatusID)
        {
            this.GenericStatusID = genericStatusID;
        }

        public int GenericStatusID { get; protected set; }

        public string GetStatement()
        {
            return string.Format("SELECT * FROM [cuttlefish].[dbo].[GenericMessage] where GenericStatusID={0}", GenericStatusID);
        }
    }
}