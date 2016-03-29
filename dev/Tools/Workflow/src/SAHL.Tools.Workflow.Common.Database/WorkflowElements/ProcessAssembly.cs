using System.Collections.Generic;

namespace SAHL.Tools.Workflow.Common.Database.WorkflowElements
{
    public partial class ProcessAssembly
    {
        public ProcessAssembly()
        {
            ProcessAssemblies = new List<ProcessAssembly>();
        }

        public virtual int Id { get; set; }

        public virtual Process Process { get; set; }

        public virtual ProcessAssembly ParentProcessAssembly { get; set; }

        public virtual IList<ProcessAssembly> ProcessAssemblies { get; set; }

        public virtual string Dllname { get; set; }

        public virtual byte[] Dlldata { get; set; }
    }
}