using SAHL.Core.Data.Configuration;
using System;

namespace SAHL.Core.Data.Dapper
{
    public class DapperUIStatementProvider : IUIStatementProvider
    {
        private IDbConfigurationProvider dbConfigurationProvider;

        public DapperUIStatementProvider(IDbConfigurationProvider dbConfigurationProvider)
        {
            this.dbConfigurationProvider = dbConfigurationProvider;
        }

        public string Get(string statementContext, string uiStatementName)
        {
            throw new NotImplementedException();
        }

        public void Add(IUIStatementsProvider uiStatementsProvider)
        {
            throw new NotImplementedException();
        }
    }
}