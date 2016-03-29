using System.CodeDom.Compiler;
using SAHL.Tools.Workflow.Common.WorkflowElements;
using System;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    public interface IAppDomainCompiler
    {
        CompilerResults Compile(Process process);

        string LastSourceCodeCompiled { get; }
    }
}