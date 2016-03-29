using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Data
{
    public class AssemblyUIStatementProvider : InMemoryUIStatementProvider
    {
        private List<IUIStatementsProvider> statementsProviders;

        public AssemblyUIStatementProvider(IUIStatementsProvider[] statementsProviders)
            : base(new ConcurrentDictionary<string, string>() { })
        {
            this.statementsProviders = new List<IUIStatementsProvider>(statementsProviders);
        }

        public override string Get(string statementContext, string uiStatementName)
        {
            string uniqueStatementName = string.Format("{0}_{1}", statementContext, uiStatementName);
            if (!base.StatementKeys.Contains(uniqueStatementName))
            {
                this.ProcessStatement(uiStatementName, statementContext);
            }
            return base.Get(statementContext, uiStatementName);
        }

        private void ProcessStatement(string uiStatementName, string statementContext)
        {
            string statementText = string.Empty;

            // find the IUIStatementsProvider that matches the statement namespace
            IUIStatementsProvider statementProvider = this.statementsProviders.Where(x => x.UIStatementContext == statementContext).SingleOrDefault();

            if (statementProvider == null)
            {
                throw new Exception(string.Format("No StatementProvider found for context {0} - {1}", statementContext, uiStatementName));
            }

            // then get the uistatement
            Type uiStatementsType = statementProvider.GetType();
            string shortStatementName = uiStatementName.Replace(string.Format("{0}.", statementContext), "").ToLower();

            FieldInfo field = uiStatementsType.GetField(shortStatementName);
            statementText = field.GetRawConstantValue().ToString();

            string uniqueStatementName = string.Format("{0}_{1}", statementContext, uiStatementName);
            base.AddStatement(uniqueStatementName, statementText);
        }

        public override void Add(IUIStatementsProvider uiStatementsProvider)
        {
            var existingStatementProvider = this.statementsProviders.Where(x => x.UIStatementContext == uiStatementsProvider.UIStatementContext).FirstOrDefault();
            if (existingStatementProvider == null)
            {
                this.statementsProviders.Add(uiStatementsProvider);
            }
        }
    }
}