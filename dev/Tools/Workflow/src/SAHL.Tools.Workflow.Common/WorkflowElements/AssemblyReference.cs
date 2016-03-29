using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAHL.Tools.Workflow.Common.WorkflowElements
{
    [Serializable]
    public class AssemblyReference : AbstractNamedElement
    {
        public AssemblyReference(string name, string fullName, string path, string version)
            : base(name)
        {
            this.FullName = fullName;
            this.Path = path;
            this.Version = version;
        }

        public string FullName { get; protected set; }

        public string Path { get; protected set; }

        public string Version { get; protected set; }

    }
}