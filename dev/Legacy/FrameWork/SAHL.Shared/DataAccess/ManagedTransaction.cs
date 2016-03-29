using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

namespace SAHL.Common.DataAccess
{
    [Serializable]
    public class ManagedTransaction : MarshalByRefObject, IManagedTransaction, ISponsor
    {
        private ITransactionManager transactionManager;
        private bool disposed = false;

        public ManagedTransaction(ITransactionManager transactionManager)
        {
            this.transactionManager = transactionManager;
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
            lease.InitialLeaseTime = TimeSpan.FromSeconds(30);
            lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            lease.SponsorshipTimeout = TimeSpan.FromSeconds(30);

            lease.Register(this);
            return lease;
        }

        public void Commit()
        {
            if (this.transactionManager != null)
            {
                this.transactionManager.CommitTransaction();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Rollback()
        {
            if (this.transactionManager != null)
            {
                this.transactionManager.RollbackTransaction();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Dispose()
        {
            if (this.transactionManager != null)
            {
                this.transactionManager = null;
            }

            ILease lease = this.Lease;
            if (lease != null)
            {
                lease.Renew(TimeSpan.Zero);
                lease.Unregister(this);
            }
            this.disposed = true;
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromSeconds(10);
        }
    }
}