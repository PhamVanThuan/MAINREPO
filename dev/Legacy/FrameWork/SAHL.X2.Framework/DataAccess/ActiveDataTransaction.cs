using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Transactions;
using SAHL.Common.DataAccess;
using SAHL.Common.Logging;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2.Framework.DataAccess
{
    public enum TranCallerType
    {
        InternalEngine,
        ExternalEngine
    };

    public class IActiveDataTransactionFactory
    {
        private static Dictionary<Guid, Obj> ExistingTrans = new Dictionary<Guid, Obj>();
        private static ManualResetEvent mre = new ManualResetEvent(false);

        internal class Obj
        {
            internal X2ActiveDataTransaction Tran;
            internal TransactionScope ts;
            internal ITransactionManager activeRecordTransactionManager;

            internal DateTime Added;

            internal string Caller = string.Empty;
            internal TranCallerType ct;
            internal DateTime dtStart;
            internal long ticks = 0;

            public Obj(string Caller)
            {
                Added = DateTime.Now;
                this.Caller = Caller;
            }

            public Obj()
            {
                Added = DateTime.Now;
            }

            public void Complete()
            {
                long t = DateTime.Now.Ticks - dtStart.Ticks;
                ticks += t;
                TimeSpan ts = new TimeSpan(t);
                if ((ts.TotalMilliseconds > 1000) && (ct == TranCallerType.ExternalEngine))
                {
                    Console.WriteLine(string.Format("{0}: [{1}:{2}]", Caller, ts.Seconds, ts.Milliseconds));
                }
            }

            public void Dispose()
            {
                if (activeRecordTransactionManager != null)
                {
                    activeRecordTransactionManager.Dispose();
                    activeRecordTransactionManager = null;
                }

                // clean this first as we pass the commitable tran into the IActiveTran
                if (null != Tran)
                {
                    Tran.Dispose();
                    Tran = null;
                }
                if (null != ts)
                {
                    ts.Dispose();
                    ts = null;
                }
            }
        }

        public static IActiveDataTransaction BeginTransaction(string Caller, TranCallerType ct)
        {
            lock (ExistingTrans)
            {
                Obj obj = new Obj(Caller);
                try
                {
                    obj.dtStart = DateTime.Now;
                    TransactionOptions t = new TransactionOptions();
                    t.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                    t.Timeout = new TimeSpan(0, 3, 0);

                    obj.ts = new TransactionScope(TransactionScopeOption.RequiresNew, t);

                    obj.Tran = new X2ActiveDataTransaction();

                    obj.Tran.BeginTransaction(Transaction.Current);

                    obj.Tran.Context.UniqueID = RequestIDFactory.GetNext();
                    lock (ExistingTrans)
                    {
                        ExistingTrans.Add(obj.Tran.ID, obj);
                        return obj.Tran;
                    }
                }
                catch (Exception ex)
                {
                    Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                    methodParameters.Add("Caller", Caller);
                    LogPlugin.Logger.LogErrorMessageWithException("ActiveDataTransaction.BeginTransaction", ex.Message, ex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });

                    try
                    {
                        if (null != obj)
                        {
                            obj.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        methodParameters.Add("Caller", Caller);
                        LogPlugin.Logger.LogErrorMessageWithException("ActiveDataTransaction.BeginTransaction", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                    }
                }
                return null;
            }
        }

        public static void CommitTransaction(IActiveDataTransaction Tran)
        {
            Obj obj = null;
            try
            {
                if (null == Tran) return;
                if (!ExistingTrans.ContainsKey(Tran.ID)) return;
                obj = ExistingTrans[Tran.ID];
                //lock (ExistingTrans)
                //{
                //    ExistingTrans.Remove(Tran.ID);
                //}


                if (null != obj.Tran.CurrentTransaction)
                {
                    obj.Tran.CommitTransaction();
                }
                else
                {
                    string WTF = "HOW";
                    Console.WriteLine(WTF);
                }

                if (obj.activeRecordTransactionManager != null)
                {
                    obj.activeRecordTransactionManager.CommitTransaction();
                }

                obj.ts.Complete();
                obj.Complete();
            }
            catch (InvalidOperationException ioe)
            {
                LogPlugin.Logger.LogErrorMessageWithException("ActiveDataTransaction.CommitTransaction", ioe.Message, ioe);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                obj.Dispose();
            }
        }

        public static void RollBackTransaction(IActiveDataTransaction Tran, bool ClearPools)
        {
            Obj obj = null;
            try
            {
                if (null == Tran) return;
                if (!ExistingTrans.ContainsKey(Tran.ID)) return;
                obj = ExistingTrans[Tran.ID];
                lock (ExistingTrans)
                {
                    ExistingTrans.Remove(Tran.ID);
                }
                if (obj.Tran.CurrentTransaction.TransactionInformation.Status == TransactionStatus.Aborted
                    || obj.Tran.CurrentTransaction.TransactionInformation.Status == TransactionStatus.InDoubt
                    || obj.Tran.CurrentTransaction.TransactionInformation.Status == TransactionStatus.Committed)
                {
                    Console.WriteLine("Odd State when trying to rollback : {0}", obj.Tran.CurrentTransaction.TransactionInformation.Status);
                }
                else
                {
                    obj.Tran.RollBackTransaction();
                }

                if (obj.activeRecordTransactionManager != null)
                {
                    obj.activeRecordTransactionManager.RollbackTransaction();
                }

                obj.Complete();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                throw ex;
            }
            finally
            {
                obj.Dispose();
            }
        }

        public static void AttachCastleTransactionManager(IActiveDataTransaction Tran, ITransactionManager transactionManager)
        {
            Obj obj = null;
            try
            {
                if (null == Tran)
                    return;
                if (!ExistingTrans.ContainsKey(Tran.ID))
                    return;

                obj = ExistingTrans[Tran.ID];
                obj.activeRecordTransactionManager = transactionManager;
                Tran.HasCastleManagerAttached = true;
                if (obj.Tran.IsStarted)
                {
                    transactionManager.BeginTransactionReadUncommitted();
                }
            }
            catch (Exception exx)
            {
                LogPlugin.Logger.LogErrorMessageWithException("ActiveDataTransaction.AttachCastleTransactionManager", "Unable to AttachCastleTransactionManager", exx);
            }
        }

        public static void DisposeTransaction(IActiveDataTransaction Tran)
        {
            Obj obj = null;
            try
            {
                if (null == Tran) return;
                if (!ExistingTrans.ContainsKey(Tran.ID)) return;
                obj = ExistingTrans[Tran.ID];
                lock (ExistingTrans)
                {
                    ExistingTrans.Remove(Tran.ID);
                }

                obj.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }
    }

    [Serializable]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class X2ActiveDataTransaction : IActiveDataTransaction
    {
        private ITransactionContext ctx = null;
        private Transaction _Current;
        private bool _StartedTransaction = false;
        private bool Complete = false;
        private Guid _ID;
        private ITransactionManager castleTransactionManager;

        public ITransactionContext Context { get { return ctx; } }

        public X2ActiveDataTransaction()
        {
            _ID = Guid.NewGuid();
        }

        public void BeginTransaction(Transaction Current)
        {
            if (!_StartedTransaction)
            {
                // these are not serialisable .. WTF
                // have to start the tran first? Or maybe use a tras controller that will do this for us?
                // some kind of factory that starts them then hands the IX2ActiveTran back
                //_ts = new TransactionScope(TransactionScopeOption.Required);
                _Current = Current;
                ctx = TransactionController.GetContext("X2");

                //TransactionController.BeginTransactions(ctx);
                _StartedTransaction = true;
            }
        }

        public Guid ID
        {
            get
            {
                return _ID;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (_StartedTransaction)
                {
                    //if (null != ctx)
                    //    TransactionController.Commit(ctx);
                    Complete = true;
                }
                else
                {
                    throw new Exception("No Call to BeginTransaction()");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Complete = true;
                _StartedTransaction = false;
            }
        }

        public void RollBackTransaction()
        {
            try
            {
                if (!_StartedTransaction) return;
                _StartedTransaction = false;

                Complete = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                Complete = true;
            }
        }

        public void Dispose()
        {
            try
            {
                _StartedTransaction = false;
                _Current = null;
                if (!Complete)
                {
                    Complete = true;
                }
                if (null != ctx)
                {
                    ctx.Dispose();
                    ctx = null;
                }
                if (castleTransactionManager != null)
                {
                    castleTransactionManager.Dispose();
                    castleTransactionManager = null;
                }
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogFormattedErrorWithException("ActiveDataTransaction.Dispose", "Error disposing tran, _Started:{0} Complete:{1}", ex);
            }
        }

        public Transaction CurrentTransaction
        {
            get
            {
                if (null != _Current)
                {
                    return _Current;
                }
                return null;
            }
        }

        public bool IsStarted
        {
            get
            {
                return _StartedTransaction;
            }
        }

        public bool HasCastleManagerAttached { get; set; }

        public ITransactionManager CastleTransactionManager
        {
            get
            {
                return castleTransactionManager;
            }
            set
            {
                if (castleTransactionManager == null)
                {
                    castleTransactionManager = value;
                    if (_StartedTransaction)
                    {
                        castleTransactionManager.BeginTransaction();
                    }
                }
            }
        }
    }
}