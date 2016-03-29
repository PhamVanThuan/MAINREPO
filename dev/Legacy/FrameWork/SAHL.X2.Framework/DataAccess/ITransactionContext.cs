using System;
namespace SAHL.X2.Framework.DataAccess
{
  public interface ITransactionContext
  {
    System.Data.IDbConnection AuditConnection { get; set; }
    SAHL.X2.Framework.DataSets.Audits AuditInfo { get; set; }
    System.Data.IDbTransaction AuditTransaction { get; set; }
    System.Data.IDbConnection DataConnection { get; set; }
    System.Data.IDbTransaction DataTransaction { get; set; }
    void DisposeContext();
  }
}
