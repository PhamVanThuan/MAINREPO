using Machine.Fakes;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using System;

namespace SAHL.Core.Testing.Fakes
{
    public class FakeDbFactory : IDbFactory
    {
        public FakeDbFactory()
        {
            this.FakedDb = new FakeDb();
        }

        public IDb NewDb()
        {
            return this.FakedDb;
        }

        public FakeDb FakedDb { get; protected set; }
    }

    public class FakeDb : WithFakes, IDb
    {
        public FakeDb()
        {
            this.DDLDbContext = An<IDdlDbContext>();

            this.DbContext = An<IDbContext>();

            this.DbReadOnlyContext = An<IReadOnlyDbContext>();
        }

        public IDdlDbContext DDLDbContext { get; protected set; }

        public IDbContext DbContext { get; protected set; }

        public IReadOnlyDbContext DbReadOnlyContext { get; protected set; }

        public IDdlDbContext DDLInAppContext()
        {
            return this.DDLDbContext;
        }

        public IDdlDbContext DDLInWorkflowContext()
        {
            return this.DDLDbContext;
        }

        public Data.Context.Builders.IReadOnlyDbContextBuilder ForReadOnly()
        {
            return new FakeReadOnlyDbContextBuilder(this.DbReadOnlyContext);
        }

        public Data.Context.Builders.IReadWriteDbContextBuilder ForReadWrite()
        {
            throw new NotImplementedException();
        }

        public Data.Context.IDbContext InAppContext()
        {
            return this.DbContext;
        }

        public Data.Context.IDbContext InEventContext()
        {
            return this.DbContext;
        }

        public Data.Context.IReadOnlyDbContext InReadOnlyAppContext()
        {
            return this.DbReadOnlyContext;
        }

        public Data.Context.IReadOnlyDbContext InReadOnlyWorkflowContext()
        {
            return this.DbReadOnlyContext;
        }

        public Data.Context.IDbContext InWorkflowContext()
        {
            return this.DbContext;
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithInheritedTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithNewTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithNoTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }

        public Data.Context.Builders.ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.DbContext);
        }
    }
}