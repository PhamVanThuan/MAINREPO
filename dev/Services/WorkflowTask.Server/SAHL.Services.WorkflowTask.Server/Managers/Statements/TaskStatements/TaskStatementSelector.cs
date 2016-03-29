using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Core.Data;
using SAHL.Core.DataStructures;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements
{
    public class TaskStatementSelector : ITaskStatementSelector
    {
        private readonly Assembly assembly;
        private const string namespaceTokenInclude = ".Statements.TaskStatements";
        private const string namespaceTokenExclude = "._template.";
        private readonly Type statementType = typeof(ISqlStatement<>);
        private readonly DelimitedRadixTree<Type> tree;

        public TaskStatementSelector(Assembly assemblyToScan)
        {
            this.assembly = assemblyToScan;
            this.tree = new DelimitedRadixTree<Type>(".");

            LoadTaskStatements();
        }

        public void LoadTaskStatements()
        {
            var types = GetTaskStatementTypes();

            foreach (var item in types)
            {
                //TODO: should be a better way to do this
                //Generic statement should be root of tree, all other statements must be added to the tree as normal
                if (typeof(IGenericStatement).IsAssignableFrom(item))
                {
                    this.tree.Root.NodeValue = item;
                }
                else
                {
                    var key = CreateLookupKey(item.FullName);
                    this.tree.Add(key, item);
                }
            }
        }

        public string CreateLookupKey(string typeFullName)
        {
            var startIndex = typeFullName.IndexOf(namespaceTokenInclude) + namespaceTokenInclude.Length + 1;
            var indexOfLastDot = typeFullName.LastIndexOf(".");
            var length = indexOfLastDot - startIndex;

            var workflowChainString = length <= 0
                ? typeFullName.Substring(startIndex)
                : typeFullName.Substring(startIndex, length);

            return workflowChainString;
        }

        public IEnumerable<Type> GetTaskStatementTypes()
        {
            return this.assembly.DefinedTypes
                .Where(a => a.IsClass
                    && !a.IsAbstract
                    && a.Namespace != null
                    && a.Namespace.Contains(namespaceTokenInclude)
                    && !a.Namespace.Contains(namespaceTokenExclude)
                    && a.GetInterfaces()
                        .Any(b => b.Namespace == this.statementType.Namespace
                            && b.Name.Equals(this.statementType.Name)
                        )
                )
                .ToList();
        }

        public Type GetStatementTypeForWorkFlow(string businessProcessName, string workFlowName, string stateName)
        {
            //TODO: Returns null if there is a statement for the State but not one for the Workflow
            //TODO: Should returns the last non-null value found in the chain
            return this.tree.GetValue(new[] { businessProcessName, workFlowName, stateName });
        }
    }
}
