using System;

namespace SAHL.Common.DataAccess
{
	public interface ITransactionManager : IDisposable
	{
		IManagedTransaction BeginTransaction();

		IManagedTransaction BeginTransaction(TransactionModeEnum mode);

        IManagedTransaction BeginTransactionReadUncommitted();

		IManagedTransaction BeginTransactionReadUncommitted(TransactionModeEnum mode);

		void CommitTransaction();

		void RollbackTransaction();

        void RegisterManagedTransaction(IManagedTransaction managedTransaction);
	}

	public enum TransactionModeEnum
	{
		Inherits,
		New
	}
}