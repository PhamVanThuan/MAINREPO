using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class ProcessAssemblyNuGetInfo
    {
        public ProcessAssemblyNuGetInfo()
        {
        }

        public virtual int Id { get; set; }

        public virtual Process Process { get; set; }

        public virtual string PackageName { get; set; }

        public virtual string PackageVersion { get; set; }
    }
}