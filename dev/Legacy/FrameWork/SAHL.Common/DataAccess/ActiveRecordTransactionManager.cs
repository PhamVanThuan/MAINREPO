using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using Castle.ActiveRecord;

namespace SAHL.Common.DataAccess
{
    [Serializable]
    public class ActiveRecordTransactionManager : MarshalByRefObject, ITransactionManager, ISponsor
    {
        private TransactionScope transactionScope;
        private bool disposed = false;
        private List<IManagedTransaction> managedTransactions;

        public ActiveRecordTransactionManager()
        {
            this.managedTransactions = new List<IManagedTransaction>();
        }

        public ILease Lease
        {
            get
            {
                return (ILease)RemotingServices.GetLifetimeService(this);
            }
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = base.InitializeLifetimeService() as ILease;

            //Set lease properties
            lease.InitialLeaseTime = TimeSpan.FromSeconds(60);
            lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            lease.SponsorshipTimeout = TimeSpan.FromSeconds(60);

            lease.Register(this);
            return lease;
        }

        public IManagedTransaction BeginTransaction()
        {
            return this.BeginTransaction(TransactionModeEnum.Inherits);
        }

        public IManagedTransaction BeginTransaction(TransactionModeEnum mode)
        {
            Castle.ActiveRecord.TransactionMode tranMode = (mode == TransactionModeEnum.Inherits ? Castle.ActiveRecord.TransactionMode.Inherits : Castle.ActiveRecord.TransactionMode.New);
            this.transactionScope = new TransactionScope(tranMode);
            return new ManagedTransaction(this);
        }

        public IManagedTransaction BeginTransactionReadUncommitted()
        {
            return this.BeginTransactionReadUncommitted(TransactionModeEnum.Inherits);
        }

        public IManagedTransaction BeginTransactionReadUncommitted(TransactionModeEnum mode)
        {
            Castle.ActiveRecord.TransactionMode tranMode = (mode == TransactionModeEnum.Inherits ? Castle.ActiveRecord.TransactionMode.Inherits : Castle.ActiveRecord.TransactionMode.New);
            this.transactionScope = new TransactionScope(tranMode, System.Data.IsolationLevel.ReadUncommitted, OnDispose.Commit);
            return new ManagedTransaction(this);
        }

        public void CommitTransaction()
        {
            if (transactionScope != null)
            {
                this.transactionScope.VoteCommit();
            }
            else
            {
                throw new NullReferenceException(Strings.TransactionIsInvalid);
            }
        }

        public void RollbackTransaction()
        {
            if (transactionScope != null)
            {
                this.transactionScope.VoteRollBack();
            }
            else
            {
                throw new NullReferenceException(Strings.TransactionIsInvalid);
            }
        }

        public void RegisterManagedTransaction(IManagedTransaction managedTransaction)
        {
            if (!this.managedTransactions.Contains(managedTransaction))
            {
                this.managedTransactions.Add(managedTransaction);
            }
        }

        public void Dispose()
        {
            if (transactionScope != null)
            {
                this.transactionScope.Dispose();
                this.transactionScope = null;
            }

            ILease lease = this.Lease;
            if (lease != null)
            {
                lease.Unregister(this);
                TimeSpan leaseTime = lease.Renew(TimeSpan.Zero);
            }
            this.disposed = true;
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromSeconds(30);
        }
    }
}