using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class Process
    {
        public Process()
        {
            ProcessAssemblies = new List<ProcessAssembly>();
            ProcessAssemblyNuGetInfos = new List<ProcessAssemblyNuGetInfo>();
            SecurityGroups = new List<SecurityGroup>();
            WorkFlows = new List<WorkFlow>();
        }

        public virtual int Id { get; set; }

        public virtual Process ParentProcess { get; set; }

        public virtual IList<ProcessAssembly> ProcessAssemblies { get; set; }

        public virtual IList<ProcessAssemblyNuGetInfo> ProcessAssemblyNuGetInfos { get; set; }

        public virtual IList<SecurityGroup> SecurityGroups { get; set; }

        public virtual IList<WorkFlow> WorkFlows { get; set; }

        public virtual string Name { get; set; }

        public virtual string Version { get; set; }

        public virtual byte[] DesignerData { get; set; }

        public virtual System.DateTime CreateDate { get; set; }

        public virtual string MapVersion { get; set; }

        public virtual bool IsLegacy { get; set; }

        public virtual string ViewableOnUserInterfaceVersion { get; set; }

        public virtual string ConfigFile { get; set; }
    }
}