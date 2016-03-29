namespace SAHL.Tools.Workflow.Common.Compiler
{
    public interface IAppDomainCompilerError
    {
        string ErrorNumber { get; }

        string ErrorText { get; }

        string FileName { get; }

        int Line { get; }

        int Column { get; }

        bool IsWarning { get; }
    }
}