using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class Process : AbstractNamedElement
    {
        private List<Workflow> workflows;
        private List<GlobalRole> roles;
        private List<UsingStatement> usingStatements;
        private List<AssemblyReference> assemblyReferences;

        public Process(string name, string productVersion, string mapVersion, string retrieved, string legacy, string viewableOnUserInterfaceVersion)
            : base(name)
        {
            this.MapVersion = mapVersion;
            this.Retrieved = retrieved;
            this.ProductVersion = productVersion;
            this.Legacy = legacy;
            this.ViewableOnUserInterfaceVersion = viewableOnUserInterfaceVersion;

            this.workflows = new List<Workflow>();
            this.roles = new List<GlobalRole>();
            this.usingStatements = new List<UsingStatement>();
            this.assemblyReferences = new List<AssemblyReference>();
        }

        public string ProductVersion { get; protected set; }

        public string MapVersion { get; protected set; }

        public string Retrieved { get; protected set; }

        public string Legacy { get; protected set; }

        public string ViewableOnUserInterfaceVersion { get; protected set; }

        public Collection<Workflow> Workflows
        {
            get
            {
                return new Collection<Workflow>(this.workflows);
            }
        }

        public void AddWorkflow(Workflow workflow)
        {
            this.workflows.Add(workflow);
        }

        public void RemoveWorkflowByName(string workflowName)
        {
            Workflow workflow = workflows.Where(x => x.Name.ToLower() == workflowName.ToLower()).SingleOrDefault();
            if (workflow != null)
            {
                RemoveWorkflow(workflow);
            }
        }

        public void RemoveWorkflow(Workflow workflow)
        {
            this.workflows.Remove(workflow);
        }

        public ReadOnlyCollection<GlobalRole> Roles
        {
            get
            {
                return new ReadOnlyCollection<GlobalRole>(this.roles);
            }
        }

        public ReadOnlyCollection<GlobalRole> DynamicGlobalRoles
        {
            get
            {
                List<GlobalRole> globalAbstractRoles = this.roles.Where(x => x.IsDynamic == true).ToList();
                List<GlobalRole> globalRoles = new List<GlobalRole>();
                foreach (AbstractRole globalRole in globalAbstractRoles)
                {
                    globalRoles.Add((GlobalRole)globalRole);
                }

                return new ReadOnlyCollection<GlobalRole>(globalRoles);
            }
        }

        public void AddRole(GlobalRole role)
        {
            this.roles.Add(role);
        }

        public void RemoveRole(GlobalRole role)
        {
            this.roles.Remove(role);
        }

        public ReadOnlyCollection<UsingStatement> UsingStatements
        {
            get
            {
                return new ReadOnlyCollection<UsingStatement>(this.usingStatements);
            }
        }

        public void AddUsingStatement(UsingStatement usingStatement)
        {
            this.usingStatements.Add(usingStatement);
        }

        public void RemoveUsingStatement(UsingStatement usingStatement)
        {
            this.usingStatements.Remove(usingStatement);
        }

        public ReadOnlyCollection<AssemblyReference> AssemblyReferences
        {
            get
            {
                return new ReadOnlyCollection<AssemblyReference>(this.assemblyReferences);
            }
        }

        public void AddAssemblyReference(AssemblyReference assemblyReference)
        {
            this.assemblyReferences.Add(assemblyReference);
        }

        public void RemoveAssemblyReference(AssemblyReference assemblyReference)
        {
            this.assemblyReferences.Remove(assemblyReference);
        }
    }
}