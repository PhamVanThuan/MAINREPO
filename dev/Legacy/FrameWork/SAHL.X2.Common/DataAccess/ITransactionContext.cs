using System;
using System.Data;

namespace SAHL.X2.Common.DataAccess
{
    public interface ITransactionContext : IDisposable
    {
        int UniqueID { get; set; }

        //IDbConnection AuditConnection { get; set; }
        //SAHL.X2.Framework.DataSets.Audits AuditInfo { get; set; }
        //IDbTransaction AuditTransaction { get; set; }
        IDbConnection DataConnection { get; set; }

        IDbTransaction DataTransaction { get; set; }

        void Dispose();
    }
}