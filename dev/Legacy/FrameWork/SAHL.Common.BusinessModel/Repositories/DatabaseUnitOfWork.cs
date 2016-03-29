using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Scopes;
using SAHL.Common.Attributes;
using NHibernate;
using System.Data;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IDataBaseUnitOfWork))]
    public class DatabaseUnitOfWork : IDataBaseUnitOfWork
    {
        ISessionScope _session = null;
        public DatabaseUnitOfWork(IDbConnection conn)
        {
            this._session = new DifferentDatabaseScope(conn);
        }

        public DatabaseUnitOfWork(DifferentDatabaseScope scope)
        {
            this._session = scope;
        }

        public bool HasTransaction
        {
            get
            {
                return true;
            }
        }

        public void Save(object ObjectToSave)
        {

        }
        public void PersistAll()
        {
            if (_session == null)
                throw new InvalidOperationException("The unit of work has already been disposed.");

            _session.Flush();

            ((TransactionScope)_session).VoteCommit();

            _session.Dispose();
            _session = null;
        }

        public void UndoAll()
        {
            if (_session == null)
                throw new InvalidOperationException("The unit of work has already been disposed.");

            ((TransactionScope)_session).VoteRollBack();

            _session.Dispose();
            _session = null;
        }
    }
}
