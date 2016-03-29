using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Data
{
    public class InMemoryUIStatementProvider : IUIStatementProvider
    {
        private ConcurrentDictionary<string, string> uiStatements;

        public InMemoryUIStatementProvider(IDictionary<string, string> uiStatements)
        {
            this.uiStatements = new ConcurrentDictionary<string, string>(uiStatements);
        }

        public virtual string Get(string statementContext, string uiStatementName)
        {
            string uniqueStatementName = string.Format("{0}_{1}", statementContext, uiStatementName);
            if (this.uiStatements.ContainsKey(uniqueStatementName))
            {
                return this.uiStatements[uniqueStatementName];
            }
            else
            {
                return null;
            }
        }

        public string[] StatementKeys { get { return this.uiStatements.Keys.ToArray(); } }

        public void AddStatement(string statementName, string statement)
        {
            if (!this.uiStatements.ContainsKey(statementName))
            {
                this.uiStatements.TryAdd(statementName, statement);
            }
        }

        public virtual void Add(IUIStatementsProvider uiStatementsProvider)
        {
            throw new System.NotImplementedException();
        }
    }
}