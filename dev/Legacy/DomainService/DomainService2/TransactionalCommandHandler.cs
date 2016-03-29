namespace DomainService2
{
    using System;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.DataAccess;
    using SAHL.X2.Framework;
	using SAHL.Common.Exceptions;

    public class TransactionalCommandHandler<T> : IHandlesDomainServiceCommandDecorator<T> where T : IDomainServiceCommand
    {
        private IHandlesDomainServiceCommand<T> innerHandler;
        private ITransactionManager transactionManager;

        public TransactionalCommandHandler(IHandlesDomainServiceCommand<T> innerHandler, ITransactionManager transactionManager)
        {
            if (innerHandler == null)
            {
                throw new ArgumentNullException(Strings.ArgInnerHandler);
            }

            this.innerHandler = innerHandler;

            if (transactionManager == null)
            {
                throw new ArgumentNullException(Strings.ArgTransactionManager);
            }

            this.transactionManager = transactionManager;
        }

        public IHandlesDomainServiceCommand<T> InnerHandler
        {
            get { return this.innerHandler; }
        }

        public void Handle(IDomainMessageCollection messages, T command)
        {
            try
            {
                // begin the transaction
                this.transactionManager.BeginTransaction();

                // allow the inner handler to execute
                this.innerHandler.Handle(messages, command);

                // if there are error messages throw an exception
                if (messages.HasErrorMessages)
                {
                    // throw new domainmessagecollection
                    throw new DomainMessageException();
                }

                // check if there are warning messages and check if they can be ignored
                if (messages.HasWarningMessages)
                {
                    if (command.IgnoreWarnings)
                    {
                        // commit the transaction
                        this.transactionManager.CommitTransaction();
                    }
                    else
                    {
                        // if warnings cannot be ignored throw an exception
                        // throw new DomainMessageException();
                    }
                }

                // commit the transaction
                this.transactionManager.CommitTransaction();
            }
            catch
            {
                /**********************************************************************************************
                 Issue -> http://sahl-fb01/fogbugz/default.asp?2786#44280 
                **********************************************************************************************/

                /**********************************************************************************************
                    RollbackTransaction causes further issues
                    this.transactionManager.RollbackTransaction();
                **********************************************************************************************
                 * NHibernate.TransactionException
                 * Begin failed with SQL exception
                    at NHibernate.Transaction.AdoTransaction.Begin(IsolationLevel isolationLevel) in d:\CSharp\NH\NH\nhibernate\src\NHibernate\Transaction\AdoTransaction.cs:line 145
                    at NHibernate.AdoNet.ConnectionManager.BeginTransaction(IsolationLevel isolationLevel) in d:\CSharp\NH\NH\nhibernate\src\NHibernate\AdoNet\ConnectionManager.cs:line 318
                    at NHibernate.Impl.SessionImpl.BeginTransaction(IsolationLevel isolationLevel) in d:\CSharp\NH\NH\nhibernate\src\NHibernate\Impl\SessionImpl.cs:line 1414
                    at Castle.ActiveRecord.TransactionScope.OpenSession(ISessionFactory sessionFactory, IInterceptor interceptor) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\Scopes\TransactionScope.cs:line 220
                    at Castle.ActiveRecord.Framework.SessionFactoryHolder.OpenSessionWithScope(ISessionScope scope, ISessionFactory sessionFactory) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\SessionFactoryHolder.cs:line 245
                    at Castle.ActiveRecord.Framework.SessionFactoryHolder.CreateScopeSession(Type type) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\SessionFactoryHolder.cs:line 311
                    at Castle.ActiveRecord.Framework.SessionFactoryHolder.CreateSession(Type type) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\SessionFactoryHolder.cs:line 182
                    at Castle.ActiveRecord.ActiveRecordBase.Execute(Type targetType, NHibernateDelegate call, Object instance) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\ActiveRecordBase.cs:line 687
                    at Castle.ActiveRecord.ActiveRecordMediator.Execute(Type targetType, NHibernateDelegate call, Object instance) in f:\git\dev-g\Legacy\Castle.ActiveRecord\src\Castle.ActiveRecord\Framework\ActiveRecordMediator.cs:line 
                 ***********************************************************************************************/
                // Paul C: 17 sept 2013
                // Commit here. THe new X2 looks at teh SPC after all calls to the map and checks for messages
                // it will decide whether to rollback the txn or allow it to carry on
                this.transactionManager.CommitTransaction();
				throw;
            }
            finally
            {
                this.transactionManager.Dispose();
            }
        }
    }
}